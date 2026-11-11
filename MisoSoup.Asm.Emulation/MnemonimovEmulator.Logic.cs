using System.Runtime.Intrinsics.X86;

namespace MisoSoup.Asm.Emulation;

public partial class MnemonimovEmulator
{
    public void Execute(Compare cmp)
    {
        ComparisonResult = cmp.Condition switch
        {
            Condition.Equal => GetOperandS(cmp.A) == GetOperandS(cmp.B),
            Condition.NotEqual => GetOperandS(cmp.A) != GetOperandS(cmp.B),
            Condition.LessThan => GetOperandS(cmp.A) < GetOperandS(cmp.B),
            Condition.LessThanOrEqual => GetOperandS(cmp.A) <= GetOperandS(cmp.B),
            Condition.GreaterThan => GetOperandS(cmp.A) > GetOperandS(cmp.B),
            Condition.GreaterThanOrEqual => GetOperandS(cmp.A) >= GetOperandS(cmp.B),
            Condition.LessThanUnsigned => GetOperandU(cmp.A) < GetOperandU(cmp.B),
            Condition.GreaterThanUnsigned => GetOperandU(cmp.A) > GetOperandU(cmp.B),
            Condition.LessThanEqualUnsigned => GetOperandU(cmp.A) <= GetOperandU(cmp.B),
            Condition.GreaterThanEqualUnsigned => GetOperandU(cmp.A) >= GetOperandU(cmp.B),
            Condition.FloatEqualApproximate => float.Abs(GetOperandF(cmp.A) - GetOperandF(cmp.B)) < Epsilon,
            Condition.FloatNotEqualApproximate => float.Abs(GetOperandF(cmp.A) - GetOperandF(cmp.B)) >= Epsilon,
            Condition.FloatLessThan => GetOperandF(cmp.A) < GetOperandF(cmp.B),
            Condition.FloatGreaterThan => GetOperandF(cmp.A) > GetOperandF(cmp.B),
            Condition.FloatNaN => float.IsNaN(GetOperandF(cmp.A)),
            Condition.FloatInf => float.IsInfinity(GetOperandF(cmp.A)),
            _ => throw new ArgumentOutOfRangeException()
        } ? 1 : 0;
    }

    public void Execute(Not not)
    {
        SetRegister(not.Destination, ~GetOperandS(not.Value));
    }

    public void Execute(And and)
    {
        SetRegister(and.Destination, GetOperandS(and.A) & GetOperandS(and.B));
    }

    public void Execute(Or orr)
    {
        SetRegister(orr.Destination, GetOperandS(orr.A) | GetOperandS(orr.B));
    }

    public void Execute(Xor xor)
    {
        SetRegister(xor.Destination, GetOperandS(xor.A) ^ GetOperandS(xor.B));
    }

    public void Execute(ArithmeticRightShift sar)
    {
        SetRegister(sar.Destination, GetOperandS(sar.Value) >> GetOperandS(sar.Shift));
    }

    public void Execute(LogicalLeftShift sll)
    {
        SetRegister(sll.Destination, GetOperandU(sll.Value) << GetOperandS(sll.Shift));
    }

    public void Execute(LogicalRightShift slr)
    {
        SetRegister(slr.Destination, GetOperandU(slr.Value) >> GetOperandS(slr.Shift));
    }

    public void Execute(RotateLeft rol)
    {
        SetRegister(rol.Destination, int.RotateLeft(GetOperandS(rol.Value), GetOperandS(rol.Shift)));
    }

    public void Execute(RotateRight ror)
    {
        SetRegister(ror.Destination, int.RotateRight(GetOperandS(ror.Value), GetOperandS(ror.Shift)));
    }
    
    private static uint ReverseBits(uint n)
    {
        
        // Swap adjacent pairs of 1-bit blocks
        n = ((n >> 1) & 0x55555555) | ((n & 0x55555555) << 1);
        // Swap adjacent pairs of 2-bit blocks
        n = ((n >> 2) & 0x33333333) | ((n & 0x33333333) << 2);
        // Swap adjacent pairs of 4-bit blocks
        n = ((n >> 4) & 0x0F0F0F0F) | ((n & 0x0F0F0F0F) << 4);
        // Swap adjacent pairs of 8-bit blocks
        n = ((n >> 8) & 0x00FF00FF) | ((n & 0x00FF00FF) << 8);
        // Swap adjacent pairs of 16-bit blocks
        return (n >> 16) | (n << 16);
    }

    public void Execute(ReverseBits rev)
    {
        SetRegister(rev.Destination, ReverseBits(GetOperandU(rev.Value)));
    }

    public void Execute(PopulationCount ppc)
    {
        SetRegister(ppc.Destination, int.PopCount(GetOperandS(ppc.Value)));
    }

    public void Execute(CountLeadingZeros clz)
    {
        SetRegister(clz.Destination, int.LeadingZeroCount(GetOperandS(clz.Value)));
    }

    public void Execute(CountTrailingZeros ctz)
    {
        SetRegister(ctz.Destination, int.TrailingZeroCount(GetOperandS(ctz.Value)));
    }

    public void Execute(SignedBitfieldExtract sbe)
    {
        var value = GetOperandS(sbe.Value);
        var offset = GetOperandS(sbe.Offset);
        var width = GetOperandS(sbe.Width);
        var mask = (int)(uint.MaxValue >> (32 - width));
        value >>= offset;
        value &= mask;
        if ((value & (1 << (width - 1))) != 0)
        {
            value |= ~mask;
        }

        SetRegister(sbe.Destination, value);
    }

    public void Execute(UnsignedBitfieldExtract ube)
    {
        var value = GetOperandU(ube.Value);
        var offset = GetOperandS(ube.Offset);
        var width = GetOperandS(ube.Width);

        var mask = uint.MaxValue >> (32 - width);
        value >>= offset;
        value &= mask;
        
        SetRegister(ube.Destination, value);
    }

    public void Execute(BitfieldInsert bfi)
    {
        var original = GetRegisterU(bfi.Destination);
        
        var value = GetOperandU(bfi.Value);
        var offset = GetOperandS(bfi.Offset);
        var width = GetOperandS(bfi.Width);

        var mask = uint.MaxValue >> (32 - width);
        
        value &= mask;
        value <<= offset;

        original &= ~(mask << offset);
        original |= value;
        
        SetRegister(bfi.Destination, original);
    }

    public void Execute(ParallelBitExtract pbx)
    {
        SetRegister(pbx.Destination, Bmi2.ParallelBitExtract(GetOperandU(pbx.Value), GetOperandU(pbx.Mask)));
    }

    public void Execute(ParallelBitDeposit pbd)
    {
        SetRegister(pbd.Destination, Bmi2.ParallelBitDeposit(GetOperandU(pbd.Value), GetOperandU(pbd.Mask)));
    }
}