using System;

namespace Mors.AppPlatform.Client
{
    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            var bootstrapper = new Bootstrapper();
            bootstrapper.BootstrapAndRun();
        }
    }
}
