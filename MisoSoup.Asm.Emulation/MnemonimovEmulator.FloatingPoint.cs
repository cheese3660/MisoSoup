namespace MisoSoup.Asm.Emulation;

public partial class MnemonimovEmulator
{
    public void Execute(FloatToInt fcti)
    {
        SetRegister(fcti.Destination, (int)GetOperandF(fcti.Value));
    }

    public void Execute(IntToFloat fctf)
    {
        SetRegister(fctf.Destination, (float)GetOperandS(fctf.Value));
    }

    public void Execute(FloatAdd fadd)
    {
        SetRegister(fadd.Destination, GetOperandF(fadd.A) + GetOperandF(fadd.B));
    }

    public void Execute(FloatSubtract fsub)
    {
        SetRegister(fsub.Destination, GetOperandF(fsub.A) - GetOperandF(fsub.B));
    }

    public void Execute(FloatMultiply fmul)
    {
        SetRegister(fmul.Destination, GetOperandF(fmul.A) * GetOperandF(fmul.B));
    }

    public void Execute(FloatFusedMultiplyAdd ffma)
    {
        SetRegister(ffma.Destination,
            float.FusedMultiplyAdd(GetOperandF(ffma.A), GetOperandF(ffma.B), GetOperandF(ffma.C)));
    }

    public void Execute(FloatDivide fdiv)
    {
        SetRegister(fdiv.Destination, GetOperandF(fdiv.Dividend) / GetOperandF(fdiv.Divisor));
    }

    public void Execute(FloatRemainder frem)
    {
        SetRegister(frem.Destination, GetOperandF(frem.Dividend) % GetOperandF(frem.Divisor));
    }

    public void Execute(FloatModulo fmod)
    {
        var dividend = GetOperandF(fmod.Dividend);
        var divisor = GetOperandF(fmod.Divisor);
        var result = dividend % divisor;
        if ((result < 0 && divisor > 0) || (result > 0 && divisor < 0)) result += divisor;
        SetRegister(fmod.Destination, result);
    }

    public void Execute(FloatPower fpow)
    {
        SetRegister(fpow.Destination, float.Pow(GetOperandF(fpow.Base), GetOperandF(fpow.Exponent)));
    }

    public void Execute(FloatSquareRoot fsqrt)
    {
        SetRegister(fsqrt.Destination, float.Sqrt(GetOperandF(fsqrt.Value)));
    }

    public void Execute(FloatReciprocalSquareRoot frsqrt)
    {
        SetRegister(frsqrt.Destination, 1 / float.Sqrt(GetOperandF(frsqrt.Value)));
    }
    
    public void Execute(FloatExponential fexp)
    {
        SetRegister(fexp.Destination, float.Exp(GetOperandF(fexp.Value)));
    }
    
    public void Execute(FloatNaturalLog flog)
    {
        SetRegister(flog.Destination, float.Log(GetOperandF(flog.Value)));
    }

    public void Execute(FloatNegate fneg)
    {
        SetRegister(fneg.Destination, -GetOperandF(fneg.Value));
    }

    public void Execute(FloatAbs fabs)
    {
        SetRegister(fabs.Destination, float.Abs(GetOperandF(fabs.Value)));
    }

    public void Execute(FloatSign fsgn)
    {
        SetRegister(fsgn.Destination, (float)float.Sign(GetOperandF(fsgn.Value)));
    }

    public void Execute(FloatMinimum fmin)
    {
        SetRegister(fmin.Destination, float.Min(GetOperandF(fmin.A), GetOperandF(fmin.B)));
    }

    public void Execute(FloatMaximum fmax)
    {
        SetRegister(fmax.Destination, float.Max(GetOperandF(fmax.A), GetOperandF(fmax.B)));
    }

    public void Execute(FloatClamp fclp)
    {
        var min = GetOperandF(fclp.Min);
        var max = GetOperandF(fclp.Max);
        if (max < min) (max, min) = (min, max);
        SetRegister(fclp.Destination, float.Clamp(GetOperandF(fclp.Value), min, max));
    }

    public void Execute(FloatWrap fwrap)
    {
        var v = GetOperandF(fwrap.Value);
        var min = GetOperandF(fwrap.Min);
        var max = GetOperandF(fwrap.Max);
        
        var range = max - min;
        var result = float.Abs(range) < Epsilon
            ? min
            : v - range * float.Floor((v - min) / range);
        if (float.Abs(result - max) < Epsilon) result = min;
        
        SetRegister(fwrap.Destination, result);
    }

    public void Execute(FloatLerp flrp)
    {
        SetRegister(flrp.Destination, float.Lerp(GetOperandF(flrp.A), GetOperandF(flrp.B), GetOperandF(flrp.T)));
    }

    public void Execute(FloatFloor fflo)
    {
        SetRegister(fflo.Destination, float.Floor(GetOperandF(fflo.Value)));
    }

    public void Execute(FloatCeiling fcei)
    {
        SetRegister(fcei.Destination, float.Ceiling(GetOperandF(fcei.Value)));
    }

    public void Execute(FloatRound frou)
    {
        SetRegister(frou.Destination, float.Round(GetOperandF(frou.Value), MidpointRounding.ToEven));
    }

    public void Execute(FloatTruncate ftru)
    {
        SetRegister(ftru.Destination, float.Truncate(GetOperandF(ftru.Value)));
    }

    public void Execute(FloatFractional ffra)
    {
        var a = GetOperandF(ffra.Value);
        SetRegister(ffra.Destination, a - float.Floor(a));
    }

    public void Execute(FloatSine fsin)
    {
        SetRegister(fsin.Destination, float.Sin(GetOperandF(fsin.Value)));
    }

    public void Execute(FloatCosine fcos)
    {
        SetRegister(fcos.Destination, float.Cos(GetOperandF(fcos.Value)));
    }

    public void Execute(FloatTangent ftan)
    {
        SetRegister(ftan.Destination, float.Tan(GetOperandF(ftan.Value)));
    }

    public void Execute(FloatArcsine fasin)
    {
        SetRegister(fasin.Destination, float.Asin(GetOperandF(fasin.Value)));
    }

    public void Execute(FloatArccosine facos)
    {
        SetRegister(facos.Destination, float.Acos(GetOperandF(facos.Value)));
    }

    public void Execute(FloatArctangent fatan)
    {
        SetRegister(fatan.Destination, float.Atan(GetOperandF(fatan.Value)));
    }

    public void Execute(FloatAtan2 fatan2)
    {
        SetRegister(fatan2.Destination, float.Atan2(GetOperandF(fatan2.Y), GetOperandF(fatan2.X)));
    }

    public void Execute(FloatRadiansToDegrees frtd)
    {
        SetRegister(frtd.Destination, float.RadiansToDegrees(GetOperandF(frtd.Value)));
    }

    public void Execute(FloatDegreesToRadians fdtr)
    {
        SetRegister(fdtr.Destination, float.DegreesToRadians(GetOperandF(fdtr.Value)));
    }
}