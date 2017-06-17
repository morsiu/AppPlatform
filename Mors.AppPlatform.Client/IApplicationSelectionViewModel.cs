namespace Mors.AppPlatform.Client
{
    internal interface IApplicationSelectionViewModel
    {
        string Description { get; }

        IApplicationPresentationViewModel PresentedApplication();
    }
}