using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using ToolsIgnota.Backend;
using ToolsIgnota.Backend.Models;
using ToolsIgnota.UI.UserControls;
using ToolsIgnota.UI.Windows;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Core;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ToolsIgnota.UI.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class InitiativeControlPage : Page
    {
        private InitiativeDisplayWindow display_window;
        private readonly CombatManagerClient _client;
        private readonly Dictionary<Guid, ImageNamePair> _creatureImageDictionary;

        public InitiativeControlPage()
        {
            this.InitializeComponent();
            _client = new CombatManagerClient();
            _creatureImageDictionary = new Dictionary<Guid, ImageNamePair>();
        }

        public async void DisplayWindowClosed()
        {
            button_launch.IsEnabled = true;
            button_pickImage.IsEnabled = false;

            await _client.StopListening();
        }

        private async void button_launch_Click(object sender, RoutedEventArgs e)
        {
            display_window = new InitiativeDisplayWindow(this, _creatureImageDictionary.Values);
            display_window.Activate();

            button_launch.IsEnabled = false;
            button_pickImage.IsEnabled = true;

            await _client.StartListening(x => this.DispatcherQueue.TryEnqueue(
                () => display_window.UpdateInitiativeDisplay(x.Data.CombatList)));
        }

        private async void button_pickImage_Click(object sender, RoutedEventArgs e)
        {
            var picker = new FileOpenPicker();
            picker.ViewMode = PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");

            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.Window);
            WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);

            StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                display_window.SetBackgroundImage(new Uri(file.Path));
            }
        }

        private void button_addCreature_Click(object sender, RoutedEventArgs e)
        {
            Guid guid = Guid.NewGuid();
            _creatureImageDictionary.Add(guid, new ImageNamePair());
            var creature = new CreatureImageEntry(guid, _creatureImageDictionary[guid], x => 
                {
                    _creatureImageDictionary.Remove(guid);
                    panel_creatureImages.Children.Remove(x);
                });
            panel_creatureImages.Children.Add(creature);
        }
    }
}
