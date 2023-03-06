using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace ToolsIgnota.Data.Abstractions
{
    public interface IFilePickerService
    {
        public Task<StorageFile> GetImage();
        public Task<StorageFile> GetFile(
            PickerViewMode viewMode,
            PickerLocationId startLocation,
            params string[] fileExtensions);
    }
}
