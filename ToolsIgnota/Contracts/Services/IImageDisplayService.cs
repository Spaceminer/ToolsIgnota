namespace ToolsIgnota.Contracts.Services;
public interface IImageDisplayService
{
    IObservable<string> BackgroundImage { get; }
    Task SetBackgroundImage(string imageUrl);
}
