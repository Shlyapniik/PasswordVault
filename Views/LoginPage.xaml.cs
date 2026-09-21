#if ANDROID
using Android.Views.InputMethods;
#endif

namespace PasswordVault.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
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

#if ANDROID
		HideKeyboard();
#else
		MasterPasswordEntry.Unfocus();
#endif

        await Shell.Current.GoToAsync(nameof(VaultPage));
    }
}