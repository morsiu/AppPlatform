using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System;
using System.IO;
using System.Linq;

namespace Mors.AppPlatform.Service.Infrastructure;

internal sealed class AspNetCoreHost
{
    private readonly WebApplication _app;
    private readonly Uri _hostUri;

    public AspNetCoreHost(
        AsyncQueryDispatcher queryDispatcher,
        AsyncCommandDispatcher commandDispatcher,
        ContentTypeAwareSerializer contentTypeAwareSerializer,
        string sitesPath,
        string hostUri)
    {
        var appBuilder = WebApplication.CreateBuilder();
        appBuilder.Services.AddCors();
        var app = appBuilder.Build();
        app.UseCors(x => x.SetPreflightMaxAge(TimeSpan.FromDays(1)).AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
        if (!string.IsNullOrEmpty(sitesPath))
        {
            app.UseStaticFiles(
                new StaticFileOptions
                {
                    FileProvider = new PhysicalFileProvider(Path.GetFullPath(sitesPath)),
                    RequestPath = "/sites",
                });
        }
        app.MapPost(
            "/api/query",
            async (HttpContext a) =>
            {
                a.Features.Get<IHttpBodyControlFeature>().AllowSynchronousIO = true;
                var query =
                    contentTypeAwareSerializer.Deserialize(
                        a.Request.Body,
                        a.Request.Headers.ContentType);
                var queryResult = await queryDispatcher.Dispatch(query);
                using var queryResultStream = new MemoryStream();
                var queryResultContentType =
                    contentTypeAwareSerializer.Serialize(
                        queryResult,
                        queryResultStream,
                        a.Request.Headers.Accept.Select(x => Tuple.Create(x, 1m)));
                a.Response.ContentType = queryResultContentType;
                queryResultStream.Seek(0, SeekOrigin.Begin);
                await queryResultStream.CopyToAsync(a.Response.Body);
            });
        app.MapPost(
            "/api/command",
            async (HttpContext a) =>
            {
                a.Features.Get<IHttpBodyControlFeature>().AllowSynchronousIO = true;
                var query =
                    contentTypeAwareSerializer.Deserialize(
                        a.Request.Body,
                        a.Request.Headers.ContentType);
                await commandDispatcher.Dispatch(query);
                a.Response.StatusCode = StatusCodes.Status204NoContent;
            });
        _app = app;
        _hostUri = new Uri(hostUri);
    }

    public void Run()
    {
        _app.Run(_hostUri.ToString());
    }
}