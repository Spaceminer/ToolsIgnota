using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ToolsIgnota.Backend.Abstractions;
using ToolsIgnota.Backend.Models;
using ToolsIgnota.UI.Models;
using ToolsIgnota.UI.Utilities;

namespace ToolsIgnota.UI.ViewModels
{
    public partial class InitiativeControlViewModel : ObservableObject
    {
        private readonly ISettings _settings;

        public InitiativeControlViewModel(ISettings settings)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public ObservableCollection<CreatureImageModel> CreatureImageList { get; set; } = new();

        [RelayCommand]
        public void AddCreatureImage()
        {
            CreatureImageList.Add(new CreatureImageModel(new CreatureImage()));
        }

        [RelayCommand]
        public void DeleteCreatureImage(Guid id)
        {
            var entry = CreatureImageList.Where(x => x.Id == id).FirstOrDefault();
            if(entry != null)
                CreatureImageList.Remove(entry);
        }

        [RelayCommand]
        public async Task UpdateCreatureImage(Guid id)
        {
            var image = await FilePicker.GetImage();
            var entry = CreatureImageList.Where(x => x.Id == id).FirstOrDefault();
            if (image != null)
                entry.Image = image.Path;
        }

        private bool CanLaunchDisplay => CombatManagerUri?.Split(":").All(x => !string.IsNullOrWhiteSpace(x)) ?? false;

        [RelayCommand(CanExecute = nameof(CanLaunchDisplay))]
        public Task LaunchDisplay()
        {
            return Task.CompletedTask;
        }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CombatManagerUri))]
        [NotifyCanExecuteChangedFor(nameof(LaunchDisplayCommand))]
        private string combatManagerIpAddress;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CombatManagerUri))]
        [NotifyCanExecuteChangedFor(nameof(LaunchDisplayCommand))]
        private string combatManagerPort;

        public string CombatManagerUri => $"{combatManagerIpAddress}:{combatManagerPort}";
    }
}
