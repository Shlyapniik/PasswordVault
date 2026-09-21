using PasswordVault.Models;
using PasswordVault.Services;

namespace PasswordVault.Views;

public partial class PasswordDetailsPage : ContentPage
{
    private readonly VaultService _vaultService;

    private PasswordEntry? _entry;

    private bool _isPasswordVisible;

    public PasswordDetailsPage(VaultService vaultService)
    {
        InitializeComponent();

        _vaultService = vaultService;
    }

    public void LoadEntry(PasswordEntry entry)
    {
        _entry = entry;

        RefreshEntry();
    }

    public void RefreshEntry()
    {
        if (_entry == null)
            return;

        TitleLabel.Text = _entry.Title;
        UsernameEntry.Text = _entry.Username;
        PasswordValueEntry.Text = _entry.Password;
        WebsiteEntry.Text = _entry.Website;
        NotesEditor.Text = _entry.Notes;
    }

    private void OnShowPasswordClicked(object sender, EventArgs e)
    {
        _isPasswordVisible = !_isPasswordVisible;

        PasswordValueEntry.IsPassword = !_isPasswordVisible;

        ShowPasswordButton.Text =
            _isPasswordVisible ? "Скрыть" : "Показать";
    }

    private async void OnEditClicked(object sender, EventArgs e)
    {
        if (_entry == null)
            return;

        var page = new EditPasswordPage(_vaultService,this);

        page.LoadEntry(_entry);

        await Navigation.PushAsync(page);
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        if (_entry == null)
            return;

        bool confirmed = await DisplayAlertAsync(
            "Удаление",
            $"Удалить запись «{_entry.Title}»?",
            "Удалить",
            "Отмена");

        if (!confirmed)
            return;

        await _vaultService.DeleteEntryAsync(_entry);

        await Navigation.PopAsync();
    }
}