namespace MisoSoup.Asm;

public static class Extensions
{
    public static string MisaEscape(this string str) =>
        $"\"{str.Replace("\0", "\\0").Replace("\t", "\\t").Replace("\n", "\\n").Replace("\'", "\\").Replace("\"", "\\\"").Replace("\\", @"\\")}\"";
}