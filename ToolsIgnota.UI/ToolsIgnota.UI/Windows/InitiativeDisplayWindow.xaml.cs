using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using ToolsIgnota.Backend;
using ToolsIgnota.Backend.Models;
using ToolsIgnota.UI.Pages;
using ToolsIgnota.UI.UserControls;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ToolsIgnota.UI.Windows
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class InitiativeDisplayWindow : Window
    {
        private readonly InitiativeControlPage _controlPage;
        private readonly Dictionary<Guid, InitiativeRecord> _records;
        private readonly IEnumerable<ImageNamePair> _creatureImages;

        public InitiativeDisplayWindow(InitiativeControlPage controlPage, IEnumerable<ImageNamePair> creatureImages)
        {
            this.InitializeComponent();
            _controlPage = controlPage ?? throw new ArgumentNullException(nameof(controlPage));
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
            _controlPage.DisplayWindowClosed();
        }

        private string FindImageUri(string name)
        {
            return _creatureImages.Where(x => name.Contains(x.Name)).FirstOrDefault()?.Image;
        }
    }
}
