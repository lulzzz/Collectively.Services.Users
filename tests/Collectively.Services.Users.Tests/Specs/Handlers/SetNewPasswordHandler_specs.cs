﻿using System;
using Collectively.Messages.Commands;
using Collectively.Common.Domain;
using Collectively.Common.Services;
using Collectively.Services.Users.Handlers;
using Collectively.Services.Users.Services;
using Collectively.Messages.Commands.Users;
using Collectively.Messages.Events.Users;
using Machine.Specifications;
using Moq;
using RawRabbit;
using It = Machine.Specifications.It;
using RawRabbit.Pipe;
using System.Threading;

namespace Collectively.Services.Users.Tests.Specs.Handlers
{
    public class SetNewPasswordHandler_specs : SpecsBase
    {
        protected static SetNewPasswordHandler SetNewPasswordHandler;
        protected static IHandler Handler;
        protected static Mock<IPasswordService> PasswordServiceMock;
        protected static Mock<IExceptionHandler> ExceptionHandlerMock;
        protected static SetNewPassword Command;

        protected static void Initialize()
        {
            ExceptionHandlerMock = new Mock<IExceptionHandler>();
            Handler = new Handler(ExceptionHandlerMock.Object);
            PasswordServiceMock = new Mock<IPasswordService>();
            SetNewPasswordHandler = new SetNewPasswordHandler(Handler, 
                BusClientMock.Object, PasswordServiceMock.Object);

            Command = new SetNewPassword
            {
                Request = new Request
                {
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.Now,
                    Culture = "en-US",
                    Name = "name",
                    Origin = "collectively",
                    Resource = "resource"
                },
                Email = "email",
                Password = "password",
                Token = "token"
            };
        }
    }

    [Subject("SetNewPasswordHandler HandleAsync")]
    public class when_handle_async_set_new_password : SetNewPasswordHandler_specs
    {
        Establish context = () => Initialize();

        Because of = () => SetNewPasswordHandler.HandleAsync(Command).Await();

        It should_call_set_new_async = () => PasswordServiceMock.Verify(x => x.SetNewAsync(
            Command.Email, Command.Token, Command.Password), Times.Once);

        It should_publish_new_password_set_event = () => VerifyPublishAsync(
            Moq.It.Is<NewPasswordSet>(m => m.RequestId == Command.Request.Id
                                            && m.Email == Command.Email),
            Times.Once);

        It should_not_publish_set_new_password_rejected = () => VerifyPublishAsync(
            Moq.It.IsAny<SetNewPasswordRejected>(),
            Times.Never);
    }


    [Subject("SetNewPasswordHandler HandleAsync")]
    public class when_handle_async_set_new_password_and_it_throws_custom_error : SetNewPasswordHandler_specs
    {
        protected static string ErrorCode = "Error";

        Establish context = () =>
        {
            Initialize();
            PasswordServiceMock.Setup(x => x.SetNewAsync(
                    Moq.It.IsAny<string>(), Moq.It.IsAny<string>(), Moq.It.IsAny<string>()))
                .Throws(new ServiceException(ErrorCode));
        };

        Because of = () => SetNewPasswordHandler.HandleAsync(Command).Await();

        It should_call_set_new_async = () => PasswordServiceMock.Verify(x => x.SetNewAsync(
            Command.Email, Command.Token, Command.Password), Times.Once);

        It should_not_publish_new_password_set_event = () => VerifyPublishAsync(
            Moq.It.IsAny<NewPasswordSet>(),
            Times.Never);

        It should_publish_set_new_password_rejected = () => VerifyPublishAsync(
            Moq.It.Is<SetNewPasswordRejected>(m => m.RequestId == Command.Request.Id
                                                    && m.Code == ErrorCode
                                                    && m.Email == Command.Email),
            Times.Once);
    }

    [Subject("SetNewPasswordHandler HandleAsync")]
    public class when_handle_async_set_new_password_and_it_fails : SetNewPasswordHandler_specs
    {
        Establish context = () =>
        {
            Initialize();
            PasswordServiceMock.Setup(x => x.SetNewAsync(
                    Moq.It.IsAny<string>(), Moq.It.IsAny<string>(), Moq.It.IsAny<string>()))
                .Throws<Exception>();
        };

        Because of = () => SetNewPasswordHandler.HandleAsync(Command).Await();

        It should_call_set_new_async = () => PasswordServiceMock.Verify(x => x.SetNewAsync(
            Command.Email, Command.Token, Command.Password), Times.Once);

        It should_not_publish_new_password_set_event = () => VerifyPublishAsync(
            Moq.It.IsAny<NewPasswordSet>(),
            Times.Never);

        It should_publish_set_new_password_rejected = () => VerifyPublishAsync(
            Moq.It.Is<SetNewPasswordRejected>(m => m.RequestId == Command.Request.Id
                                                    && m.Code == OperationCodes.Error
                                                    && m.Email == Command.Email),
            Times.Once);
    }
}