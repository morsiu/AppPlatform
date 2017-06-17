namespace Mors.AppPlatform.Client
{
    internal interface IAvailableApplication
    {
        string Description { get; }

        IActiveApplication ActiveApplication();
    }
}