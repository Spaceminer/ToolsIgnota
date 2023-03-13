using System.Reactive.Linq;
using System.Reactive.Subjects;
using ToolsIgnota.Contracts.Services;
using ToolsIgnota.Core.Models;

namespace ToolsIgnota.Core.Services;

public class InitiativeDisplayService : IInitiativeDisplayService
{
    private const string ImagesKey = "CreatureImages";
    private const string VisibleKey = "InitiativeVisible";

    private readonly ILocalSettingsService _localSettingsService;
    private readonly ISubject<IEnumerable<CreatureImage>> _creatureImagesSubject = new ReplaySubject<IEnumerable<CreatureImage>>(1);
    private readonly ISubject<bool> _isVisibleSubject = new ReplaySubject<bool>(1);

    public InitiativeDisplayService(ILocalSettingsService localSettingsService)
    {
        _localSettingsService = localSettingsService;
    }

    public async Task InitializeAsync()
    {
        var creatureImages = await _localSettingsService.ReadSettingAsync<IEnumerable<CreatureImage>>(ImagesKey);
        _creatureImagesSubject.OnNext(creatureImages ?? Enumerable.Empty<CreatureImage>());

        var isVisible = await _localSettingsService.ReadSettingAsync<bool>(VisibleKey);
        _isVisibleSubject.OnNext(isVisible);
    }

    public IObservable<IEnumerable<CreatureImage>> CreatureImages => _creatureImagesSubject.AsObservable();

    public IObservable<bool> IsVisible => _isVisibleSubject.AsObservable();

    public async Task SaveCreatureImages(IEnumerable<CreatureImage> imageNamePairs)
    {
        await _localSettingsService.SaveSettingAsync(ImagesKey, imageNamePairs);
        _creatureImagesSubject.OnNext(imageNamePairs);
    }

    public async Task SetIsVisible(bool isVisible)
    {
        await _localSettingsService.SaveSettingAsync(VisibleKey, isVisible);
        _isVisibleSubject.OnNext(isVisible);
    }
}
