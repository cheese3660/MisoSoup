namespace MisoSoup.Asm;

// The labels part is important because 
public abstract record Instruction
{
    public abstract string Text { get; }
}

#region Directives

public record Embed(MisaType Type, params Operand[] Operands) : Instruction
{
    public override string Text => $"emb {Type.AsString()} {string.Join(',', Operands.Select(x => x.ToString()))}";
}

public record Reserve(MisaType Type, int Count) : Instruction
{
    public override string Text => $"res {Type.AsString()} {Count}";
}

#endregion

#region Control Flow

public record Nop() : Instruction
{
    public override string Text => "nop";
}

public record Call(Operand Operand) : Instruction
{
    public override string Text => $"cal {Operand}";
}

public record CallPa(Operand Operand) : Instruction
{
    public override string Text => $"cala {Operand}";
}

public record Return() : Instruction
{
    public override string Text => $"ret";
}

public record Jump(Operand Operand) : Instruction
{
    public override string Text => $"jmp {Operand}";
}

public record JumpPa(Operand Operand) : Instruction
{
    public override string Text => $"jmpa {Operand}";
}

public record JumpTrue(Operand Operand) : Instruction
{
    public override string Text => $"jtr {Operand}";
}

public record JumpTruePa(Operand Operand) : Instruction
{
    public override string Text => $"jtra {Operand}";
}

public record JumpFalse(Operand Operand) : Instruction
{
    public override string Text => $"jfs {Operand}";
}

public record JumpFalsePa(Operand Operand) : Instruction
{
    public override string Text => $"jfsa {Operand}";
}

#endregion

#region System

public record Syscall(Operand Operand) : Instruction
{
    public override string Text => $"syscall {Operand}";
}

public record Break() : Instruction
{
    public override string Text => $"break";
}

public record Yield() : Instruction
{
    public override string Text => $"yield";
}

public record Exit() : Instruction
{
    public override string Text => $"exit";
}

#endregion

#region Data Transfer

public record Load(MisaType Type, Register Destination, Operand Address) : Instruction
{
    public override string Text => $"lod {Type.AsString()}, {Destination}, {Address}";
}

public record Store(MisaType Type, Operand Address, Operand Value) : Instruction
{
    public override string Text => $"str {Type.AsString()}, {Address}, {Value}";
}

public record ComputeEffectiveAddress(Operand BaseAddress, Operand Index, Operand Scale)
    : Instruction
{
    public override string Text => $"cea {BaseAddress}, {Index}, {Scale}";
}

public record LoadEffective(MisaType Type, Register Dest, Operand Displacement) : Instruction
{
    public override string Text => $"lde {Type.AsString()}, {Dest}, {Displacement}";
}

public record StoreEffective(MisaType Type, Operand Displacement, Operand Value) : Instruction
{
    public override string Text => $"ste {Type.AsString()}, {Displacement}, {Value}";
}

public record ToProgramRelative(Register Dest, Operand Address) : Instruction
{
    public override string Text => $"tpr {Dest}, {Address}";
}

public record ToProgramAbsolute(Register Dest, Operand Address) : Instruction
{
    public override string Text => $"tpa {Dest}, {Address}";
}

public record Push(Operand Value) : Instruction
{
    public override string Text => $"psh {Value}";
}

public record Pop(Register Dest) : Instruction
{
    public override string Text => $"pop {Dest}";
}

public record Move(Register Destination, Operand Source) : Instruction
{
    public override string Text => $"mov {Destination}, {Source}";
}

public record ConditionalMove(Register Destination, Operand Source) : Instruction
{
    public override string Text => $"mvc {Destination}, {Source}";
}

public record Select(Register Destination, Operand IfTrue, Operand IfFalse) : Instruction
{
    public override string Text => $"select {Destination}, {IfTrue}, {IfFalse}";
}

public record Swap(Register A, Register B) : Instruction
{
    public override string Text => $"swap {A}, {B}";
}

#endregion

#region Arithmetic

public record Add(Register Destination, Operand A, Operand B) : Instruction
{
    public override string Text => $"add {Destination}, {A}, {B}";
}

public record Subtract(Register Destination, Operand A, Operand B) : Instruction
{
    public override string Text => $"sub {Destination}, {A}, {B}";
}

public record Increment(Register Destination) : Instruction
{
    public override string Text => $"inc {Destination}";
}

public record Decrement(Register Destination) : Instruction
{
    public override string Text => $"dec {Destination}";
}

public record Multiply(Register Destination, Operand A, Operand B) : Instruction
{
    public override string Text => $"mul {Destination}, {A}, {B}";
}

public record MultiplyHigh(Register Destination, Operand A, Operand B) : Instruction
{
    public override string Text => $"mlh {Destination}, {A}, {B}";
}

public record FusedMultiplyAdd(Register Destination, Operand A, Operand B, Operand C)
    : Instruction
{
    public override string Text => $"fma {Destination}, {A}, {B}, {C}";
}

public record Power(Register Destination, Operand Base, Operand Exponent) : Instruction
{
    public override string Text => $"pow {Destination}, {Base}, {Exponent}";
}

public record Divide(Register Destination, Operand Dividend, Operand Divisor) : Instruction
{
    public override string Text => $"div {Dividend}, {Divisor}";
}

public record Remainder(Register Destination, Operand Dividend, Operand Divisor) : Instruction
{
    public override string Text => $"rem {Dividend}, {Divisor}";
}

public record Modulo(Register Destination, Operand Dividend, Operand Divisor) : Instruction
{
    public override string Text => $"mod {Dividend}, {Divisor}";
}

public record Negate(Register Destination, Operand Value) : Instruction
{
    public override string Text => $"neg {Destination}, {Value}";
}

public record Absolute(Register Destination, Operand Value) : Instruction
{
    public override string Text => $"abs {Destination}, {Value}";
}

public record Sign(Register Destination, Operand Value) : Instruction
{
    public override string Text => $"sgn {Destination}, {Value}";
}

public record Minimum(Register Destination, Operand A, Operand B) : Instruction
{
    public override string Text => $"min {Destination}, {A}, {B}";
}

public record Maximum(Register Destination, Operand A, Operand B) : Instruction
{
    public override string Text => $"max {Destination}, {A}, {B}";
}

public record Clamp(Register Destination, Operand Value, Operand Min, Operand Max) : Instruction
{
    public override string Text => $"clamp {Destination}, {Value}, {Min}, {Max}";
}

public record Random(Register Destination, Operand Min, Operand Max) : Instruction
{
    public override string Text => $"random {Destination}, {Min}, {Max}";
}

public record MultiplyHighUnsigned(Register Destination, Operand A, Operand B) : Instruction
{
    public override string Text => $"mlhu {Destination}, {A}, {B}";
}

public record DivideUnsigned(Register Destination, Operand Dividend, Operand Divisor)
    : Instruction
{
    public override string Text => $"divu {Destination}, {Dividend}, {Divisor}";
}

public record RemainderUnsigned(Register Destination, Operand Value, Operand Remainder)
    : Instruction
{
    public override string Text => $"remu {Destination}, {Value}, {Remainder}";
}

public record MinimumUnsigned(Register Destination, Operand A, Operand B)
    : Instruction
{
    public override string Text => $"minu {Destination}, {A}, {B}";
}

public record MaximumUnsigned(Register Destination, Operand A, Operand B) : Instruction
{
    public override string Text => $"maxu {Destination}, {A}, {B}";
}

public record ClampUnsigned(Register Destination, Operand Value, Operand A, Operand B)
    : Instruction
{
    public override string Text => $"clpu {Destination}, {Value}, {A}, {B}";
}

#endregion

#region Logic & Bitwise

public record Compare(Condition Condition, Operand A, Operand B) : Instruction
{
    public override string Text => $"cmp {Condition.AsString()}, {A}, {B}";
}

public record Not(Register Destination, Operand Value) : Instruction
{
    public override string Text => $"not {Destination}, {Value}";
}

public record And(Register Destination, Operand A, Operand B) : Instruction
{
    public override string Text => $"and {Destination}, {A}, {B}";
}

public record Or(Register Destination, Operand A, Operand B) : Instruction
{
    public override string Text => $"orr {Destination}, {A}, {B}";
}

public record Xor(Register Destination, Operand A, Operand B) : Instruction
{
    public override string Text => $"xor {Destination}, {A}, {B}";
}

public record ArithmeticRightShift(Register Destination, Operand Value, Operand Shift)
    : Instruction
{
    public override string Text => $"sar {Destination}, {Value}, {Shift}";
}

public record LogicalLeftShift(Register Destination, Operand Value, Operand Shift) : Instruction
{
    public override string Text => $"sll {Destination}, {Value}, {Shift}";
}

public record LogicalRightShift(Register Destination, Operand Value, Operand Shift) : Instruction
{
    public override string Text => $"slr {Destination}, {Value}, {Shift}";
}

public record RotateLeft(Register Destination, Operand Value, Operand Shift) : Instruction
{
    public override string Text => $"rol {Destination}, {Value}, {Shift}";
}

public record RotateRight(Register Destination, Operand Value, Operand Shift) : Instruction
{
    public override string Text => $"ror {Destination}, {Value}, {Shift}";
}

public record ReverseBits(Register Destination, Operand Value) : Instruction
{
    public override string Text => $"rvb {Destination}, {Value}";
}

public record PopulationCount(Register Destination, Operand Value) : Instruction
{
    public override string Text => $"ppc {Destination}, {Value}";
}

public record CountLeadingZeros(Register Destination, Operand Value) : Instruction
{
    public override string Text => $"clz {Destination}, {Value}";
}

public record CountTrailingZeros(Register Destination, Operand Value) : Instruction
{
    public override string Text => $"ctz {Destination}, {Value}";
}

public record SignedBitfieldExtract(Register Destination, Operand Value, Operand Offset, Operand Width)
    : Instruction
{
    public override string Text => $"sbx {Destination}, {Value}, {Offset}, {Width}";
}

public record UnsignedBitfieldExtract(Register Destination, Operand Value, Operand Offset, Operand Width)
    : Instruction
{
    public override string Text => $"ubx {Destination}, {Value}, {Offset}, {Width}";
}

public record BitfieldInsert(Register Destination, Operand Value, Operand Offset, Operand Width)
    : Instruction
{
    public override string Text => $"bfi {Destination}, {Value}, {Offset}, {Width}";
}

public record ParallelBitExtract(Register Destination, Operand Value, Operand Mask) : Instruction
{
    public override string Text => $"pbx  {Destination}, {Value}, {Mask}";
}

public record ParallelBitDeposit(Register Destination, Operand Value, Operand Mask) : Instruction
{
    public override string Text => $"pbd  {Destination}, {Value}, {Mask}";
}

#endregion

#region Floating Point

public record FloatToInt(Register Destination, Operand Value) : Instruction
{
    public override string Text => $"fcti {Destination}, {Value}";
}

public record IntToFloat(Register Destination, Operand Value) : Instruction
{
    public override string Text => $"fctf {Destination}, {Value}";
}

public record FloatAdd(Register Destination, Operand A, Operand B) : Instruction
{
    public override string Text => $"fadd {Destination}, {A}, {B}";
}

public record FloatSubtract(Register Destination, Operand A, Operand B) : Instruction
{
    public override string Text => $"fsub {Destination}, {A}, {B}";
}

public record FloatMultiply(Register Destination, Operand A, Operand B) : Instruction
{
    public override string Text => $"fmul {Destination}, {A}, {B}";
}

public record FloatFusedMultiplyAdd(Register Destination, Operand A, Operand B, Operand C)
    : Instruction
{
    public override string Text => $"ffma {Destination}, {A}, {B}, {C}";
}

public record FloatDivide(Register Destination, Operand Dividend, Operand Divisor) : Instruction
{
    public override string Text => $"fdiv {Destination}, {Dividend}, {Divisor}";
}

public record FloatRemainder(Register Destination, Operand Dividend, Operand Divisor) : Instruction
{
    public override string Text => $"frem {Destination}, {Dividend}, {Divisor}";
}

public record FloatModulo(Register Destination, Operand Dividend, Operand Divisor) : Instruction
{
    public override string Text => $"fmod {Destination}, {Dividend}, {Divisor}";
}

public record FloatPower(Register Destination, Operand Base, Operand Exponent) : Instruction
{
    public override string Text => $"fpow {Destination}, {Base}, {Exponent}";
}

public record FloatSquareRoot(Register Destination, Operand Value) : Instruction
{
    public override string Text => $"fsqrt {Destination}, {Value}";
}

public record FloatReciprocalSquareRoot(Register Destination, Operand Value) : Instruction
{
    public override string Text => $"frsqrt {Destination}, {Value}";
}

public record FloatExponential(Register Destination, Operand Value) : Instruction
{
    public override string Text => $"fexp {Destination}, {Value}";
}

public record FloatNaturalLog(Register Destination, Operand Value) : Instruction
{
    public override string Text => $"flog {Destination}, {Value}";
}

public record FloatNegate(Register Destination, Operand Value) : Instruction
{
    public override string Text => $"fneg {Destination}, {Value}";
}

public record FloatAbs(Register Destination, Operand Value) : Instruction
{
    public override string Text => $"fabs {Destination}, {Value}";
}

public record FloatSign(Register Destination, Operand Value) : Instruction
{
    public override string Text => $"fsgn {Destination}, {Value}";
}

public record FloatMinimum(Register Destination, Operand A, Operand B) : Instruction
{
    public override string Text => $"fmin {Destination}, {A}, {B}";
}

public record FloatMaximum(Register Destination, Operand A, Operand B) : Instruction
{
    public override string Text => $"fmax {Destination}, {A}, {B}";
}

public record FloatClamp(Register Destination, Operand Value, Operand Min, Operand Max) : Instruction
{
    public override string Text => $"fclp {Destination}, {Value}, {Min}, {Max}";
}

public record FloatWrap(Register Destination, Operand Value, Operand Min, Operand Max)
    : Instruction
{
    public override string Text => $"fwrap {Destination}, {Value}, {Min}, {Max}";
}

public record FloatLerp(Register Destination, Operand A, Operand B, Operand T)
    : Instruction
{
    public override string Text => $"flrp {Destination}, {A}, {B}, {T}";
}

public record FloatRandom(Register Destination, Operand Min, Operand Max) : Instruction
{
    public override string Text => $"frnd {Destination}, {Min}, {Max}";
}

public record FloatFloor(Register Destination, Operand Value) : Instruction
{
    public override string Text => $"fflo {Destination}, {Value}";
}

public record FloatCeiling(Register Destination, Operand Value) : Instruction
{
    public override string Text => $"fcei {Destination}, {Value}";
}

public record FloatRound(Register Destination, Operand Value) : Instruction
{
    public override string Text => $"frou {Destination}, {Value}";
}

public record FloatTruncate(Register Destination, Operand Value) : Instruction
{
    public override string Text => $"ftru {Destination}, {Value}";
}

public record FloatFractional(Register Destination, Operand Value) : Instruction
{
    public override string Text => $"ffra {Destination}, {Value}";
}

public record FloatSine(Register Destination, Operand Value) : Instruction
{
    public override string Text => $"fsin {Destination}, {Value}";
}

public record FloatCosine(Register Destination, Operand Value) : Instruction
{
    public override string Text => $"fcos {Destination}, {Value}";
}

public record FloatTangent(Register Destination, Operand Value) : Instruction
{
    public override string Text => $"ftan {Destination}, {Value}";
}

public record FloatArcsine(Register Destination, Operand Value) : Instruction
{
    public override string Text => $"fasin {Destination}, {Value}";
}

public record FloatArccosine(Register Destination, Operand Value) : Instruction
{
    public override string Text => $"facos {Destination}, {Value}";
}

public record FloatArctangent(Register Destination, Operand Value) : Instruction
{
    public override string Text => $"fatan {Destination}, {Value}";
}

public record FloatAtan2(Register Destination, Operand Y, Operand X) : Instruction
{
    public override string Text => $"fatan2 {Destination}, {Y}, {X}";
}

public record FloatRadiansToDegrees(Register Destination, Operand Value) : Instruction
{
    public override string Text => $"frtd {Destination}, {Value}";
}

public record FloatDegreesToRadians(Register Destination, Operand Value) : Instruction
{
    public override string Text => $"fdtr {Destination}, {Value}";
}
#endregion

#region Drawing

public record GetBackBufferPixel(Register Destination, Operand X, Operand Y) : Instruction
{
    public override string Text => $"gbpx {Destination}, {X}, {Y}";
}

public record SetBackBufferPixel(Operand X, Operand Y, Operand Luma) : Instruction
{
    public override string Text => $"sbpx {X}, {Y}, {Luma}";
}

public record GetTexturePixel(Register Destination, Operand Address, Operand X, Operand Y)
    : Instruction
{
    public override string Text => $"gtpx {Destination}, {Address}, {X}, {Y}";
}

public record SetTexturePixel(Operand Address, Operand X, Operand Y, Operand Luma) : Instruction
{
    public override string Text => $"stpx {Address}, {X}, {Y}, {Luma}";
}

public record Normalize(Register Destination, Operand Value) : Instruction
{
    public override string Text => $"norm {Destination}, {Value}";
}

public record Denormalize(Register Destination, Operand Value) : Instruction
{
    public override string Text => $"dnrm {Destination}, {Value}";
}
#endregion

#region Vector

public record VectorPush(RegisterVector Range) : Instruction
{
    public override string Text => $"vpsh {Range.LongString()}";
}

public record VectorPop(RegisterVector Range) : Instruction
{
    public override string Text => $"vpop {Range.LongString()}";
}

public record VectorMove(RegisterVector Destination, Operand Source) : Instruction
{
    public override string Text => $"vmov {Destination.LongString()}, {Source}";
}

public record VectorFloatAdd(RegisterVector Destination, Operand A, Operand B) : Instruction
{
    public override string Text => $"vfadd {Destination.LongString()}, {A}, {B}";
}

public record VectorFloatSubtract(RegisterVector Destination, Operand A, Operand B) : Instruction
{
    public override string Text => $"vfsub {Destination.LongString()}, {A}, {B}";
}

public record VectorFloatMultiply(RegisterVector Destination, Operand A, Operand B) : Instruction
{
    public override string Text => $"vfmul {Destination.LongString()}, {A}, {B}";
}

public record VectorFloatFusedMultiplyAdd(RegisterVector Destination, Operand A, Operand B, Operand C)
    : Instruction
{
    public override string Text => $"vffma {Destination.LongString()}, {A}, {B}, {C}";
}

public record VectorFloatDivide(RegisterVector Destination, Operand A, Operand B) : Instruction
{
    public override string Text => $"vfdiv {Destination.LongString()}, {A}, {B}";
}

#endregion