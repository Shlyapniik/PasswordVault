using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PasswordVault.Models;
using PasswordVault.Services;
using PasswordVault.Views;
using CommunityToolkit.Maui.Alerts;

namespace PasswordVault.ViewModels;

public partial class PasswordDetailsViewModel : ObservableObject
{
    private readonly VaultService _vaultService;

    private PasswordEntry? _entry;

    [ObservableProperty]
    private string title = string.Empty;

    [ObservableProperty]
    private string username = string.Empty;

    [ObservableProperty]
    private string password = string.Empty;
    
    [ObservableProperty]
    private bool isPasswordHidden = true;

    [ObservableProperty]
    private string website = string.Empty;

    [ObservableProperty]
    private string notes = string.Empty;

    public PasswordDetailsViewModel(
        VaultService vaultService)
    {
        _vaultService = vaultService;
    }

    public void LoadEntry(PasswordEntry entry)
    {
        _entry = entry;

        Title = entry.Title;
        Username = entry.Username;
        Password = entry.Password;
        Website = entry.Website;
        Notes = entry.Notes;
    }

    public void Refresh()
    {
        if (_entry == null)
            return;

        Title = _entry.Title;
        Username = _entry.Username;
        Password = _entry.Password;
        Website = _entry.Website;
        Notes = _entry.Notes;
    }

    [RelayCommand]
    private void TogglePasswordVisibility()
    {
        IsPasswordHidden = !IsPasswordHidden;
    }

    [RelayCommand]
    private async Task CopyPasswordAsync()
    {
        if (string.IsNullOrEmpty(Password))
            return;

        await Clipboard.Default.SetTextAsync(Password);

        await Snackbar.Make(
            "Пароль скопирован",
            duration: TimeSpan.FromSeconds(2))
            .Show();
    }

    [RelayCommand]
    private async Task EditAsync()
    {
        if (_entry == null)
            return;

        var page = App.Current!
            .Handler!
            .MauiContext!
            .Services
            .GetRequiredService<EditPasswordPage>();

        page.LoadEntry(_entry);

        await Shell.Current.Navigation.PushAsync(page);
    }

    [RelayCommand]
    private async Task DeleteAsync()
    {
        if (_entry == null)
            return;

        bool confirmed =
            await Shell.Current.DisplayAlertAsync(
                "Удаление",
                $"Удалить запись «{Title}»?",
                "Удалить",
                "Отмена");

        if (!confirmed)
            return;

        await _vaultService.DeleteEntryAsync(_entry);

        await Shell.Current.Navigation.PopAsync();
    }


}