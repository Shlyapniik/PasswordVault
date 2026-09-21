using Microsoft.Extensions.Logging;
using PasswordVault.Services;
using PasswordVault.Data;

namespace PasswordVault
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            builder.Services.AddSingleton<VaultService>();
            builder.Services.AddSingleton<DatabaseService>();
            builder.Services.AddSingleton<PasswordGeneratorService>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
