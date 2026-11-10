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
}