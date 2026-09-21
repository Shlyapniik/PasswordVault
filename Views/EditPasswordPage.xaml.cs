using PasswordVault.Models;
using PasswordVault.Services;

namespace PasswordVault.Views;

public partial class EditPasswordPage : ContentPage
{
    private readonly VaultService _vaultService;
    private readonly PasswordDetailsPage _detailsPage;

    private PasswordEntry? _entry;

    public EditPasswordPage(VaultService vaultService, PasswordDetailsPage detailsPage)
    {
        InitializeComponent();

        _vaultService = vaultService;
        _detailsPage = detailsPage;
    }

    public void LoadEntry(PasswordEntry entry)
    {
        _entry = entry;

        TitleEntry.Text = entry.Title;
        UsernameEntry.Text = entry.Username;
        PasswordEntry.Text = entry.Password;
        WebsiteEntry.Text = entry.Website;
        NotesEditor.Text = entry.Notes;
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (_entry == null)
            return;

        if (string.IsNullOrWhiteSpace(TitleEntry.Text))
        {
            await DisplayAlertAsync(
                "Ошибка",
                "Введите название записи.",
                "OK");

            return;
        }

        if (string.IsNullOrWhiteSpace(PasswordEntry.Text))
        {
            await DisplayAlertAsync(
                "Ошибка",
                "Введите пароль.",
                "OK");

            return;
        }

        _entry.Title = TitleEntry.Text;
        _entry.Username = UsernameEntry.Text ?? string.Empty;
        _entry.Password = PasswordEntry.Text;
        _entry.Website = WebsiteEntry.Text ?? string.Empty;
        _entry.Notes = NotesEditor.Text ?? string.Empty;

        await _vaultService.UpdateEntryAsync(_entry);
        _detailsPage.RefreshEntry();
        await Navigation.PopAsync();
    }
}