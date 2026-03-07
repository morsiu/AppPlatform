using System;
using System.Net;
using System.Runtime.Serialization;

namespace Mors.AppPlatform.Service.Client
{
    public sealed class QueryRequest<TResult>
    {
        private readonly DataContractSerializer _serializer;
        private readonly Uri _requestUri;
        private readonly object _query;

        public QueryRequest(Uri requestUri, object query, DataContractSerializer serializer)
        {
            _requestUri = requestUri;
            _query = query;
            _serializer = serializer;
        }

        public TResult Run()
        {
            var request = WebRequest.CreateHttp(_requestUri);
            request.Method = "POST";
            request.ContentType = "application/xml";
            request.Accept = "application/xml";
            var requestStream = request.GetRequestStream();
            _serializer.WriteObject(requestStream, _query);
            var response = request.GetResponse();
            var responseStream = response.GetResponseStream();
            var result = (TResult)_serializer.ReadObject(responseStream);
            return result;
        }
    }
}
