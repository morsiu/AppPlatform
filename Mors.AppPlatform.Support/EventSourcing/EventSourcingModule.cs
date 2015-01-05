using System;
using System.Collections.Generic;
using Mors.AppPlatform.Support.EventSourcing.Dependencies;
using Mors.AppPlatform.Support.EventSourcing.Storage;

namespace Mors.AppPlatform.Support.EventSourcing
{
    public sealed class EventSourcingModule
    {
        private readonly Func<IEnumerable<Type>, IEventStore> _eventStoreFactory;
        private readonly HashSet<Type> _typesOfEventsToStore = new HashSet<Type>();
        private readonly EventReplayConfigurator _eventReplayConfigurator = new EventReplayConfigurator();
        private readonly EventWriteConfigurator _eventWriteConfigurator = new EventWriteConfigurator();
        private readonly IEventBus _eventBus;
        private readonly Type _idImplementationType;

        public EventSourcingModule(
            IEventBus eventBus,
            Func<IEnumerable<Type>, IEventStore> eventStoreFactory,
            Type idImplementationType)
        {
            _eventBus = eventBus;
            _eventStoreFactory = eventStoreFactory;
            _idImplementationType = idImplementationType;
        }

        public void ReplayEvents()
        {
            var eventStore = GetEventStore();
            var storedEvents = eventStore.GetReader();
            var eventReplayer = GetReplayer();
            eventReplayer.Replay(storedEvents);
        }

        public void StoreNewEvents()
        {
            var eventStore = GetEventStore();
            var eventWriter = eventStore.GetWriter();
            _eventWriteConfigurator.Configure(_eventBus, eventWriter);
        }

        public void Register<TEvent>(Action<TEvent> replayHandler)
        {
            var eventType = typeof(TEvent);
            _eventWriteConfigurator.Add<TEvent>();
            _eventReplayConfigurator.Add(replayHandler);
            _typesOfEventsToStore.Add(eventType);
        }

        private IEventStore GetEventStore()
        {
            var eventStore = _eventStoreFactory(GetSupportedEventTypes());
            return eventStore;
        }

        private IEnumerable<Type> GetSupportedEventTypes()
        {
            foreach (var eventType in _typesOfEventsToStore)
            {
                yield return eventType;
            }
            yield return _idImplementationType;
        }

        private EventReplayer GetReplayer()
        {
            var replayer = new EventReplayer();
            _eventReplayConfigurator.Configure(replayer);
            return replayer;
        }
    }
}
