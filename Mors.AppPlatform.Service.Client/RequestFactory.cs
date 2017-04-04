using System;
using System.Runtime.Serialization;

namespace Mors.AppPlatform.Service.Client
{
    public sealed class RequestFactory
    {
        private readonly Uri _queryRequestUri;
        private readonly Uri _commandRequestUri;
        private readonly NetDataContractSerializer _serializer;

        public RequestFactory(Uri commandRequestUri, Uri queryRequestUri)
        {
            _commandRequestUri = commandRequestUri;
            _queryRequestUri = queryRequestUri;
            _serializer = new NetDataContractSerializer();
        }

        public CommandRequest CreateCommandRequest(object command)
        {
            return new CommandRequest(_commandRequestUri, command, _serializer);
        }

        public QueryRequest<TResult> CreateQueryRequest<TResult>(object query)
        {
            return new QueryRequest<TResult>(_queryRequestUri, query, _serializer);
        }
    }
}
