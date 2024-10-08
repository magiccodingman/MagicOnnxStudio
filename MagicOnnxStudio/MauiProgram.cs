using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using Microsoft.Maui.Platform;
using MudBlazor.Services;
#if WINDOWS
using Windows.Graphics;
#endif

namespace MagicOnnxStudio
{
    public static class MauiProgram
    {
        const int WindowWidth = 1280;
        const int WindowHeight = 800;
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()  
                 .ConfigureLifecycleEvents(events =>
                 {
#if WINDOWS
                     events.AddWindows(windows =>
                     {
                         windows.OnWindowCreated(window =>
                         {
                             IntPtr windowHandle = WinRT.Interop.WindowNative.GetWindowHandle(window);
                             var windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(windowHandle);
                             var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(windowId);

                             if (appWindow != null)
                             {
                                 // Set window size
                                 appWindow.Resize(new SizeInt32(WindowWidth, WindowHeight));
                             }
                         });
                     });
#endif
                 })
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddMudServices();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
