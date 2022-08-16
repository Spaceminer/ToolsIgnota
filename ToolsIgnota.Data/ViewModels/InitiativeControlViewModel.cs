using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ToolsIgnota.Data.Abstractions;
using ToolsIgnota.Data.Models;
using ToolsIgnota.UI.Models;

namespace ToolsIgnota.UI.ViewModels
{
    public partial class InitiativeControlViewModel : ObservableObject
    {
        private readonly IFilePickerService _filePickerService;
        private readonly ICreatureImageService _creatureImageService;

        public ObservableCollection<CreatureImageModel> CreatureImageList { get; set; } = new();

        public InitiativeControlViewModel(
            IFilePickerService filePickerService,
            ICreatureImageService creatureImageService)
        {
            _filePickerService = filePickerService ?? throw new ArgumentNullException(nameof(filePickerService));
            _creatureImageService = creatureImageService ?? throw new ArgumentNullException(nameof(creatureImageService));

            using var cancellation = new CancellationTokenSource();
            creatureImageService.GetCreatureImages().Subscribe(images =>
            {
                CreatureImageList =
                    new ObservableCollection<CreatureImageModel>(
                        images.Select(x => new CreatureImageModel(x)));
                cancellation.Cancel();
            }, cancellation.Token);
        }

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
            {
                CreatureImageList.Remove(entry);
                SaveCreatureImages();
            }
        }

        [RelayCommand]
        public async Task UpdateCreatureImage(Guid id)
        {
            var image = await _filePickerService.GetImage();
            var entry = CreatureImageList.Where(x => x.Id == id).FirstOrDefault();
            if (image != null)
            {
                entry.Image = image.Path;
                SaveCreatureImages();
            }
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

        public void SaveCreatureImages()
        {
            _creatureImageService.SaveCreatureImages(CreatureImageList.Select(x => new CreatureImage { Name = x.Name, Image = x.Image }));
        }
    }
}
