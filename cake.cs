#:sdk Cake.Sdk@6.1.1
#:package Cake.Npm@5.1.0

var target = Argument("target", "Publish");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

const string Destination = @"D:\appplatform";

Task("BuildService")
    .Does(() =>
    {
        DotNetBuild(@".\Mors.AppPlatform.Service\Mors.AppPlatform.Service.csproj", new DotNetBuildSettings
        {
            Configuration = "Release",
            Verbosity = DotNetVerbosity.Minimal,
        });
    });

Task("PublishService")
    .IsDependentOn("BuildService")
    .Does(() =>
    {
        const string Source = @".\Mors.AppPlatform.Service\bin\Release\net10.0-windows";
        var target = System.IO.Path.Combine(Destination, "bin");
        CleanDirectory(target);
        CopyDirectory(Source, target);
        System.IO.File.WriteAllText(
            System.IO.Path.Combine(target, "appsettings.json"),
            """
            {
              "EventFilePath": "../data/events.txt",
              "HostUri": "http://localhost:65363",
              "SitesPath": "../sites"
            }
            """);
    });

Task("BuildJourneysWebApp")
    .Does(() =>
    {
        NpmRunScript((NpmRunScriptSettings)(new NpmRunScriptSettings
        {
            ScriptName = "build",
        }.FromPath(@".\Journeys\Mors.Journeys.Application.Client.Web")));
    });

Task("BuildWordsWebApp")
    .Does(() =>
    {
        NpmRunScript((NpmRunScriptSettings)(new NpmRunScriptSettings
        {
            ScriptName = "build",
        }.FromPath(@".\Words\Mors.Words.Web")));
    });

var sitesDestination = System.IO.Path.Combine(Destination, "sites");

Task("PublishWordsWebApp")
    .IsDependentOn("BuildWordsWebApp")
    .Does(() =>
    {
        const string Source = @".\Words\Mors.Words.Web\dist";
        var target = System.IO.Path.Combine(sitesDestination, "words");
        CleanDirectory(target);
        CopyDirectory(Source, target);
    });

Task("PublishJourneysWebApp")
    .IsDependentOn("BuildJourneysWebApp")
    .Does(() =>
    {
        const string Source = @".\Journeys\Mors.Journeys.Application.Client.Web\dist";
        var target = System.IO.Path.Combine(sitesDestination, "journeys");
        CopyDirectory(Source, target);
    });

Task("Build")
    .IsDependentOn("BuildService")
    .IsDependentOn("BuildWordsWebApp")
    .IsDependentOn("BuildJourneysWebApp")
    .Does(() => { });

Task("Publish")
    .IsDependentOn("PublishService")
    .IsDependentOn("PublishWordsWebApp")
    .IsDependentOn("PublishJourneysWebApp")
    .Does(() => { });

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target); 