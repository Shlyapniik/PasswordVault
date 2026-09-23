namespace PasswordVault.Models;

public class VaultMetadata
{
    public byte[] Salt { get; set; } = Array.Empty<byte>();

    public byte[] VerificationData { get; set; } = Array.Empty<byte>();
}