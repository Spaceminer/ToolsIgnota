using System;
using System.Reactive.Subjects;
using ToolsIgnota.Data.Abstractions;

namespace ToolsIgnota.Data.Services
{
    public class InitiativeDisplayService : IInitiativeDisplayService
    {
        private ISubject<Uri> _backgroundImageSubject = 
            new BehaviorSubject<Uri>(null);

        public IObservable<Uri> GetBackgroundImage()
        {
            return _backgroundImageSubject;
        }

        public void SetBackgroundImage(Uri imageUri)
        {
            _backgroundImageSubject.OnNext(imageUri);
        }
    }
}
