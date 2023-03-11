using ToolsIgnota.Contracts.Services;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace ToolsIgnota.Services;

public class FilePickerService : IFilePickerService
{
    public async Task<StorageFile> GetImage()
    {
        var picker = new Windows.Storage.Pickers.FileOpenPicker();
        var windowHandle = App.MainWindow.GetWindowHandle();
        WinRT.Interop.InitializeWithWindow.Initialize(picker, windowHandle);

        picker.ViewMode = PickerViewMode.Thumbnail;
        picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
        picker.FileTypeFilter.Add(".jpg");
        picker.FileTypeFilter.Add(".jpeg");
        picker.FileTypeFilter.Add(".png");

        return await picker.PickSingleFileAsync();
    }
}
