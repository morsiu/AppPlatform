﻿using Journeys.Adapters;
using Nancy;
using System.Runtime.Serialization;

namespace Journeys.Service.Modules
{
    public class CommandModule : NancyModule
    {
        private readonly ServiceCommandDispatcher _dispatcher;
        private readonly NetDataContractSerializer _serializer = new NetDataContractSerializer();

        public CommandModule(ServiceCommandDispatcher dispatcher)
        {
            _dispatcher = dispatcher;

            Post["/api/command"] = HandleCommandPost;
        }

        private dynamic HandleCommandPost(dynamic parameters)
        {
            var command = DeserializeRequest();
            _dispatcher.Dispatch(command);
            return PrepareResponse();
        }

        private object DeserializeRequest()
        {
            var xmlRequestStream = Request.Body;
            var request = _serializer.Deserialize(xmlRequestStream);
            return request;
        }

        private Response PrepareResponse()
        {
            return new Response { StatusCode = HttpStatusCode.OK };
        }
    }
}