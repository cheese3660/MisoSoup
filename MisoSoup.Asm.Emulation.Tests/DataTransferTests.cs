namespace MisoSoup.Asm.Emulation.Tests;

public class DataTransferTests
{
    [Test]
    [TestCase(MisaType.Uint8, 67)]
    [TestCase(MisaType.Int8, -5)]
    [TestCase(MisaType.Uint16, 666)]
    [TestCase(MisaType.Int16, -420)]
    [TestCase(MisaType.Uint32, 666000)]
    [TestCase(MisaType.Int32, -250000)]
    [TestCase(MisaType.Uint8, 0)]
    [TestCase(MisaType.Uint8, 255)]
    [TestCase(MisaType.Uint8, 128)]
    [TestCase(MisaType.Int8, -128)]
    [TestCase(MisaType.Int8, 127)]
    [TestCase(MisaType.Int8, -1)]
    [TestCase(MisaType.Uint16, 65535)]
    [TestCase(MisaType.Uint16, 32768)]
    [TestCase(MisaType.Int16, -32768)]
    [TestCase(MisaType.Int16, 32767)]
    [TestCase(MisaType.Int16, -1)]
    [TestCase(MisaType.Uint32, -1)]
    [TestCase(MisaType.Uint32, int.MinValue)]
    [TestCase(MisaType.Int32, int.MinValue)]
    [TestCase(MisaType.Int32, int.MaxValue)]
    [TestCase(MisaType.Int32, 0)]
    public static void LoadWorks(MisaType type, int val)
    {
        var program = new Program();
        program.Mark("data");
        program.Emit(new Embed(type, val));
        program.Mark("main");
        program.Emit(new Load(type, Register.a0, "data"));
        program.Emit(new Exit());

        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());

        var emulator = new MnemonimovEmulator(4, program);
        emulator.RunProcess("main");

        Assert.That(emulator.GetRegisterS(Register.a0), Is.EqualTo(val));
    }

    [Test]
    [TestCase(MisaType.Uint8, 67)]
    [TestCase(MisaType.Int8, -5)]
    [TestCase(MisaType.Uint16, 666)]
    [TestCase(MisaType.Int16, -420)]
    [TestCase(MisaType.Uint32, 666000)]
    [TestCase(MisaType.Int32, -250000)]
    [TestCase(MisaType.Uint8, 0)]
    [TestCase(MisaType.Uint8, 255)]
    [TestCase(MisaType.Uint8, 128)]
    [TestCase(MisaType.Int8, -128)]
    [TestCase(MisaType.Int8, 127)]
    [TestCase(MisaType.Int8, -1)]
    [TestCase(MisaType.Uint16, 65535)]
    [TestCase(MisaType.Uint16, 32768)]
    [TestCase(MisaType.Int16, -32768)]
    [TestCase(MisaType.Int16, 32767)]
    [TestCase(MisaType.Uint32, int.MaxValue)]
    [TestCase(MisaType.Int32, int.MinValue)]
    [TestCase(MisaType.Int32, int.MaxValue)]
    [TestCase(MisaType.Int32, -1)]
    public static void StoreWorks(MisaType type, int val)
    {
        var program = new Program();
        program.Mark("data");
        program.Emit(new Reserve(type, 1));
        program.Mark("main");
        program.Emit(new Store(type, "data", val));
        program.Emit(new Exit());

        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());

        var emulator = new MnemonimovEmulator(4, program);
        emulator.RunProcess("main");

        var addr = emulator.Memory.ResolveLabel("data");
        switch (type)
        {
            case MisaType.Uint8:
                Assert.That(emulator.Memory.ReadByte(addr), Is.EqualTo(val));
                break;
            case MisaType.Uint16:
                Assert.That(emulator.Memory.ReadUShort(addr), Is.EqualTo(val));
                break;
            case MisaType.Uint32:
                Assert.That(emulator.Memory.ReadUInt(addr), Is.EqualTo(val));
                break;
            case MisaType.Int8:
                Assert.That(emulator.Memory.ReadSbyte(addr), Is.EqualTo(val));
                break;
            case MisaType.Int16:
                Assert.That(emulator.Memory.ReadShort(addr), Is.EqualTo(val));
                break;
            case MisaType.Int32:
                Assert.That(emulator.Memory.ReadInt(addr), Is.EqualTo(val));
                break;
        }
    }

    [Test]
    [TestCase(0, 1)]
    [TestCase(1, 1)]
    [TestCase(2, 1)]
    [TestCase(0, 2)]
    [TestCase(1, 2)]
    [TestCase(2, 2)]
    [TestCase(0, 4)]
    [TestCase(1, 4)]
    [TestCase(2, 4)]
    [TestCase(-1, 4)]
    [TestCase(-2, 1)]
    [TestCase(1, -4)]
    [TestCase(3, 0)]
    [TestCase(0, 0)]
    [TestCase(5, 3)]
    [TestCase(2, 8)]
    public static void ComputeEffectiveAddressWorks(int index, int scale)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new ComputeEffectiveAddress("main", index, scale));
        program.Emit(new Exit());

        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());

        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");

        var addr = emulator.Memory.ResolveLabel("main");
        Assert.That(emulator.GetRegisterS(Register.ea), Is.EqualTo(addr + index * scale));
    }

    [Test]
    [TestCase(0, 67)]
    [TestCase(4, -67)]
    [TestCase(8, 420)]
    [TestCase(12, 42069)]
    [TestCase(16, 6742069)]
    [TestCase(1, 17407)]
    [TestCase(2, 4456447)]
    [TestCase(3, 1140850687)]
    public static void LoadEffectiveWorks(int displacement, int expectedValue)
    {
        var program = new Program();
        program.Mark("data");
        program.Emit(new Embed(MisaType.Int32, 67, -67, 420, 42069, 6742069));
        program.Mark("main");
        program.Emit(new ToProgramAbsolute(Register.ea, "data"));
        program.Emit(new LoadEffective(MisaType.Int32, Register.a0, displacement));
        program.Emit(new Exit());

        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());

        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");

        Assert.That(emulator.GetRegisterS(Register.a0), Is.EqualTo(expectedValue));
    }

    [Test]
    [TestCase(0)]
    [TestCase(4)]
    [TestCase(8)]
    [TestCase(12)]
    [TestCase(16)]
    [TestCase(1)]
    [TestCase(2)]
    [TestCase(3)]
    public static void StoreEffectiveWorks(int displacement)
    {
        var program = new Program();
        program.Mark("data");
        program.Emit(new Reserve(MisaType.Int32, 5));
        program.Mark("main");
        program.Emit(new ToProgramAbsolute(Register.ea, "data"));
        program.Emit(new StoreEffective(MisaType.Int32, displacement, 6742069));
        program.Emit(new Exit());

        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());

        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");

        var addr = emulator.Memory.ResolveLabel("data");
        Assert.That(emulator.Memory.ReadInt(addr + displacement), Is.EqualTo(6742069));
    }

    [Test]
    public static void ToProgramRelativeWorks()
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new ToProgramRelative(Register.a0, MnemonimovEmulator.ReservedSpace + 5));
        program.Emit(new Exit());

        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());

        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");

        Assert.That(emulator.GetRegisterS(Register.a0), Is.EqualTo(5));
    }

    [Test]
    public static void ToProgramAbsoluteWorks()
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new ToProgramAbsolute(Register.ea, "main"));
        program.Emit(new Exit());

        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());

        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");

        Assert.That(emulator.GetRegisterS(Register.ea), Is.EqualTo(emulator.Memory.ResolveLabel("main")));
    }

    [Test]
    public static void PushWorks()
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new Push(67));
        program.Emit(new Push(420));
        program.Emit(new Push(42069));
        program.Emit(new Push(6742069));
        program.Emit(new Exit());

        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());

        var emulator = new MnemonimovEmulator(256, program);
        var stackStart = emulator.StackPointer;
        emulator.RunProcess("main");
        var stackEnd = emulator.StackPointer;

        using (Assert.EnterMultipleScope())
        {
            Assert.That(stackStart - stackEnd, Is.EqualTo(16));
            Assert.That(emulator.Memory.ReadInt(stackEnd), Is.EqualTo(67));
            Assert.That(emulator.Memory.ReadInt(stackEnd + 4), Is.EqualTo(420));
            Assert.That(emulator.Memory.ReadInt(stackEnd + 8), Is.EqualTo(42069));
            Assert.That(emulator.Memory.ReadInt(stackEnd + 12), Is.EqualTo(6742069));
        }
    }

    [Test]
    public static void PopWorks()
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new Push(67));
        program.Emit(new Push(420));
        program.Emit(new Push(42069));
        program.Emit(new Push(6742069));
        program.Emit(new Pop(Register.a0));
        program.Emit(new Pop(Register.a1));
        program.Emit(new Pop(Register.a2));
        program.Emit(new Pop(Register.a3));
        program.Emit(new Exit());

        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());

        var emulator = new MnemonimovEmulator(256, program);
        var stackStart = emulator.StackPointer;
        emulator.RunProcess("main");
        var stackEnd = emulator.StackPointer;

        using (Assert.EnterMultipleScope())
        {
            Assert.That(stackStart, Is.EqualTo(stackEnd));
            Assert.That(emulator.GetRegisterS(Register.a0), Is.EqualTo(6742069));
            Assert.That(emulator.GetRegisterS(Register.a1), Is.EqualTo(42069));
            Assert.That(emulator.GetRegisterS(Register.a2), Is.EqualTo(420));
            Assert.That(emulator.GetRegisterS(Register.a3), Is.EqualTo(67));
        }
    }


    private static IEnumerable<TestCaseData> MoveData
    {
        get
        {
            for (byte i = 0; i < 32; i++)
            {
                yield return new TestCaseData(new Register(RegisterBank.Saved, i),
                    670000 + (int)i + (int)RegisterBank.Saved * 32);
                if (i < 16)
                {
                    yield return new TestCaseData(new Register(RegisterBank.Temporary, i),
                        670000 + (int)i + (int)RegisterBank.Temporary * 32);
                    yield return new TestCaseData(new Register(RegisterBank.Argument, i),
                        670000 + (int)i + (int)RegisterBank.Argument * 32);
                }
            }
        }
    }

    [Test]
    [TestCaseSource(nameof(MoveData))]
    public static void ImmediateMoveWorks(Register target, int testValue)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new Move(target, testValue));
        program.Emit(new Exit());

        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());

        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");

        Assert.That(emulator.GetRegisterS(target), Is.EqualTo(testValue));
    }

    [Test]
    [TestCaseSource(nameof(MoveData))]
    public static void RegisterMoveWorks(Register source, int value)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new Move(source, value));
        program.Emit(new Move(Register.fp, source));
        program.Emit(new Exit());

        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());

        var emulator = new MnemonimovEmulator(256, program);
        emulator.RunProcess("main");

        Assert.That(emulator.FramePointer, Is.EqualTo(value));
    }

    [Test]
    public static void ConditionalMoveMovesWhenTrue()
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new Move(Register.a0, 67));
        program.Emit(new Move(Register.cr, "true"));
        program.Emit(new ConditionalMove(Register.a0, 420));
        program.Emit(new Exit());

        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());

        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");

        Assert.That(emulator.GetRegisterS(Register.a0), Is.EqualTo(420));
    }

    [Test]
    public static void ConditionalMoveDoesNotMoveWhenFalse()
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new Move(Register.a0, 67));
        program.Emit(new Move(Register.cr, "false"));
        program.Emit(new ConditionalMove(Register.a0, 420));
        program.Emit(new Exit());

        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());

        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");

        Assert.That(emulator.GetRegisterS(Register.a0), Is.EqualTo(67));
    }

    [Test]
    public static void SelectSelectsFirstValueWhenTrue()
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new Move(Register.cr, "true"));
        program.Emit(new Select(Register.a0, 67, 420));
        program.Emit(new Exit());

        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());

        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");

        Assert.That(emulator.GetRegisterS(Register.a0), Is.EqualTo(67));
    }

    [Test]
    public static void SelectSelectsSecondValueWhenFalse()
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new Move(Register.cr, "false"));
        program.Emit(new Select(Register.a0, 67, 420));
        program.Emit(new Exit());

        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());

        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");

        Assert.That(emulator.GetRegisterS(Register.a0), Is.EqualTo(420));
    }


    [Test]
    public static void SwapWorks()
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new Move(Register.a0, 67));
        program.Emit(new Move(Register.a1, 420));
        program.Emit(new Swap(Register.a0, Register.a1));
        program.Emit(new Exit());

        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());

        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");

        Assert.That(emulator.GetRegisterS(Register.a0), Is.EqualTo(420));
        Assert.That(emulator.GetRegisterS(Register.a1), Is.EqualTo(67));
    }
}