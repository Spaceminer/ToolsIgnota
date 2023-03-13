using CommunityToolkit.Mvvm.ComponentModel;
using System.Reactive.Linq;
using ToolsIgnota.Contracts.Services;

namespace ToolsIgnota.ViewModels;

public partial class ControlInitiativeViewModel : ObservableRecipient
{
    private readonly IInitiativeDisplayService _initiativeDisplayService;

    [ObservableProperty]
    private bool _initiativeVisible;

    public ControlInitiativeViewModel(IInitiativeDisplayService initiativeDisplayService)
    {
        _initiativeDisplayService = initiativeDisplayService ?? throw new ArgumentNullException(nameof(initiativeDisplayService));
        _initiativeDisplayService.IsVisible.FirstAsync().Subscribe(x => InitiativeVisible = x);
    }

    public async Task SetInitiativeVisible(bool isVisible)
    {
        await _initiativeDisplayService.SetIsVisible(isVisible);
    }
}
