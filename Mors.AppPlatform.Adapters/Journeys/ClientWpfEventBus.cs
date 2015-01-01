using System;
using Mors.AppPlatform.Support.Events;

namespace Mors.AppPlatform.Adapters.Journeys
{
    internal sealed class ClientWpfEventBus : Mors.Journeys.Application.Client.Wpf.IEventBus
    {
        private readonly IEventBus _eventBus;

        public ClientWpfEventBus(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public void Publish<TEvent>(TEvent @event)
        {
            _eventBus.Publish(@event);
        }

        public void RegisterListener<TEvent>(Action<TEvent> handler)
        {
            _eventBus.RegisterListener(new EventListener<TEvent>(handler));
        }
    }
}
