using System;
using Mors.AppPlatform.Adapters.Dispatching;
using Mors.AppPlatform.Service.Client;
using Mors.AppPlatform.Support.Dispatching;
using Mors.Journeys.Data;

namespace Mors.AppPlatform.Adapters.Journeys
{
    internal sealed class ClientWpfQueryDispatcher : Mors.Journeys.Application.Client.Wpf.IQueryDispatcher
    {
        private readonly HandlerDispatcher _handlerDispatcher;
        private readonly RequestFactory _requestFactory;

        public ClientWpfQueryDispatcher(RequestFactory requestFactory, HandlerDispatcher handlerDispatcher)
        {
            _requestFactory = requestFactory;
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
            var queryRequest = _requestFactory.CreateQueryRequest<TResult>(query);
            var result = queryRequest.Run();
            return result;
        }

        private TResult DispatchInternal<TResult>(IQuery<TResult> querySpecification)
        {
            var query = new Query(querySpecification);
            return (TResult)query.Dispatch(_handlerDispatcher);
        }

        private bool IsInternal(Type queryType)
        {
            return !queryType.IsPublic;
        }
    }
}
