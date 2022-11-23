using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Mors.AppPlatform.Service.Infrastructure;
using Nancy;

namespace Mors.AppPlatform.Service.Modules
{
    internal sealed class QueryModule : NancyModule
    {
        private readonly AsyncQueryDispatcher _dispatcher;
        private readonly ContentTypeAwareSerializer _serializer = new ContentTypeAwareSerializer();

        public QueryModule(AsyncQueryDispatcher dispatcher)
        {
            _dispatcher = dispatcher;

            Options("/api/query", x => new Response().WithStatusCode(HttpStatusCode.OK));
            Post("/api/query", HandleQueryPost);
        }

        private async Task<dynamic> HandleQueryPost(dynamic parameters, CancellationToken cancellationToken)
        {
            var query = DeserializeRequestBody();
            var result = await _dispatcher.Dispatch(query);
            return PrepareResponse(result);
        }

        private object DeserializeRequestBody()
        {
            var requestBody = _serializer.Deserialize(Request.Body, Request.Headers.ContentType);
            return requestBody;
        }

        private Response PrepareResponse(object result)
        {
            var responseStream = new MemoryStream();
            var responseContentType = _serializer.Serialize(result, responseStream, Request.Headers.Accept);
            responseStream.Seek(0, SeekOrigin.Begin);
            var response = Response.FromStream(responseStream, responseContentType);
            return response;
        }
    }
}