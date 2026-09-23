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
        await _database.CreateTableAsync<VaultMetadata>();

        await MigratePasswordEntryTableAsync();

        await _database.CreateTableAsync<PasswordEntry>();
    }

    private async Task MigratePasswordEntryTableAsync()
    {
        var columns = await _database.QueryAsync<TableColumnInfo>(
            "PRAGMA table_info(PasswordEntry)");

        if (columns.Count == 0)
            return;

        bool needsMigration = columns.Any(column =>
            column.Name.Equals("Username", StringComparison.OrdinalIgnoreCase) ||
            column.Name.Equals("Password", StringComparison.OrdinalIgnoreCase) ||
            column.Name.Equals("Website", StringComparison.OrdinalIgnoreCase) ||
            column.Name.Equals("Notes", StringComparison.OrdinalIgnoreCase));

        if (!needsMigration)
            return;

        await _database.ExecuteAsync(
            "ALTER TABLE PasswordEntry RENAME TO PasswordEntry_Legacy");

        await _database.CreateTableAsync<PasswordEntry>();

        await _database.ExecuteAsync(
            """
        INSERT INTO PasswordEntry (Id, Title, EncryptedData)
        SELECT Id, Title, EncryptedData
        FROM PasswordEntry_Legacy
        """);

        await _database.ExecuteAsync(
            "DROP TABLE PasswordEntry_Legacy");
    }

    private class TableColumnInfo
    {
        [Column("name")]
        public string Name { get; set; } = string.Empty;
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

    public async Task<VaultMetadata?> GetVaultMetadataAsync()
    {
        await _initializationTask;

        return await _database
            .Table<VaultMetadata>()
            .FirstOrDefaultAsync();
    }

    public async Task SaveVaultMetadataAsync(VaultMetadata metadata)
    {
        await _initializationTask;

        await _database.InsertAsync(metadata);
    }
}