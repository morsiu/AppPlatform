using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Mors.AppPlatform.Service.Client
{
    public sealed class RequestFactory
    {
        private readonly Uri _queryRequestUri;
        private readonly Uri _commandRequestUri;
        private readonly DataContractSerializer _serializer;

        public RequestFactory(
            Uri commandRequestUri,
            Uri queryRequestUri,
            IEnumerable<Type> knownTypes)
        {
            _commandRequestUri = commandRequestUri;
            _queryRequestUri = queryRequestUri;
            _serializer = new DataContractSerializer(typeof(object), knownTypes);
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
