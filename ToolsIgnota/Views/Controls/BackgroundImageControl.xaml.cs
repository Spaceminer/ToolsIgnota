using Microsoft.UI.Xaml.Controls;
using ToolsIgnota.ViewModels;

namespace ToolsIgnota.Views;
public sealed partial class BackgroundImageControl : UserControl
{
    public BackgroundImageViewModel ViewModel { get; }

    public BackgroundImageControl()
    {
        ViewModel = App.GetService<BackgroundImageViewModel>();
        InitializeComponent();
    }
}
