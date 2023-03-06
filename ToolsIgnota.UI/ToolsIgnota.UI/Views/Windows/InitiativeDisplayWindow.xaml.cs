using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Linq;
using ToolsIgnota.Data.Models;
using ToolsIgnota.UI.Views.UserControls;

namespace ToolsIgnota.UI.Views.Windows
{
    public sealed partial class InitiativeDisplayWindow : Window
    {
        private readonly Action _windowClosedCallback;
        private readonly Dictionary<Guid, InitiativeRecord> _records;
        private readonly IEnumerable<CreatureImage> _creatureImages;

        public InitiativeDisplayWindow(IEnumerable<CreatureImage> creatureImages, Action windowClosedCallback)
        {
            this.InitializeComponent();
            _windowClosedCallback = windowClosedCallback ?? throw new ArgumentNullException(nameof(windowClosedCallback));
            _records = new Dictionary<Guid, InitiativeRecord>();
            _creatureImages = creatureImages ?? throw new ArgumentNullException(nameof(creatureImages));
        }

        public void SetBackgroundImage(Uri path)
        {
            ((ImageBrush)container.Background).ImageSource = new BitmapImage(path);
        }

        public void UpdateInitiativeDisplay(IEnumerable<CMCreature> creatures)
        {

            foreach(var creature in creatures)
            {
                if(!_records.ContainsKey(creature.ID))
                {
                    _records.Add(creature.ID, new InitiativeRecord
                    {
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

            var orderedCreatures = creatures.OrderByDescending(x => x.InitiativeCount).ToList();
            int activeIndex = orderedCreatures.FindIndex(x => x.IsActive);
            int offset = (activeIndex + (orderedCreatures.Count() / 2) + 1) % orderedCreatures.Count();
            var offsetCreatures = orderedCreatures.Skip(offset).Concat(orderedCreatures.Take(offset)).ToList();

            panel_initiative.Children.Clear();
            for(int i = 0; i < offsetCreatures.Count(); i++)
            {
                CMCreature creature = offsetCreatures[i];
                if(i == offsetCreatures.Count() - 1)
                {
                    _records[creature.ID] = new InitiativeRecord
                    {
                        Image = FindImageUri(creature.Name),
                        DisplayName = creature.Name,
                        IsHighlighted = creature.IsActive,
                    };
                }
                panel_initiative.Children.Add(_records[creature.ID]);
            }
        }

        private void Window_Closed(object sender, WindowEventArgs args)
        {
            _windowClosedCallback();
        }

        private string FindImageUri(string name)
        {
            return _creatureImages.Where(x => name.Contains(x.Name)).FirstOrDefault()?.Image;
        }
    }
}
