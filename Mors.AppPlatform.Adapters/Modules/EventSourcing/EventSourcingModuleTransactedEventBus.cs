using System;
using Mors.AppPlatform.Support.Events;
using Mors.AppPlatform.Support.Transactions;

namespace Mors.AppPlatform.Adapters.Modules.EventSourcing
{
    internal sealed class EventSourcingModuleTransactedEventBus : Mors.AppPlatform.Support.EventSourcing.IEventBus, ITransactional<Mors.AppPlatform.Support.EventSourcing.IEventBus>
    {
        private readonly ITransactional<IEventBus> _eventBus;

        public EventSourcingModuleTransactedEventBus(Mors.AppPlatform.Support.Events.IEventBus eventBus)
        {
            _eventBus = eventBus.Lift();
        }

        public void RegisterListener<TEvent>(Action<TEvent> handler)
        {
            _eventBus.Object.RegisterListener(new Mors.AppPlatform.Support.Events.EventListener<TEvent>(handler));
        }

        public ITransactional<Mors.AppPlatform.Support.EventSourcing.IEventBus> Lift()
        {
            return this;
        }

        public Mors.AppPlatform.Support.EventSourcing.IEventBus Object
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
    }
}
