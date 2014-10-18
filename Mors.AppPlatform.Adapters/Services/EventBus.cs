using Mors.AppPlatform.Support.Events;
using Mors.AppPlatform.Common.Transactions;
using System;

namespace Mors.AppPlatform.Adapters.Modules.Command
{
    public sealed class CommandEventBus : Common.Services.IEventBus
    {
        private readonly Support.Events.IEventBus _eventBus;

        public CommandEventBus(Support.Events.IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public void RegisterListener<TEvent>(Action<TEvent> handler)
        {
            _eventBus.RegisterListener(new EventListener<TEvent>(handler));
        }

        public void Publish<TEvent>(TEvent @event)
        {
            _eventBus.Publish(@event);
        }

        public ITransactional<Common.Services.IEventBus> Lift()
        {
            return new TransactedEventBus(_eventBus);
        }

        ITransactional<Common.Services.IEventBus> IProvideTransactional<Common.Services.IEventBus>.Lift()
        {
            throw new NotImplementedException();
        }
    }
}
