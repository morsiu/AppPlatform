namespace Mors.AppPlatform.Adapters.Modules.Domain
{
    public sealed class DomainEventBusAdapter : Journeys.Domain.Infrastructure.IEventBus
    {
        private readonly Mors.AppPlatform.Support.Events.IEventBus _eventBus;

        public DomainEventBusAdapter(Mors.AppPlatform.Support.Events.IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public void Publish<TEvent>(TEvent @event)
        {
            _eventBus.Publish(@event);
        }
    }
}
