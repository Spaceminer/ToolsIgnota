using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using ToolsIgnota.Contracts.Services;
using ToolsIgnota.Core.Models;
using ToolsIgnota.Data.Models;
using ToolsIgnota.Helpers;
using ToolsIgnota.Models;

namespace ToolsIgnota.ViewModels;

public partial class InitiativeDisplayViewModel : ObservableRecipient, IDisposable
{
    private readonly ICreatureImageService _creatureImageService;
    private readonly ICombatManagerService _combatManagerService;
    private readonly List<IDisposable> _subscriptions = new();

    [ObservableProperty] private int _activeCreatureIndex = 0;
    [ObservableProperty] private string _roundNumber = "Round 1";

    private IEnumerable<CreatureImage> _creatureImages;
    private Dictionary<Guid, CMCreature> _creatureLookup = new();
    private bool _disposed;

    public ObservableCollection<InitiativeCreatureModel> CreatureList { get; set; } = new();

    public InitiativeDisplayViewModel(
        ICreatureImageService creatureImageService,
        ICombatManagerService combatManagerService)
    {
        _creatureImageService = creatureImageService ?? throw new ArgumentNullException(nameof(creatureImageService));
        _combatManagerService = combatManagerService ?? throw new ArgumentNullException(nameof(combatManagerService));

        _subscriptions.Add(_creatureImageService.CreatureImages.Subscribe(NewCreatureImages));
        _subscriptions.Add(_combatManagerService.Round.SubscribeOnDisplay(x => RoundNumber = $"Round {x}"));
        _subscriptions.Add(_combatManagerService.Creatures.SubscribeOnDisplay(UpdateCreatureList));
    }

    private void NewCreatureImages(IEnumerable<CreatureImage> creatureImages)
    {
        _creatureImages = creatureImages.OrderByDescending(x => x.Name.Length).ToList();
        UpdateCreatureImages();
    }

    private void UpdateCreatureImages()
    {
        foreach (var c in CreatureList)
        {
            var newImage = _creatureImages.FirstOrDefault(image => c.CreatureName.Contains(image.Name))?.Image ?? "..";
            if (c.CreatureImage != newImage)
                c.CreatureImage = newImage;
        }
    }

    private void UpdateCreatureList(IEnumerable<CMCreature> creatures)
    {
        // 1. Remove deleted creatures
        var newCreaturesById = creatures.ToImmutableDictionary(x => x.ID);
        var creaturesToRemove = CreatureList.Where(x => !newCreaturesById.ContainsKey(x.Id)).ToList();
        foreach(var c in creaturesToRemove)
        {
            CreatureList.Remove(c);
        }

        // 2. Add new creatures
        var existingCreaturesById = CreatureList.ToImmutableDictionary(x => x.Id);
        var creaturesToAdd = creatures.Where(x => !existingCreaturesById.ContainsKey(x.ID)).ToList();
        foreach (var c in creaturesToAdd)
        {
            CreatureList.Add(new InitiativeCreatureModel(c));
        }

        // 3. Sort by initative
        var sortedCreatures = creatures.OrderByDescending(x => x.InitiativeCount);
        if (creatures.Any(x => _creatureLookup.GetValueOrDefault(x.ID)?.InitiativeCount.CompareTo(x.InitiativeCount) != 0))
        {
            var initiativeCreaturesById = CreatureList.ToImmutableDictionary(x => x.Id);
            for (var i = 0; i < sortedCreatures.Count(); i++)
            {
                CreatureList.Move(CreatureList.IndexOf(initiativeCreaturesById[sortedCreatures.ElementAt(i).ID]), i);
            }
        }

        // 4. Scroll to active
        var activeIndex = sortedCreatures.TakeWhile(x => !x.IsActive).Count();
        App.DisplayWindow.DispatcherQueue.TryEnqueue(async () =>
        {
            // This is here because the carousel has to recieve the new list
            // before the index is set, or the events fire in the wrong order
            // and no animation plays.
            await Task.Delay(10);
            ActiveCreatureIndex = activeIndex;
        });

        UpdateCreatureImages();

        if (!CreatureList.Any())
            CreatureList.Add(new InitiativeCreatureModel());

        _creatureLookup = creatures.ToDictionary(x => x.ID);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _subscriptions.ForEach(x => x.Dispose());
            }
            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
