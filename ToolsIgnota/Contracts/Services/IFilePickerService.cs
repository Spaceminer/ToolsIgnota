using Windows.Storage;

namespace ToolsIgnota.Contracts.Services;

public interface IFilePickerService
{
    Task<StorageFile> GetImage();
}
