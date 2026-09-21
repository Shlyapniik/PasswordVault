namespace PasswordVault.Services;

public class PasswordGeneratorService
{
    private const string Lowercase = "abcdefghijklmnopqrstuvwxyz";
    private const string Uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string Digits = "0123456789";
    private const string Symbols = "!@#$%^&*()-_=+[]{}";

    public string Generate(
        int length = 16,
        bool useLowercase = true,
        bool useUppercase = true,
        bool useDigits = true,
        bool useSymbols = true)
    {
        string characters = string.Empty;

        if (useLowercase)
            characters += Lowercase;

        if (useUppercase)
            characters += Uppercase;

        if (useDigits)
            characters += Digits;

        if (useSymbols)
            characters += Symbols;

        if (string.IsNullOrEmpty(characters))
            throw new ArgumentException(
                "Должен быть выбран хотя бы один тип символов.");

        if (length < 4)
            throw new ArgumentException(
                "Длина пароля должна быть не менее 4 символов.");

        var random = new Random();

        return new string(
            Enumerable
                .Range(0, length)
                .Select(_ => characters[random.Next(characters.Length)])
                .ToArray());
    }
}