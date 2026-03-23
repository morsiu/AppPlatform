using System;
using System.Collections.Generic;
using System.Threading;

namespace Mors.AppPlatform.Support.Dispatching;

public sealed class HandlerQueue : IHandlerSource, IHandlerSink
{
    private readonly Queue<Action> _queuedHandlers = new();
    private readonly ManualResetEvent _nonEmptyQueueEvent = new(false);
    private readonly object _accessLock = new();

    public WaitHandle NonEmptyEvent
    {
        get { return _nonEmptyQueueEvent; }
    }

    public void Enqueue(Action handler)
    {
        lock (_accessLock)
        {
            _queuedHandlers.Enqueue(handler);
            _nonEmptyQueueEvent.Set();
        }
    }

    public Action Dequeue()
    {
        _nonEmptyQueueEvent.WaitOne();
        lock (_accessLock)
        {
            if (_queuedHandlers.Count == 1)
            {
                _nonEmptyQueueEvent.Reset();
            }
            return _queuedHandlers.Dequeue();
        }
    }
}