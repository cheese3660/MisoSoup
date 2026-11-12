namespace MisoSoup.Asm.Emulation.Tests;

public class VectorTests
{
    [Test]
    public void VectorPushWorks()
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new Move(Register.a0, 67));
        program.Emit(new Move(Register.a1, 420));
        program.Emit(new Move(Register.a2, 69));
        program.Emit(new VectorPush(Register.a0 + Register.a2));
        program.Emit(new Exit());

        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());

        var emulator = new MnemonimovEmulator(64, program);
        var startSp = emulator.StackPointer;
        emulator.RunProcess("main");
        var endSp = emulator.StackPointer;

        using (Assert.EnterMultipleScope())
        {
            Assert.That(startSp - endSp, Is.EqualTo(12));
            Assert.That(emulator.Memory.ReadInt(endSp), Is.EqualTo(69));
            Assert.That(emulator.Memory.ReadInt(endSp + 4), Is.EqualTo(420));
            Assert.That(emulator.Memory.ReadInt(endSp + 8), Is.EqualTo(67));
        }
    }

    [Test]
    public void VectorPopWorks()
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new Push(67));
        program.Emit(new Push(420));
        program.Emit(new Push(69));
        program.Emit(new VectorPop(Register.a0 + Register.a2));
        program.Emit(new Exit());

        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());

        var emulator = new MnemonimovEmulator(64, program);
        var startSp = emulator.StackPointer;
        emulator.RunProcess("main");
        var endSp = emulator.StackPointer;

        using (Assert.EnterMultipleScope())
        {
            Assert.That(startSp - endSp, Is.EqualTo(0));
            Assert.That(emulator.GetRegisterS(Register.a0), Is.EqualTo(67));
            Assert.That(emulator.GetRegisterS(Register.a1), Is.EqualTo(420));
            Assert.That(emulator.GetRegisterS(Register.a2), Is.EqualTo(69));
        }
    }

    [Test]
    public void VectorMoveWorks()
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new VectorMove(Register.a0 + Register.a15, 5));
        program.Emit(new Exit());

        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());

        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");

        using (Assert.EnterMultipleScope())
        {
            for (byte i = 0; i < 16; i++)
            {
                Assert.That(emulator.GetRegisterS(new Register(RegisterBank.Argument, i)), Is.EqualTo(5));
            }
        }
    }

    [Test]
    public void VectorFloatAddWorks()
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new Move(Register.a0, 32.0f));
        program.Emit(new Move(Register.a1, 410.0f));
        program.Emit(new Move(Register.a2, 30.0f));
        program.Emit(new Move(Register.a3, 35.0f));
        program.Emit(new Move(Register.a4, 10.0f));
        program.Emit(new Move(Register.a5, 39.0f));
        program.Emit(new VectorFloatAdd(Register.a0 + 3, Register.a0 + 3, Register.a3 + 3));
        program.Emit(new Exit());

        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());

        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");

        using (Assert.EnterMultipleScope())
        {
            Assert.That(emulator.GetRegisterF(Register.a0), Is.EqualTo(67.0f));
            Assert.That(emulator.GetRegisterF(Register.a1), Is.EqualTo(420.0f));
            Assert.That(emulator.GetRegisterF(Register.a2), Is.EqualTo(69.0f));
        }
    }

    [Test]
    public void VectorFloatSubWorks()
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new Move(Register.a0, 32.0f));
        program.Emit(new Move(Register.a1, 410.0f));
        program.Emit(new Move(Register.a2, 30.0f));
        program.Emit(new Move(Register.a3, -35.0f));
        program.Emit(new Move(Register.a4, -10.0f));
        program.Emit(new Move(Register.a5, -39.0f));
        program.Emit(new VectorFloatSubtract(Register.a0 + 3, Register.a0 + 3, Register.a3 + 3));
        program.Emit(new Exit());

        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());

        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");

        using (Assert.EnterMultipleScope())
        {
            Assert.That(emulator.GetRegisterF(Register.a0), Is.EqualTo(67.0f));
            Assert.That(emulator.GetRegisterF(Register.a1), Is.EqualTo(420.0f));
            Assert.That(emulator.GetRegisterF(Register.a2), Is.EqualTo(69.0f));
        }
    }

    [Test]
    public void VectorFloatMultiplyWorks()
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new Move(Register.a0, 33.5f));
        program.Emit(new Move(Register.a1, 210.0f));
        program.Emit(new Move(Register.a2, 34.5f));
        program.Emit(new VectorFloatMultiply(Register.a0 + 3, Register.a0 + 3, 2f));
        program.Emit(new Exit());

        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());

        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");

        using (Assert.EnterMultipleScope())
        {
            Assert.That(emulator.GetRegisterF(Register.a0), Is.EqualTo(67.0f));
            Assert.That(emulator.GetRegisterF(Register.a1), Is.EqualTo(420.0f));
            Assert.That(emulator.GetRegisterF(Register.a2), Is.EqualTo(69.0f));
        }
    }

    [Test]
    public void VectorFloatFusedMultiplyAddWorks()
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new Move(Register.a0, 0f));
        program.Emit(new Move(Register.a1, 176.5f));
        program.Emit(new Move(Register.a2, 1.0f));
        program.Emit(new VectorFloatFusedMultiplyAdd(Register.a0 + 3, Register.a0 + 3, 2f, 67f));
        program.Emit(new Exit());

        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());

        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");

        using (Assert.EnterMultipleScope())
        {
            Assert.That(emulator.GetRegisterF(Register.a0), Is.EqualTo(67.0f));
            Assert.That(emulator.GetRegisterF(Register.a1), Is.EqualTo(420.0f));
            Assert.That(emulator.GetRegisterF(Register.a2), Is.EqualTo(69.0f));
        }
    }

    [Test]
    public void VectorFloatDivideWorks()
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new Move(Register.a0, 67f * 2.5f));
        program.Emit(new Move(Register.a1, 420f * 2.5f));
        program.Emit(new Move(Register.a2, 69f * 2.5f));
        program.Emit(new VectorFloatDivide(Register.a0 + 3, Register.a0 + 3, 2.5f));
        program.Emit(new Exit());

        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());

        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");

        using (Assert.EnterMultipleScope())
        {
            Assert.That(emulator.GetRegisterF(Register.a0), Is.EqualTo(67.0f));
            Assert.That(emulator.GetRegisterF(Register.a1), Is.EqualTo(420f));
            Assert.That(emulator.GetRegisterF(Register.a2), Is.EqualTo(69.0f));
        }
    }
}