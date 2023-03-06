using System;

namespace ToolsIgnota.Data.Abstractions
{
    public interface IInitiativeDisplayService
    {
        public void SetBackgroundImage(Uri imageUri);
        public IObservable<Uri> GetBackgroundImage();
    }
}
