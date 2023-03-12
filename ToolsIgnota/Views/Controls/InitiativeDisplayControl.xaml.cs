using Microsoft.UI.Xaml.Controls;
using ToolsIgnota.ViewModels;

namespace ToolsIgnota.Views;
public sealed partial class InitiativeDisplayControl : UserControl
{
    public InitiativeDisplayViewModel ViewModel { get; }

    public InitiativeDisplayControl()
    {
        ViewModel = App.GetService<InitiativeDisplayViewModel>();
        InitializeComponent();
    }
}
