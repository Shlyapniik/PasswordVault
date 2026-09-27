using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PasswordVault.Models;
using PasswordVault.Services;

namespace PasswordVault.ViewModels;

public partial class AddPasswordViewModel : ObservableObject
{
    private readonly VaultService _vaultService;
    private readonly PasswordGeneratorService _passwordGeneratorService;

    [ObservableProperty]
    private string title = string.Empty;

    [ObservableProperty]
    private string username = string.Empty;

    [ObservableProperty]
    private string password = string.Empty;

    [ObservableProperty]
    private int passwordLength = 16;

    [ObservableProperty]
    private string website = string.Empty;

    [ObservableProperty]
    private string notes = string.Empty;

    [ObservableProperty]
    private bool isBusy;

    public AddPasswordViewModel(
        VaultService vaultService,
        PasswordGeneratorService passwordGeneratorService)
    {
        _vaultService = vaultService;
        _passwordGeneratorService = passwordGeneratorService;
    }

    [RelayCommand]
    private void GeneratePassword()
    {
        Password = _passwordGeneratorService.Generate(
            (int)PasswordLength);
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (IsBusy)
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

            var entry = new PasswordEntry
            {
                Title = Title.Trim(),
                Username = Username,
                Password = Password,
                Website = Website,
                Notes = Notes
            };

            await _vaultService.AddEntryAsync(entry);

            await Shell.Current.Navigation.PopAsync();
        }
        finally
        {
            IsBusy = false;
        }
    }
}