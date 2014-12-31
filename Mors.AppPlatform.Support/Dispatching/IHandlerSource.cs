using System;
using System.Threading;

namespace Mors.AppPlatform.Support.Dispatching
{
    public interface IHandlerSource
    {
        WaitHandle NonEmptyEvent { get; }

        Action Dequeue();
    }
}
