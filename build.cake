#addin nuget:?package=Cake.Npm&version=2.0.0

var target = Argument("target", "Publish");

Task("BuildService")
    .Does(() =>
{
    MSBuild(@".\Mors.AppPlatform.Service\Mors.AppPlatform.Service.csproj", new MSBuildSettings
    {
        Configuration = "Release",
        MaxCpuCount = 0,
        NodeReuse = true,
        Verbosity = Verbosity.Minimal,
    });
});

Task("PublishService")
    .IsDependentOn("BuildService")
    .Does(() =>
{
    const string Source = @".\Mors.AppPlatform.Service\bin\Release\net472";
    const string Target = @"\\192.168.0.200\appplatform\bin";
    CopyFiles(Source + @"\*.dll", Target);
    CopyFiles(Source + @"\*.pdb", Target);
});

Task("BuildWordsApp")
    .Does(() =>
{
    NpmRunScript((NpmRunScriptSettings)(new NpmRunScriptSettings {
        ScriptName = "build",
    }.FromPath(@".\Words\Mors.Words.Web")));
});

const string TargetSites = @"\\192.168.0.200\appplatform\sites";

Task("PublishWordsApp")
    .IsDependentOn("BuildWordsApp")
    .Does(() =>
{
    const string Source = @".\Words\Mors.Words.Web\dist";
    CopyDirectory(Source, TargetSites + @"\words");
});

Task("PublishJourneysApp")
    .Does(() =>
{
    CopyDirectory(
        @".\Journeys\Mors.Journeys.Application.Client.Web\src",
        TargetSites + @"\journeys");
});

Task("Build")
    .IsDependentOn("BuildService")
    .IsDependentOn("BuildWordsApp")
    .Does(() => { });

Task("Publish")
    .IsDependentOn("PublishService")
    .IsDependentOn("PublishWordsApp")
    .IsDependentOn("PublishJourneysApp")
    .Does(() => { });

RunTarget(target);