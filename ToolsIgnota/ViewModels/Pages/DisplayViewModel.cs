using CommunityToolkit.Mvvm.ComponentModel;
using ToolsIgnota.Contracts.Services;

namespace ToolsIgnota.ViewModels;

public partial class DisplayViewModel : ObservableRecipient, IDisposable
{
    private readonly IInitiativeDisplayService _initiativeDisplayService;
    private readonly List<IDisposable> _subscriptions = new();
    private bool disposed;

    [ObservableProperty] private bool _showInitiative;

    public DisplayViewModel(IInitiativeDisplayService initiativeDisplayService)
    {
        _initiativeDisplayService = initiativeDisplayService ?? throw new ArgumentNullException(nameof(initiativeDisplayService));
        _subscriptions.Add(_initiativeDisplayService.IsVisible.Subscribe(x => ShowInitiative = x));
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                _subscriptions.ForEach(x => x.Dispose());
            }
            disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
