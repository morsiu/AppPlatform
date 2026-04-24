namespace Mors.AppPlatform.Service;

public sealed class Program
{
    public static void Main()
    {
        var configuration = Settings.Default();
        var service = Service.Create(configuration);
        service.Run();
    }
}