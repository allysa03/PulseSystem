using InventorySystemDepEd.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http;

#if WINDOWS
using Microsoft.Maui.LifecycleEvents;
using WinRT.Interop;
using System;
using System.Runtime.InteropServices;
#endif

namespace InventorySystemDepEd
{
    public static class MauiProgram
    {
#if WINDOWS
        const int SW_MAXIMIZE = 3;

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
#endif

        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

#if WINDOWS
            builder.ConfigureLifecycleEvents(events =>
            {
                events.AddWindows(windows =>
                {
                    windows.OnWindowCreated(window =>
                    {
                        var hwnd = WindowNative.GetWindowHandle(window);
                        ShowWindow(hwnd, SW_MAXIMIZE);
                    });
                });
            });
#endif

            // =====================================
            // 🌐 HTTP CLIENT (CALLS YOUR API ONLY)
            // =====================================
            builder.Services.AddScoped(sp =>
            {
                return new HttpClient
                {
                    //BaseAddress = new Uri("http://10.20.11.56:5170/")
                    BaseAddress = new Uri("http://localhost:5170/")
                };
            });

            

            // =====================================
            // 🟢 CLIENT SERVICES (API WRAPPERS ONLY)
            // =====================================
            builder.Services.AddScoped<UserServiceEfCore>();
            //builder.Services.AddScoped<OfficeServiceEfCore>();
            //builder.Services.AddScoped<PersonnelsServiceEfCore>();
            builder.Services.AddScoped<NotificationServiceEfCore>();
            builder.Services.AddScoped<SettingServiceEfCore>();
            builder.Services.AddScoped<PositionServiceEfCore>();
            builder.Services.AddScoped<LogoServiceEfCore>();
            var app = builder.Build();

            // =====================================
            // OPTIONAL STARTUP LOG ONLY
            // =====================================
            using (var scope = app.Services.CreateScope())
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<App>>();

                logger.LogInformation("🔥 MAUI Client started successfully!");
            }

            return app;
        }
    }
}