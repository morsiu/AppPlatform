using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Mors.AppPlatform.Support.Dispatching
{
    public sealed class DependentHandlerSource : IHandlerSource
    {
        private readonly WaitHandle[] _dependenciesWaitHandles;
        private readonly IHandlerSource _source;

        public DependentHandlerSource(
            IHandlerSource source,
            IEnumerable<WaitHandle> dependenciesWaitHandles)
        {
            _source = source;
            _dependenciesWaitHandles = dependenciesWaitHandles.ToArray();
        }

        public WaitHandle NonEmptyEvent
        {
            get { return _source.NonEmptyEvent; }
        }

        public Action Dequeue()
        {
            var handler = _source.Dequeue();
            WaitHandle.WaitAll(_dependenciesWaitHandles);
            return handler;
        }
    }
}
