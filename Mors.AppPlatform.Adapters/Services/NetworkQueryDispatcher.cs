using System;
using Mors.AppPlatform.Adapters.Dispatching;
using Mors.AppPlatform.Common;
using Mors.AppPlatform.Service.Client;
using Mors.AppPlatform.Support.Dispatching;

namespace Mors.AppPlatform.Adapters.Services
{
    public sealed class NetworkQueryDispatcher : Common.Services.IQueryDispatcher
    {
        private readonly Uri _queryRequestUri;
        private readonly HandlerDispatcher _handlerDispatcher;

        public NetworkQueryDispatcher(Uri queryRequestUri, HandlerDispatcher handlerDispatcher)
        {
            _queryRequestUri = queryRequestUri;
            _handlerDispatcher = handlerDispatcher;
        }

        public TResult Dispatch<TResult>(IQuery<TResult> query)
        {
            var queryType = query.GetType();
            if (IsInternal(queryType))
            {
                return DispatchInternal(query);
            }
            else
            {
                return DispatchExternal(query);
            }
        }

        private TResult DispatchExternal<TResult>(IQuery<TResult> query)
        {
            var queryRequest = new QueryRequest<TResult>(_queryRequestUri, query);
            var result = queryRequest.Run();
            return result;
        }

        private TResult DispatchInternal<TResult>(IQuery<TResult> querySpecification)
        {
            var query = new Query<TResult>(querySpecification);
            return query.Dispatch(_handlerDispatcher);
        }

        private bool IsInternal(Type queryType)
        {
            return !queryType.IsPublic;
        }
    }
}