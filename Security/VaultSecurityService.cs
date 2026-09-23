using PasswordVault.Models;
using System.Security.Cryptography;

namespace PasswordVault.Security;

public class VaultSecurityService
{
    private const string VerificationText =
        "PasswordVault verification";

    private readonly KeyDerivationService _keyDerivationService;
    private readonly EncryptionService _encryptionService;
    private readonly PasswordDataSerializer _serializer;

    private byte[]? _encryptionKey;

    public bool IsUnlocked => _encryptionKey != null;

    public VaultSecurityService(
        KeyDerivationService keyDerivationService,
        EncryptionService encryptionService,
        PasswordDataSerializer serializer)
    {
        _keyDerivationService = keyDerivationService;
        _encryptionService = encryptionService;
        _serializer = serializer;
    }

    public bool Unlock(
        string masterPassword,
        byte[] salt,
        byte[] verificationData)
    {
        byte[] key = _keyDerivationService.DeriveKey(
            masterPassword,
            salt);

        try
        {
            string verification =
                _encryptionService.Decrypt(
                    verificationData,
                    key);

            if (verification != VerificationText)
            {
                CryptographicOperations.ZeroMemory(key);
                return false;
            }

            _encryptionKey = key;

            return true;
        }
        catch (CryptographicException)
        {
            CryptographicOperations.ZeroMemory(key);
            return false;
        }
    }

    public void Lock()
    {
        if (_encryptionKey != null)
        {
            CryptographicOperations.ZeroMemory(
                _encryptionKey);

            _encryptionKey = null;
        }
    }

    public byte[] Encrypt(string plaintext)
    {
        if (_encryptionKey == null)
            throw new InvalidOperationException(
                "Хранилище заблокировано.");

        return _encryptionService.Encrypt(
            plaintext,
            _encryptionKey);
    }

    public string Decrypt(byte[] encryptedData)
    {
        if (_encryptionKey == null)
            throw new InvalidOperationException(
                "Хранилище заблокировано.");

        return _encryptionService.Decrypt(
            encryptedData,
            _encryptionKey);
    }

    public byte[] EncryptPasswordData(
        EncryptedPasswordData data)
    {
        string json = _serializer.Serialize(data);

        return Encrypt(json);
    }

    public EncryptedPasswordData DecryptPasswordData(
        byte[] encryptedData)
    {
        string json = Decrypt(encryptedData);

        return _serializer.Deserialize(json);
    }
}