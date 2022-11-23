using Nancy;
using Nancy.Bootstrapper;
using Nancy.ErrorHandling;

namespace Mors.AppPlatform.Service.Modules
{
    internal sealed class CorsBootstrapper : IApplicationStartup
    {
        public static void ConfigureInternalConfiguration(NancyInternalConfiguration configuration)
        {
            configuration.StatusCodeHandlers.Add(typeof(StatusCodeHandler));
        }

        private static Response WithCorsHeaders(Response response)
        {
            return response
                .WithHeader("Access-Control-Allow-Headers", "Accept, Content-Type")
                .WithHeader("Access-Control-Allow-Origin", "*")
                .WithHeader("Access-Control-Allow-Methods", "POST, OPTIONS")
                .WithHeader("Access-Control-Max-Age", "86400");
        }

        public void Initialize(IPipelines pipelines)
        {
            pipelines.AfterRequest.AddItemToEndOfPipeline(context => { if (context.Response != null) WithCorsHeaders(context.Response); });
            pipelines.OnError.AddItemToEndOfPipeline((context, _) => context.Response != null ? WithCorsHeaders(context.Response) : null);
        }

        private sealed class StatusCodeHandler : IStatusCodeHandler
        {
            public void Handle(HttpStatusCode statusCode, NancyContext context)
            {
                if (context.Response != null)
                {
                    context.Response = WithCorsHeaders(context.Response);
                }
            }

            public bool HandlesStatusCode(HttpStatusCode statusCode, NancyContext context)
            {
                return true;
            }
        }
    }
}
