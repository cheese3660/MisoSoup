namespace MisoSoup.Sil;

public enum PrimitiveType : byte
{
    Void,
    Boolean,
    Unsigned8,
    Unsigned16,
    Unsigned32,
    Unsigned64,
    Signed8,
    Signed16,
    Signed32,
    Signed64,
    Float32,
    Float64,
    Pointer
}

public static class PrimitiveTypeExtensions
{
    public static string AsString(this PrimitiveType t) => t switch
    {
        PrimitiveType.Void => "void",
        PrimitiveType.Boolean => "bool",
        PrimitiveType.Unsigned8 => "u8",
        PrimitiveType.Unsigned16 => "u16",
        PrimitiveType.Unsigned32 => "u32",
        PrimitiveType.Unsigned64 => "u64",
        PrimitiveType.Signed8 => "i8",
        PrimitiveType.Signed16 => "i16",
        PrimitiveType.Signed32 => "i32",
        PrimitiveType.Signed64 => "i64",
        PrimitiveType.Float32 => "f32",
        PrimitiveType.Float64 => "f64",
        PrimitiveType.Pointer => "ptr",
        _ => throw new ArgumentOutOfRangeException(nameof(t), t, null)
    };
}