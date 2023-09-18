using System.Text;

namespace CS.Lab1;

public class CaesarAlgorithm
{
    public const string DEFAULT_DICTIONARY = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private readonly string dictionary;
    public CaesarAlgorithm(string dictionary = DEFAULT_DICTIONARY)
    {
        this.dictionary = dictionary;
    }

    public static string NormalizeText(string text) =>
        text.ToUpper().Replace(" ", "");

    public static string GetCustomDictionary(string text)
    {
        StringBuilder dictionary = new();
        foreach (char c in NormalizeText(text) + DEFAULT_DICTIONARY)
        {
            if (!dictionary.ToString().Contains(c))
            {
                dictionary.Append(c);
            }
        }
        return dictionary.ToString();
    }

    public string Encrypt(string plainText, int key)
    {
        StringBuilder cipherText = new();
        foreach (char c in plainText)
        {
            int index = dictionary.IndexOf(c);
            if (index < 0)
            {
                throw new Exception($"Character {c} not found in dictionary");
            }
            else
            {
                int newIndex = (index + key) % dictionary.Length;
                cipherText.Append(dictionary[newIndex]);
            }
        }
        return cipherText.ToString();
    }

    public string Decrypt(string cipherText, int key)
    {
        StringBuilder plainText = new();
        foreach (char c in cipherText)
        {
            int index = dictionary.IndexOf(c);
            if (index < 0)
            {
                throw new Exception($"Character {c} not found in dictionary");
            }
            else
            {
                int newIndex = (index - key + dictionary.Length) % dictionary.Length;
                plainText.Append(dictionary[newIndex]);
            }
        }
        return plainText.ToString();
    }
}