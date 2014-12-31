using System.Threading;

namespace Mors.AppPlatform.Support.Synchronization
{
    public sealed class Counter
    {
        private readonly EventWaitHandle _zeroReachedEvent;
        private int _count;

        public Counter(int initialCount)
        {
            _count = initialCount;
            _zeroReachedEvent = new ManualResetEvent(true);
        }

        public WaitHandle ZeroReachedEvent { get { return _zeroReachedEvent; } }

        public void Increase()
        {
            if (Interlocked.Increment(ref _count) == 1)
            {
                _zeroReachedEvent.Reset();
            }
        }

        public void Decrease()
        {
            if (Interlocked.Decrement(ref _count) == 0)
            {
                _zeroReachedEvent.Set();
            }
        }

        public void WaitForZero()
        {
            _zeroReachedEvent.WaitOne();
        }
    }
}
