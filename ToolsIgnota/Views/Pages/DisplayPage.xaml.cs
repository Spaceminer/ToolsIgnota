using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;

using ToolsIgnota.Contracts.Services;
using ToolsIgnota.Helpers;
using ToolsIgnota.ViewModels;

using Windows.System;

namespace ToolsIgnota.Views;

public sealed partial class DisplayPage : Page
{
    public DisplayViewModel ViewModel
    {
        get;
    }

    public DisplayPage(DisplayViewModel viewModel)
    {
        ViewModel = viewModel;
        InitializeComponent();

        App.DisplayWindow.ExtendsContentIntoTitleBar = true;
        App.DisplayWindow.SetTitleBar(AppTitleBar);
        App.DisplayWindow.Activated += DisplayWindow_Activated;
    }

    private void DisplayWindow_Activated(object sender, WindowActivatedEventArgs args)
    {
        
    }
}
