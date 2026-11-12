using System.Text;
using MisoSoup.Common;

namespace MisoSoup.Sil;

public abstract record SilType;

public record SilPrimitive(PrimitiveType Type) : SilType
{
    public override string ToString() => Type.AsString();
}

public record SilVector(PrimitiveType BaseType, byte Count) : SilType
{
    public override string ToString() => $"<{BaseType.AsString()}:{Count}>";
}

public record SilArray(SilType BaseType, int Length) : SilType
{
    public override string ToString() => $"[{BaseType}:{Length}]";
}

public record SilBitfield(PrimitiveType BaseType, params RecordArray<BitfieldEntryType> Entries) : SilType
{
    public override string ToString() =>
        $"bitfield {{{BaseType.AsString()}:{string.Join(',', Entries.Select(x => x.ToString()))}}}";
}


public record SilStructure(StructureFlags Flags, params RecordArray<SilType> Fields) : SilType
{
    public override string ToString()
    {
        StringBuilder sb = new();
        sb.Append("struct ");
        if (Flags.HasFlag(StructureFlags.Packed)) sb.Append(".packed ");
        sb.Append('{');
        sb.Append(string.Join(',', Fields.Select(x => x.ToString())));
        sb.Append('}');
        return sb.ToString();
    }
}