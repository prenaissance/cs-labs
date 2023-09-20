using System.Collections.ObjectModel;
using System.Text;

namespace CS.Lab2;

public class VigenereAlgorithm
{
    public const string DEFAULT_ALPHABET = "AĂÎBCDEFGHIJKLMNOPQRSȘTȚUVWXYZ";
    private readonly string alphabet;
    private readonly ReadOnlyDictionary<char, int> alphabetIndexes;
    private readonly ReadOnlyDictionary<int, char> alphabetChars;
    private readonly int[] keyArray;
    public VigenereAlgorithm(string encryptionKey, string dictionary = DEFAULT_ALPHABET)
    {
        this.alphabet = dictionary;
        var keyValues = dictionary.ToCharArray().Select((character, index) => (character, index));
        this.alphabetIndexes = new ReadOnlyDictionary<char, int>(
            keyValues.ToDictionary(x => x.character, x => x.index)
        );
        this.alphabetChars = new ReadOnlyDictionary<int, char>(
            keyValues.ToDictionary(x => x.index, x => x.character)
        );

        string normalizedKey = NormalizeText(encryptionKey);
        this.keyArray = normalizedKey
            .ToCharArray()
            .Distinct()
            .Select(c => alphabetIndexes[c])
            .ToArray();
    }

    public static string NormalizeText(string text) =>
        text.ToUpper().Replace(" ", "");

    public string Encrypt(string plainText) =>
        string.Join("",
            NormalizeText(plainText)
                .Select((c, index) =>
                {
                    int keyIndex = index % keyArray.Length;
                    int key = keyArray[keyIndex];
                    int plainTextIndex = alphabetIndexes[c];
                    int cipherTextIndex = (plainTextIndex + key) % alphabet.Length;
                    return alphabetChars[cipherTextIndex];
                })
        );

    public string Decrypt(string cipherText) =>
        string.Join("",
            NormalizeText(cipherText)
                .Select((c, index) =>
                {
                    int keyIndex = index % keyArray.Length;
                    int key = keyArray[keyIndex];
                    int cipherTextIndex = alphabetIndexes[c];
                    int plainTextIndex = (cipherTextIndex - key + alphabet.Length) % alphabet.Length;
                    return alphabetChars[plainTextIndex];
                })
        );
}