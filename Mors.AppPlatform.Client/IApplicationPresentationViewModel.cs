using System.Windows;

namespace Mors.AppPlatform.Client
{
    internal interface IApplicationPresentationViewModel
    {
        UIElement Content { get; }

        string Title { get; }
    }
}