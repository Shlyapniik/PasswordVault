using PasswordVault.Models;
using SQLite;

namespace PasswordVault.Data;

public class DatabaseService
{
    private readonly SQLiteAsyncConnection _database;
    private readonly Task _initializationTask;

    public DatabaseService()
    {
        string databasePath = Path.Combine(
            FileSystem.AppDataDirectory,
            "passwordvault.db3");

        _database = new SQLiteAsyncConnection(databasePath);

        _initializationTask = InitializeAsync();
    }

    private async Task InitializeAsync()
    {
        await _database.CreateTableAsync<PasswordEntry>();
    }

    public async Task<List<PasswordEntry>> GetEntriesAsync()
    {
        await _initializationTask;

        return await _database
            .Table<PasswordEntry>()
            .ToListAsync();
    }

    public async Task AddEntryAsync(PasswordEntry entry)
    {
        await _initializationTask;

        await _database.InsertAsync(entry);
    }

    public async Task UpdateEntryAsync(PasswordEntry entry)
    {
        await _initializationTask;

        await _database.UpdateAsync(entry);
    }

    public async Task DeleteEntryAsync(PasswordEntry entry)
    {
        await _initializationTask;

        await _database.DeleteAsync(entry);
    }
}