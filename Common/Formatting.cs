namespace CS.Common;

public static class Formatting
{
    public static string FormatToHex(byte[] bytes) => FormatToHex(bytes, bytes.Length * 2);
    public static string FormatToHex(byte[] bytes, int nibbles) => string.Join(
        "-",
        Convert
            .ToHexString(bytes)
            .ToCharArray()
            .Take(nibbles)
            .Select((character, index) => (character, index))
            .GroupBy(tuple => tuple.index / 2)
            .Select(group => new string(group.Select(tuple => tuple.character).ToArray()))
    );
}