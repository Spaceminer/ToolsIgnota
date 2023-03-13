using Microsoft.UI.Xaml.Controls;

using ToolsIgnota.ViewModels;

namespace ToolsIgnota.Views;

public sealed partial class ControlInitiativePage : Page
{
    public ControlInitiativeViewModel ViewModel { get; }

    public ControlInitiativePage()
    {
        ViewModel = App.GetService<ControlInitiativeViewModel>();
        InitializeComponent();
    }

    private async void InitiativeVisibleSwitch_Toggled(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        await ViewModel.SetInitiativeVisible(InitiativeVisibleSwitch.IsOn).ConfigureAwait(false);
    }
}
