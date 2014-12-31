using System;
using System.Threading;
using Mors.AppPlatform.Support.Transactions;

namespace Mors.AppPlatform.Support.Dispatching
{
    public sealed class TransactedHandlerSource : IHandlerSource
    {
        private readonly IHandlerSource _source;
        private readonly Transaction _ambientTransaction;

        public TransactedHandlerSource(IHandlerSource source, Transaction ambientTransaction)
        {
            _source = source;
            _ambientTransaction = ambientTransaction;
        }

        public WaitHandle NonEmptyEvent
        {
            get { return _source.NonEmptyEvent; }
        }

        public Action Dequeue()
        {
            var handler = _source.Dequeue();
            return () => _ambientTransaction.Run(handler);
        }
    }
}
