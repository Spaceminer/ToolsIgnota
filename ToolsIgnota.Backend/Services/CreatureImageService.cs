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
using Windows.Storage;

namespace ToolsIgnota.Data.Services
{
    public class CreatureImageService : ICreatureImageService
    {
        private readonly string _settingName = "CreatureImages";
        private readonly ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        private ISubject<IEnumerable<CreatureImage>> _imagesSubject;

        public IObservable<IEnumerable<CreatureImage>> GetCreatureImages()
        {
            if (_imagesSubject == null)
            {
                try
                {
                    var startingValue = localSettings.Values.ContainsKey(_settingName)
                    ? JsonSerializer.Deserialize<IEnumerable<CreatureImage>>((string)localSettings.Values[_settingName])
                    : Enumerable.Empty<CreatureImage>();
                    _imagesSubject = new BehaviorSubject<IEnumerable<CreatureImage>>(startingValue);
                }
                catch (JsonException ex)
                {
                    _imagesSubject = new BehaviorSubject<IEnumerable<CreatureImage>>(Enumerable.Empty<CreatureImage>());
                }
            }

            return _imagesSubject.AsObservable();
        }

        public void SaveCreatureImages(IEnumerable<CreatureImage> imageNamePairs)
        {
            localSettings.Values[_settingName] = JsonSerializer.Serialize(imageNamePairs);
            _imagesSubject.OnNext(imageNamePairs);
        }
    }
}
