using CommunityToolkit.WinUI.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using ToolsIgnota.ViewModels;

namespace ToolsIgnota.Views;
public sealed partial class CreatureImageSettingsControl : UserControl
{
    public CreatureImageSettingsViewModel ViewModel { get; }

    public CreatureImageSettingsControl()
    {
        ViewModel = App.GetService<CreatureImageSettingsViewModel>();
        InitializeComponent();
    }

    private void TextBox_KeyDown(object sender, Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e)
    {
        if(e.Key == Windows.System.VirtualKey.Enter)
        {
            var item = CreatureImageList.ContainerFromIndex(CreatureImageList.Items.Count-1).FindDescendant<TextBox>();
            item?.Focus(FocusState.Keyboard);
        }
    }

    private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (sender is TextBox { Text: "" } box)
            return;

        var lastItem = CreatureImageList.Items.Last();
        var thisItem = ((sender as TextBox)?.Parent as StackPanel)?.DataContext;

        if (lastItem == thisItem)
        {
            ViewModel.AddNewFinalCreatureImage();
        }
    }
}
