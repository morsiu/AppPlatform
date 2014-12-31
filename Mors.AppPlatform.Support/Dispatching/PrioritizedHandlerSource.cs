using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Journeys.Support.Synchronization;

namespace Mors.AppPlatform.Support.Dispatching
{
    public sealed class PrioritizedHandlerSource : IHandlerSource
    {
        private readonly IEnumerable<IHandlerSource> _sourcesInDescendingPriorityOrder;
        private readonly AggregateWaitHandle _sourcesNonEmptyEvents;

        public PrioritizedHandlerSource(IEnumerable<IHandlerSource> sourcesInDescendingPriorityOrder)
        {
            _sourcesInDescendingPriorityOrder = sourcesInDescendingPriorityOrder;
            _sourcesNonEmptyEvents = new AggregateWaitHandle(_sourcesInDescendingPriorityOrder.Select(q => q.NonEmptyEvent));
        }

        public WaitHandle NonEmptyEvent
        {
            get { throw new NotImplementedException(); }
        }

        public Action Dequeue()
        {
            while (true)
            {
                _sourcesNonEmptyEvents.WaitAny();
                foreach (var queue in _sourcesInDescendingPriorityOrder)
                {
                    if (queue.NonEmptyEvent.WaitOne(0))
                    {
                        return queue.Dequeue();
                    }
                }
            }
        }
    }
}
