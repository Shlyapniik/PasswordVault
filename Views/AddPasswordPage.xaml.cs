using PasswordVault.Models;
using PasswordVault.Services;

namespace PasswordVault.Views;

public partial class AddPasswordPage : ContentPage
{
    private readonly VaultService _vaultService;

    public AddPasswordPage(VaultService vaultService)
    {
        InitializeComponent();

        _vaultService = vaultService;
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TitleEntry.Text))
        {
            await DisplayAlert(
                "Ошибка",
                "Введите название записи.",
                "OK");

            return;
        }

        if (string.IsNullOrWhiteSpace(PasswordEntry.Text))
        {
            await DisplayAlert(
                "Ошибка",
                "Введите пароль.",
                "OK");

            return;
        }

        var entry = new PasswordEntry
        {
            Title = TitleEntry.Text,
            Username = UsernameEntry.Text ?? string.Empty,
            Password = PasswordEntry.Text,
            Website = WebsiteEntry.Text ?? string.Empty,
            Notes = NotesEditor.Text ?? string.Empty
        };

        await _vaultService.AddEntryAsync(entry);

        await Shell.Current.GoToAsync("..");
    }
}