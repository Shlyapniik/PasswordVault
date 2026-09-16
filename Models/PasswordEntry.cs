using SQLite;

namespace PasswordVault.Models;

public class PasswordEntry
{
    [PrimaryKey]
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Title { get; set; } = string.Empty;

    public string Username { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string Website { get; set; } = string.Empty;

    public string Notes { get; set; } = string.Empty;
}