using System;
using System.Reflection;
using Mors.AppPlatform.Support.Events;
using Mors.AppPlatform.Support.EventSourcing;

namespace Mors.AppPlatform.Adapters.Words
{
    internal sealed class ApplicationEventBus
    {
        private readonly IEventBus _eventBus;
        private readonly EventSourcingModule _eventSourcingModule;

        public ApplicationEventBus(IEventBus eventBus, EventSourcingModule eventSourcingModule)
        {
            _eventBus = eventBus;
            _eventSourcingModule = eventSourcingModule;
        }

        public void Publish(object @event)
        {
            var publishMethod = typeof(IEventBus).GetMethod("Publish").MakeGenericMethod(@event.GetType());
            publishMethod.Invoke(_eventBus, new[] { @event });
        }

        public void RegisterListener(Type eventType, Action<object> eventHandler)
        {
            var registerListener = typeof(ApplicationEventBus).GetMethod(nameof(ApplicationEventBus.RegisterListenerGeneric), BindingFlags.NonPublic | BindingFlags.Instance).MakeGenericMethod(eventType);
            registerListener.Invoke(this, new[] { eventHandler });
        }

        private void RegisterListenerGeneric<TEvent>(Action<object> listener)
        {
            _eventBus.RegisterListener((TEvent x) => listener(x));
            _eventSourcingModule.Register((TEvent x) => listener(x));
        }
    }
}
