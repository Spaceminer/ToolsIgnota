using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ToolsIgnota.Contracts.Services;

namespace ToolsIgnota.ViewModels;

public partial class ControlImagesViewModel : ObservableRecipient
{
    private readonly IImageDisplayService _imageDisplayService;
    private readonly IFilePickerService _filePickerService;

    public ControlImagesViewModel(
        IImageDisplayService imageDisplayService,
        IFilePickerService filePickerService)
    {
        _imageDisplayService = imageDisplayService ?? throw new ArgumentNullException(nameof(imageDisplayService));
        _filePickerService = filePickerService ?? throw new ArgumentNullException(nameof(filePickerService));
    }

    [RelayCommand]
    public async Task PickBackgroundImage()
    {
        var file = await _filePickerService.PickImage();
        if (file == null)
            return;
        await _imageDisplayService.SetBackgroundImage(file.Path);
    }
}
