#if ANDROID
using Android.Views.InputMethods;
using PasswordVault.Security;
using PasswordVault.Services;
#endif
using PasswordVault.Models;
using PasswordVault.Security;
using PasswordVault.Services;

namespace PasswordVault.Views;

public partial class LoginPage : ContentPage
{

    private readonly VaultService _vaultService;
    private readonly VaultSecurityService _vaultSecurity;
    private readonly VaultSetupService _vaultSetup;

	public LoginPage(
        VaultService vaultService,
        VaultSecurityService vaultSecurity,
        VaultSetupService vaultSetup)
	{
		InitializeComponent();

        _vaultService = vaultService;
        _vaultSecurity = vaultSecurity;
        _vaultSetup = vaultSetup;
	}

#if ANDROID
    private void HideKeyboard()
    {
        var activity = Platform.CurrentActivity;

        if (activity?.CurrentFocus == null)
            return;

        var inputMethodManager =
            activity.GetSystemService(Android.Content.Context.InputMethodService)
            as InputMethodManager;

        inputMethodManager?.HideSoftInputFromWindow(
            activity.CurrentFocus.WindowToken,
            HideSoftInputFlags.None);

        MasterPasswordEntry.Unfocus();
    }
#endif

    private async void OnUnlockClicked(object sender, EventArgs e)
	{
		string password = MasterPasswordEntry.Text ?? string.Empty;

		if (string.IsNullOrWhiteSpace(password))
		{
			await DisplayAlertAsync(
                "Error",
				"Write master-password",
				"OK");

			return;
		}

        VaultMetadata? metadata = await _vaultService.GetVaultMetadataAsync();

        if (metadata == null)
        {
            metadata = _vaultSetup.CreateMetadata(password);

            await _vaultService.SaveVaultMetadataAsync(metadata);
        }

        bool unlocked = _vaultSecurity.Unlock(
            password,
            metadata.Salt,
            metadata.VerificationData);

#if ANDROID
		HideKeyboard();
#else
		MasterPasswordEntry.Unfocus();
#endif

        if (!unlocked)
        {
            await DisplayAlertAsync(
                "Ошибка",
                "Неверный мастер-пароль.",
                "ОК");

            return;
        }

        await Shell.Current.GoToAsync(nameof(VaultPage));
    }
}