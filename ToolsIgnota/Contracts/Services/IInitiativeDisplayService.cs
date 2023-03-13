using ToolsIgnota.Core.Models;

namespace ToolsIgnota.Contracts.Services;
public interface IInitiativeDisplayService: IOnInitialize
{
    public IObservable<IEnumerable<CreatureImage>> CreatureImages { get; }
    public IObservable<bool> IsVisible { get; }

    public Task SaveCreatureImages(IEnumerable<CreatureImage> imageNamePairs);
    public Task SetIsVisible(bool isVisible);
}
