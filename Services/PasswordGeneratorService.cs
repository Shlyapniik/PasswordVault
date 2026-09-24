using System.Security.Cryptography;
using System.Text;

namespace PasswordVault.Services;

public class PasswordGeneratorService
{
    private const string Lowercase =
        "abcdefghijklmnopqrstuvwxyz";

    private const string Uppercase =
        "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    private const string Digits =
        "0123456789";

    private const string Special =
        "!@#$%^&*()-_=+";

    public string Generate(
        int length,
        bool useLowercase = true,
        bool useUppercase = true,
        bool useDigits = true,
        bool useSpecial = true)
    {
        var characterGroups = new List<string>();

        if (useLowercase)
            characterGroups.Add(Lowercase);

        if (useUppercase)
            characterGroups.Add(Uppercase);

        if (useDigits)
            characterGroups.Add(Digits);

        if (useSpecial)
            characterGroups.Add(Special);

        if (characterGroups.Count == 0)
            throw new ArgumentException(
                "Не выбрана ни одна группа символов.");

        if (length < characterGroups.Count)
            throw new ArgumentException(
                "Длина пароля слишком мала.");

        var result = new List<char>(length);

        foreach (var group in characterGroups)
        {
            int index = RandomNumberGenerator.GetInt32(
                group.Length);

            result.Add(group[index]);
        }

        string allCharacters =
            string.Concat(characterGroups);

        while (result.Count < length)
        {
            int index = RandomNumberGenerator.GetInt32(
                allCharacters.Length);

            result.Add(allCharacters[index]);
        }

        Shuffle(result);

        return new string(result.ToArray());
    }

    private static void Shuffle(List<char> characters)
    {
        for (int i = characters.Count - 1; i > 0; i--)
        {
            int j = RandomNumberGenerator.GetInt32(i + 1);

            (characters[i], characters[j]) =
                (characters[j], characters[i]);
        }
    }
}