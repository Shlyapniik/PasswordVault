using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PasswordVault.Models;
using PasswordVault.Security;
using PasswordVault.Services;
using PasswordVault.Views;

namespace PasswordVault.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    private readonly VaultService _vaultService;
    private readonly VaultSecurityService _vaultSecurity;
    private readonly VaultSetupService _vaultSetup;

    [ObservableProperty]
    private string masterPassword = string.Empty;

    [ObservableProperty]
    private bool isBusy;

    public LoginViewModel(
        VaultService vaultService,
        VaultSecurityService vaultSecurity,
        VaultSetupService vaultSetup)
    {
        _vaultService = vaultService;
        _vaultSecurity = vaultSecurity;
        _vaultSetup = vaultSetup;
    }

    [RelayCommand]
    private async Task UnlockAsync()
    {
        if (IsBusy)
            return;

        if (string.IsNullOrWhiteSpace(MasterPassword))
        {
            await Shell.Current.DisplayAlertAsync(
                "Ошибка",
                "Введите мастер-пароль.",
                "OK");

            return;
        }

        try
        {
            IsBusy = true;

            VaultMetadata? metadata =
                await _vaultService.GetVaultMetadataAsync();

            if (metadata == null)
            {
                metadata = _vaultSetup.CreateMetadata(
                    MasterPassword);

                await _vaultService.SaveVaultMetadataAsync(
                    metadata);
            }

            bool unlocked = _vaultSecurity.Unlock(
                MasterPassword,
                metadata.Salt,
                metadata.VerificationData);

            if (!unlocked)
            {
                await Shell.Current.DisplayAlertAsync(
                    "Ошибка",
                    "Неверный мастер-пароль.",
                    "OK");

                return;
            }

            MasterPassword = string.Empty;

            await Shell.Current.GoToAsync(
                nameof(VaultPage));
        }
        finally
        {
            IsBusy = false;
        }
    }
}