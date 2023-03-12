using Microsoft.UI.Xaml.Controls;

using ToolsIgnota.ViewModels;

namespace ToolsIgnota.Views;

public sealed partial class ControlImagesPage : Page
{
    public ControlImagesViewModel ViewModel
    {
        get;
    }

    public ControlImagesPage()
    {
        ViewModel = App.GetService<ControlImagesViewModel>();
        InitializeComponent();
    }
}
