using System;
using Mors.AppPlatform.Adapters.Dispatching;
using Mors.AppPlatform.Support.Dispatching;
using Mors.AppPlatform.Support.Transactions;

namespace Mors.AppPlatform.Adapters.Journeys
{
    internal sealed class ApplicationCommandHandlerRegistry : Mors.Journeys.Application.ICommandHandlerRegistry
    {
        private readonly IHandlerRegistry _handlerRegistry;
        private readonly Transaction _transaction;

        public ApplicationCommandHandlerRegistry(IHandlerRegistry handlerRegistry, Transaction transaction)
        {
            _handlerRegistry = handlerRegistry;
            _transaction = transaction;
        }

        public void SetHandler<TCommand>(Action<TCommand> handler)
        {
            var commandKey = CommandKey.From<TCommand>();
            _handlerRegistry.Set(
                commandKey,
                command =>
                {
                    _transaction.Run(() => handler((TCommand)command));
                    return null;
                });
        }
    }
}
