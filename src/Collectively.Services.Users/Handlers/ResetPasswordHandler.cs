﻿using System;
using System.Threading.Tasks;
using Collectively.Messages.Commands;
using Collectively.Common.Services;
using Collectively.Services.Users.Services;

using Collectively.Messages.Commands.Users;
using Collectively.Messages.Events.Users;
using Collectively.Messages.Commands.Mailing;
using RawRabbit;

namespace Collectively.Services.Users.Handlers
{
    public class ResetPasswordHandler : ICommandHandler<ResetPassword>
    {
        private readonly IHandler _handler;
        private readonly IBusClient _bus;
        private readonly IPasswordService _passwordService;
        private readonly IOneTimeSecuredOperationService _oneTimeSecuredOperationService;

        public ResetPasswordHandler(IHandler handler,
            IBusClient bus, 
            IPasswordService passwordService,
            IOneTimeSecuredOperationService oneTimeSecuredOperationService)
        {
            _handler = handler;
            _bus = bus;
            _passwordService = passwordService;
            _oneTimeSecuredOperationService = oneTimeSecuredOperationService;
        }

        public async Task HandleAsync(ResetPassword command)
        {
            var operationId = Guid.NewGuid();
            await _handler
                .Run(async () =>
                {
                    await _passwordService.ResetAsync(operationId, command.Email);
                })
                .OnSuccess(async () =>
                {
                    var operation = await _oneTimeSecuredOperationService.GetAsync(operationId);
                    await _bus.PublishAsync(new SendResetPasswordEmailMessage
                    {
                        Email = command.Email,
                        Endpoint = command.Endpoint,
                        Token = operation.Value.Token,
                        Request = Request.From<SendResetPasswordEmailMessage>(command.Request)
                    });
                })
                .OnCustomError(async ex => await _bus.PublishAsync(new ResetPasswordRejected(command.Request.Id,
                    ex.Message, ex.Code, command.Email)))
                .OnError(async (ex, logger) =>
                {
                    logger.Error(ex, "Error occured while resetting password");
                    await _bus.PublishAsync(new ResetPasswordRejected(command.Request.Id,
                        ex.Message, OperationCodes.Error, command.Email));
                })
                .ExecuteAsync();
        }
    }
}