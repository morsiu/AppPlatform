﻿using System.Threading;
using System.Threading.Tasks;
using Mors.AppPlatform.Adapters.Services;
using Mors.AppPlatform.Service.Infrastructure;
using Nancy;

namespace Mors.AppPlatform.Service.Modules
{
    internal sealed class CommandModule : NancyModule
    {
        private readonly AsyncCommandDispatcher _dispatcher;
        private readonly ContentTypeAwareSerializer _serializer = new ContentTypeAwareSerializer();

        public CommandModule(AsyncCommandDispatcher dispatcher)
        {
            _dispatcher = dispatcher;

            Post["/api/command", true] = HandleCommandPost;
        }

        private async Task<dynamic> HandleCommandPost(dynamic parameters, CancellationToken cancellationToken)
        {
            var command = DeserializeRequest();
            await _dispatcher.Dispatch(command);
            return PrepareResponse();
        }

        private object DeserializeRequest()
        {
            var request = _serializer.Deserialize(Request.Body, Request.Headers.ContentType);
            return request;
        }

        private Response PrepareResponse()
        {
            return new Response { StatusCode = HttpStatusCode.OK };
        }
    }
}