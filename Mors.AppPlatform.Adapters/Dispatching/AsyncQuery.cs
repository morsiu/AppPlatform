using System;
using System.Threading.Tasks;
using Mors.AppPlatform.Support.Dispatching;
using Mors.AppPlatform.Support.Dispatching.Exceptions;
using Mors.AppPlatform.Adapters.Messages;

namespace Mors.AppPlatform.Adapters.Dispatching
{
    public sealed class AsyncQuery
    {
        private readonly object _querySpecification;

        public AsyncQuery(object querySpecification)
        {
            _querySpecification = querySpecification;
        }

        public Task<object> Schedule(AsyncHandlerScheduler scheduler)
        {
            var queryKey = new QueryKey(_querySpecification.GetType());
            try
            {
                return scheduler.Schedule(queryKey, _querySpecification);
            }
            catch (HandlerNotFoundException)
            {
                throw new InvalidOperationException(string.Format(FailureMessages.NoHandlerRegisteredForQueryOfType, queryKey));
            }
        }
    }
}
