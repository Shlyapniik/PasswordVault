using PasswordVault.Data;
using PasswordVault.Models;

namespace PasswordVault.Services;

public class VaultService
{
    private readonly DatabaseService _database;

    public VaultService(DatabaseService database)
    {
        _database = database;
    }

    public async Task<List<PasswordEntry>> GetEntriesAsync()
    {
        return await _database.GetEntriesAsync();
    }

    public async Task AddEntryAsync(PasswordEntry entry)
    {
        await _database.AddEntryAsync(entry);
    }

    public async Task UpdateEntryAsync(PasswordEntry entry)
    {
        await _database.UpdateEntryAsync(entry);
    }

    public async Task DeleteEntryAsync(PasswordEntry entry)
    {
        await _database.DeleteEntryAsync(entry);
    }
}