using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.Serialization;

namespace Mors.AppPlatform.Service.Client;

public sealed class RequestFactory
{
    private readonly Uri _commandRequestUri;
    private readonly HttpClient _httpClient;
    private readonly Uri _queryRequestUri;
    private readonly DataContractSerializer _serializer;

    public RequestFactory(
        Uri commandRequestUri,
        Uri queryRequestUri,
        IEnumerable<Type> knownTypes)
    {
        _commandRequestUri = commandRequestUri;
        _queryRequestUri = queryRequestUri;
        _httpClient = new HttpClient();
        _serializer = new DataContractSerializer(typeof(object), knownTypes);
    }

    public CommandRequest CreateCommandRequest(object command)
    {
        return new CommandRequest(_httpClient, _commandRequestUri, command, _serializer);
    }

    public QueryRequest<TResult> CreateQueryRequest<TResult>(object query)
        where TResult : notnull
    {
        return new QueryRequest<TResult>(_httpClient, _queryRequestUri, query, _serializer);
    }
}