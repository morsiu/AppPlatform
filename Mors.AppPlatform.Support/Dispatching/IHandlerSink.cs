using System;

namespace Mors.AppPlatform.Support.Dispatching
{
    public interface IHandlerSink
    {
        void Enqueue(Action handler);
    }
}
