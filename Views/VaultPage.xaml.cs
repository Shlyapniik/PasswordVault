using PasswordVault.Models;
using PasswordVault.Services;

namespace PasswordVault.Views;

public partial class VaultPage : ContentPage
{
    private readonly VaultService _vaultService;

    public VaultPage(VaultService vaultService)
    {
        InitializeComponent();

        _vaultService = vaultService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        PasswordList.ItemsSource =
            await _vaultService.GetEntriesAsync();
    }

    private async void OnAddPasswordClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(AddPasswordPage));
    }

    private async void OnPasswordTapped(object sender, TappedEventArgs e)
    {
        if (sender is not Border border)
            return;

        if (border.BindingContext is not PasswordEntry entry)
            return;

        var page = new PasswordDetailsPage(_vaultService);

        page.LoadEntry(entry);

        await Navigation.PushAsync(page);
    }
}