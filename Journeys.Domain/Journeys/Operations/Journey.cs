﻿using Journeys.Domain.Infrastructure;
using Journeys.Domain.Infrastructure.Collections;
using Journeys.Domain.Infrastructure.Exceptions;
using Journeys.Domain.Infrastructure.Markers;
using Journeys.Domain.Infrastructure.Messages;
using Journeys.Domain.Journeys.Capabilities;
using Journeys.Domain.Journeys.Events;
using Journeys.Domain.People;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Journeys.Domain.Journeys.Operations
{
    [Aggregate]
    public class Journey
    {
        private readonly IEventBus _eventBus;
        private readonly Id<Journey> _id;
        private readonly DateTime _dateOfOccurence;
        private readonly Distance _routeDistance;
        private readonly ImmutableList<Lift> _lifts = ImmutableList<Lift>.Empty;

        public Journey(Id<Journey> id, DateTime dateOfOccurence, Distance routeDistance, IEventBus eventBus)
        {
            _dateOfOccurence = dateOfOccurence;
            _eventBus = eventBus;
            _id = id;
            _routeDistance = routeDistance;
        }

        private Journey(Journey journey, ImmutableList<Lift> lifts)
        {
            _dateOfOccurence = journey._dateOfOccurence;
            _eventBus = journey._eventBus;
            _id = journey._id;
            _lifts = lifts;
            _routeDistance = journey._routeDistance;
        }

        public Journey AddLift(Id<Person> personId, Distance liftDistance)
        {
            if (_lifts.Any(aLift => aLift.EqualsByPerson(personId))) 
                throw new InvariantViolatedException(FailureMessages.JourneyAlreadyContainsLiftWithSamePerson);
            if (liftDistance > _routeDistance) 
                throw new InvariantViolatedException(FailureMessages.CannotAddLiftWithDistanceLargerThanJourneyDistance);
            var lift = new Lift(personId, liftDistance);
            var lifts = _lifts.Add(lift);
            _eventBus.Publish<LiftAddedEvent>(new LiftAddedEvent(_id, personId, liftDistance));
            return new Journey(this, lifts);
        }
    }
}
