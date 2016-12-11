using Mors.AppPlatform.Service.Properties;

namespace Mors.AppPlatform.Service
{
    public sealed class Program
    {
        public static void Main()
        {
            var configuration = Settings.Default;
            var bootstrapper = new Bootstrapper();
            bootstrapper.Bootstrap(configuration);
            bootstrapper.RunService();
        }
    }
}