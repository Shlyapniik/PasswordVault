using SQLite;

namespace PasswordVault.Models;

public class PasswordEntry
{
    [PrimaryKey]
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public byte[] EncryptedData { get; set; } = Array.Empty<byte>();

    [Ignore]
    public string Username { get; set; } = string.Empty;
    [Ignore]
    public string Password { get; set; } = string.Empty;
    [Ignore]
    public string Website { get; set; } = string.Empty;
    [Ignore]
    public string Notes { get; set; } = string.Empty;
}