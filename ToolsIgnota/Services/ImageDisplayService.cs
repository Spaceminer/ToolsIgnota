using System.Reactive.Linq;
using System.Reactive.Subjects;
using ToolsIgnota.Contracts.Services;

namespace ToolsIgnota.Services;

public class ImageDisplayService : IImageDisplayService
{
    private readonly ISubject<string> _backgroundImageSubject = new BehaviorSubject<string>("..");

    public IObservable<string> BackgroundImage => _backgroundImageSubject.AsObservable();

    public Task SetBackgroundImage(string imageUrl)
    {
        _backgroundImageSubject.OnNext(imageUrl);
        return Task.CompletedTask;
    }
}
