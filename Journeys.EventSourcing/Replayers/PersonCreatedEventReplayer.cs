﻿using Journeys.Domain.Infrastructure;
using Journeys.Domain.People;
using Journeys.Eventing;
using Journeys.Events;

namespace Journeys.EventSourcing.Replayers
{
    internal class PersonCreatedEventReplayer
    {
        private readonly IRepositories _repositories;
        private readonly IEventBus _eventBus;

        public PersonCreatedEventReplayer(IRepositories repositories, IEventBus eventBus)
        {
            _repositories = repositories;
            _eventBus = eventBus;
        }

        public void Replay(PersonCreatedEvent @event)
        {
            var personId = @event.PersonId;
            var person = new Person(personId, @event.PersonName, _eventBus);
            _repositories.Store(person);
        }
    }
}
