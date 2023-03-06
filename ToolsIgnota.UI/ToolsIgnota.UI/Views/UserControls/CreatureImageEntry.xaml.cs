using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using ToolsIgnota.Data.Models;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ToolsIgnota.UI.Views.UserControls
{
    public sealed partial class CreatureImageEntry : UserControl
    {
        private readonly Action<string> _updateTextCallback;
        private readonly Action<string> _updateImageCallback;
        private readonly Action<CreatureImageEntry> _deleteCallback;

        public CreatureImageEntry(
            Action<string> updateTextCallback,
            Action<string> updateImageCallback,
            Action<CreatureImageEntry> deleteCallback)
        {
            this.InitializeComponent();
            _updateTextCallback = updateTextCallback;
            _updateImageCallback = updateImageCallback;
            _deleteCallback = deleteCallback;
        }

        private void button_delete_Click(object sender, RoutedEventArgs e)
        {
            _deleteCallback(this);
        }

        private void textbox_name_TextChanged(object sender, TextChangedEventArgs e)
        {
            _updateTextCallback(textbox_name.Text);
        }

        private async void button_image_Click(object sender, RoutedEventArgs e)
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
                image_button_image.Source = new BitmapImage(new Uri(file.Path));
                _updateImageCallback(file.Path);
            }
        }
    }
}
