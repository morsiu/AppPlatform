using System.Windows;

namespace Mors.AppPlatform.Client
{
    internal interface IActiveApplication
    {
        UIElement Content { get; }

        string Title { get; }
    }
}