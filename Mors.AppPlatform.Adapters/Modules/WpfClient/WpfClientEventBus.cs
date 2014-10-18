using System;

namespace Mors.AppPlatform.Adapters.Modules.WpfClient
{
    public sealed class WpfClientEventBus : Application.Client.Wpf.IEventBus
    {
        private readonly Mors.AppPlatform.Support.Events.IEventBus _eventBus;

        public WpfClientEventBus(Mors.AppPlatform.Support.Events.IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public void Publish<TEvent>(TEvent @event)
        {
            _eventBus.Publish(@event);
        }

        public void Subscribe<TEvent>(Action<TEvent> listener)
        {
            _eventBus.RegisterListener(new Mors.AppPlatform.Support.Events.EventListener<TEvent>(listener));
        }
    }
}
