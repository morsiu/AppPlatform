using System.Threading;
using System.Threading.Tasks;
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

            Options("/api/command", x => new Response().WithStatusCode(HttpStatusCode.OK));
            Post("/api/command", HandleCommandPost);
        }

        private async Task<dynamic> HandleCommandPost(dynamic parameters, CancellationToken cancellationToken)
        {
            var command = DeserializeRequestBody();
            await _dispatcher.Dispatch(command);
            return PrepareResponse();
        }

        private object DeserializeRequestBody()
        {
            var requestBody = _serializer.Deserialize(Request.Body, Request.Headers.ContentType);
            return requestBody;
        }

        private Response PrepareResponse()
        {
            return new Response { StatusCode = HttpStatusCode.OK };
        }
    }
}
