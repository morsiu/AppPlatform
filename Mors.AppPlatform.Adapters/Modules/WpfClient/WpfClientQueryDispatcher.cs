using System;
using Mors.AppPlatform.Adapters.Dispatching;
using Mors.AppPlatform.Common;
using Mors.AppPlatform.Service.Client;

namespace Mors.AppPlatform.Adapters.Modules.WpfClient
{
    public sealed class WpfClientQueryDispatcher : Application.Client.Wpf.IQueryDispatcher
    {
        private readonly Uri _queryRequestUri;
        private readonly Mors.AppPlatform.Support.Dispatching.HandlerDispatcher _handlerDispatcher;

        public WpfClientQueryDispatcher(Uri queryRequestUri, Mors.AppPlatform.Support.Dispatching.HandlerDispatcher handlerDispatcher)
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
            return query.Execute(_handlerDispatcher);
        }

        private bool IsInternal(Type queryType)
        {
            return !queryType.IsPublic;
        }
    }
}
