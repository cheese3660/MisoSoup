namespace MisoSoup.Asm;

public enum MisaType
{
    Uint8,
    Uint16,
    Uint32,
    Int8,
    Int16,
    Int32,
    Float32,
    String,
    File
}

public static class MisaTypeExtensions
{
    public static string AsString(this MisaType type) =>
        type switch
        {
            MisaType.Uint8 => "u8t",
            MisaType.Uint16 => "u16t",
            MisaType.Uint32 => "u32t",
            MisaType.Int8 => "i8t",
            MisaType.Int16 => "i16t",
            MisaType.Int32 => "i32t",
            MisaType.Float32 => "f32t",
            MisaType.String => "string",
            MisaType.File => "file",
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
}