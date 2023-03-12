using Microsoft.UI.Xaml.Controls;

using ToolsIgnota.ViewModels;

namespace ToolsIgnota.Views;

public sealed partial class ControlInitiativePage : Page
{
    public ControlInitiativeViewModel ViewModel
    {
        get;
    }

    public ControlInitiativePage()
    {
        ViewModel = App.GetService<ControlInitiativeViewModel>();
        InitializeComponent();
    }
}
