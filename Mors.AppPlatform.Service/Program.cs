using System;
using Mors.AppPlatform.Service.Host;
using Mors.AppPlatform.Service.Properties;
using Nancy.Hosting.Self;

namespace Mors.AppPlatform.Service
{
    public sealed class Program
    {
        public static void Main()
        {
            var configuration = Settings.Default;
            var bootstrapper = new Bootstrapper();
            bootstrapper.Bootstrap(configuration.EventFilePath);
            var hostBoostrapper = new HostBootstrapper(bootstrapper.QueryDispatcher, bootstrapper.CommandDispatcher, configuration.SitePath);
            var host = new NancyHost(
                hostBoostrapper,
                new HostConfiguration { UrlReservations = new UrlReservations { CreateAutomatically = configuration.CreateUrlReservation, User = configuration.UrlReservationUser } },
                new Uri(configuration.HostUri));
            host.Start();

            bootstrapper.RunScheduledHandlers();
        }
    }
}