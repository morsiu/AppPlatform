using Microsoft.Extensions.Configuration;

namespace Mors.AppPlatform.Service;

internal sealed record Settings(string EventFilePath, string HostUri, string SitesPath)
{
    public static Settings Default()
    {
        return new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().Get<Settings>();
    }
}