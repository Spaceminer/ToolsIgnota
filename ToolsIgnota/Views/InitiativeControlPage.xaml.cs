using Microsoft.UI.Xaml.Controls;

using ToolsIgnota.ViewModels;

namespace ToolsIgnota.Views;

public sealed partial class InitiativeControlPage : Page
{
    public InitiativeControlViewModel ViewModel
    {
        get;
    }

    public InitiativeControlPage()
    {
        ViewModel = App.GetService<InitiativeControlViewModel>();
        InitializeComponent();
    }
}
