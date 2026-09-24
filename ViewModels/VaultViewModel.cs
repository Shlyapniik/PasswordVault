using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PasswordVault.Models;
using PasswordVault.Security;
using PasswordVault.Services;
using System.Collections.ObjectModel;

namespace PasswordVault.ViewModels;

public partial class VaultViewModel : ObservableObject
{
    private readonly VaultService _vaultService;
    private readonly VaultSecurityService _vaultSecurity;

    private List<PasswordEntry> _allEntries = new();

    [ObservableProperty]
    private string searchText = string.Empty;

    [ObservableProperty]
    private ObservableCollection<PasswordEntry> entries = new();

    public VaultViewModel(
        VaultService vaultService,
        VaultSecurityService vaultSecurity)
    {
        _vaultService = vaultService;
        _vaultSecurity = vaultSecurity;
    }

    [RelayCommand]
    public async Task LoadAsync()
    {
        _allEntries = await _vaultService.GetEntriesAsync();

        ApplyFilter();
    }

    partial void OnSearchTextChanged(
        string value)
    {
        ApplyFilter();
    }

    private void ApplyFilter()
    {
        var filteredEntries = _allEntries
            .Where(entry =>
                string.IsNullOrWhiteSpace(SearchText) ||
                entry.Title.Contains(
                    SearchText,
                    StringComparison.OrdinalIgnoreCase) ||
                entry.Username.Contains(
                    SearchText,
                    StringComparison.OrdinalIgnoreCase) ||
                entry.Website.Contains(
                    SearchText,
                    StringComparison.OrdinalIgnoreCase))
            .ToList();

        Entries =
            new ObservableCollection<PasswordEntry>(
                filteredEntries);
    }

    [RelayCommand]
    private async Task LockAsync()
    {
        _vaultSecurity.Lock();

        await Shell.Current.Navigation
            .PopToRootAsync();
    }
}