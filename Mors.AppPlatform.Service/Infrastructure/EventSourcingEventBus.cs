using System;
using Mors.AppPlatform.Support.Events;

namespace Mors.AppPlatform.Adapters.Services
{
    public sealed class EventSourcingEventBus : Support.EventSourcing.Dependencies.IEventBus
    {
        private readonly IEventBus _eventBus;

        public EventSourcingEventBus(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public void RegisterListener<TEvent>(Action<TEvent> handler)
        {
            _eventBus.RegisterListener(new EventListener<TEvent>(handler));
        }
    }
}