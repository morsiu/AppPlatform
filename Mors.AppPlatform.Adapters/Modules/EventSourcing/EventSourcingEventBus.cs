using Mors.AppPlatform.Support.Events;
using Mors.AppPlatform.Support.Transactions;
using Mors.AppPlatform.Adapters.Modules.Domain;

namespace Mors.AppPlatform.Adapters.Modules.EventSourcing
{
    public sealed class EventSourcingEventBus : IEventBus
    {
        private readonly Mors.AppPlatform.Support.Events.IEventBus _eventBus;

        public EventSourcingEventBus(Mors.AppPlatform.Support.Events.IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public Journeys.Domain.Infrastructure.IEventBus ForDomain()
        {
            return new DomainEventBusAdapter(_eventBus);
        }

        public ITransactional<IEventBus> Lift()
        {
            return new EventSourcingTransactedEventBus(_eventBus);
        }
    }
}
