namespace MisoSoup.Asm.Emulation;

public partial class MnemonimovEmulator
{
    public void Execute(Subtract sub)
    {
        SetRegister(sub.Destination, GetOperandS(sub.A) - GetOperandS(sub.B));
    }
    
    public void Execute(Add add)
    {
        SetRegister(add.Destination, GetOperandS(add.A) + GetOperandS(add.B));
    }

    public void Execute(Increment inc)
    {
        SetRegister(inc.Destination, GetRegisterS(inc.Destination) + 1);
    }

    public void Execute(Decrement dec)
    {
        SetRegister(dec.Destination, GetRegisterS(dec.Destination) - 1);
    }

    public void Execute(Multiply mul)
    {
        SetRegister(mul.Destination, GetOperandS(mul.A) * GetOperandS(mul.B));
    }

    public void Execute(MultiplyHigh mlh)
    {
        var res = (long)GetOperandS(mlh.A) * GetOperandS(mlh.B);
        var s = (int)(res >> 32);
        SetRegister(mlh.Destination, s);
    }

    public void Execute(FusedMultiplyAdd fma)
    {
        SetRegister(fma.Destination, GetOperandS(fma.A) * GetOperandS(fma.B) + GetOperandS(fma.C));
    }

    // The game runs on .NET 8 x64, where an out-of-range or NaN double -> int conversion yields int.MinValue.
    // .NET 9+ saturates instead, so the range check is done explicitly.
    private static int IntPow(int b, int e)
    {
        var result = Math.Round(Math.Pow(b, e));
        return result >= int.MinValue && result <= int.MaxValue ? (int)result : int.MinValue;
    }

    public void Execute(Power pow)
    {
        SetRegister(pow.Destination, IntPow(GetOperandS(pow.Base), GetOperandS(pow.Exponent)));
    }

    public void Execute(Divide div)
    {
        SetRegister(div.Destination, GetOperandS(div.Dividend) / GetOperandS(div.Divisor));
    }

    public void Execute(Remainder rem)
    {
        SetRegister(rem.Destination, GetOperandS(rem.Dividend) % GetOperandS(rem.Divisor));
    }
    
    private static int Mod(int dividend, int divisor)
    {
        var remainder = dividend % divisor;

        if ((remainder < 0 && divisor > 0) || (remainder > 0 && divisor < 0))
        {
            remainder += divisor;
        }
        
        return remainder;
    }

    public void Execute(Modulo mod)
    {
        SetRegister(mod.Destination, Mod(GetOperandS(mod.Dividend), GetOperandS(mod.Divisor)));
    }

    public void Execute(Negate neg)
    {
        SetRegister(neg.Destination, -GetOperandS(neg.Value));
    }

    public void Execute(Absolute abs)
    {
        SetRegister(abs.Destination, int.Abs(GetOperandS(abs.Value)));
    }

    public void Execute(Sign sgn)
    {
        SetRegister(sgn.Destination, int.Sign(GetOperandS(sgn.Value)));
    }

    public void Execute(Minimum min)
    {
        SetRegister(min.Destination, int.Min(GetOperandS(min.A), GetOperandS(min.B)));
    }

    public void Execute(Maximum max)
    {
        SetRegister(max.Destination, int.Max(GetOperandS(max.A), GetOperandS(max.B)));
    }

    public void Execute(Clamp clp)
    {
        var max = GetOperandS(clp.Max);
        var min = GetOperandS(clp.Min);
        if (max < min)
        {
            (max, min) = (min, max);
        }
        SetRegister(clp.Destination, int.Clamp(GetOperandS(clp.Value), min, max));
    }

    public void Execute(MultiplyHighUnsigned mlhu)
    {
        var high = (ulong)GetOperandU(mlhu.A) * GetOperandU(mlhu.B);
        var low = (uint)(high >> 32);
        SetRegister(mlhu.Destination, low);
    }

    public void Execute(DivideUnsigned divu)
    {
        SetRegister(divu.Destination, GetOperandU(divu.Dividend) / GetOperandU(divu.Divisor));
    }

    public void Execute(RemainderUnsigned remu)
    {
        SetRegister(remu.Destination, GetOperandU(remu.Dividend) % GetOperandU(remu.Divisor));
    }

    public void Execute(MinimumUnsigned minu)
    {
        SetRegister(minu.Destination, uint.Min(GetOperandU(minu.A), GetOperandU(minu.B)));
    }

    public void Execute(MaximumUnsigned maxu)
    {
        SetRegister(maxu.Destination, uint.Max(GetOperandU(maxu.A), GetOperandU(maxu.B)));
    }

    public void Execute(ClampUnsigned clpu)
    {
        var max = GetOperandU(clpu.Max);
        var min = GetOperandU(clpu.Min);
        if (min > max)
        {
            (min, max) = (max, min);
        }
        SetRegister(clpu.Destination, uint.Clamp(GetOperandU(clpu.Value), min, max));
    }
}