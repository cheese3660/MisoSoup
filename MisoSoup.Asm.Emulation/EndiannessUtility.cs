namespace MisoSoup.Asm.Emulation;

public static class EndiannessUtility
{
    public static byte[] GetBytes(int x)
    {
        var result = BitConverter.GetBytes(x);
        if (BitConverter.IsLittleEndian)
            Array.Reverse(result);
        return result;
    }

    public static byte[] GetBytes(short x)
    {
        var result = BitConverter.GetBytes(x);
        if (BitConverter.IsLittleEndian)
            Array.Reverse(result);
        return result;
    }

    public static byte[] GetBytes(float x)
    {
        var result = BitConverter.GetBytes(x);
        if (BitConverter.IsLittleEndian)
            Array.Reverse(result);
        return result;
    }

    public static int IntFromBytes(byte[] b) => BitConverter.ToInt32(BitConverter.IsLittleEndian ? [.. b.Reverse()] : b);
    public static short ShortFromBytes(byte[] b) => BitConverter.ToInt16(BitConverter.IsLittleEndian ? [.. b.Reverse()] : b);
    public static float FloatFromBytes(byte[] b) => BitConverter.ToSingle(BitConverter.IsLittleEndian ? [.. b.Reverse()] : b);
}