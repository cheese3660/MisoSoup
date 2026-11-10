namespace MisoSoup.Asm;

public abstract record Operand
{
    public static implicit operator Operand(string s) => new DefinedOperand(s);
    public static implicit operator Operand(int i) => new IntegerOperand(i);
    public static implicit operator Operand(float f) => new FloatOperand(f);
    public static implicit operator Operand(Register r) => new RegisterOperand(r);
    public static implicit operator Operand(RegisterVector v) => new VectorOperand(v);
}

public record RegisterOperand(Register Register) : Operand
{
    public override string ToString() => $"{Register}";
}

public record VectorOperand(RegisterVector Vector) : Operand
{
    public override string ToString() => $"{Vector}";
}

public record DefinedOperand(string Label) : Operand
{
    public override string ToString() => $"{Label}";
}

public record IntegerOperand(int Value) : Operand
{
    public override string ToString() => $"{Value}";
}

public record FloatOperand(float Value) : Operand
{
    public override string ToString() =>
        float.IsPositiveInfinity(Value) ? "INF" :
        float.IsNegativeInfinity(Value) ? "-INF" :
        float.IsNaN(Value) ? "NAN" : $"{Value}";
}

public record StringOperand(string Value) : Operand
{
    public override string ToString() => Value.MisaEscape();
}

public record FileOperand(string Value) : Operand
{
    public override string ToString() => Value.MisaEscape();
}