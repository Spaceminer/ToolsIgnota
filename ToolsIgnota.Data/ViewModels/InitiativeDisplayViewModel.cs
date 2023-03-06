using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ToolsIgnota.Data.Abstractions;
using ToolsIgnota.Data.Models;
using ToolsIgnota.UI.Models;

namespace ToolsIgnota.UI.ViewModels
{
    public partial class InitiativeDisplayViewModel : ObservableObject
    {
        private readonly ICreatureImageService _creatureImageService;
        private readonly ICombatManagerService _combatManagerService;
        private readonly IInitiativeDisplayService _initiativeDisplayService;

        public InitiativeDisplayViewModel(
            ICreatureImageService creatureImageService,
            ICombatManagerService combatManagerService,
            IInitiativeDisplayService initiativeDisplayService)
        {
            _creatureImageService = creatureImageService ?? throw new ArgumentNullException(nameof(creatureImageService));
            _combatManagerService = combatManagerService ?? throw new ArgumentNullException(nameof(combatManagerService));
            _initiativeDisplayService = initiativeDisplayService ?? throw new ArgumentNullException(nameof(initiativeDisplayService));

            _creatureImageService.GetCreatureImages().Subscribe(x => _creatureImages = x);
            _combatManagerService.GetCreatures().Subscribe(x => UpdateInitiativeDisplay(x));
            _combatManagerService.GetRound().Subscribe(roundNumber => RoundLabel = $"Round {roundNumber}");
            _initiativeDisplayService.GetBackgroundImage().Subscribe(SetBackgroundImage);
        }

        [ObservableProperty]
        private string roundLabel;
        [ObservableProperty]
        private string backgroundImage;

        private Dictionary<Guid, InitiativeRecordModel> _records = new();
        private IEnumerable<CreatureImage> _creatureImages = Enumerable.Empty<CreatureImage>();

        public ObservableCollection<InitiativeRecordModel> InitiativeRecords { get; set; } = new ObservableCollection<InitiativeRecordModel>();

        public void SetBackgroundImage(Uri path)
        {
            BackgroundImage = path?.ToString() ?? "C:\\Users\\rjshe\\source\\repos\\ToolsIgnota\\ToolsIgnota.UI\\ToolsIgnota.UI\\Assets\\Tavern.jpg";
            //((ImageBrush)container.Background).ImageSource = new BitmapImage(path);
        }

        public void BackgroundImageFailed()
        {
            return;
        }

        private void UpdateInitiativeDisplay(IEnumerable<CMCreature> creatures)
        {
            foreach (var creature in creatures)
            {
                if (!_records.ContainsKey(creature.ID))
                {
                    _records.Add(creature.ID, new InitiativeRecordModel
                    {
                        CreatureId = creature.ID,
                        Image = FindImageUri(creature.Name),
                        DisplayName = creature.Name,
                        IsHighlighted = creature.IsActive,
                    });
                }
                else
                {
                    _records[creature.ID].IsHighlighted = creature.IsActive;
                }
            }
            InitiativeRecords.Clear();

            if (!creatures.Any())
                return;

            var orderedCreatures = creatures.OrderByDescending(x => x.InitiativeCount).ToList();
            int activeIndex = orderedCreatures.FindIndex(x => x.IsActive);
            int offset = (activeIndex + (orderedCreatures.Count() / 2) + 1) % orderedCreatures.Count();
            var offsetCreatures = orderedCreatures.Skip(offset).Concat(orderedCreatures.Take(offset)).ToList();
            
            for (int i = 0; i < offsetCreatures.Count(); i++)
            {
                CMCreature creature = offsetCreatures[i];
                if (i == offsetCreatures.Count() - 1)
                {
                    _records[creature.ID] = new InitiativeRecordModel
                    {
                        Image = FindImageUri(creature.Name),
                        DisplayName = creature.Name,
                        IsHighlighted = creature.IsActive,
                    };
                }
                InitiativeRecords.Add(_records[creature.ID]);
            }
        }

        private string FindImageUri(string name)
        {
            return _creatureImages.Where(x => name.Contains(x.Name)).FirstOrDefault()?.Image;
        }
    }
}
