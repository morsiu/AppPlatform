using Mors.AppPlatform.Support.Events;

namespace Mors.AppPlatform.Adapters.Words
{
    internal sealed class ApplicationEventBus
    {
        private readonly IEventBus _eventBus;

        public ApplicationEventBus(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public void Publish(object @event)
        {
            var publishMethod = typeof(IEventBus).GetMethod("Publish").MakeGenericMethod(@event.GetType());
            publishMethod.Invoke(_eventBus, new[] { @event });
        }
    }
}
