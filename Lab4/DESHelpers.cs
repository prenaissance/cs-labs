using System.Text;

namespace CS.Lab4;

public static class DESHelpers
{
    private readonly static int[] doubleShiftIndices = new[]
    {
        1, 2, 9, 16
    };
    private static byte[] DESCircularLeftShift(byte[] bytes, int shifts = 0)
    {
        if (shifts == 0)
            return bytes;

        // 28 bits
        byte[] shiftedBytes = new byte[4];
        for (int i = 0; i < 3; i++)
        {
            byte b1 = bytes[i];
            byte b2 = bytes[i + 1];
            byte b1Part = (byte)(b1 << 1);
            byte b2Part = (byte)(b2 >> 7);
            shiftedBytes[i] = (byte)(b1Part | b2Part);
        }
        shiftedBytes[3] = (byte)((bytes[3] << 1 | bytes[0] >> 3) & 0xF0);
        return DESCircularLeftShift(shiftedBytes, shifts - 1);
    }
    //
    // Summary:
    //   strip every bit at the end of the byte to get a 7 byte sequence
    //   check via https://www.rapidtables.com/convert/number/ascii-to-binary.html
    //         and https://www.rapidtables.com/convert/number/hex-to-binary.html
    public static byte[] GetDESKey(string inputKey)
    {
        if (inputKey.Length != 8)
            throw new ArgumentException("The key must be 8 characters long");

        byte[] key = Encoding.ASCII
            .GetBytes(inputKey)
            .Select(b => (byte)(b & 0b11111110))
            .ToArray();
        byte[] desKey = new byte[7];
        for (int i = 0; i < 7; i++)
        {
            byte b1 = key[i];
            byte b2 = key[i + 1];
            byte b1Part = (byte)(b1 << i);
            byte b2Part = (byte)(b2 >> (7 - i));
            desKey[i] = (byte)(b1Part | b2Part);
        }
        return desKey;
    }

    public static (byte[] cKey, byte[] dKey) GetCAndDKeys(byte[] desKey, int round = 0)
    {
        if (desKey.Length != 7)
            throw new ArgumentException("The K+ key must be 7 bytes long");

        byte[] cKey = new byte[4];
        byte[] dKey = new byte[4];
        for (int i = 0; i < 3; i++)
        {
            cKey[i] = desKey[i];
        }
        cKey[3] = (byte)(desKey[3] & 0xF0);
        for (int i = 0; i < 3; i++)
        {
            byte b1 = desKey[i + 3];
            byte b2 = desKey[i + 4];
            byte b1Part = (byte)(b1 << 4);
            byte b2Part = (byte)(b2 >> 4);
            dKey[i] = (byte)(b1Part | b2Part);
        }
        dKey[3] = (byte)(desKey[6] << 4);

        int totalShifts = round + doubleShiftIndices.Count(i => i <= round);
        cKey = DESCircularLeftShift(cKey, totalShifts);
        dKey = DESCircularLeftShift(dKey, totalShifts);
        return (cKey, dKey);
    }
}