using System.Security.Cryptography;
using System.Text;

namespace PasswordVault.Security;

public class EncryptionService
{
    private const int NonceSize = 12;
    private const int TagSize = 16;

    public byte[] Encrypt(string plaintext, byte[] key)
    {
        byte[] plaintextBytes = Encoding.UTF8.GetBytes(plaintext);

        byte[] nonce = RandomNumberGenerator.GetBytes(NonceSize);
        byte[] ciphertext = new byte[plaintextBytes.Length];
        byte[] tag = new byte[TagSize];

        using var aes = new AesGcm(key, TagSize);

        aes.Encrypt(
            nonce,
            plaintextBytes,
            ciphertext,
            tag);

        byte[] result = new byte[
            nonce.Length +
            tag.Length +
            ciphertext.Length];

        Buffer.BlockCopy(
            nonce,
            0,
            result,
            0,
            nonce.Length);

        Buffer.BlockCopy(
            tag,
            0,
            result,
            nonce.Length,
            tag.Length);

        Buffer.BlockCopy(
            ciphertext,
            0,
            result,
            nonce.Length + tag.Length,
            ciphertext.Length);

        return result;
    }

    public string Decrypt(byte[] encryptedData, byte[] key)
    {
        byte[] nonce = encryptedData[..NonceSize];
        byte[] tag = encryptedData[
            NonceSize..(NonceSize + TagSize)];

        byte[] ciphertext = encryptedData[
            (NonceSize + TagSize)..];

        byte[] plaintext = new byte[ciphertext.Length];

        using var aes = new AesGcm(key, TagSize);

        aes.Decrypt(
            nonce,
            ciphertext,
            tag,
            plaintext);

        return Encoding.UTF8.GetString(plaintext);
    }
}