using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Journeys.Support.Synchronization
{
    public sealed class AggregateWaitHandle
    {
        private readonly WaitHandle[] _waitHadles;

        public AggregateWaitHandle(IEnumerable<WaitHandle> waitHandles)
        {
            _waitHadles = waitHandles.ToArray();
        }

        public void WaitAny()
        {
            WaitHandle.WaitAny(_waitHadles);
        }
    }
}
