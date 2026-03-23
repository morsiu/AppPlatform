using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;

namespace Mors.AppPlatform.Service.Client;

public sealed class QueryRequest<TResult>
{
    private readonly HttpClient _httpClient;
    private readonly object _query;
    private readonly Uri _requestUri;
    private readonly DataContractSerializer _serializer;

    public QueryRequest(HttpClient httpClient, Uri requestUri, object query, DataContractSerializer serializer)
    {
        _httpClient = httpClient;
        _query = query;
        _requestUri = requestUri;
        _serializer = serializer;
    }

    public TResult Run()
    {
        using var requestStream = new MemoryStream();
        _serializer.WriteObject(requestStream, _query);
        requestStream.Seek(0, SeekOrigin.Begin);
        using var response =
            _httpClient.Send(
                new HttpRequestMessage(HttpMethod.Post, _requestUri) 
                {
                    Headers = { { "Accept", "application/xml" } },
                    Content = 
                        new StreamContent(requestStream)
                        {
                            Headers = { { "Content-Type", "application/xml" } }
                        },
                });
        response.EnsureSuccessStatusCode();
        var responseStream = response.Content.ReadAsStream();
        var result = (TResult)_serializer.ReadObject(responseStream);
        return result;
    }
}