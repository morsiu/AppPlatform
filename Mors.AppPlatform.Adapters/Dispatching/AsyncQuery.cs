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

        public Task<object> Execute(AsyncHandlerDispatcher dispatcher)
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
    }
}
