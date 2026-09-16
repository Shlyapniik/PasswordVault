namespace PasswordVault.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
	}

	private async void OnUnlockClicked(object sender, EventArgs e)
	{
		string password = MasterPasswordEntry.Text ?? string.Empty;

		if (string.IsNullOrWhiteSpace(password))
		{
			await DisplayAlert(
                "Error",
				"Write master-password",
				"OK");

			return;
		}

        await Shell.Current.GoToAsync(nameof(VaultPage));
    }
}