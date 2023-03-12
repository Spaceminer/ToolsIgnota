using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ToolsIgnota.Contracts.Services;
using ToolsIgnota.Core.Models;
using ToolsIgnota.Models;

namespace ToolsIgnota.ViewModels;

public partial class CreatureImageSettingsViewModel : ObservableRecipient
{
    private readonly ICreatureImageService _creatureImageService;
    private readonly IFilePickerService _filePickerService;

    public ObservableCollection<CreatureImageModel> CreatureImageList { get; set; } = new();

    public CreatureImageSettingsViewModel(
        ICreatureImageService creatureImageService,
        IFilePickerService filePickerService)
    {
        _creatureImageService = creatureImageService ?? throw new ArgumentNullException(nameof(creatureImageService));
        _filePickerService = filePickerService ?? throw new ArgumentNullException(nameof(filePickerService));

        _creatureImageService.CreatureImages.FirstAsync().Subscribe(images =>
        {
            CreatureImageList =
                new ObservableCollection<CreatureImageModel>(
                    images.Select(x => NewCreatureImageModel(x)))
                {
                    NewCreatureImageModel()
                };
        });
    }

    [RelayCommand]
    public async Task DeleteCreatureImage(Guid id)
    {
        var entry = CreatureImageList.Where(x => x.Id == id).FirstOrDefault();
        if (entry != null)
        {
            CreatureImageList.Remove(entry);
            await SaveCreatureImages();
        }
    }

    [RelayCommand]
    public async Task PickImageForCreatureImage(Guid id)
    {
        var image = await _filePickerService.PickImage();
        var entry = CreatureImageList.Where(x => x.Id == id).FirstOrDefault();
        if (image != null && entry != null)
        {
            entry.Image = image.Path;
            await SaveCreatureImages();
        }
    }

    public void AddNewFinalCreatureImage()
    {
        CreatureImageList.Add(NewCreatureImageModel());
    }

    private async Task SaveCreatureImages()
    {
        var badEntries = CreatureImageList
            .SkipLast(1)
            .Where(x => string.IsNullOrWhiteSpace(x.Name))
            .ToList();
        foreach (var badEntry in badEntries)
        {
            CreatureImageList.Remove(badEntry);
        }
        if(CreatureImageList.Last().Name != "")
            CreatureImageList.Add(NewCreatureImageModel());

        await _creatureImageService.SaveCreatureImages(
            CreatureImageList
                .SkipLast(1)
                .Select(x => new CreatureImage { Name = x.Name, Image = x.Image })
                .ToList());
    }

    private void CreatureImageModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == "Name")
        {
            SaveCreatureImages().ConfigureAwait(false);
        }
    }

    private CreatureImageModel NewCreatureImageModel(CreatureImage? creatureImage = null)
    {
        var result = new CreatureImageModel(creatureImage ?? new CreatureImage());
        result.PropertyChanged += CreatureImageModel_PropertyChanged;
        return result;
    }
}
