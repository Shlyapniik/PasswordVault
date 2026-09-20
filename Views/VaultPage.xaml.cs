using PasswordVault.Models;
using PasswordVault.Services;

namespace PasswordVault.Views;

public partial class VaultPage : ContentPage
{
    private readonly VaultService _vaultService;
    private List<PasswordEntry> _allEntries = new();

    public VaultPage(VaultService vaultService)
    {
        InitializeComponent();

        _vaultService = vaultService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        _allEntries = await _vaultService.GetEntriesAsync();

        PasswordList.ItemsSource = _allEntries;
    }

    private void OnSearchTextChanged(object sended, TextChangedEventArgs e)
    {
        string searchText = e.NewTextValue?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(searchText))
        {
            PasswordList.ItemsSource = _allEntries;
            return;
        }

        var filteredEntries = _allEntries
            .Where(entry =>
                entry.Title.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                entry.Username.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                entry.Website.Contains(searchText, StringComparison.OrdinalIgnoreCase))
            .ToList();

        PasswordList.ItemsSource = filteredEntries;
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