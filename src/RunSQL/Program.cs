using System.IO;
using System.Reflection;
using Avalonia;
using Avalonia.Logging.Serilog;

namespace RunSQL
{
    class Program
    {
        public static void Main(string[] args)
        {
            // https://github.com/dotnet/project-system/issues/2239
            Directory.SetCurrentDirectory(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location));

            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        }

        public static AppBuilder BuildAvaloniaApp() =>
            AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToDebug();
    }
}
