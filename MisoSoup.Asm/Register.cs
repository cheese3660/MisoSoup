namespace MisoSoup.Asm;

public enum RegisterBank
{
    Temporary,
    Argument,
    Saved,
    Special,
}

public readonly record struct Register(RegisterBank Bank, byte Number)
{
    public override string ToString() => Bank switch
    {
        RegisterBank.Temporary => $"t{Number}",
        RegisterBank.Argument => $"a{Number}",
        RegisterBank.Saved => $"s{Number}",
        RegisterBank.Special => Number switch
        {
            0 => "zr",
            1 => "cr",
            2 => "ea",
            3 => "pa",
            4 => "ba",
            5 => "sp",
            6 => "fp",
            7 => "pc",
            _ => throw new ArgumentOutOfRangeException()
        },
        _ => throw new ArgumentOutOfRangeException()
    };

    public static Register t0 = new Register(RegisterBank.Temporary, 0);
    public static Register t1 = new Register(RegisterBank.Temporary, 1);
    public static Register t2 = new Register(RegisterBank.Temporary, 2);
    public static Register t3 = new Register(RegisterBank.Temporary, 3);
    public static Register t4 = new Register(RegisterBank.Temporary, 4);
    public static Register t5 = new Register(RegisterBank.Temporary, 5);
    public static Register t6 = new Register(RegisterBank.Temporary, 6);
    public static Register t7 = new Register(RegisterBank.Temporary, 7);
    public static Register t8 = new Register(RegisterBank.Temporary, 8);
    public static Register t9 = new Register(RegisterBank.Temporary, 9);
    public static Register t10 = new Register(RegisterBank.Temporary, 10);
    public static Register t11 = new Register(RegisterBank.Temporary, 11);
    public static Register t12 = new Register(RegisterBank.Temporary, 12);
    public static Register t13 = new Register(RegisterBank.Temporary, 13);
    public static Register t14 = new Register(RegisterBank.Temporary, 14);
    public static Register t15 = new Register(RegisterBank.Temporary, 15);
    
    public static Register a0 = new Register(RegisterBank.Argument, 0);
    public static Register a1 = new Register(RegisterBank.Argument, 1);
    public static Register a2 = new Register(RegisterBank.Argument, 2);
    public static Register a3 = new Register(RegisterBank.Argument, 3);
    public static Register a4 = new Register(RegisterBank.Argument, 4);
    public static Register a5 = new Register(RegisterBank.Argument, 5);
    public static Register a6 = new Register(RegisterBank.Argument, 6);
    public static Register a7 = new Register(RegisterBank.Argument, 7);
    public static Register a8 = new Register(RegisterBank.Argument, 8);
    public static Register a9 = new Register(RegisterBank.Argument, 9);
    public static Register a10 = new Register(RegisterBank.Argument, 10);
    public static Register a11 = new Register(RegisterBank.Argument, 11);
    public static Register a12 = new Register(RegisterBank.Argument, 12);
    public static Register a13 = new Register(RegisterBank.Argument, 13);
    public static Register a14 = new Register(RegisterBank.Argument, 14);
    public static Register a15 = new Register(RegisterBank.Argument, 15);
    
    public static Register s0 = new Register(RegisterBank.Saved, 0);
    public static Register s1 = new Register(RegisterBank.Saved, 1);
    public static Register s2 = new Register(RegisterBank.Saved, 2);
    public static Register s3 = new Register(RegisterBank.Saved, 3);
    public static Register s4 = new Register(RegisterBank.Saved, 4);
    public static Register s5 = new Register(RegisterBank.Saved, 5);
    public static Register s6 = new Register(RegisterBank.Saved, 6);
    public static Register s7 = new Register(RegisterBank.Saved, 7);
    public static Register s8 = new Register(RegisterBank.Saved, 8);
    public static Register s9 = new Register(RegisterBank.Saved, 9);
    public static Register s10 = new Register(RegisterBank.Saved, 10);
    public static Register s11 = new Register(RegisterBank.Saved, 11);
    public static Register s12 = new Register(RegisterBank.Saved, 12);
    public static Register s13 = new Register(RegisterBank.Saved, 13);
    public static Register s14 = new Register(RegisterBank.Saved, 14);
    public static Register s15 = new Register(RegisterBank.Saved, 15);
    public static Register s16 = new Register(RegisterBank.Saved, 16);
    public static Register s17 = new Register(RegisterBank.Saved, 17);
    public static Register s18 = new Register(RegisterBank.Saved, 18);
    public static Register s19 = new Register(RegisterBank.Saved, 19);
    public static Register s20 = new Register(RegisterBank.Saved, 20);
    public static Register s21 = new Register(RegisterBank.Saved, 21);
    public static Register s22 = new Register(RegisterBank.Saved, 22);
    public static Register s23 = new Register(RegisterBank.Saved, 23);
    public static Register s24 = new Register(RegisterBank.Saved, 24);
    public static Register s25 = new Register(RegisterBank.Saved, 25);
    public static Register s26 = new Register(RegisterBank.Saved, 26);
    public static Register s27 = new Register(RegisterBank.Saved, 27);
    public static Register s28 = new Register(RegisterBank.Saved, 28);
    public static Register s29 = new Register(RegisterBank.Saved, 29);
    public static Register s30 = new Register(RegisterBank.Saved, 30);
    public static Register s31 = new Register(RegisterBank.Saved, 31);

    public static Register zr = new Register(RegisterBank.Special, 0);
    public static Register cr = new Register(RegisterBank.Special, 1);
    public static Register ea = new Register(RegisterBank.Special, 2);
    public static Register pa = new Register(RegisterBank.Special, 3);
    public static Register ba = new Register(RegisterBank.Special, 4);
    public static Register sp = new Register(RegisterBank.Special, 5);
    public static Register fp = new Register(RegisterBank.Special, 6);
    public static Register pc = new Register(RegisterBank.Special, 7);

    public static IReadOnlyList<Register> AllNormal =
    [
        a0, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, t0, t1, t2, t3, t4, t5, t6, t7, t8, t9,
        t10, t11, t12, t13, t14, t15, s0, s1, s2, s3, s4, s5, s6, s7, s8, s9, s10, s11, s12, s13, s14, s15, s16, s17,
        s18, s19, s20, s21, s22, s23, s24, s25, s26, s27, s28, s29, s30, s31
    ];
}

public readonly record struct RegisterVector(RegisterBank Bank, byte Start, byte Count)
{
    public string LongString() => Bank switch
    {
        RegisterBank.Temporary => $"t{Start}..{Start + Count - 1}",
        RegisterBank.Argument => $"a{Start}..{Start + Count - 1}",
        RegisterBank.Saved => $"s{Start}..{Start + Count - 1}",
        _ => throw new ArgumentOutOfRangeException()
    };

    public override string ToString() => Bank switch
    {
        RegisterBank.Temporary => $"t{Start}..",
        RegisterBank.Argument => $"a{Start}..",
        RegisterBank.Saved => $"s{Start}..",
        _ => throw new ArgumentOutOfRangeException()
    };
}