using System.Text.Json;
using PasswordVault.Models;

namespace PasswordVault.Security;

public class PasswordDataSerializer
{
    public string Serialize(EncryptedPasswordData data)
    {
        return JsonSerializer.Serialize(data);
    }

    public EncryptedPasswordData Deserialize(string json)
    {
        return JsonSerializer.Deserialize<EncryptedPasswordData>(json)
            ?? throw new InvalidOperationException(
                "Не удалось расшифровать данные записи.");
    }
}