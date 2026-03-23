using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Mors.AppPlatform.Support.Synchronization;

public sealed class AggregateWaitHandle
{
    private readonly WaitHandle[] _waitHandles;

    public AggregateWaitHandle(IEnumerable<WaitHandle> waitHandles)
    {
        _waitHandles = waitHandles.ToArray();
    }

    public void WaitAny()
    {
        WaitHandle.WaitAny(_waitHandles);
    }
}