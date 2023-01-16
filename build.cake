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
    CopyFiles(Source + @"\*.exe", Target);
    CopyFiles(Source + @"\*.dll", Target);
    CopyFiles(Source + @"\*.pdb", Target);
});

Task("BuildJourneysApp")
    .Does(() =>
{
    NpmRunScript((NpmRunScriptSettings)(new NpmRunScriptSettings {
        ScriptName = "build",
    }.FromPath(@".\Journeys\Mors.Journeys.Application.Client.Web")));
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
    .IsDependentOn("BuildJourneysApp")
    .Does(() =>
{
    CopyDirectory(
        @".\Journeys\Mors.Journeys.Application.Client.Web\dist",
        TargetSites + @"\journeys");
});

Task("Build")
    .IsDependentOn("BuildService")
    .IsDependentOn("BuildWordsApp")
    .IsDependentOn("BuildJourneysApp")
    .Does(() => { });

Task("Publish")
    .IsDependentOn("PublishService")
    .IsDependentOn("PublishWordsApp")
    .IsDependentOn("PublishJourneysApp")
    .Does(() => { });

RunTarget(target);