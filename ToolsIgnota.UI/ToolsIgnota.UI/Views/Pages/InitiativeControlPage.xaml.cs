using Microsoft.Extensions.DependencyInjection;
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
using System.Threading.Tasks;
using ToolsIgnota.Backend;
using ToolsIgnota.Backend.Abstractions;
using ToolsIgnota.Backend.Extensions;
using ToolsIgnota.Backend.Models;
using ToolsIgnota.UI.ViewModels;
using ToolsIgnota.UI.Views.UserControls;
using ToolsIgnota.UI.Views.Windows;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Core;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ToolsIgnota.UI.Views.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class InitiativeControlPage : Page
    {
        private readonly InitiativeControlViewModel VM;

        public InitiativeControlPage()
        {
            this.InitializeComponent();
            var container = ((App)Application.Current).Container;
            VM = new InitiativeControlViewModel(new SettingsRepository());
                //ActivatorUtilities.GetServiceOrCreateInstance<InitiativeControlViewModel>(container);
        }

        #region OldCode
        //private InitiativeDisplayWindow display_window;
        //private readonly CombatManagerClient _client;
        //private readonly Dictionary<Guid, CreatureImage> _creatureImageDictionary;
        //private readonly ISettings _settings;

        //private readonly InitiativeControlViewModel VM;

        //public InitiativeControlPage(ISettings settings)
        //{
        //    this.InitializeComponent();
        //    _client = new CombatManagerClient();
        //    _creatureImageDictionary = new Dictionary<Guid, CreatureImage>();
        //    _settings = settings ?? throw new ArgumentNullException(nameof(settings));

        //    LoadCreatureImageDictionary();
        //}

        //private void LoadCreatureImageDictionary()
        //{
        //    var creatureImageList = _settings.GetCreatureImages();
        //    _creatureImageDictionary.Clear();
        //    creatureImageList.Iter(x => AddCreatureImageEntry(Guid.NewGuid(), x));
        //}

        //private void SaveCreatureImageDictionary()
        //{
        //    _settings.SaveCreatureImages(_creatureImageDictionary.Values);
        //}

        //private void AddCreatureImageEntry(Guid guid, CreatureImage pair)
        //{
        //    _creatureImageDictionary.Add(guid, pair);
        //    var creature = new CreatureImageEntry(
        //        text => { _creatureImageDictionary[guid].Name = text; SaveCreatureImageDictionary(); },
        //        image => { _creatureImageDictionary[guid].Image = image; SaveCreatureImageDictionary(); }, 
        //        uiElement =>
        //        {
        //            _creatureImageDictionary.Remove(guid);
        //            panel_creatureImages.Children.Remove(uiElement);
        //            SaveCreatureImageDictionary();
        //        });
        //    panel_creatureImages.Children.Add(creature);
        //}

        //private async void button_launch_Click(object sender, RoutedEventArgs e)
        //{
        //    display_window = new InitiativeDisplayWindow(_creatureImageDictionary.Values, () =>
        //    {
        //        button_launch.IsEnabled = true;
        //        button_pickImage.IsEnabled = false;
        //        _client.StopListening();
        //    });

        //    button_launch.IsEnabled = false;
        //    button_pickImage.IsEnabled = true;

        //    display_window.Activate();

        //    try
        //    {
        //        await _client.StartListening(x => this.DispatcherQueue.TryEnqueue(
        //        () => display_window.UpdateInitiativeDisplay(x.Data.CombatList)));
        //    }
        //    catch (TaskCanceledException)
        //    {
        //        display_window.Close();
        //    }
        //}

        //private async void button_pickImage_Click(object sender, RoutedEventArgs e)
        //{
        //    var picker = new FileOpenPicker();
        //    picker.ViewMode = PickerViewMode.Thumbnail;
        //    picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
        //    picker.FileTypeFilter.Add(".jpg");
        //    picker.FileTypeFilter.Add(".jpeg");
        //    picker.FileTypeFilter.Add(".png");

        //    var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.Window);
        //    WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);

        //    StorageFile file = await picker.PickSingleFileAsync();
        //    if (file != null)
        //    {
        //        display_window.SetBackgroundImage(new Uri(file.Path));
        //    }
        //}

        //private void button_addCreature_Click(object sender, RoutedEventArgs e)
        //{
        //    AddCreatureImageEntry(Guid.NewGuid(), new CreatureImage());
        //}
        #endregion
    }
}
