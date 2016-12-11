using System;
using Mors.AppPlatform.Adapters.Dispatching;
using Mors.AppPlatform.Support.Dispatching;
using Mors.AppPlatform.Support.Transactions;

namespace Mors.AppPlatform.Adapters.Words
{
    internal sealed class ApplicationCommandHandlerRegistry
    {
        private readonly IHandlerRegistry _handlerRegistry;
        private readonly Transaction _transaction;
        private readonly ApplicationEventBus _eventBus;

        public ApplicationCommandHandlerRegistry(
            IHandlerRegistry handlerRegistry,
            Transaction transaction,
            ApplicationEventBus eventBus)
        {
            _handlerRegistry = handlerRegistry;
            _transaction = transaction;
            _eventBus = eventBus;
        }

        public void Register(Type commandType, Action<object, Mors.Words.EventPublisher> handler)
        {
            var commandKey = CommandKey.From(commandType);
            _handlerRegistry.Set(
                commandKey,
                command =>
                {
                    _transaction.Run(() => handler(command, _eventBus.Publish));
                    return null;
                });
        }
    }
}
