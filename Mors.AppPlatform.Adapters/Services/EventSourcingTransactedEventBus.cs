using System;
using Mors.AppPlatform.Common.Transactions;
using Mors.AppPlatform.Support.Events;

namespace Mors.AppPlatform.Adapters.Services
{
    internal sealed class EventSourcingTransactedEventBus : Support.EventSourcing.IEventBus, ITransactional<Support.EventSourcing.IEventBus>
    {
        private readonly ITransactional<IEventBus> _eventBus;

        public EventSourcingTransactedEventBus(IEventBus eventBus)
        {
            _eventBus = eventBus.Lift();
        }

        public ITransactional<Support.EventSourcing.IEventBus> Lift()
        {
            return this;
        }

        public Support.EventSourcing.IEventBus Object
        {
            get { return this; }
        }

        public void Abort()
        {
            _eventBus.Abort();
        }

        public void Commit()
        {
            _eventBus.Commit();
        }

        public void RegisterListener<TEvent>(Action<TEvent> handler)
        {
            _eventBus.Object.RegisterListener(new EventListener<TEvent>(handler));
        }
    }
}
