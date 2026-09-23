using Microsoft.Extensions.Logging;
using PasswordVault.Services;
using PasswordVault.Data;
using PasswordVault.Security;

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

            builder.Services.AddSingleton<KeyDerivationService>();
            builder.Services.AddSingleton<EncryptionService>();
            builder.Services.AddSingleton<VaultSecurityService>();
            builder.Services.AddSingleton<VaultSetupService>();
            builder.Services.AddSingleton<PasswordDataSerializer>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
