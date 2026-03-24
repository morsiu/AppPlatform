using System.Collections.Generic;
using System.ComponentModel;

namespace Mors.AppPlatform.Client;

internal sealed class WindowViewModel : INotifyPropertyChanged
{
    private IActiveApplication _activeApplication;

    public WindowViewModel(IEnumerable<IAvailableApplication> applications)
    {
        Applications = applications;
        _activeApplication = new AppPlatformPlaceholder();
    }

    public IEnumerable<IAvailableApplication> Applications { get; }

    public IAvailableApplication SelectedApplication
    {
        get;
        set
        {
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedApplication)));
            ActiveApplication = field?.ActiveApplication() ?? new AppPlatformPlaceholder();
        }
    }

    public IActiveApplication ActiveApplication
    {
        get => _activeApplication;
        set
        {
            _activeApplication = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ActiveApplication)));
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
}