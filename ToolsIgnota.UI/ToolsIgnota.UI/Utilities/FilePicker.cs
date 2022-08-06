using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace ToolsIgnota.UI.Utilities
{
    public static class FilePicker
    {
        public static async Task<StorageFile> GetImage()
        {
            return await GetFile(
                PickerViewMode.Thumbnail,
                PickerLocationId.PicturesLibrary,
                ".png", ".jpg", ".bmp");
        }

        public static async Task<StorageFile> GetFile(
            PickerViewMode viewMode, 
            PickerLocationId startLocation, 
            params string[] fileExtensions)
        {
            var picker = new FileOpenPicker();
            picker.ViewMode = viewMode;
            picker.SuggestedStartLocation = startLocation;
            foreach(string extension in fileExtensions)
            {
                picker.FileTypeFilter.Add(extension);
            }

            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.Window);
            WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);

            return await picker.PickSingleFileAsync();
        }
    }
}
