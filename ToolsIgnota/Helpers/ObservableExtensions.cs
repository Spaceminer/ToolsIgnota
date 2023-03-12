namespace ToolsIgnota.Helpers;

public static class ObservableExtensions
{
    public static IDisposable SubscribeOnMain<T>(this IObservable<T> observable, Action<T> action)
    {
        return observable.Subscribe(x =>
        {
            App.MainWindow.DispatcherQueue.TryEnqueue(() => action(x));
        });
    }

    public static IDisposable SubscribeOnDisplay<T>(this IObservable<T> observable, Action<T> action)
    {
        return observable.Subscribe(x =>
        {
            App.MainWindow.DispatcherQueue.TryEnqueue(() => action(x));
        });
    }
}
