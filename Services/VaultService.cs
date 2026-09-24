using PasswordVault.Data;
using PasswordVault.Models;
using PasswordVault.Security;
using System.Security.Cryptography;

namespace PasswordVault.Services;

public class VaultService
{
    private readonly DatabaseService _database;
    private readonly VaultSecurityService _vaultSecurity;

    public VaultService(
        DatabaseService database,
        VaultSecurityService vaultSecurity)
    {
        _database = database;
        _vaultSecurity = vaultSecurity;
    }

    public async Task<List<PasswordEntry>> GetEntriesAsync()
    {
        var entries = await _database.GetEntriesAsync();

        foreach (var entry in entries)
        {
            if (entry.EncryptedData.Length == 0)
                continue;

            try
            {
                var decryptedData =
                    _vaultSecurity.DecryptPasswordData(
                        entry.EncryptedData);

                entry.Username = decryptedData.Username;
                entry.Password = decryptedData.Password;
                entry.Website = decryptedData.Website;
                entry.Notes = decryptedData.Notes;
            }
            catch (CryptographicException)
            {
                entry.Username = string.Empty;
                entry.Password = string.Empty;
                entry.Website = string.Empty;
                entry.Notes =
                    "Не удалось расшифровать данные записи.";
            }
            catch (InvalidOperationException)
            {
                throw;
            }
        }

        return entries;
    }

    public async Task AddEntryAsync(PasswordEntry entry)
    {
        var encryptedData =
            new EncryptedPasswordData
            {
                Username = entry.Username,
                Password = entry.Password,
                Website = entry.Website,
                Notes = entry.Notes
            };

        entry.EncryptedData =
            _vaultSecurity.EncryptPasswordData(
                encryptedData);

        entry.Username = string.Empty;
        entry.Password = string.Empty;
        entry.Website = string.Empty;
        entry.Notes = string.Empty;

        await _database.AddEntryAsync(entry);
    }

    public async Task UpdateEntryAsync(PasswordEntry entry)
    {
        var encryptedData =
            new EncryptedPasswordData
            {
                Username = entry.Username,
                Password = entry.Password,
                Website = entry.Website,
                Notes = entry.Notes
            };

        entry.EncryptedData =
            _vaultSecurity.EncryptPasswordData(
                encryptedData);

        await _database.UpdateEntryAsync(entry);
    }

    public async Task DeleteEntryAsync(PasswordEntry entry)
    {
        await _database.DeleteEntryAsync(entry);
    }

    public async Task<VaultMetadata?> GetVaultMetadataAsync()
    {
        return await _database.GetVaultMetadataAsync();
    }

    public async Task SaveVaultMetadataAsync(
        VaultMetadata metadata)
    {
        await _database.SaveVaultMetadataAsync(metadata);
    }
}