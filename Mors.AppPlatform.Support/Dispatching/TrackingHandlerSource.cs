using System;
using System.Threading;
using Mors.AppPlatform.Support.Synchronization;

namespace Mors.AppPlatform.Support.Dispatching
{
    public sealed class TrackingHandlerSource : IHandlerSource
    {
        private readonly IHandlerSource _source;
        private readonly Counter _runningHandlersCounter = new Counter(0);

        public TrackingHandlerSource(IHandlerSource source)
        {
            _source = source;
        }

        public WaitHandle NoRunningHandlersEvent
        {
            get { return _runningHandlersCounter.ZeroReachedEvent; }
        }

        public WaitHandle NonEmptyEvent
        {
            get { return _source.NonEmptyEvent; }
        }

        public Action Dequeue()
        {
            var handler = _source.Dequeue();
            return
                () =>
                {
                    _runningHandlersCounter.Increase();
                    try
                    {
                        handler();
                    }
                    finally
                    {
                        _runningHandlersCounter.Decrease();
                    }
                };
        }
    }
}
