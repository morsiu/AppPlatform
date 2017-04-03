using System;
using Mors.AppPlatform.Adapters.Dispatching;
using Mors.AppPlatform.Support.Dispatching;
using Mors.AppPlatform.Support.Transactions;
using Mors.AppPlatform.Support.Repositories;

namespace Mors.AppPlatform.Adapters.Words
{
    internal sealed class ApplicationCommandHandlerRegistry
    {
        private readonly IHandlerRegistry _handlerRegistry;
        private readonly Transaction _transaction;
        private readonly ApplicationEventBus _eventBus;
        private readonly GuidIdFactory _idFactory;

        public ApplicationCommandHandlerRegistry(
            IHandlerRegistry handlerRegistry,
            Transaction transaction,
            ApplicationEventBus eventBus,
            GuidIdFactory idFactory)
        {
            _handlerRegistry = handlerRegistry;
            _transaction = transaction;
            _eventBus = eventBus;
            _idFactory = idFactory;
        }

        public void Register(Type commandType, Action<object, Mors.Words.EventPublisher, Mors.Words.IdFactory> handler)
        {
            var commandKey = CommandKey.From(commandType);
            _handlerRegistry.Set(
                commandKey,
                command =>
                {
                    _transaction.Run(() => handler(command, _eventBus.Publish, () => _idFactory.Create()));
                    return null;
                });
        }
    }
}
