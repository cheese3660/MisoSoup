namespace MisoSoup.Asm.Emulation.Tests;

public class ArithmeticTests
{
    [Test]
    [TestCase(-6322149, -297027835)]
    [TestCase(-760812918, 2990628)]
    [TestCase(399385637, 686794270)]
    [TestCase(519203567, -746582957)]
    [TestCase(703719337, -469503547)]
    [TestCase(-450941043, -572031037)]
    [TestCase(-28407453, 394850071)]
    [TestCase(176279888, -869569106)]
    [TestCase(-924135310, 133430107)]
    [TestCase(-478307884, 141619858)]
    [TestCase(-368028624, 639594266)]
    [TestCase(-207435833, -988316658)]
    [TestCase(267945641, 179041383)]
    [TestCase(-238590805, 551018501)]
    [TestCase(-475971829, -122431872)]
    [TestCase(-230418222, 893319858)]
    [TestCase(int.MinValue, 1)]
    [TestCase(int.MaxValue, -1)]
    [TestCase(0, int.MinValue)]
    [TestCase(int.MinValue, int.MinValue)]
    [TestCase(-1, int.MaxValue)]
    [TestCase(0, 0)]
    public void SubtractWorks(int a, int b)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new Subtract(Register.a0, a, b));
        program.Emit(new Exit());

        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());

        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");

        Assert.That(emulator.GetRegisterS(Register.a0), Is.EqualTo(a - b));
    }

    [Test]
    [TestCase(318018638, 190681760)]
    [TestCase(221870822, 798255255)]
    [TestCase(891439123, 579618860)]
    [TestCase(-270114838, -178598717)]
    [TestCase(-631003704, -92363269)]
    [TestCase(-84682138, -1060592392)]
    [TestCase(1050760247, 578676225)]
    [TestCase(-278941488, 434701638)]
    [TestCase(-336260891, -234089964)]
    [TestCase(-1014904321, -942924603)]
    [TestCase(-411847060, 80409878)]
    [TestCase(-5209129, -335030531)]
    [TestCase(200097069, -15511623)]
    [TestCase(-944437417, -536187528)]
    [TestCase(-176554258, 988309128)]
    [TestCase(-180651937, -847743435)]
    [TestCase(int.MaxValue, 1)]
    [TestCase(int.MinValue, -1)]
    [TestCase(int.MinValue, int.MinValue)]
    [TestCase(int.MaxValue, int.MaxValue)]
    [TestCase(int.MaxValue, int.MinValue)]
    [TestCase(0, 0)]
    public void AddWorks(int a, int b)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new Add(Register.a0, a, b));
        program.Emit(new Exit());

        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());

        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");

        Assert.That(emulator.GetRegisterS(Register.a0), Is.EqualTo(a + b));
    }

    [Test]
    [TestCase(235532124, 5)]
    [TestCase(1535325, 16)]
    [TestCase(5, 3)]
    [TestCase(5203324, 32)]
    [TestCase(int.MaxValue, 1)]
    [TestCase(-1, 1)]
    [TestCase(-3, 5)]
    [TestCase(int.MaxValue - 1, 2)]
    [TestCase(0, 0)]
    public void IncrementWorks(int incrementBase, int times)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new Move(Register.a0, incrementBase));
        for (var i = 0; i < times; i++) program.Emit(new Increment(Register.a0));
        program.Emit(new Exit());

        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());

        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");

        Assert.That(emulator.GetRegisterS(Register.a0), Is.EqualTo(incrementBase + times));
    }

    [Test]
    [TestCase(235532124, 5)]
    [TestCase(1535325, 16)]
    [TestCase(5, 3)]
    [TestCase(5203324, 32)]
    [TestCase(int.MinValue, 1)]
    [TestCase(0, 1)]
    [TestCase(2, 5)]
    [TestCase(int.MinValue + 1, 2)]
    [TestCase(0, 0)]
    public void DecrementWorks(int decrementBase, int times)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new Move(Register.a0, decrementBase));
        for (var i = 0; i < times; i++) program.Emit(new Decrement(Register.a0));
        program.Emit(new Exit());

        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());

        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");

        Assert.That(emulator.GetRegisterS(Register.a0), Is.EqualTo(decrementBase - times));
    }

    [Test]
    [TestCase(16, 5)]
    [TestCase(128, 523)]
    [TestCase(32000, 64000)]
    [TestCase(-32000, 64000)]
    [TestCase(-32000, -64000)]
    [TestCase(65536, 65536)]
    [TestCase(int.MaxValue, 2)]
    [TestCase(int.MinValue, -1)]
    [TestCase(int.MinValue, 2)]
    [TestCase(-1, -1)]
    [TestCase(0, int.MinValue)]
    [TestCase(46341, 46341)]
    public void MultiplyWorks(int a, int b)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new Multiply(Register.a0, a, b));
        program.Emit(new Exit());

        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());

        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");

        Assert.That(emulator.GetRegisterS(Register.a0), Is.EqualTo(a * b));
    }

    [Test]
    [TestCase(65536, 65536)]
    [TestCase(-65536, 65536)]
    [TestCase(2_000_000_000, 5)]
    [TestCase(-2_000_000_000, 5)]
    [TestCase(-1, 1)]
    [TestCase(-1, -1)]
    [TestCase(1, int.MinValue)]
    [TestCase(int.MinValue, int.MinValue)]
    [TestCase(int.MinValue, int.MaxValue)]
    [TestCase(int.MaxValue, int.MaxValue)]
    [TestCase(0, int.MinValue)]
    [TestCase(5, 3)]
    public void MultiplyHighWorks(int a, int b)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new MultiplyHigh(Register.a0, a, b));
        program.Emit(new Exit());

        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());

        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");

        Assert.That(emulator.GetRegisterS(Register.a0), Is.EqualTo(((long)a * b) >> 32));
    }

    [Test]
    [TestCase(5, 10, 30)]
    [TestCase(-5, 10, 30)]
    [TestCase(5, -10, 30)]
    [TestCase(-5, -10, 30 )]
    [TestCase(5, 10, -30)]
    [TestCase(-5, 10, -30 )]
    [TestCase(5, -10, -30)]
    [TestCase(-5, -10, -30 )]
    [TestCase(int.MaxValue, 2, 2)]
    [TestCase(int.MinValue, -1, 0)]
    [TestCase(65536, 65536, -1)]
    [TestCase(0, int.MaxValue, int.MinValue)]
    [TestCase(int.MaxValue, 1, 1)]
    public void FusedMultiplyAddWorks(int a, int b, int c)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new FusedMultiplyAdd(Register.a0, a, b, c));
        program.Emit(new Exit());
        
        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());
        
        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");
        
        Assert.That(emulator.GetRegisterS(Register.a0), Is.EqualTo((a * b) + c));
    }

    [Test]
    [TestCase(2, 3, 8)]
    [TestCase(-2, 3, -8)]
    [TestCase(10, 5, 100_000)]
    [TestCase(366634, 0, 1)]
    // The game computes (int)Math.Round(Math.Pow(b, e)) on .NET 8 x64, where an out-of-range
    // double -> int conversion yields int.MinValue rather than wrapping or saturating
    [TestCase(2, 31, int.MinValue)]
    [TestCase(2, 32, int.MinValue)]
    [TestCase(-2, 31, int.MinValue)]
    [TestCase(0, 0, 1)]
    [TestCase(0, 5, 0)]
    [TestCase(-1, 3, -1)]
    [TestCase(-1, 4, 1)]
    [TestCase(3, 21, int.MinValue)]
    [TestCase(int.MaxValue, 1, int.MaxValue)]
    [TestCase(int.MaxValue, 2, int.MinValue)]
    public void PowerWorks(int b, int e, int r)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new Power(Register.a0, b, e));
        program.Emit(new Exit());
        
        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());
        
        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");

        Assert.That(emulator.GetRegisterS(Register.a0), Is.EqualTo(r));
    }
    
    [Test]
    [TestCase(50, 5)]
    [TestCase(2_000_000_000, 1_000_000)]
    [TestCase(51, 5)]
    [TestCase(6, -3)]
    [TestCase(-51, 5)]
    [TestCase(51, -5)]
    [TestCase(-51, -5)]
    [TestCase(3, 5)]
    [TestCase(-3, 5)]
    [TestCase(0, 7)]
    [TestCase(int.MinValue, 1)]
    [TestCase(int.MinValue, 2)]
    [TestCase(int.MaxValue, -1)]
    [TestCase(int.MinValue, int.MaxValue)]
    public void DivideWorks(int a, int b)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new Divide(Register.a0, a, b));
        program.Emit(new Exit());
        
        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());
        
        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");
        
        Assert.That(emulator.GetRegisterS(Register.a0), Is.EqualTo(a / b));
    }
    
    [Test]
    [TestCase(51, 5)]
    [TestCase(-51, 5)]
    [TestCase(51, -5)]
    [TestCase(-51, -5)]
    [TestCase(-50, 5)]
    [TestCase(0, 5)]
    [TestCase(3, 5)]
    [TestCase(-3, 5)]
    [TestCase(int.MinValue, 2)]
    [TestCase(int.MinValue, int.MaxValue)]
    [TestCase(int.MaxValue, int.MinValue)]
    [TestCase(int.MaxValue, 1)]
    public void RemainderWorks(int a, int b)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new Remainder(Register.a0, a, b));
        program.Emit(new Exit());
        
        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());
        
        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");
        
        Assert.That(emulator.GetRegisterS(Register.a0), Is.EqualTo(a % b));
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
    
    [Test]
    [TestCase(51, 5)]
    [TestCase(-51, 5)]
    [TestCase(51, -5)]
    [TestCase(-51, -5)]
    [TestCase(-50, 5)]
    [TestCase(50, -5)]
    [TestCase(0, 5)]
    [TestCase(-1, 5)]
    [TestCase(1, -5)]
    [TestCase(3, 5)]
    [TestCase(int.MinValue, int.MaxValue)]
    [TestCase(int.MaxValue, int.MinValue)]
    [TestCase(int.MinValue, 2)]
    public void ModuloWorks(int a, int b)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new Modulo(Register.a0, a, b));
        program.Emit(new Exit());
        
        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());
        
        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");
        
        Assert.That(emulator.GetRegisterS(Register.a0), Is.EqualTo(Mod(a,b)));
    }
    
    [Test]
    [TestCase(1)]
    [TestCase(214142)]
    [TestCase(3)]
    [TestCase(-5)]
    [TestCase(-31232)]
    [TestCase(-1241244)]
    [TestCase(0)]
    [TestCase(int.MinValue)]
    [TestCase(int.MaxValue)]
    [TestCase(int.MinValue + 1)]
    public void NegateWorks(int a)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new Negate(Register.a0, a));
        program.Emit(new Exit());
        
        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());
        
        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");
        
        Assert.That(emulator.GetRegisterS(Register.a0), Is.EqualTo(-a));
    }
    
    [Test]
    [TestCase(1)]
    [TestCase(214142)]
    [TestCase(3)]
    [TestCase(-5)]
    [TestCase(-31232)]
    [TestCase(-1241244)]
    [TestCase(0)]
    [TestCase(int.MaxValue)]
    [TestCase(int.MinValue + 1)]
    [TestCase(-1)]
    public void AbsoluteWorks(int a)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new Absolute(Register.a0, a));
        program.Emit(new Exit());
        
        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());
        
        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");
        
        Assert.That(emulator.GetRegisterS(Register.a0), Is.EqualTo(int.Abs(a)));
    }

    [Test]
    [TestCase(0)]
    [TestCase(1)]
    [TestCase(214142)]
    [TestCase(3)]
    [TestCase(-5)]
    [TestCase(-31232)]
    [TestCase(-1241244)]
    [TestCase(int.MinValue)]
    [TestCase(int.MaxValue)]
    [TestCase(-1)]
    public void SignWorks(int a)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new Sign(Register.a0, a));
        program.Emit(new Exit());
        
        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());
        
        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");
        
        Assert.That(emulator.GetRegisterS(Register.a0), Is.EqualTo(int.Sign(a)));
    }
    
    [Test]
    [TestCase(-1070809278, -45111061)]
    [TestCase(-826645256, 99996228)]
    [TestCase(666409456, 349463574)]
    [TestCase(738762087, -156393565)]
    [TestCase(-93894278, -392238477)]
    [TestCase(-707083475, -774962954)]
    [TestCase(-593218450, -527455434)]
    [TestCase(-336510755, -796775389)]
    [TestCase(-574927384, -1034339186)]
    [TestCase(-1016482158, -854797390)]
    [TestCase(-374991766, 663279045)]
    [TestCase(-692291998, 305430962)]
    [TestCase(-788100335, -883643310)]
    [TestCase(698812156, 962713311)]
    [TestCase(73417084, -703536015)]
    [TestCase(-271277638, -75047398)]
    [TestCase(-1, 1)]
    [TestCase(5, 5)]
    [TestCase(int.MinValue, int.MaxValue)]
    [TestCase(int.MaxValue, int.MinValue)]
    [TestCase(0, -1)]
    public void MinimumWorks(int a, int b)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new Minimum(Register.a0, a, b));
        program.Emit(new Exit());
        
        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());
        
        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");

        Assert.That(emulator.GetRegisterS(Register.a0), Is.EqualTo(int.Min(a, b)));
    }
    
    [Test]
    [TestCase(-997966934, -629115788)]
    [TestCase(-53438362, 1055127360)]
    [TestCase(-351348561, -1014497324)]
    [TestCase(939333122, -911689743)]
    [TestCase(492950775, -606898054)]
    [TestCase(1015415056, -50351932)]
    [TestCase(-1031662148, 986525297)]
    [TestCase(927787466, -945228503)]
    [TestCase(-589182155, -152425553)]
    [TestCase(-250243025, 1062007888)]
    [TestCase(340792567, 850655261)]
    [TestCase(291851437, 119070983)]
    [TestCase(-860099450, -143448100)]
    [TestCase(583464561, -575815537)]
    [TestCase(-584090684, -623272384)]
    [TestCase(417046445, 360917161)]
    [TestCase(-1, 1)]
    [TestCase(5, 5)]
    [TestCase(int.MinValue, int.MaxValue)]
    [TestCase(int.MaxValue, int.MinValue)]
    [TestCase(0, -1)]
    public void MaximumWorks(int a, int b)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new Maximum(Register.a0, a, b));
        program.Emit(new Exit());
        
        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());
        
        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");
        
        Assert.That(emulator.GetRegisterS(Register.a0), Is.EqualTo(int.Max(a, b)));
    }
    
    [Test]
    [TestCase(-531254, -20, 30)]
    [TestCase(5, 0, 10)]
    [TestCase(52523253, -1000, -100)]
    [TestCase(-20, -20, 30)]
    [TestCase(30, -20, 30)]
    [TestCase(-21, -20, 30)]
    [TestCase(31, -20, 30)]
    [TestCase(7, 7, 7)]
    [TestCase(int.MinValue, int.MinValue, int.MaxValue)]
    [TestCase(int.MaxValue, int.MinValue, int.MaxValue)]
    [TestCase(-1, 0, int.MaxValue)]
    public void ClampWorks(int a, int b, int c)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new Clamp(Register.a0, a, b, c));
        program.Emit(new Exit());
        
        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());
        
        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");
        
        Assert.That(emulator.GetRegisterS(Register.a0), Is.EqualTo(int.Clamp(a, b, c)));
    }

    [Test]
    [TestCase(5, 10, 0, 5)]
    [TestCase(-5, 10, 0, 0)]
    [TestCase(15, 10, 0, 10)]
    [TestCase(10, 10, 0, 10)]
    [TestCase(0, 10, 0, 0)]
    [TestCase(0, 1, -1, 0)]
    [TestCase(5, -1, -10, -1)]
    [TestCase(-20, -1, -10, -10)]
    [TestCase(int.MinValue, int.MaxValue, int.MinValue, int.MinValue)]
    [TestCase(int.MaxValue, int.MaxValue, int.MinValue, int.MaxValue)]
    public void ClampSwapsBoundsWhenMaxLessThanMin(int value, int min, int max, int expected)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new Clamp(Register.a0, value, min, max));
        program.Emit(new Exit());

        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());

        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");

        Assert.That(emulator.GetRegisterS(Register.a0), Is.EqualTo(expected));
    }

    [Test]
    [TestCase(4_000_000_000, 4_000_000_000)]
    [TestCase(4_000_000_000, 5u)]
    [TestCase(uint.MaxValue, uint.MaxValue)]
    [TestCase(uint.MaxValue, 1u)]
    [TestCase(0x80000000u, 2u)]
    [TestCase(0x80000000u, 0x80000000u)]
    [TestCase(0u, uint.MaxValue)]
    [TestCase(0xFFFFu, 0x10001u)]
    public void MultiplyHighUnsignedWorks(uint a, uint b)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new MultiplyHighUnsigned(Register.a0, a, b));
        program.Emit(new Exit());
        
        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());
        
        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");
        
        Assert.That(emulator.GetRegisterU(Register.a0), Is.EqualTo(((ulong)a * b) >> 32));
    }
    
    [Test]
    [TestCase(4_000_000_000, 5u)]
    [TestCase(4_000_000_001, 5u)]
    [TestCase(uint.MaxValue, 1u)]
    [TestCase(uint.MaxValue, 2u)]
    [TestCase(0x80000000u, uint.MaxValue)]
    [TestCase(5u, uint.MaxValue)]
    [TestCase(uint.MaxValue, uint.MaxValue)]
    [TestCase(0x80000000u, 2u)]
    [TestCase(0u, 3u)]
    public void DivideUnsignedWorks(uint a, uint b)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new DivideUnsigned(Register.a0, a, b));
        program.Emit(new Exit());
        
        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());
        
        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");
        
        Assert.That(emulator.GetRegisterU(Register.a0), Is.EqualTo(a / b));
    }

    [Test]
    [TestCase(4_000_000_001, 5u)]
    [TestCase(uint.MaxValue, 2u)]
    [TestCase(0x80000000u, 3u)]
    [TestCase(5u, uint.MaxValue)]
    [TestCase(0xFFFFFFFEu, uint.MaxValue)]
    [TestCase(uint.MaxValue, 0x80000000u)]
    [TestCase(0u, 7u)]
    [TestCase(4_000_000_000u, 4_000_000_000u)]
    public void RemainderUnsignedWorks(uint a, uint b)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new RemainderUnsigned(Register.a0, a, b));
        program.Emit(new Exit());
        
        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());
        
        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");
        
        Assert.That(emulator.GetRegisterU(Register.a0), Is.EqualTo(a % b));
    }
    
    [Test]
    [TestCase(4_000_000_000u, 5u)]
    [TestCase(4_000_000_000, 4_000_000_001u)]
    [TestCase(0u, uint.MaxValue)]
    [TestCase(0x7FFFFFFFu, 0x80000000u)]
    [TestCase(uint.MaxValue, 1u)]
    [TestCase(5u, 5u)]
    public void MinimumUnsignedWorks(uint a, uint b)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new MinimumUnsigned(Register.a0, a, b));
        program.Emit(new Exit());
        
        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());
        
        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");

        Assert.That(emulator.GetRegisterU(Register.a0), Is.EqualTo(uint.Min(a, b)));
    }
    
    [Test]
    [TestCase(4_000_000_000u, 5u)]
    [TestCase(4_000_000_000, 4_000_000_001u)]
    [TestCase(0u, uint.MaxValue)]
    [TestCase(0x7FFFFFFFu, 0x80000000u)]
    [TestCase(uint.MaxValue, 1u)]
    [TestCase(5u, 5u)]
    public void MaximumUnsignedWorks(uint a, uint b)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new MaximumUnsigned(Register.a0, a, b));
        program.Emit(new Exit());
        
        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());
        
        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");

        Assert.That(emulator.GetRegisterU(Register.a0), Is.EqualTo(uint.Max(a, b)));
    }

    [Test]
    [TestCase(4_000_000_000u, 4_000_000_001u, 4_000_000_005u)]
    [TestCase(4_000_000_003u, 4_000_000_001u, 4_000_000_005u)]
    [TestCase(4_000_000_007u, 4_000_000_001u, 4_000_000_005u)]
    [TestCase(0x80000000u, 0u, 0x7FFFFFFFu)]
    [TestCase(5u, 0x80000000u, uint.MaxValue)]
    [TestCase(uint.MaxValue, 0u, uint.MaxValue)]
    [TestCase(0u, 0u, uint.MaxValue)]
    [TestCase(7u, 7u, 7u)]
    [TestCase(0x7FFFFFFFu, 0x80000000u, uint.MaxValue)]
    public void ClampUnsignedWorks(uint a, uint b, uint c)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new ClampUnsigned(Register.a0, a, b, c));
        program.Emit(new Exit());
        
        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());
        
        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");
        
        Assert.That(emulator.GetRegisterU(Register.a0), Is.EqualTo(uint.Clamp(a, b,c)));
    }

    [Test]
    [TestCase(5u, 10u, 0u, 5u)]
    [TestCase(15u, 10u, 0u, 10u)]
    [TestCase(10u, 10u, 0u, 10u)]
    [TestCase(0u, 10u, 0u, 0u)]
    [TestCase(0x80000000u, 0x7FFFFFFFu, 0u, 0x7FFFFFFFu)]
    [TestCase(5u, uint.MaxValue, 0x80000000u, 0x80000000u)]
    [TestCase(uint.MaxValue, uint.MaxValue, 0x80000000u, uint.MaxValue)]
    [TestCase(5u, 0xFFFFFFFFu, 0xFFFFFFF6u, 0xFFFFFFF6u)]
    public void ClampUnsignedSwapsBoundsWhenMaxLessThanMin(uint value, uint min, uint max, uint expected)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new ClampUnsigned(Register.a0, value, min, max));
        program.Emit(new Exit());

        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());

        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");

        Assert.That(emulator.GetRegisterU(Register.a0), Is.EqualTo(expected));
    }
}