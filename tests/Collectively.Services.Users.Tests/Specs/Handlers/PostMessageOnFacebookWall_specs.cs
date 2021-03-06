﻿using System;
using Collectively.Messages.Commands;
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
    public abstract class PostMessageOnFacebookWall_specs : SpecsBase
    {
        protected static PostOnFacebookWallHandler PostOnFacebookWallHandler;
        protected static IHandler Handler;
        protected static Mock<IFacebookService> FacebookServiceMock;
        protected static Mock<IExceptionHandler> ExceptionHandlerMock;
        protected static PostOnFacebookWall Command;

        protected static void Initialize()
        {
            InitializeBus();
            ExceptionHandlerMock = new Mock<IExceptionHandler>();
            Handler = new Handler(ExceptionHandlerMock.Object);
            FacebookServiceMock = new Mock<IFacebookService>();
            PostOnFacebookWallHandler = new PostOnFacebookWallHandler(Handler,
                BusClientMock.Object, FacebookServiceMock.Object);

            Command = new PostOnFacebookWall
            {
                AccessToken = "token",
                Message = "message",
                UserId = "userId",
                Request = new Request
                {
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.Now,
                    Culture = "en-US",
                    Name = "name",
                    Origin = "collectively",
                    Resource = "resource"
                }
            };
        }
    }

    [Subject("PostOnFacebookWallHandler HandleAsync")]
    public class when_handle_async_post_message_on_facebook : PostMessageOnFacebookWall_specs
    {
        Establish context = () => Initialize();

        Because of = () => PostOnFacebookWallHandler.HandleAsync(Command).Await();

        It should_call_post_on_wall_async = () => FacebookServiceMock.Verify(x => x.PostOnWallAsync(
                Command.AccessToken, Command.Message),
            Times.Once);

        It should_publish_message_posted_event = () => VerifyPublishAsync(
                Moq.It.Is<MessageOnFacebookWallPosted>(m => m.RequestId == Command.Request.Id
                                                            && m.UserId == Command.UserId
                                                            && m.Message == Command.Message),
            Times.Once);

        It should_not_publish_post_message_rejected = () => VerifyPublishAsync(
            Moq.It.IsAny<PostOnFacebookWallRejected>(),
            Times.Never);

    }

    [Subject("PostOnFacebookWallHandler HandleAsync")]
    public class when_handle_async_post_message_on_facebook_and_it_fails : PostMessageOnFacebookWall_specs
    {
        Establish context = () =>
        {
            Initialize();
            FacebookServiceMock.Setup(x => x.PostOnWallAsync(Moq.It.IsAny<string>(), Moq.It.IsAny<string>()))
                .Throws<Exception>();
        };

        Because of = () => PostOnFacebookWallHandler.HandleAsync(Command).Await();

        It should_call_post_on_wall_async = () => FacebookServiceMock.Verify(x => x.PostOnWallAsync(
                Command.AccessToken, Command.Message),
            Times.Once);

        It should_publish_message_posted_event = () => VerifyPublishAsync(
                Moq.It.IsAny<MessageOnFacebookWallPosted>(),
            Times.Never);

        It should_not_publish_post_message_rejected = () => VerifyPublishAsync(
                Moq.It.Is<PostOnFacebookWallRejected>(m => m.RequestId == Command.Request.Id
                                                                  && m.UserId == Command.UserId
                                                                  && m.Code == OperationCodes.Error
                                                                  && m.Message == Command.Message),
            Times.Once);

    }
}