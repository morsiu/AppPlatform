﻿using Journeys.Domain.Exceptions;
using Journeys.Domain.Identities;
using Journeys.Domain.Journeys.Operations;
using Journeys.Domain.Routes.Operations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journeys.Domain.Test.Journeys.Operations
{
    [TestClass]
    public class JourneyFactoryTest
    {
        [TestMethod]
        public void ShouldBeAbleToSetDateOfJourney()
        {
            var factory = new JourneyFactory();

            factory.SetDateOfOccurence(new DateTime());
        }

        [TestMethod]
        public void ShouldBeAbleToAddLift()
        {
            var factory = new JourneyFactory();

            factory.AddLift(new Lift());
        }

        [TestMethod]
        public void ShouldBeAbleToSetRoute()
        {
            var factory = new JourneyFactory();

            factory.SetRoute(new Id<Route>(1));
        }

        [TestMethod]
        public void ShouldBeAbleToBuildJourney()
        {
            var factory = new JourneyFactory();

            factory.SetDateOfOccurence(new DateTime());
            factory.SetRoute(new Id<Route>(1));
            factory.AddLift(new Lift());

            factory.BuildJourney();
        }

        [TestMethod]
        [ExpectedException(typeof(EntityBuildException))]
        public void ShouldNotBuildWithoutDateOfOccurence()
        {
            var factory = new JourneyFactory();
            factory.SetRoute(new Id<Route>(1));

            factory.BuildJourney();
        }

        [TestMethod]
        [ExpectedException(typeof(EntityBuildException))]
        public void ShouldNotBuildWithoutRouteId()
        {
            var factory = new JourneyFactory();
            factory.SetDateOfOccurence(new DateTime());

            factory.BuildJourney();
        }
    }
}
