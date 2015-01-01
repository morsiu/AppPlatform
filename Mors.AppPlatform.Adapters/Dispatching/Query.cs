using System;
using System.Threading.Tasks;
using Mors.AppPlatform.Adapters.Messages;
using Mors.AppPlatform.Support.Dispatching;
using Mors.AppPlatform.Support.Dispatching.Exceptions;
using Mors.Journeys.Data;

namespace Mors.AppPlatform.Adapters.Dispatching
{
    public sealed class Query<TResult>
    {
        private readonly IQuery<TResult> _querySpecification;

        public Query(IQuery<TResult> querySpecification)
        {
            _querySpecification = querySpecification;
        }

        public TResult Dispatch(HandlerDispatcher dispatcher)
        {
            var queryKey = QueryKey.From(_querySpecification);
            try
            {
                return (TResult)dispatcher.Dispatch(queryKey, _querySpecification);
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
