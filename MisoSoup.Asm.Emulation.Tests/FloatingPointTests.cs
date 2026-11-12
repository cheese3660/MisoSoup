namespace MisoSoup.Asm.Emulation.Tests;

public class FloatingPointTests
{
    [TestCase(0.0f)]
    [TestCase(-0.0f)]
    [TestCase(1.5f)]
    [TestCase(-1.5f)]
    [TestCase(1.9999999f)]
    [TestCase(-1.9999999f)]
    [TestCase(0.49999997f)]
    [TestCase(123456.79f)]
    [TestCase(16777217.0f)]
    [TestCase(1e9f)]
    [TestCase(-1e9f)]
    [TestCase(2147483520.0f)]
    [TestCase(-2147483648.0f)]
    [TestCase(float.Epsilon)]
    [TestCase(-float.Epsilon)]
    public void FloatToIntWorks(float f)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new FloatToInt(Register.a0, f));
        program.Emit(new Exit());

        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());

        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");

        Assert.That(emulator.GetOperandS(Register.a0), Is.EqualTo((int)f));
    }

    [TestCase(0)]
    [TestCase(1)]
    [TestCase(-1)]
    [TestCase(16777216)]
    [TestCase(16777217)]
    [TestCase(16777219)]
    [TestCase(-16777217)]
    [TestCase(33554433)]
    [TestCase(123456789)]
    [TestCase(2147483520)]
    [TestCase(2147483584)]
    [TestCase(int.MaxValue)]
    [TestCase(int.MinValue)]
    public void IntToFloatWorks(int i)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new IntToFloat(Register.a0, i));
        program.Emit(new Exit());

        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());

        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");

        Assert.That(emulator.GetOperandF(Register.a0), Is.EqualTo((float)i));
    }

    [TestCase(1.0f, 2.0f)]
    [TestCase(0.1f, 0.2f)]
    [TestCase(1.0f, -1.0f)]
    [TestCase(-0.0f, -0.0f)]
    [TestCase(0.0f, -0.0f)]
    [TestCase(float.MaxValue, float.MaxValue)]
    [TestCase(float.MaxValue, -float.MaxValue)]
    [TestCase(float.PositiveInfinity, float.NegativeInfinity)]
    [TestCase(float.PositiveInfinity, 1.0f)]
    [TestCase(float.NegativeInfinity, float.NegativeInfinity)]
    [TestCase(float.NaN, 1.0f)]
    [TestCase(1.0f, float.NaN)]
    [TestCase(1e20f, 1.0f)]
    [TestCase(16777216.0f, 1.0f)]
    [TestCase(16777216.0f, 2.0f)]
    [TestCase(float.Epsilon, float.Epsilon)]
    [TestCase(float.Epsilon, -float.Epsilon)]
    [TestCase(-1.5f, 0.5f)]
    public void FloatAddWorks(float a, float b)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new FloatAdd(Register.a0, a, b));
        program.Emit(new Exit());

        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());

        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");

        Assert.That(emulator.GetOperandF(Register.a0), Is.EqualTo(a + b));
    }

    [TestCase(1.0f, 2.0f)]
    [TestCase(0.3f, 0.1f)]
    [TestCase(1.0f, 1.0f)]
    [TestCase(0.0f, -0.0f)]
    [TestCase(-0.0f, 0.0f)]
    [TestCase(float.MaxValue, -float.MaxValue)]
    [TestCase(float.PositiveInfinity, float.PositiveInfinity)]
    [TestCase(float.NegativeInfinity, float.PositiveInfinity)]
    [TestCase(float.PositiveInfinity, float.NegativeInfinity)]
    [TestCase(float.NaN, float.NaN)]
    [TestCase(float.NaN, 1.0f)]
    [TestCase(1.0000001f, 1.0f)]
    [TestCase(float.Epsilon, float.Epsilon)]
    [TestCase(1e20f, 1.0f)]
    [TestCase(-5.0f, -5.0f)]
    public void FloatSubtractWorks(float a, float b)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new FloatSubtract(Register.a0, a, b));
        program.Emit(new Exit());

        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());

        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");

        Assert.That(emulator.GetOperandF(Register.a0), Is.EqualTo(a - b));
    }

    [TestCase(2.0f, 3.0f)]
    [TestCase(-2.0f, 3.0f)]
    [TestCase(0.1f, 10.0f)]
    [TestCase(1.5f, 1.5f)]
    [TestCase(float.MaxValue, 2.0f)]
    [TestCase(float.MaxValue, -2.0f)]
    [TestCase(float.PositiveInfinity, 0.0f)]
    [TestCase(float.NegativeInfinity, -1.0f)]
    [TestCase(0.0f, -1.0f)]
    [TestCase(-0.0f, -0.0f)]
    [TestCase(float.Epsilon, 0.5f)]
    [TestCase(float.Epsilon, 2.0f)]
    [TestCase(1e-20f, 1e-20f)]
    [TestCase(1e20f, 1e20f)]
    [TestCase(float.NaN, 0.0f)]
    [TestCase(float.PositiveInfinity, float.PositiveInfinity)]
    public void FloatMultiplyWorks(float a, float b)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new FloatMultiply(Register.a0, a, b));
        program.Emit(new Exit());

        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());

        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");

        Assert.That(emulator.GetOperandF(Register.a0), Is.EqualTo(a * b));
    }

    [TestCase(2.0f, 3.0f, 4.0f)]
    [TestCase(0.1f, 10.0f, -1.0f)]
    [TestCase(0.33333334f, 3.0f, -1.0f)]
    [TestCase(float.MaxValue, 2.0f, -float.MaxValue)]
    [TestCase(1e20f, 1e20f, float.NegativeInfinity)]
    [TestCase(1e20f, 1e20f, -1e38f)]
    [TestCase(16777216.0f, 1.0f, 1.0f)]
    [TestCase(float.PositiveInfinity, 0.0f, 1.0f)]
    [TestCase(0.0f, float.PositiveInfinity, 1.0f)]
    [TestCase(1.0f, 1.0f, float.NaN)]
    [TestCase(-1.0f, 0.0f, 0.0f)]
    [TestCase(-0.0f, 1.0f, -0.0f)]
    [TestCase(float.PositiveInfinity, 1.0f, float.NegativeInfinity)]
    [TestCase(float.Epsilon, 0.5f, 0.0f)]
    [TestCase(1.0000001f, 1.0000001f, -1.0000002f)]
    public void FloatFusedMultiplyAddWorks(float a, float b, float c)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new FloatFusedMultiplyAdd(Register.a0, a, b, c));
        program.Emit(new Exit());

        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());

        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");

        Assert.That(emulator.GetOperandF(Register.a0), Is.EqualTo(float.FusedMultiplyAdd(a, b, c)));
    }

    [TestCase(6.0f, 3.0f)]
    [TestCase(1.0f, 3.0f)]
    [TestCase(-7.0f, 2.0f)]
    [TestCase(1.0f, 0.0f)]
    [TestCase(-1.0f, 0.0f)]
    [TestCase(1.0f, -0.0f)]
    [TestCase(0.0f, 0.0f)]
    [TestCase(0.0f, 5.0f)]
    [TestCase(-0.0f, 5.0f)]
    [TestCase(float.PositiveInfinity, float.PositiveInfinity)]
    [TestCase(1.0f, float.PositiveInfinity)]
    [TestCase(-1.0f, float.PositiveInfinity)]
    [TestCase(float.PositiveInfinity, 2.0f)]
    [TestCase(float.MaxValue, 0.5f)]
    [TestCase(float.Epsilon, 2.0f)]
    [TestCase(1e-38f, 1e10f)]
    [TestCase(float.NaN, 1.0f)]
    [TestCase(1.0f, float.NaN)]
    public void FloatDivideWorks(float a, float b)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new FloatDivide(Register.a0, a, b));
        program.Emit(new Exit());

        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());

        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");

        Assert.That(emulator.GetOperandF(Register.a0), Is.EqualTo(a / b));
    }

    [TestCase(5.5f, 2.0f)]
    [TestCase(-5.5f, 2.0f)]
    [TestCase(5.5f, -2.0f)]
    [TestCase(-5.5f, -2.0f)]
    [TestCase(3.0f, 1.5f)]
    [TestCase(-3.0f, 1.5f)]
    [TestCase(0.3f, 0.1f)]
    [TestCase(0.0f, 1.0f)]
    [TestCase(-0.0f, 1.0f)]
    [TestCase(1.0f, 0.0f)]
    [TestCase(float.PositiveInfinity, 1.0f)]
    [TestCase(1.0f, float.PositiveInfinity)]
    [TestCase(-1.0f, float.PositiveInfinity)]
    [TestCase(16777216.0f, 3.0f)]
    [TestCase(1e10f, 3.0f)]
    [TestCase(float.NaN, 1.0f)]
    [TestCase(1.0f, float.NaN)]
    [TestCase(float.Epsilon, 1.0f)]
    public void FloatRemainderWorks(float a, float b)
    {
        var program = new Program();
        program.Mark("main");

        program.Emit(new FloatRemainder(Register.a0, a, b));
        program.Emit(new Exit());

        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());

        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");

        Assert.That(emulator.GetOperandF(Register.a0), Is.EqualTo(a % b));
    }

    [TestCase(5.5f, 2.0f)]
    [TestCase(-5.5f, 2.0f)]
    [TestCase(5.5f, -2.0f)]
    [TestCase(-5.5f, -2.0f)]
    [TestCase(3.0f, 1.5f)]
    [TestCase(-3.0f, 1.5f)]
    [TestCase(0.3f, 0.1f)]
    [TestCase(0.0f, 1.0f)]
    [TestCase(-1.0f, 3.0f)]
    [TestCase(1.0f, -3.0f)]
    [TestCase(1.0f, 0.0f)]
    [TestCase(float.PositiveInfinity, 1.0f)]
    [TestCase(1.0f, float.PositiveInfinity)]
    [TestCase(-1.0f, float.PositiveInfinity)]
    [TestCase(1e10f, 3.0f)]
    [TestCase(float.NaN, 1.0f)]
    [TestCase(1.0f, float.NaN)]
    [TestCase(-float.Epsilon, 1.0f)]
    public void FloatModuloWorks(float a, float b)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new FloatModulo(Register.a0, a, b));
        program.Emit(new Exit());

        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());

        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");

        var check = a % b;
        if ((check < 0 && b > 0) || (check > 0 && b < 0)) check += b;
        
        Assert.That(emulator.GetOperandF(Register.a0), Is.EqualTo(check));
    }

    [TestCase(2.0f, 3.0f)]
    [TestCase(2.0f, 0.5f)]
    [TestCase(4.0f, 0.5f)]
    [TestCase(2.0f, -1.0f)]
    [TestCase(-2.0f, 3.0f)]
    [TestCase(-2.0f, 2.0f)]
    [TestCase(-2.0f, 0.5f)]
    [TestCase(0.0f, 0.0f)]
    [TestCase(float.NaN, 0.0f)]
    [TestCase(1.0f, float.NaN)]
    [TestCase(1.0f, float.PositiveInfinity)]
    [TestCase(-1.0f, float.PositiveInfinity)]
    [TestCase(0.0f, -1.0f)]
    [TestCase(-0.0f, -1.0f)]
    [TestCase(-0.0f, 3.0f)]
    [TestCase(0.0f, 3.0f)]
    [TestCase(float.PositiveInfinity, -1.0f)]
    [TestCase(float.NegativeInfinity, 3.0f)]
    [TestCase(float.NegativeInfinity, 2.0f)]
    [TestCase(0.5f, float.PositiveInfinity)]
    [TestCase(2.0f, float.PositiveInfinity)]
    [TestCase(2.0f, float.NegativeInfinity)]
    [TestCase(2.0f, 128.0f)]
    [TestCase(10.0f, -45.0f)]
    [TestCase(10.0f, -46.0f)]
    [TestCase(float.NaN, 1.0f)]
    public void FloatPowerWorks(float a, float b)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new FloatPower(Register.a0, a, b));
        program.Emit(new Exit());

        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());

        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");

        Assert.That(emulator.GetOperandF(Register.a0), Is.EqualTo(float.Pow(a, b)));
    }

    [TestCase(0.0f)]
    [TestCase(-0.0f)]
    [TestCase(1.0f)]
    [TestCase(2.0f)]
    [TestCase(4.0f)]
    [TestCase(0.25f)]
    [TestCase(-1.0f)]
    [TestCase(-float.Epsilon)]
    [TestCase(float.PositiveInfinity)]
    [TestCase(float.NegativeInfinity)]
    [TestCase(float.NaN)]
    [TestCase(float.MaxValue)]
    [TestCase(float.Epsilon)]
    [TestCase(1e-38f)]
    [TestCase(16777216.0f)]
    public void FloatSquareRootWorks(float a)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new FloatSquareRoot(Register.a0, a));
        program.Emit(new Exit());

        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());

        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");

        Assert.That(emulator.GetOperandF(Register.a0), Is.EqualTo(float.Sqrt(a)));
    }

    [TestCase(1.0f)]
    [TestCase(4.0f)]
    [TestCase(0.25f)]
    [TestCase(2.0f)]
    [TestCase(0.0f)]
    [TestCase(-0.0f)]
    [TestCase(-1.0f)]
    [TestCase(float.PositiveInfinity)]
    [TestCase(float.NegativeInfinity)]
    [TestCase(float.NaN)]
    [TestCase(float.Epsilon)]
    [TestCase(float.MaxValue)]
    [TestCase(1e-38f)]
    public void FloatReciprocalSquareRootWorks(float a)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new FloatReciprocalSquareRoot(Register.a0, a));
        program.Emit(new Exit());

        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());

        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");

        Assert.That(emulator.GetOperandF(Register.a0), Is.EqualTo(1f / float.Sqrt(a)));
    }

    [TestCase(0.0f)]
    [TestCase(-0.0f)]
    [TestCase(1.0f)]
    [TestCase(-1.0f)]
    [TestCase(0.5f)]
    [TestCase(1e-10f)]
    [TestCase(88.72f)]
    [TestCase(88.73f)]
    [TestCase(89.0f)]
    [TestCase(-87.33f)]
    [TestCase(-103.0f)]
    [TestCase(-104.0f)]
    [TestCase(float.PositiveInfinity)]
    [TestCase(float.NegativeInfinity)]
    [TestCase(float.NaN)]
    public void FloatExponentialWorks(float a)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new FloatExponential(Register.a0, a));
        program.Emit(new Exit());

        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());

        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");

        Assert.That(emulator.GetOperandF(Register.a0), Is.EqualTo(float.Exp(a)));
    }

    [TestCase(1.0f)]
    [TestCase(float.E)]
    [TestCase(10.0f)]
    [TestCase(0.5f)]
    [TestCase(0.0f)]
    [TestCase(-0.0f)]
    [TestCase(-1.0f)]
    [TestCase(-float.Epsilon)]
    [TestCase(float.PositiveInfinity)]
    [TestCase(float.NegativeInfinity)]
    [TestCase(float.NaN)]
    [TestCase(float.Epsilon)]
    [TestCase(1e-38f)]
    [TestCase(float.MaxValue)]
    public void FloatNaturalLogarithmWorks(float a)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new FloatNaturalLog(Register.a0, a));
        program.Emit(new Exit());

        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());

        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");

        Assert.That(emulator.GetOperandF(Register.a0), Is.EqualTo(float.Log(a)));
    }

    [TestCase(0.0f)]
    [TestCase(-0.0f)]
    [TestCase(1.0f)]
    [TestCase(-1.0f)]
    [TestCase(1.5f)]
    [TestCase(float.PositiveInfinity)]
    [TestCase(float.NegativeInfinity)]
    [TestCase(float.NaN)]
    [TestCase(float.MaxValue)]
    [TestCase(-float.MaxValue)]
    [TestCase(float.Epsilon)]
    public void FloatNegateWorks(float a)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new FloatNegate(Register.a0, a));
        program.Emit(new Exit());

        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());

        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");

        Assert.That(emulator.GetOperandF(Register.a0), Is.EqualTo(-a));
    }

    [TestCase(0.0f)]
    [TestCase(-0.0f)]
    [TestCase(1.0f)]
    [TestCase(-1.0f)]
    [TestCase(-1.5f)]
    [TestCase(float.PositiveInfinity)]
    [TestCase(float.NegativeInfinity)]
    [TestCase(float.NaN)]
    [TestCase(float.MaxValue)]
    [TestCase(-float.MaxValue)]
    [TestCase(float.Epsilon)]
    [TestCase(-float.Epsilon)]
    public void FloatAbsWorks(float a)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new FloatAbs(Register.a0, a));
        program.Emit(new Exit());

        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());

        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");

        Assert.That(emulator.GetOperandF(Register.a0), Is.EqualTo(float.Abs(a)));
    }

    [TestCase(0.0f)]
    [TestCase(-0.0f)]
    [TestCase(1.0f)]
    [TestCase(-1.0f)]
    [TestCase(0.5f)]
    [TestCase(-0.5f)]
    [TestCase(123.0f)]
    [TestCase(float.PositiveInfinity)]
    [TestCase(float.NegativeInfinity)]
    [TestCase(float.MaxValue)]
    [TestCase(-float.MaxValue)]
    [TestCase(float.Epsilon)]
    [TestCase(-float.Epsilon)]
    public void FloatSignWorks(float a)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new FloatSign(Register.a0, a));
        program.Emit(new Exit());

        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());

        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");

        Assert.That(emulator.GetOperandF(Register.a0), Is.EqualTo(float.Sign(a)));
    }

    [TestCase(1.0f, 2.0f)]
    [TestCase(2.0f, 1.0f)]
    [TestCase(-1.0f, 1.0f)]
    [TestCase(1.0f, 1.0f)]
    [TestCase(0.0f, -0.0f)]
    [TestCase(-0.0f, 0.0f)]
    [TestCase(float.NaN, 1.0f)]
    [TestCase(1.0f, float.NaN)]
    [TestCase(float.NaN, float.NaN)]
    [TestCase(float.PositiveInfinity, float.NegativeInfinity)]
    [TestCase(float.NegativeInfinity, 1.0f)]
    [TestCase(float.MaxValue, float.PositiveInfinity)]
    [TestCase(float.Epsilon, 0.0f)]
    [TestCase(-float.Epsilon, 0.0f)]
    public void FloatMinimumWorks(float a, float b)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new FloatMinimum(Register.a0, a, b));
        program.Emit(new Exit());

        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());

        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");

        Assert.That(emulator.GetOperandF(Register.a0), Is.EqualTo(float.Min(a, b)));
    }

    [TestCase(1.0f, 2.0f)]
    [TestCase(2.0f, 1.0f)]
    [TestCase(-1.0f, 1.0f)]
    [TestCase(1.0f, 1.0f)]
    [TestCase(0.0f, -0.0f)]
    [TestCase(-0.0f, 0.0f)]
    [TestCase(float.NaN, 1.0f)]
    [TestCase(1.0f, float.NaN)]
    [TestCase(float.NaN, float.NaN)]
    [TestCase(float.PositiveInfinity, float.NegativeInfinity)]
    [TestCase(float.NegativeInfinity, 1.0f)]
    [TestCase(float.MaxValue, float.PositiveInfinity)]
    [TestCase(float.Epsilon, 0.0f)]
    [TestCase(-float.Epsilon, 0.0f)]
    public void FloatMaximumWorks(float a, float b)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new FloatMaximum(Register.a0, a, b));
        program.Emit(new Exit());

        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());

        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");

        Assert.That(emulator.GetOperandF(Register.a0), Is.EqualTo(float.Max(a, b)));
    }

    [TestCase(5.0f, 0.0f, 10.0f)]
    [TestCase(-5.0f, 0.0f, 10.0f)]
    [TestCase(15.0f, 0.0f, 10.0f)]
    [TestCase(0.0f, 0.0f, 10.0f)]
    [TestCase(10.0f, 0.0f, 10.0f)]
    [TestCase(-0.0f, 0.0f, 10.0f)]
    [TestCase(0.5f, 0.5f, 0.5f)]
    [TestCase(3.0f, 1.0f, 1.0f)]
    [TestCase(float.PositiveInfinity, 0.0f, 1.0f)]
    [TestCase(float.NegativeInfinity, 0.0f, 1.0f)]
    [TestCase(float.NaN, 0.0f, 1.0f)]
    [TestCase(1.0f, float.NegativeInfinity, float.PositiveInfinity)]
    [TestCase(float.MaxValue, -1.0f, 1.0f)]
    [TestCase(float.Epsilon, 0.0f, 1.0f)]
    [TestCase(-float.Epsilon, 0.0f, 1.0f)]
    public void FloatClampWorks(float v, float min, float max)
    {
        var program = new Program();
        program.Mark("main");

        program.Emit(new FloatClamp(Register.a0, v, min, max));
        program.Emit(new Exit());
        Console.WriteLine("--- EMULATED PROGRAM ---");

        Console.WriteLine(program.Dump());
        var emulator = new MnemonimovEmulator(0, program);

        emulator.RunProcess("main");
        Assert.That(emulator.GetOperandF(Register.a0), Is.EqualTo(float.Clamp(v, min, max)));
    }

    [TestCase(5.0f, 10.0f, 0.0f)]
    [TestCase(-5.0f, 10.0f, 0.0f)]
    [TestCase(15.0f, 10.0f, 0.0f)]
    [TestCase(0.0f, 10.0f, 0.0f)]
    [TestCase(10.0f, 10.0f, 0.0f)]
    [TestCase(0.5f, 1.0f, -1.0f)]
    [TestCase(-2.0f, 1.0f, -1.0f)]
    [TestCase(float.PositiveInfinity, 1.0f, 0.0f)]
    [TestCase(float.NegativeInfinity, 1.0f, 0.0f)]
    [TestCase(2.0f, float.PositiveInfinity, float.NegativeInfinity)]
    [TestCase(float.NaN, 1.0f, 0.0f)]
    public void FloatClampSwapWorks(float v, float max, float min)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new FloatClamp(Register.a0, v, max, min));
        program.Emit(new Exit());

        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());

        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");

        Assert.That(emulator.GetOperandF(Register.a0), Is.EqualTo(float.Clamp(v, min, max)));
    }

    [TestCase(5.0f, 0.0f, 10.0f)]
    [TestCase(15.0f, 0.0f, 10.0f)]
    [TestCase(-5.0f, 0.0f, 10.0f)]
    [TestCase(25.0f, 0.0f, 10.0f)]
    [TestCase(-25.0f, 0.0f, 10.0f)]
    [TestCase(0.0f, 0.0f, 10.0f)]
    [TestCase(10.0f, 0.0f, 10.0f)]
    [TestCase(370.0f, 0.0f, 360.0f)]
    [TestCase(-10.0f, 0.0f, 360.0f)]
    [TestCase(4.0f, -float.Pi, float.Pi)]
    [TestCase(-4.0f, -float.Pi, float.Pi)]
    [TestCase(5.0f, 10.0f, 0.0f)]
    [TestCase(15.0f, 10.0f, 0.0f)]
    [TestCase(2.5f, 1.0f, 2.0f)]
    [TestCase(1.0f, 2.0f, 2.0f)]
    [TestCase(1.0f, 2.0f, 2.00005f)]
    [TestCase(9.99995f, 0.0f, 10.0f)]
    [TestCase(-0.00001f, 0.0f, 10.0f)]
    [TestCase(9.9998f, 0.0f, 10.0f)]
    [TestCase(float.NaN, 0.0f, 1.0f)]
    [TestCase(float.PositiveInfinity, 0.0f, 1.0f)]
    public void FloatWrapWorks(float v, float min, float max)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new FloatWrap(Register.a0, v, min, max));
        program.Emit(new Exit());

        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());

        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");

        var range = max - min;
        var expected = float.Abs(range) < MnemonimovEmulator.Epsilon
            ? min
            : v - range * float.Floor((v - min) / range);
        if (float.Abs(expected - max) < MnemonimovEmulator.Epsilon) expected = min;

        Assert.That(emulator.GetOperandF(Register.a0), Is.EqualTo(expected));
    }

    [TestCase(0.0f, 10.0f, 0.5f)]
    [TestCase(0.0f, 10.0f, 0.0f)]
    [TestCase(0.0f, 10.0f, 1.0f)]
    [TestCase(0.0f, 10.0f, 2.0f)]
    [TestCase(0.0f, 10.0f, -1.0f)]
    [TestCase(10.0f, 0.0f, 0.25f)]
    [TestCase(-5.0f, 5.0f, 0.5f)]
    [TestCase(1.0f, 1.0f, 0.3f)]
    [TestCase(0.0f, float.PositiveInfinity, 0.5f)]
    [TestCase(0.0f, float.PositiveInfinity, 0.0f)]
    [TestCase(float.MaxValue, -float.MaxValue, 0.5f)]
    [TestCase(1e20f, 1.0f, 1.0f)]
    [TestCase(1e20f, 1.0f, 0.0f)]
    [TestCase(float.NaN, 1.0f, 0.0f)]
    [TestCase(0.0f, 1.0f, float.NaN)]
    [TestCase(0.1f, 0.7f, 0.3f)]
    public void FloatLerpWorks(float a, float b, float t)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new FloatLerp(Register.a0, a, b, t));
        program.Emit(new Exit());

        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());

        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");

        Assert.That(emulator.GetOperandF(Register.a0), Is.EqualTo(float.Lerp(a, b, t)));
    }

    [TestCase(0.0f)]
    [TestCase(-0.0f)]
    [TestCase(1.0f)]
    [TestCase(-1.0f)]
    [TestCase(1.5f)]
    [TestCase(-1.5f)]
    [TestCase(2.5f)]
    [TestCase(-2.5f)]
    [TestCase(0.5f)]
    [TestCase(-0.5f)]
    [TestCase(0.25f)]
    [TestCase(-0.25f)]
    [TestCase(1.4999999f)]
    [TestCase(0.49999997f)]
    [TestCase(-0.49999997f)]
    [TestCase(8388607.5f)]
    [TestCase(8388609.0f)]
    [TestCase(1e20f)]
    [TestCase(-1e20f)]
    [TestCase(float.PositiveInfinity)]
    [TestCase(float.NegativeInfinity)]
    [TestCase(float.NaN)]
    [TestCase(float.Epsilon)]
    [TestCase(-float.Epsilon)]
    public void FloatFloorWorks(float a)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new FloatFloor(Register.a0, a));
        program.Emit(new Exit());

        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());

        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");

        Assert.That(emulator.GetOperandF(Register.a0), Is.EqualTo(float.Floor(a)));
    }

    [TestCase(0.0f)]
    [TestCase(-0.0f)]
    [TestCase(1.0f)]
    [TestCase(-1.0f)]
    [TestCase(1.5f)]
    [TestCase(-1.5f)]
    [TestCase(2.5f)]
    [TestCase(-2.5f)]
    [TestCase(0.5f)]
    [TestCase(-0.5f)]
    [TestCase(0.25f)]
    [TestCase(-0.25f)]
    [TestCase(1.4999999f)]
    [TestCase(0.49999997f)]
    [TestCase(-0.49999997f)]
    [TestCase(8388607.5f)]
    [TestCase(8388609.0f)]
    [TestCase(1e20f)]
    [TestCase(-1e20f)]
    [TestCase(float.PositiveInfinity)]
    [TestCase(float.NegativeInfinity)]
    [TestCase(float.NaN)]
    [TestCase(float.Epsilon)]
    [TestCase(-float.Epsilon)]
    public void FloatCeilingWorks(float a)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new FloatCeiling(Register.a0, a));
        program.Emit(new Exit());

        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());

        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");

        Assert.That(emulator.GetOperandF(Register.a0), Is.EqualTo(float.Ceiling(a)));
    }

    [TestCase(0.0f)]
    [TestCase(-0.0f)]
    [TestCase(1.0f)]
    [TestCase(-1.0f)]
    [TestCase(1.5f)]
    [TestCase(-1.5f)]
    [TestCase(2.5f)]
    [TestCase(-2.5f)]
    [TestCase(0.5f)]
    [TestCase(-0.5f)]
    [TestCase(0.25f)]
    [TestCase(-0.25f)]
    [TestCase(1.4999999f)]
    [TestCase(0.49999997f)]
    [TestCase(-0.49999997f)]
    [TestCase(8388607.5f)]
    [TestCase(8388609.0f)]
    [TestCase(1e20f)]
    [TestCase(-1e20f)]
    [TestCase(float.PositiveInfinity)]
    [TestCase(float.NegativeInfinity)]
    [TestCase(float.NaN)]
    [TestCase(float.Epsilon)]
    [TestCase(-float.Epsilon)]
    [TestCase(3.5f)]
    [TestCase(-3.5f)]
    [TestCase(4.5f)]
    public void FloatRoundWorks(float a)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new FloatRound(Register.a0, a));
        program.Emit(new Exit());

        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());
        var emulator = new MnemonimovEmulator(0, program);

        emulator.RunProcess("main");
        Assert.That(emulator.GetOperandF(Register.a0), Is.EqualTo(float.Round(a, MidpointRounding.ToEven)));
    }

    [TestCase(0.0f)]
    [TestCase(-0.0f)]
    [TestCase(1.0f)]
    [TestCase(-1.0f)]
    [TestCase(1.5f)]
    [TestCase(-1.5f)]
    [TestCase(2.5f)]
    [TestCase(-2.5f)]
    [TestCase(0.5f)]
    [TestCase(-0.5f)]
    [TestCase(0.25f)]
    [TestCase(-0.25f)]
    [TestCase(1.4999999f)]
    [TestCase(0.49999997f)]
    [TestCase(-0.49999997f)]
    [TestCase(8388607.5f)]
    [TestCase(8388609.0f)]
    [TestCase(1e20f)]
    [TestCase(-1e20f)]
    [TestCase(float.PositiveInfinity)]
    [TestCase(float.NegativeInfinity)]
    [TestCase(float.NaN)]
    [TestCase(float.Epsilon)]
    [TestCase(-float.Epsilon)]
    public void FloatTruncateWorks(float a)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new FloatTruncate(Register.a0, a));
        program.Emit(new Exit());

        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());

        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");

        Assert.That(emulator.GetOperandF(Register.a0), Is.EqualTo(float.Truncate(a)));
    }

    [TestCase(0.0f)]
    [TestCase(-0.0f)]
    [TestCase(1.0f)]
    [TestCase(-1.0f)]
    [TestCase(1.5f)]
    [TestCase(-1.5f)]
    [TestCase(2.5f)]
    [TestCase(-2.5f)]
    [TestCase(0.5f)]
    [TestCase(-0.5f)]
    [TestCase(0.25f)]
    [TestCase(-0.25f)]
    [TestCase(1.4999999f)]
    [TestCase(0.49999997f)]
    [TestCase(-0.49999997f)]
    [TestCase(8388607.5f)]
    [TestCase(8388609.0f)]
    [TestCase(1e20f)]
    [TestCase(-1e20f)]
    [TestCase(float.PositiveInfinity)]
    [TestCase(float.NegativeInfinity)]
    [TestCase(float.NaN)]
    [TestCase(float.Epsilon)]
    [TestCase(-float.Epsilon)]
    [TestCase(-1.25f)]
    [TestCase(-0.75f)]
    [TestCase(1.25f)]
    public void FloatFractionalWorks(float a)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new FloatFractional(Register.a0, a));
        program.Emit(new Exit());

        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());

        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");

        Assert.That(emulator.GetOperandF(Register.a0), Is.EqualTo(a - float.Floor(a)));
    }

    [TestCase(0.0f)]
    [TestCase(-0.0f)]
    [TestCase(1.0f)]
    [TestCase(-1.0f)]
    [TestCase(0.5f)]
    [TestCase(float.Pi / 2)]
    [TestCase(-(float.Pi / 2))]
    [TestCase(float.Pi)]
    [TestCase(-float.Pi)]
    [TestCase(float.Tau)]
    [TestCase(100.0f)]
    [TestCase(1e10f)]
    [TestCase(float.Epsilon)]
    [TestCase(float.PositiveInfinity)]
    [TestCase(float.NegativeInfinity)]
    [TestCase(float.NaN)]
    public void FloatSineWorks(float a)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new FloatSine(Register.a0, a));
        program.Emit(new Exit());

        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());

        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");

        Assert.That(emulator.GetOperandF(Register.a0), Is.EqualTo(float.Sin(a)));
    }

    [TestCase(0.0f)]
    [TestCase(-0.0f)]
    [TestCase(1.0f)]
    [TestCase(-1.0f)]
    [TestCase(0.5f)]
    [TestCase(float.Pi / 2)]
    [TestCase(-(float.Pi / 2))]
    [TestCase(float.Pi)]
    [TestCase(-float.Pi)]
    [TestCase(float.Tau)]
    [TestCase(100.0f)]
    [TestCase(1e10f)]
    [TestCase(float.Epsilon)]
    [TestCase(float.PositiveInfinity)]
    [TestCase(float.NegativeInfinity)]
    [TestCase(float.NaN)]
    public void FloatCosineWorks(float a)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new FloatCosine(Register.a0, a));
        program.Emit(new Exit());

        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());

        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");

        Assert.That(emulator.GetOperandF(Register.a0), Is.EqualTo(float.Cos(a)));
    }

    [TestCase(0.0f)]
    [TestCase(-0.0f)]
    [TestCase(1.0f)]
    [TestCase(-1.0f)]
    [TestCase(0.5f)]
    [TestCase(float.Pi / 2)]
    [TestCase(-(float.Pi / 2))]
    [TestCase(float.Pi)]
    [TestCase(-float.Pi)]
    [TestCase(float.Tau)]
    [TestCase(100.0f)]
    [TestCase(1e10f)]
    [TestCase(float.Epsilon)]
    [TestCase(float.PositiveInfinity)]
    [TestCase(float.NegativeInfinity)]
    [TestCase(float.NaN)]
    public void FloatTangentWorks(float a)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new FloatTangent(Register.a0, a));
        program.Emit(new Exit());

        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());

        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");

        Assert.That(emulator.GetOperandF(Register.a0), Is.EqualTo(float.Tan(a)));
    }

    [TestCase(0.0f)]
    [TestCase(-0.0f)]
    [TestCase(1.0f)]
    [TestCase(-1.0f)]
    [TestCase(0.5f)]
    [TestCase(-0.5f)]
    [TestCase(0.99999994f)]
    [TestCase(1.0000001f)]
    [TestCase(-1.0000001f)]
    [TestCase(2.0f)]
    [TestCase(float.PositiveInfinity)]
    [TestCase(float.NegativeInfinity)]
    [TestCase(float.NaN)]
    [TestCase(float.Epsilon)]
    public void FloatArcsineWorks(float a)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new FloatArcsine(Register.a0, a));
        program.Emit(new Exit());

        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());

        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");

        Assert.That(emulator.GetOperandF(Register.a0), Is.EqualTo(float.Asin(a)));
    }

    [TestCase(0.0f)]
    [TestCase(-0.0f)]
    [TestCase(1.0f)]
    [TestCase(-1.0f)]
    [TestCase(0.5f)]
    [TestCase(-0.5f)]
    [TestCase(0.99999994f)]
    [TestCase(1.0000001f)]
    [TestCase(-1.0000001f)]
    [TestCase(2.0f)]
    [TestCase(float.PositiveInfinity)]
    [TestCase(float.NegativeInfinity)]
    [TestCase(float.NaN)]
    [TestCase(float.Epsilon)]
    public void FloatArccosineWorks(float a)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new FloatArccosine(Register.a0, a));
        program.Emit(new Exit());

        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());

        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");

        Assert.That(emulator.GetOperandF(Register.a0), Is.EqualTo(float.Acos(a)));
    }

    [TestCase(0.0f)]
    [TestCase(-0.0f)]
    [TestCase(1.0f)]
    [TestCase(-1.0f)]
    [TestCase(0.5f)]
    [TestCase(1e10f)]
    [TestCase(-1e10f)]
    [TestCase(float.PositiveInfinity)]
    [TestCase(float.NegativeInfinity)]
    [TestCase(float.NaN)]
    [TestCase(float.Epsilon)]
    [TestCase(float.MaxValue)]
    public void FloatArctangentWorks(float a)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new FloatArctangent(Register.a0, a));
        program.Emit(new Exit());

        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());

        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");

        Assert.That(emulator.GetOperandF(Register.a0), Is.EqualTo(float.Atan(a)));
    }

    [TestCase(0.0f, 1.0f)]
    [TestCase(0.0f, -1.0f)]
    [TestCase(-0.0f, -1.0f)]
    [TestCase(0.0f, 0.0f)]
    [TestCase(-0.0f, 0.0f)]
    [TestCase(0.0f, -0.0f)]
    [TestCase(-0.0f, -0.0f)]
    [TestCase(1.0f, 0.0f)]
    [TestCase(-1.0f, 0.0f)]
    [TestCase(1.0f, 1.0f)]
    [TestCase(-1.0f, -1.0f)]
    [TestCase(1.0f, -1.0f)]
    [TestCase(float.PositiveInfinity, float.PositiveInfinity)]
    [TestCase(float.PositiveInfinity, float.NegativeInfinity)]
    [TestCase(float.NegativeInfinity, float.NegativeInfinity)]
    [TestCase(float.NegativeInfinity, float.PositiveInfinity)]
    [TestCase(1.0f, float.PositiveInfinity)]
    [TestCase(1.0f, float.NegativeInfinity)]
    [TestCase(-1.0f, float.NegativeInfinity)]
    [TestCase(float.PositiveInfinity, 1.0f)]
    [TestCase(float.NaN, 1.0f)]
    [TestCase(1.0f, float.NaN)]
    [TestCase(float.Epsilon, float.MaxValue)]
    public void FloatAtan2Works(float a, float b)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new FloatAtan2(Register.a0, a, b));
        program.Emit(new Exit());

        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());

        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");

        Assert.That(emulator.GetOperandF(Register.a0), Is.EqualTo(float.Atan2(a, b)));
    }

    [TestCase(0.0f)]
    [TestCase(-0.0f)]
    [TestCase(float.Pi)]
    [TestCase(-float.Pi)]
    [TestCase(float.Pi / 2)]
    [TestCase(float.Tau)]
    [TestCase(1.0f)]
    [TestCase(float.PositiveInfinity)]
    [TestCase(float.NegativeInfinity)]
    [TestCase(float.NaN)]
    [TestCase(float.MaxValue)]
    [TestCase(float.Epsilon)]
    public void FloatRadiansToDegreesWorks(float a)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new FloatRadiansToDegrees(Register.a0, a));
        program.Emit(new Exit());

        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());

        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");

        Assert.That(emulator.GetRegisterF(Register.a0), Is.EqualTo(float.RadiansToDegrees(a)));
    }

    [TestCase(0.0f)]
    [TestCase(-0.0f)]
    [TestCase(180.0f)]
    [TestCase(-180.0f)]
    [TestCase(90.0f)]
    [TestCase(360.0f)]
    [TestCase(1.0f)]
    [TestCase(720.0f)]
    [TestCase(float.PositiveInfinity)]
    [TestCase(float.NegativeInfinity)]
    [TestCase(float.NaN)]
    [TestCase(float.MaxValue)]
    [TestCase(float.Epsilon)]
    public void FloatDegreesToRadiansWorks(float a)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new FloatDegreesToRadians(Register.a0, a));
        program.Emit(new Exit());

        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());

        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");

        Assert.That(emulator.GetRegisterF(Register.a0), Is.EqualTo(float.DegreesToRadians(a)));
    }
}