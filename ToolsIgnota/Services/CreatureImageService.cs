using System.Reactive.Linq;
using System.Reactive.Subjects;
using ToolsIgnota.Contracts.Services;
using ToolsIgnota.Core.Models;

namespace ToolsIgnota.Core.Services;

public class CreatureImageService : ICreatureImageService
{

    private const string SettingsKey = "CreatureImages";

    private readonly ILocalSettingsService _localSettingsService;
    private readonly ISubject<IEnumerable<CreatureImage>> _creatureImagesSubject = new ReplaySubject<IEnumerable<CreatureImage>>(1);

    public CreatureImageService(ILocalSettingsService localSettingsService)
    {
        _localSettingsService = localSettingsService;
    }

    public async Task InitializeAsync()
    {
        var creatureImages = await _localSettingsService.ReadSettingAsync<IEnumerable<CreatureImage>>(SettingsKey);
        _creatureImagesSubject.OnNext(creatureImages ?? Enumerable.Empty<CreatureImage>());
    }

    public IObservable<IEnumerable<CreatureImage>> CreatureImages => _creatureImagesSubject.AsObservable();

    public async Task SaveCreatureImages(IEnumerable<CreatureImage> imageNamePairs)
    {
        await _localSettingsService.SaveSettingAsync(SettingsKey, imageNamePairs);
    }
}
