using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PasswordVault.Models;
using PasswordVault.Security;
using PasswordVault.Services;
using PasswordVault.Views;
using System.Collections.ObjectModel;

namespace PasswordVault.ViewModels;

public partial class VaultViewModel : ObservableObject
{
    private readonly VaultService _vaultService;
    private readonly VaultSecurityService _vaultSecurity;
    private readonly IServiceProvider _serviceProvider;

    private List<PasswordEntry> _allEntries = new();

    [ObservableProperty]
    private string searchText = string.Empty;

    [ObservableProperty]
    private ObservableCollection<PasswordEntry> entries = new();

    public VaultViewModel(
        VaultService vaultService,
        VaultSecurityService vaultSecurity,
        IServiceProvider serviceProvider)
    {
        _vaultService = vaultService;
        _vaultSecurity = vaultSecurity;
        _serviceProvider = serviceProvider;
    }

    [RelayCommand]
    public async Task LoadAsync()
    {
        SearchText = string.Empty;

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

        Entries.Clear();

        foreach (var entry in filteredEntries)
        {
            Entries.Add(entry);
        }
    }

    [RelayCommand]
    private async Task LockAsync()
    {
        _vaultSecurity.Lock();

        await Shell.Current.Navigation
            .PopToRootAsync();
    }

    [RelayCommand]
    private async Task OpenEntryAsync(PasswordEntry? entry)
    {
        if (entry == null)
            return;

        var page =
            _serviceProvider.GetRequiredService<PasswordDetailsPage>();

        page.LoadEntry(entry);

        await Shell.Current.Navigation.PushAsync(page);
    }

    [RelayCommand]
    private async Task AddEntryAsync()
    {
        await Shell.Current.GoToAsync(
            nameof(AddPasswordPage));
    }
}