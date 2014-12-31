using System;
using Mors.AppPlatform.Support.Events;

namespace Mors.AppPlatform.Adapters.Services
{
    public sealed class EventBus : Common.Services.IEventBus
    {
        private readonly IEventBus _eventBus;

        public EventBus(IEventBus eventBus)
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
    }
}
