namespace MisoSoup.Asm.Emulation.Tests;

public class EmulationTests
{
    [Test]
    // The most basic of tests for this emulator, that a process can actually run
    public void AProcessCanActuallyExecute()
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new Exit());
        
        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());
        
        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");
    }

    [Test]
    [TestCase(MisaType.Int8, 128)]
    [TestCase(MisaType.Int8, -129)]
    [TestCase(MisaType.Uint8, 256)]
    [TestCase(MisaType.Uint8, -1)]
    [TestCase(MisaType.Int16, 32768)]
    [TestCase(MisaType.Int16, -32769)]
    [TestCase(MisaType.Uint16, 65536)]
    [TestCase(MisaType.Uint16, -1)]
    [TestCase(MisaType.Uint32, -1)]
    [TestCase(MisaType.Uint32, int.MinValue)]
    public static void EmbedOutOfRangeThrows(MisaType type, int val)
    {
        var program = new Program();
        program.Mark("data");
        program.Emit(new Embed(type, val));
        program.Mark("main");
        program.Emit(new Exit());

        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());

        Assert.That(() => new MnemonimovEmulator(4, program), Throws.Exception.With.Message.Contains("does not fit"));
    }

    [Test]
    [TestCase(MisaType.Int8, 128u)]
    [TestCase(MisaType.Uint8, 256u)]
    [TestCase(MisaType.Int16, 32768u)]
    [TestCase(MisaType.Uint16, 65536u)]
    [TestCase(MisaType.Int32, 0x80000000u)]
    [TestCase(MisaType.Int32, uint.MaxValue)]
    public static void EmbedUnsignedOutOfRangeThrows(MisaType type, uint val)
    {
        var program = new Program();
        program.Mark("data");
        program.Emit(new Embed(type, val));
        program.Mark("main");
        program.Emit(new Exit());

        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());

        Assert.That(() => new MnemonimovEmulator(4, program), Throws.Exception.With.Message.Contains("does not fit"));
    }
}