using System;
using System.Net;
using System.Runtime.Serialization;

namespace Mors.AppPlatform.Service.Client
{
    public sealed class CommandRequest
    {
        private readonly NetDataContractSerializer _serializer;
        private readonly Uri _requestUri;
        private readonly object _command;

        public CommandRequest(Uri requestUri, object command, NetDataContractSerializer serializer)
        {
            _requestUri = requestUri;
            _command = command;
            _serializer = serializer;
        }

        public void Run()
        {
            var request = WebRequest.CreateHttp(_requestUri);
            request.Method = "POST";
            request.ContentType = "application/xml";
            request.Accept = "application/xml";
            var requestStream = request.GetRequestStream();
            _serializer.Serialize(requestStream, _command);
            request.GetResponse();
        }
    }
}
