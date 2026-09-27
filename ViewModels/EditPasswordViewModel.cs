using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PasswordVault.Models;
using PasswordVault.Services;

namespace PasswordVault.ViewModels;

public partial class EditPasswordViewModel : ObservableObject
{
    private readonly VaultService _vaultService;
    private readonly PasswordGeneratorService _passwordGeneratorService;

    private PasswordEntry? _entry;

    [ObservableProperty]
    private string title = string.Empty;

    [ObservableProperty]
    private string username = string.Empty;

    [ObservableProperty]
    private string password = string.Empty;

    [ObservableProperty]
    private string website = string.Empty;

    [ObservableProperty]
    private string notes = string.Empty;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private double passwordLength = 16;

    [ObservableProperty]
    private bool useLowercase = true;

    [ObservableProperty]
    private bool useUppercase = true;

    [ObservableProperty]
    private bool useDigits = true;

    [ObservableProperty]
    private bool useSpecial = true;

    public EditPasswordViewModel(
        VaultService vaultService,
        PasswordGeneratorService passwordGeneratorService)
    {
        _vaultService = vaultService;
        _passwordGeneratorService = passwordGeneratorService;
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

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (IsBusy)
            return;

        if (_entry == null)
            return;

        if (string.IsNullOrWhiteSpace(Title))
        {
            await Shell.Current.DisplayAlertAsync(
                "Ошибка",
                "Введите название записи.",
                "OK");

            return;
        }

        try
        {
            IsBusy = true;

            _entry.Title = Title.Trim();
            _entry.Username = Username;
            _entry.Password = Password;
            _entry.Website = Website;
            _entry.Notes = Notes;

            await _vaultService.UpdateEntryAsync(_entry);

            await Shell.Current.Navigation.PopAsync();
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task GeneratePassword()
    {
        try
        {
            Password = _passwordGeneratorService.Generate(
                (int)PasswordLength,
                UseLowercase,
                UseUppercase,
                UseDigits,
                UseSpecial);
        }
        catch (ArgumentException ex)
        {
            await Shell.Current.DisplayAlertAsync(
                "Не удалось создать пароль",
                ex.Message,
                "OK");
        }
    }
}