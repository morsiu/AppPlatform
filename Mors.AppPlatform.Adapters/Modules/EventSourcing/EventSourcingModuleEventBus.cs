using System;
using Mors.AppPlatform.Support.Transactions;

namespace Mors.AppPlatform.Adapters.Modules.EventSourcing
{
    public sealed class EventSourcingModuleEventBus : Mors.AppPlatform.Support.EventSourcing.IEventBus
    {
        private readonly Mors.AppPlatform.Support.Events.IEventBus _eventBus;

        public EventSourcingModuleEventBus(Mors.AppPlatform.Support.Events.IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public void RegisterListener<TEvent>(Action<TEvent> handler)
        {
            _eventBus.RegisterListener(new Mors.AppPlatform.Support.Events.EventListener<TEvent>(handler));
        }

        public ITransactional<Mors.AppPlatform.Support.EventSourcing.IEventBus> Lift()
        {
            return new EventSourcingModuleTransactedEventBus(_eventBus);
        }
    }
}
