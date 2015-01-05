using System;
using System.Threading.Tasks;
using Mors.AppPlatform.Adapters.Messages;
using Mors.AppPlatform.Support.Dispatching;
using Mors.AppPlatform.Support.Dispatching.Exceptions;
using Mors.Journeys.Data;

namespace Mors.AppPlatform.Adapters.Dispatching
{
    public sealed class Query
    {
        private readonly object _querySpecification;

        public Query(object querySpecification)
        {
            _querySpecification = querySpecification;
        }

        public object Dispatch(HandlerDispatcher dispatcher)
        {
            var queryKey = new QueryKey(_querySpecification.GetType());
            try
            {
                return dispatcher.Dispatch(queryKey, _querySpecification);
            }
            catch (HandlerNotFoundException)
            {
                throw new InvalidOperationException(string.Format(FailureMessages.NoHandlerRegisteredForQueryOfType, queryKey));
            }
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
