using System.Collections.ObjectModel;
using System.Reactive.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using ToolsIgnota.Contracts.Services;
using ToolsIgnota.Models;

namespace ToolsIgnota.ViewModels;

public partial class BackgroundImageViewModel : ObservableRecipient, IDisposable
{
    private readonly IImageDisplayService _imageDisplayService;
    private readonly List<IDisposable> _subscriptions = new();

    [ObservableProperty] private string _imageUrl = "..";

    private bool _disposed;

    public ObservableCollection<InitiativeCreatureModel> CreatureList { get; set; } = new();

    public BackgroundImageViewModel(IImageDisplayService imageDisplayService)
    {
        _imageDisplayService = imageDisplayService ?? throw new ArgumentNullException(nameof(imageDisplayService));

        _subscriptions.Add(_imageDisplayService.BackgroundImage.Subscribe(SetBackgroundImage));
    }

    private void SetBackgroundImage(string imageUrl)
    {
        ImageUrl = imageUrl;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _subscriptions.ForEach(x => x.Dispose());
            }
            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
