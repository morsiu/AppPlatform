﻿using Journeys.Commands;
using Journeys.Data;
using Journeys.Domain.Infrastructure;
using Journeys.Domain.Journeys.Capabilities;
using Journeys.Domain.Journeys.Data;
using Journeys.Domain.Journeys.Operations;
using Journeys.Domain.People;
using Journeys.Eventing;
using Journeys.Queries;
using Journeys.Query;
using System;

namespace Journeys.Command.CommandHandlers
{
    internal class AddJourneyCommandHandler
    {
        private readonly IEventBus _eventBus;
        private readonly IDomainRepository<Person> _personRepository;
        private readonly IQueryDispatcher _queryDispatcher;

        public AddJourneyCommandHandler(
            IEventBus eventBus,
            IDomainRepository<Person> personRepository,
            IQueryDispatcher queryDispatcher)
        {
            _personRepository = personRepository;
            _queryDispatcher = queryDispatcher;
            _eventBus = eventBus;
        }

        public void Execute(
            AddJourneyCommand command,
            IDomainRepository<Journey> journeyRepository)
        {
            var journeyDistance = new Distance(command.JourneyDistance, DistanceUnit.Kilometer);
            var liftDistance = new Distance(command.LiftDistance, DistanceUnit.Kilometer);
            var personId = GetPersonWithName(command.PersonName);
            var journeyId = new Id<Journey>(command.JourneyId);
            var journey = new Journey(journeyId, command.JourneyDateOfOccurrence, journeyDistance, _eventBus)
                .AddLift(new Id<Person>(personId), liftDistance);
            journeyRepository.Store(journey);
        }

        private Id<Person> GetPersonWithName(string personName)
        {
            var personId = _queryDispatcher.Dispatch(new GetIdOfPersonWithNameQuery(personName));
            if (!personId.HasValue)
            {
                personId = Guid.NewGuid();
                var person = new Person(new Id<Person>(personId.Value), personName, _eventBus);
                _personRepository.Store(person);
            }
            return new Id<Person>(personId.Value);
        }
    }
}
