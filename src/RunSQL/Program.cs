using Avalonia;
using Avalonia.Logging.Serilog;

namespace RunSQL
{
    class Program
    {
        public static void Main(string[] args) =>
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);

        public static AppBuilder BuildAvaloniaApp() =>
            AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToDebug();
    }
}
