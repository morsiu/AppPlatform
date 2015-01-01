using System;
using Mors.AppPlatform.Support.Events;

namespace Mors.AppPlatform.Adapters.Journeys
{
    internal sealed class ApplicationEventBus : Mors.Journeys.Application.IEventBus
    {
        private readonly IEventBus _eventBus;

        public ApplicationEventBus(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public void RegisterListener<TEvent>(Action<TEvent> handler)
        {
            _eventBus.RegisterListener(new EventListener<TEvent>(handler));
        }

        public void Publish(object @event)
        {
            var publishMethod = typeof(IEventBus).GetMethod("Publish").MakeGenericMethod(@event.GetType());
            publishMethod.Invoke(_eventBus, new[] { @event });
        }
    }
}
