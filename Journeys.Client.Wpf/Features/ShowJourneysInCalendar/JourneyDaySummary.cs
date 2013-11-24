﻿using Journeys.Client.Wpf.Infrastructure.Extensions;
using Journeys.Queries.Dtos.JourneysByPassengerThenMonthThenDay;
using System.ComponentModel;

namespace Journeys.Client.Wpf.Features.ShowJourneysInCalendar
{
    internal class JourneyDaySummary : INotifyPropertyChanged
    {
        private Value _value;

        public JourneyDaySummary(Value value)
        {
            _value = value;
        }

        public void Change(Value value)
        {
            _value = value;
            PropertyChanged.Raise(() => LiftSummary);
            PropertyChanged.Raise(() => JourneySummary);
        }

        public string LiftSummary { get { return string.Format("{0} / {1}", _value.LiftCount, _value.LiftDistance); } }

        public string JourneySummary { get { return string.Format("{0} / {1}", _value.JourneyCount, _value.JourneyDistance); } }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
