using Microsoft.Extensions.Logging;
using PasswordVault.Data;
using PasswordVault.Security;
using PasswordVault.Services;
using PasswordVault.ViewModels;
using PasswordVault.Views;
using CommunityToolkit.Maui;

namespace PasswordVault
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
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

            builder.Services.AddSingleton<VaultViewModel>();
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<AddPasswordViewModel>();
            builder.Services.AddTransient<PasswordDetailsViewModel>();
            builder.Services.AddTransient<PasswordDetailsPage>();
            builder.Services.AddTransient<EditPasswordViewModel>();
            builder.Services.AddTransient<EditPasswordPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
