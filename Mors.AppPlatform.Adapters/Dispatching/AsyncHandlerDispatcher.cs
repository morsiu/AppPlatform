using System;
using System.Threading;
using System.Threading.Tasks;
using Journeys.Support.Synchronization;
using Mors.AppPlatform.Support.Dispatching;
using Mors.AppPlatform.Support.Synchronization;

namespace Mors.AppPlatform.Adapters.Dispatching
{
    public sealed class AsyncHandlerDispatcher
    {
        private readonly IHandlerQueue _commandQueue;
        private readonly IHandlerQueue _queryQueue;
        private readonly Counter _runningCommandHandlerCount;
        private readonly Counter _runningQueueHandlerCount;
        private readonly AggregateWaitHandle _anyQueueNonEmptyEvent;

        public AsyncHandlerDispatcher(
            IHandlerQueue commandQueue,
            IHandlerQueue queryQueue)
        {
            _commandQueue = commandQueue;
            _queryQueue = queryQueue;
            _runningCommandHandlerCount = new Counter(0);
            _runningQueueHandlerCount = new Counter(0);
            _anyQueueNonEmptyEvent = new AggregateWaitHandle(new[] { _commandQueue.NonEmptyEvent, _queryQueue.NonEmptyEvent});
        }

        public void Run()
        {
            while (true)
            {
                TryRunNextCommandHandler();
                TryRunNextQueueHandler();
                WaitForNewHandlersInQueues();
            }
        }

        private void WaitForNewHandlersInQueues()
        {
            _anyQueueNonEmptyEvent.WaitAny();
        }

        private void TryRunNextQueueHandler()
        {
            Action queueHandler;
            if (_queryQueue.TryDequeue(out queueHandler))
            {
                _runningCommandHandlerCount.Wait();
                RunHandler(queueHandler, _runningQueueHandlerCount);
            }
        }

        private void TryRunNextCommandHandler()
        {
            Action commandHandler;
            if (_commandQueue.TryDequeue(out commandHandler))
            {
                _runningCommandHandlerCount.Wait();
                _runningQueueHandlerCount.Wait();
                RunHandler(commandHandler, _runningCommandHandlerCount);
            }
        }

        private void RunHandler(Action handler, Counter runningCount)
        {
            runningCount.Increase();
            Task.Run(handler).ContinueWith(t => runningCount.Decrease());
        }
    }
}
