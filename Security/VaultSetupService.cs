using PasswordVault.Models;

namespace PasswordVault.Security;

public class VaultSetupService
{
    private const string VerificationText =
        "PasswordVault verification";

    private readonly KeyDerivationService _keyDerivationService;
    private readonly EncryptionService _encryptionService;

    public VaultSetupService(
        KeyDerivationService keyDerivationService,
        EncryptionService encryptionService)
    {
        _keyDerivationService = keyDerivationService;
        _encryptionService = encryptionService;
    }

    public VaultMetadata CreateMetadata(string masterPassword)
    {
        byte[] salt = _keyDerivationService.GenerateSalt();

        byte[] key = _keyDerivationService.DeriveKey(
            masterPassword,
            salt);

        byte[] verificationData =
            _encryptionService.Encrypt(
                VerificationText,
                key);

        return new VaultMetadata
        {
            Salt = salt,
            VerificationData = verificationData
        };
    }
}