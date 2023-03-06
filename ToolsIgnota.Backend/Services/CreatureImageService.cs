using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ToolsIgnota.Data.Abstractions;
using ToolsIgnota.Data.Models;
using ToolsIgnota.Data.Utilities;
using Windows.Storage;

namespace ToolsIgnota.Data.Services
{
    public class CreatureImageService : ICreatureImageService
    {
        private ISubject<IEnumerable<CreatureImage>> _imagesSubject = 
            new BehaviorSubject<IEnumerable<CreatureImage>>(LocalSettings.CreatureImages);

        public IObservable<IEnumerable<CreatureImage>> GetCreatureImages()
        {
            return _imagesSubject.AsObservable();
        }

        public void SaveCreatureImages(IEnumerable<CreatureImage> imageNamePairs)
        {
            LocalSettings.CreatureImages = imageNamePairs;
            _imagesSubject.OnNext(imageNamePairs);
        }
    }
}
