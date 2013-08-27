﻿using System;
using Journeys.Common;

namespace Journeys.Queries.Dtos
{
    public class JourneyWithLift
    {
        public JourneyWithLift(
            IId journeyId,
            IId passengerId,
            DateTime dateOfOccurrence,
            decimal routeDistance,
            string passengerName,
            decimal passengerLiftDistance)
        {
            JourneyId = journeyId;
            PassengerId = passengerId;
            DateOfOccurrence = dateOfOccurrence;
            RouteDistance = routeDistance;
            PassengerName = passengerName;
            PassengerLiftDistance = passengerLiftDistance;
        }

        public IId JourneyId { get; private set; }

        public IId PassengerId { get; private set; }

        public DateTime DateOfOccurrence { get; private set; }

        public decimal RouteDistance { get; private set; }

        public string PassengerName { get; private set; }

        public decimal PassengerLiftDistance { get; private set; }
    }
}
