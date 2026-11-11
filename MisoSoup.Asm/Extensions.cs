using System.Globalization;

namespace MisoSoup.Asm;

public static class Extensions
{
    public static string MisaEscape(this string str) =>
        $"\"{str.Replace("\0", "\\0").Replace("\t", "\\t").Replace("\n", "\\n").Replace("\'", "\\").Replace("\"", "\\\"").Replace("\\", @"\\")}\"";


    public static string MisaFloat(this float f)
    {
        if (float.IsPositiveInfinity(f)) return "INF";
        if (float.IsNegativeInfinity(f)) return "-INF";
        if (float.IsNaN(f)) return "NAN";
        var roundTrip = f.ToString("R", CultureInfo.InvariantCulture);
        var index = roundTrip.IndexOf('E');
        if (index == -1)
        {
            if (!roundTrip.Contains('.')) roundTrip += ".0";
            return roundTrip;
        }
        var count = int.Parse(roundTrip[(index + 1) ..], CultureInfo.InvariantCulture);
        var cur = roundTrip[.. index];
        var negative = cur[0] == '-';
        if (negative) cur = cur[1..];
        
        switch (count)
        {
            case < 0:
            {
                // We do a shift left here
                var dot = cur.IndexOf('.');
                cur = dot switch
                {
                    -1 => "0." + new string('0', -count - 1) + cur,
                    1 => "0." + new string('0', -count - 1) + cur[0] + cur[2..],
                    _ => cur
                };
                break;
            }
            case > 0:
            {
                // We do a shift right here
                var dot = cur.IndexOf('.');
                switch (dot)
                {
                    case -1:
                        cur = cur + new string('0', count);
                        break;
                    case 1:
                    {
                        var post = cur.Length - 2;
                        count -= post;
                        cur = count switch
                        {
                            < 0 => cur[0] + cur[2..^-count] + "." + cur[^-count..],
                            0 => cur[0] + cur[2..],
                            _ => cur[0] + cur[2..] + new string('0', count)
                        };
                        break;
                    }
                }

                break;
            }
        }
        
        if (!cur.Contains('.')) cur += ".0";
        if (negative) cur = "-" + cur;
        return cur;
    }
}