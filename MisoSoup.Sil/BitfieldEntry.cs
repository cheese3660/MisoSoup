namespace MisoSoup.Sil;

public enum BitfieldEntryType
{
    Flag,
    Padding,
    Unsigned,
    Signed
}

public record struct BitfieldEntry(BitfieldEntryType EntryType, byte Length)
{
    public override string ToString() => EntryType switch
    {
        BitfieldEntryType.Flag => "b",
        BitfieldEntryType.Padding => $"p{Length}",
        BitfieldEntryType.Unsigned => $"u{Length}",
        BitfieldEntryType.Signed => $"s{Length}",
        _ => throw new ArgumentOutOfRangeException()
    };
}