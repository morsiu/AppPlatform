using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;

namespace Mors.AppPlatform.Service.Client;

public sealed class CommandRequest
{
    private readonly object _command;
    private readonly HttpClient _httpClient;
    private readonly Uri _requestUri;
    private readonly DataContractSerializer _serializer;

    public CommandRequest(HttpClient httpClient, Uri requestUri, object command, DataContractSerializer serializer)
    {
        _httpClient = httpClient;
        _requestUri = requestUri;
        _command = command;
        _serializer = serializer;
    }

    public void Run()
    {
        using var requestStream = new MemoryStream();
        _serializer.WriteObject(requestStream, _command);
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
    }
}