using System.Runtime.Intrinsics.X86;

namespace MisoSoup.Asm.Emulation.Tests;

public class LogicTests
{
    [Test]
    [TestCase(Condition.Equal, 1, 2, false)]
    [TestCase(Condition.Equal, 2, 2, true)]
    [TestCase(Condition.Equal, 2, 1, false)]
    [TestCase(Condition.Equal, -2, -1, false)]
    [TestCase(Condition.Equal, -1, -1, true)]
    [TestCase(Condition.Equal, -1, -2, false)]
    [TestCase(Condition.Equal, -1, 1, false)]
    [TestCase(Condition.Equal, 1, -1, false)]
    [TestCase(Condition.Equal, 0, 0, true)]
    [TestCase(Condition.Equal, 0, -1, false)]
    [TestCase(Condition.Equal, -1, 0, false)]
    [TestCase(Condition.Equal, int.MinValue, int.MaxValue, false)]
    [TestCase(Condition.Equal, int.MaxValue, int.MinValue, false)]
    [TestCase(Condition.Equal, int.MinValue, int.MinValue, true)]
    [TestCase(Condition.Equal, int.MaxValue, int.MaxValue, true)]
    [TestCase(Condition.Equal, int.MaxValue - 1, int.MaxValue, false)]
    [TestCase(Condition.Equal, int.MinValue, int.MinValue + 1, false)]
    [TestCase(Condition.NotEqual, 1, 2, true)]
    [TestCase(Condition.NotEqual, 2, 2, false)]
    [TestCase(Condition.NotEqual, 2, 1, true)]
    [TestCase(Condition.NotEqual, -2, -1, true)]
    [TestCase(Condition.NotEqual, -1, -1, false)]
    [TestCase(Condition.NotEqual, -1, -2, true)]
    [TestCase(Condition.NotEqual, -1, 1, true)]
    [TestCase(Condition.NotEqual, 1, -1, true)]
    [TestCase(Condition.NotEqual, 0, 0, false)]
    [TestCase(Condition.NotEqual, 0, -1, true)]
    [TestCase(Condition.NotEqual, -1, 0, true)]
    [TestCase(Condition.NotEqual, int.MinValue, int.MaxValue, true)]
    [TestCase(Condition.NotEqual, int.MaxValue, int.MinValue, true)]
    [TestCase(Condition.NotEqual, int.MinValue, int.MinValue, false)]
    [TestCase(Condition.NotEqual, int.MaxValue, int.MaxValue, false)]
    [TestCase(Condition.NotEqual, int.MaxValue - 1, int.MaxValue, true)]
    [TestCase(Condition.NotEqual, int.MinValue, int.MinValue + 1, true)]
    [TestCase(Condition.LessThan, 1, 2, true)]
    [TestCase(Condition.LessThan, 2, 2, false)]
    [TestCase(Condition.LessThan, 2, 1, false)]
    [TestCase(Condition.LessThan, -2, -1, true)]
    [TestCase(Condition.LessThan, -1, -1, false)]
    [TestCase(Condition.LessThan, -1, -2, false)]
    [TestCase(Condition.LessThan, -1, 1, true)]
    [TestCase(Condition.LessThan, 1, -1, false)]
    [TestCase(Condition.LessThan, 0, 0, false)]
    [TestCase(Condition.LessThan, 0, -1, false)]
    [TestCase(Condition.LessThan, -1, 0, true)]
    [TestCase(Condition.LessThan, int.MinValue, int.MaxValue, true)]
    [TestCase(Condition.LessThan, int.MaxValue, int.MinValue, false)]
    [TestCase(Condition.LessThan, int.MinValue, int.MinValue, false)]
    [TestCase(Condition.LessThan, int.MaxValue, int.MaxValue, false)]
    [TestCase(Condition.LessThan, int.MaxValue - 1, int.MaxValue, true)]
    [TestCase(Condition.LessThan, int.MinValue, int.MinValue + 1, true)]
    [TestCase(Condition.LessThanOrEqual, 1, 2, true)]
    [TestCase(Condition.LessThanOrEqual, 2, 2, true)]
    [TestCase(Condition.LessThanOrEqual, 2, 1, false)]
    [TestCase(Condition.LessThanOrEqual, -2, -1, true)]
    [TestCase(Condition.LessThanOrEqual, -1, -1, true)]
    [TestCase(Condition.LessThanOrEqual, -1, -2, false)]
    [TestCase(Condition.LessThanOrEqual, -1, 1, true)]
    [TestCase(Condition.LessThanOrEqual, 1, -1, false)]
    [TestCase(Condition.LessThanOrEqual, 0, 0, true)]
    [TestCase(Condition.LessThanOrEqual, 0, -1, false)]
    [TestCase(Condition.LessThanOrEqual, -1, 0, true)]
    [TestCase(Condition.LessThanOrEqual, int.MinValue, int.MaxValue, true)]
    [TestCase(Condition.LessThanOrEqual, int.MaxValue, int.MinValue, false)]
    [TestCase(Condition.LessThanOrEqual, int.MinValue, int.MinValue, true)]
    [TestCase(Condition.LessThanOrEqual, int.MaxValue, int.MaxValue, true)]
    [TestCase(Condition.LessThanOrEqual, int.MaxValue - 1, int.MaxValue, true)]
    [TestCase(Condition.LessThanOrEqual, int.MinValue, int.MinValue + 1, true)]
    [TestCase(Condition.GreaterThan, 1, 2, false)]
    [TestCase(Condition.GreaterThan, 2, 2, false)]
    [TestCase(Condition.GreaterThan, 2, 1, true)]
    [TestCase(Condition.GreaterThan, -2, -1, false)]
    [TestCase(Condition.GreaterThan, -1, -1, false)]
    [TestCase(Condition.GreaterThan, -1, -2, true)]
    [TestCase(Condition.GreaterThan, -1, 1, false)]
    [TestCase(Condition.GreaterThan, 1, -1, true)]
    [TestCase(Condition.GreaterThan, 0, 0, false)]
    [TestCase(Condition.GreaterThan, 0, -1, true)]
    [TestCase(Condition.GreaterThan, -1, 0, false)]
    [TestCase(Condition.GreaterThan, int.MinValue, int.MaxValue, false)]
    [TestCase(Condition.GreaterThan, int.MaxValue, int.MinValue, true)]
    [TestCase(Condition.GreaterThan, int.MinValue, int.MinValue, false)]
    [TestCase(Condition.GreaterThan, int.MaxValue, int.MaxValue, false)]
    [TestCase(Condition.GreaterThan, int.MaxValue - 1, int.MaxValue, false)]
    [TestCase(Condition.GreaterThan, int.MinValue, int.MinValue + 1, false)]
    [TestCase(Condition.GreaterThanOrEqual, 1, 2, false)]
    [TestCase(Condition.GreaterThanOrEqual, 2, 2, true)]
    [TestCase(Condition.GreaterThanOrEqual, 2, 1, true)]
    [TestCase(Condition.GreaterThanOrEqual, -2, -1, false)]
    [TestCase(Condition.GreaterThanOrEqual, -1, -1, true)]
    [TestCase(Condition.GreaterThanOrEqual, -1, -2, true)]
    [TestCase(Condition.GreaterThanOrEqual, -1, 1, false)]
    [TestCase(Condition.GreaterThanOrEqual, 1, -1, true)]
    [TestCase(Condition.GreaterThanOrEqual, 0, 0, true)]
    [TestCase(Condition.GreaterThanOrEqual, 0, -1, true)]
    [TestCase(Condition.GreaterThanOrEqual, -1, 0, false)]
    [TestCase(Condition.GreaterThanOrEqual, int.MinValue, int.MaxValue, false)]
    [TestCase(Condition.GreaterThanOrEqual, int.MaxValue, int.MinValue, true)]
    [TestCase(Condition.GreaterThanOrEqual, int.MinValue, int.MinValue, true)]
    [TestCase(Condition.GreaterThanOrEqual, int.MaxValue, int.MaxValue, true)]
    [TestCase(Condition.GreaterThanOrEqual, int.MaxValue - 1, int.MaxValue, false)]
    [TestCase(Condition.GreaterThanOrEqual, int.MinValue, int.MinValue + 1, false)]
    // Unsigned conditions on signed operands
    [TestCase(Condition.LessThanUnsigned, -1, 1, false)]
    [TestCase(Condition.LessThanUnsigned, 1, -1, true)]
    [TestCase(Condition.LessThanUnsigned, int.MinValue, int.MaxValue, false)]
    [TestCase(Condition.LessThanUnsigned, int.MaxValue, int.MinValue, true)]
    [TestCase(Condition.LessThanUnsigned, -1, -1, false)]
    [TestCase(Condition.LessThanUnsigned, 0, -1, true)]
    [TestCase(Condition.LessThanEqualUnsigned, -1, 1, false)]
    [TestCase(Condition.LessThanEqualUnsigned, 1, -1, true)]
    [TestCase(Condition.LessThanEqualUnsigned, int.MinValue, int.MaxValue, false)]
    [TestCase(Condition.LessThanEqualUnsigned, int.MaxValue, int.MinValue, true)]
    [TestCase(Condition.LessThanEqualUnsigned, -1, -1, true)]
    [TestCase(Condition.LessThanEqualUnsigned, 0, -1, true)]
    [TestCase(Condition.GreaterThanUnsigned, -1, 1, true)]
    [TestCase(Condition.GreaterThanUnsigned, 1, -1, false)]
    [TestCase(Condition.GreaterThanUnsigned, int.MinValue, int.MaxValue, true)]
    [TestCase(Condition.GreaterThanUnsigned, int.MaxValue, int.MinValue, false)]
    [TestCase(Condition.GreaterThanUnsigned, -1, -1, false)]
    [TestCase(Condition.GreaterThanUnsigned, 0, -1, false)]
    [TestCase(Condition.GreaterThanEqualUnsigned, -1, 1, true)]
    [TestCase(Condition.GreaterThanEqualUnsigned, 1, -1, false)]
    [TestCase(Condition.GreaterThanEqualUnsigned, int.MinValue, int.MaxValue, true)]
    [TestCase(Condition.GreaterThanEqualUnsigned, int.MaxValue, int.MinValue, false)]
    [TestCase(Condition.GreaterThanEqualUnsigned, -1, -1, true)]
    [TestCase(Condition.GreaterThanEqualUnsigned, 0, -1, false)]
    public void SignedCompareOperationsWork(Condition condition, int a, int b, bool expected)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new Compare(condition, a, b));
        program.Emit(new Exit());
        
        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());
        
        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");
        
        Assert.That(emulator.ComparisonResult, Is.EqualTo(expected ? 1 : 0));
    }

    [Test]
    [TestCase(Condition.LessThanUnsigned, 1u, 2u, true)]
    [TestCase(Condition.LessThanUnsigned, 2u, 2u, false)]
    [TestCase(Condition.LessThanUnsigned, 2u, 1u, false)]
    [TestCase(Condition.LessThanUnsigned, 0u, 0u, false)]
    [TestCase(Condition.LessThanUnsigned, 0u, 1u, true)]
    [TestCase(Condition.LessThanUnsigned, 1u, 0u, false)]
    [TestCase(Condition.LessThanUnsigned, 0u, uint.MaxValue, true)]
    [TestCase(Condition.LessThanUnsigned, uint.MaxValue, 0u, false)]
    [TestCase(Condition.LessThanUnsigned, uint.MaxValue, uint.MaxValue, false)]
    [TestCase(Condition.LessThanUnsigned, 0x7FFFFFFFu, 0x80000000u, true)]
    [TestCase(Condition.LessThanUnsigned, 0x80000000u, 0x7FFFFFFFu, false)]
    [TestCase(Condition.LessThanUnsigned, 0x80000000u, 0x80000000u, false)]
    [TestCase(Condition.LessThanUnsigned, 1u, uint.MaxValue, true)]
    [TestCase(Condition.LessThanUnsigned, uint.MaxValue - 1, uint.MaxValue, true)]
    [TestCase(Condition.LessThanEqualUnsigned, 1u, 2u, true)]
    [TestCase(Condition.LessThanEqualUnsigned, 2u, 2u, true)]
    [TestCase(Condition.LessThanEqualUnsigned, 2u, 1u, false)]
    [TestCase(Condition.LessThanEqualUnsigned, 0u, 0u, true)]
    [TestCase(Condition.LessThanEqualUnsigned, 0u, 1u, true)]
    [TestCase(Condition.LessThanEqualUnsigned, 1u, 0u, false)]
    [TestCase(Condition.LessThanEqualUnsigned, 0u, uint.MaxValue, true)]
    [TestCase(Condition.LessThanEqualUnsigned, uint.MaxValue, 0u, false)]
    [TestCase(Condition.LessThanEqualUnsigned, uint.MaxValue, uint.MaxValue, true)]
    [TestCase(Condition.LessThanEqualUnsigned, 0x7FFFFFFFu, 0x80000000u, true)]
    [TestCase(Condition.LessThanEqualUnsigned, 0x80000000u, 0x7FFFFFFFu, false)]
    [TestCase(Condition.LessThanEqualUnsigned, 0x80000000u, 0x80000000u, true)]
    [TestCase(Condition.LessThanEqualUnsigned, 1u, uint.MaxValue, true)]
    [TestCase(Condition.LessThanEqualUnsigned, uint.MaxValue - 1, uint.MaxValue, true)]
    [TestCase(Condition.GreaterThanUnsigned, 1u, 2u, false)]
    [TestCase(Condition.GreaterThanUnsigned, 2u, 2u, false)]
    [TestCase(Condition.GreaterThanUnsigned, 2u, 1u, true)]
    [TestCase(Condition.GreaterThanUnsigned, 0u, 0u, false)]
    [TestCase(Condition.GreaterThanUnsigned, 0u, 1u, false)]
    [TestCase(Condition.GreaterThanUnsigned, 1u, 0u, true)]
    [TestCase(Condition.GreaterThanUnsigned, 0u, uint.MaxValue, false)]
    [TestCase(Condition.GreaterThanUnsigned, uint.MaxValue, 0u, true)]
    [TestCase(Condition.GreaterThanUnsigned, uint.MaxValue, uint.MaxValue, false)]
    [TestCase(Condition.GreaterThanUnsigned, 0x7FFFFFFFu, 0x80000000u, false)]
    [TestCase(Condition.GreaterThanUnsigned, 0x80000000u, 0x7FFFFFFFu, true)]
    [TestCase(Condition.GreaterThanUnsigned, 0x80000000u, 0x80000000u, false)]
    [TestCase(Condition.GreaterThanUnsigned, 1u, uint.MaxValue, false)]
    [TestCase(Condition.GreaterThanUnsigned, uint.MaxValue - 1, uint.MaxValue, false)]
    [TestCase(Condition.GreaterThanEqualUnsigned, 1u, 2u, false)]
    [TestCase(Condition.GreaterThanEqualUnsigned, 2u, 2u, true)]
    [TestCase(Condition.GreaterThanEqualUnsigned, 2u, 1u, true)]
    [TestCase(Condition.GreaterThanEqualUnsigned, 0u, 0u, true)]
    [TestCase(Condition.GreaterThanEqualUnsigned, 0u, 1u, false)]
    [TestCase(Condition.GreaterThanEqualUnsigned, 1u, 0u, true)]
    [TestCase(Condition.GreaterThanEqualUnsigned, 0u, uint.MaxValue, false)]
    [TestCase(Condition.GreaterThanEqualUnsigned, uint.MaxValue, 0u, true)]
    [TestCase(Condition.GreaterThanEqualUnsigned, uint.MaxValue, uint.MaxValue, true)]
    [TestCase(Condition.GreaterThanEqualUnsigned, 0x7FFFFFFFu, 0x80000000u, false)]
    [TestCase(Condition.GreaterThanEqualUnsigned, 0x80000000u, 0x7FFFFFFFu, true)]
    [TestCase(Condition.GreaterThanEqualUnsigned, 0x80000000u, 0x80000000u, true)]
    [TestCase(Condition.GreaterThanEqualUnsigned, 1u, uint.MaxValue, false)]
    [TestCase(Condition.GreaterThanEqualUnsigned, uint.MaxValue - 1, uint.MaxValue, false)]
    // Signedness-independent conditions on unsigned operands
    [TestCase(Condition.Equal, uint.MaxValue, uint.MaxValue, true)]
    [TestCase(Condition.Equal, 0x80000000u, 0x7FFFFFFFu, false)]
    [TestCase(Condition.Equal, 0u, 0u, true)]
    [TestCase(Condition.Equal, 0u, uint.MaxValue, false)]
    [TestCase(Condition.NotEqual, uint.MaxValue, uint.MaxValue, false)]
    [TestCase(Condition.NotEqual, 0x80000000u, 0x7FFFFFFFu, true)]
    [TestCase(Condition.NotEqual, 0u, 0u, false)]
    [TestCase(Condition.NotEqual, 0u, uint.MaxValue, true)]
    // Signed conditions on unsigned operands
    [TestCase(Condition.LessThan, uint.MaxValue, 1u, true)]
    [TestCase(Condition.LessThan, 1u, uint.MaxValue, false)]
    [TestCase(Condition.LessThan, 0x80000000u, 0x7FFFFFFFu, true)]
    [TestCase(Condition.LessThan, 0x7FFFFFFFu, 0x80000000u, false)]
    [TestCase(Condition.LessThanOrEqual, uint.MaxValue, 1u, true)]
    [TestCase(Condition.LessThanOrEqual, 1u, uint.MaxValue, false)]
    [TestCase(Condition.LessThanOrEqual, 0x80000000u, 0x7FFFFFFFu, true)]
    [TestCase(Condition.LessThanOrEqual, 0x7FFFFFFFu, 0x80000000u, false)]
    [TestCase(Condition.GreaterThan, uint.MaxValue, 1u, false)]
    [TestCase(Condition.GreaterThan, 1u, uint.MaxValue, true)]
    [TestCase(Condition.GreaterThan, 0x80000000u, 0x7FFFFFFFu, false)]
    [TestCase(Condition.GreaterThan, 0x7FFFFFFFu, 0x80000000u, true)]
    [TestCase(Condition.GreaterThanOrEqual, uint.MaxValue, 1u, false)]
    [TestCase(Condition.GreaterThanOrEqual, 1u, uint.MaxValue, true)]
    [TestCase(Condition.GreaterThanOrEqual, 0x80000000u, 0x7FFFFFFFu, false)]
    [TestCase(Condition.GreaterThanOrEqual, 0x7FFFFFFFu, 0x80000000u, true)]
    public void UnsignedCompareOperationsWork(Condition condition, uint a, uint b, bool expected)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new Compare(condition, a, b));
        program.Emit(new Exit());
        
        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());
        
        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");
        
        Assert.That(emulator.ComparisonResult, Is.EqualTo(expected ? 1 : 0));
    }
    
    

    [Test]
    // feqa/fneqa are MathF.Abs(a - b) < 0.0001f / MathF.Abs(a - b) >= 0.0001f in the game's VM.
    // Any comparison involving NaN is false, so NaN and INF - INF (= NaN) make both feqa and fneqa false.
    // Epsilon is absolute, not relative: adjacent large floats are further apart than 0.0001f.
    [TestCase(Condition.FloatEqualApproximate, 0.0f, 0.0001f, false)]
    [TestCase(Condition.FloatEqualApproximate, 0.0f, 0.00009f, true)]
    [TestCase(Condition.FloatEqualApproximate, 0.0f, 0.00011f, false)]
    [TestCase(Condition.FloatEqualApproximate, 0.0001f, 0.0f, false)]
    [TestCase(Condition.FloatEqualApproximate, 1.0f, 1.00005f, true)]
    [TestCase(Condition.FloatEqualApproximate, 1.0f, 1.0000999f, true)]
    [TestCase(Condition.FloatEqualApproximate, 1.0f, 1.0001f, false)]
    [TestCase(Condition.FloatEqualApproximate, 1.0f, 1.0002f, false)]
    [TestCase(Condition.FloatEqualApproximate, -1.0f, -1.00005f, true)]
    [TestCase(Condition.FloatEqualApproximate, -0.00005f, 0.00004f, true)]
    [TestCase(Condition.FloatEqualApproximate, 1e-10f, -1e-10f, true)]
    [TestCase(Condition.FloatEqualApproximate, float.Epsilon, -float.Epsilon, true)]
    [TestCase(Condition.FloatEqualApproximate, 1000000.0f, 1000000.1f, false)]
    [TestCase(Condition.FloatEqualApproximate, 16777216.0f, 16777218.0f, false)]
    [TestCase(Condition.FloatNotEqualApproximate, 0.0f, 0.0001f, true)]
    [TestCase(Condition.FloatNotEqualApproximate, 0.0f, 0.00009f, false)]
    [TestCase(Condition.FloatNotEqualApproximate, 0.0f, 0.00011f, true)]
    [TestCase(Condition.FloatNotEqualApproximate, 0.0001f, 0.0f, true)]
    [TestCase(Condition.FloatNotEqualApproximate, 1.0f, 1.00005f, false)]
    [TestCase(Condition.FloatNotEqualApproximate, 1.0f, 1.0000999f, false)]
    [TestCase(Condition.FloatNotEqualApproximate, 1.0f, 1.0001f, true)]
    [TestCase(Condition.FloatNotEqualApproximate, 1.0f, 1.0002f, true)]
    [TestCase(Condition.FloatNotEqualApproximate, -1.0f, -1.00005f, false)]
    [TestCase(Condition.FloatNotEqualApproximate, -0.00005f, 0.00004f, false)]
    [TestCase(Condition.FloatNotEqualApproximate, 1e-10f, -1e-10f, false)]
    [TestCase(Condition.FloatNotEqualApproximate, float.Epsilon, -float.Epsilon, false)]
    [TestCase(Condition.FloatNotEqualApproximate, 1000000.0f, 1000000.1f, true)]
    [TestCase(Condition.FloatNotEqualApproximate, 16777216.0f, 16777218.0f, true)]
    [TestCase(Condition.FloatEqualApproximate, 1.0f, 1.0f, true)]
    [TestCase(Condition.FloatEqualApproximate, 0.0f, 0.0f, true)]
    [TestCase(Condition.FloatEqualApproximate, 0.0f, -0.0f, true)]
    [TestCase(Condition.FloatEqualApproximate, -1.5f, -1.5f, true)]
    [TestCase(Condition.FloatEqualApproximate, float.MaxValue, float.MaxValue, true)]
    [TestCase(Condition.FloatEqualApproximate, float.Epsilon, float.Epsilon, true)]
    [TestCase(Condition.FloatEqualApproximate, 1.0f, 2.0f, false)]
    [TestCase(Condition.FloatEqualApproximate, -1.0f, 1.0f, false)]
    [TestCase(Condition.FloatEqualApproximate, 1e30f, -1e30f, false)]
    [TestCase(Condition.FloatEqualApproximate, float.MaxValue, -float.MaxValue, false)]
    [TestCase(Condition.FloatEqualApproximate, float.NaN, float.NaN, false)]
    [TestCase(Condition.FloatEqualApproximate, float.NaN, 1.0f, false)]
    [TestCase(Condition.FloatEqualApproximate, float.PositiveInfinity, float.PositiveInfinity, false)]
    [TestCase(Condition.FloatEqualApproximate, float.PositiveInfinity, float.NegativeInfinity, false)]
    [TestCase(Condition.FloatEqualApproximate, float.PositiveInfinity, 1.0f, false)]
    [TestCase(Condition.FloatNotEqualApproximate, 1.0f, 1.0f, false)]
    [TestCase(Condition.FloatNotEqualApproximate, 0.0f, 0.0f, false)]
    [TestCase(Condition.FloatNotEqualApproximate, 0.0f, -0.0f, false)]
    [TestCase(Condition.FloatNotEqualApproximate, -1.5f, -1.5f, false)]
    [TestCase(Condition.FloatNotEqualApproximate, float.MaxValue, float.MaxValue, false)]
    [TestCase(Condition.FloatNotEqualApproximate, float.Epsilon, float.Epsilon, false)]
    [TestCase(Condition.FloatNotEqualApproximate, 1.0f, 2.0f, true)]
    [TestCase(Condition.FloatNotEqualApproximate, -1.0f, 1.0f, true)]
    [TestCase(Condition.FloatNotEqualApproximate, 1e30f, -1e30f, true)]
    [TestCase(Condition.FloatNotEqualApproximate, float.MaxValue, -float.MaxValue, true)]
    [TestCase(Condition.FloatNotEqualApproximate, float.NaN, float.NaN, false)]
    [TestCase(Condition.FloatNotEqualApproximate, float.NaN, 1.0f, false)]
    [TestCase(Condition.FloatNotEqualApproximate, float.PositiveInfinity, float.PositiveInfinity, false)]
    [TestCase(Condition.FloatNotEqualApproximate, float.PositiveInfinity, float.NegativeInfinity, true)]
    [TestCase(Condition.FloatNotEqualApproximate, float.PositiveInfinity, 1.0f, true)]
    [TestCase(Condition.FloatLessThan, 1.0f, 2.0f, true)]
    [TestCase(Condition.FloatLessThan, 2.0f, 1.0f, false)]
    [TestCase(Condition.FloatLessThan, 1.0f, 1.0f, false)]
    [TestCase(Condition.FloatLessThan, -1.0f, 1.0f, true)]
    [TestCase(Condition.FloatLessThan, 1.0f, -1.0f, false)]
    [TestCase(Condition.FloatLessThan, -0.0f, 0.0f, false)]
    [TestCase(Condition.FloatLessThan, 0.0f, -0.0f, false)]
    [TestCase(Condition.FloatLessThan, 0.0f, float.Epsilon, true)]
    [TestCase(Condition.FloatLessThan, -float.Epsilon, 0.0f, true)]
    [TestCase(Condition.FloatLessThan, -float.MaxValue, float.MaxValue, true)]
    [TestCase(Condition.FloatLessThan, float.NegativeInfinity, -float.MaxValue, true)]
    [TestCase(Condition.FloatLessThan, float.MaxValue, float.PositiveInfinity, true)]
    [TestCase(Condition.FloatLessThan, float.NegativeInfinity, float.PositiveInfinity, true)]
    [TestCase(Condition.FloatLessThan, float.PositiveInfinity, float.NegativeInfinity, false)]
    [TestCase(Condition.FloatLessThan, float.PositiveInfinity, float.PositiveInfinity, false)]
    [TestCase(Condition.FloatLessThan, float.NaN, 1.0f, false)]
    [TestCase(Condition.FloatLessThan, 1.0f, float.NaN, false)]
    [TestCase(Condition.FloatLessThan, float.NaN, float.NaN, false)]
    [TestCase(Condition.FloatLessThan, float.NaN, float.PositiveInfinity, false)]
    [TestCase(Condition.FloatGreaterThan, 2.0f, 1.0f, true)]
    [TestCase(Condition.FloatGreaterThan, 1.0f, 2.0f, false)]
    [TestCase(Condition.FloatGreaterThan, 1.0f, 1.0f, false)]
    [TestCase(Condition.FloatGreaterThan, 1.0f, -1.0f, true)]
    [TestCase(Condition.FloatGreaterThan, -1.0f, 1.0f, false)]
    [TestCase(Condition.FloatGreaterThan, 0.0f, -0.0f, false)]
    [TestCase(Condition.FloatGreaterThan, -0.0f, 0.0f, false)]
    [TestCase(Condition.FloatGreaterThan, float.Epsilon, 0.0f, true)]
    [TestCase(Condition.FloatGreaterThan, 0.0f, -float.Epsilon, true)]
    [TestCase(Condition.FloatGreaterThan, float.MaxValue, -float.MaxValue, true)]
    [TestCase(Condition.FloatGreaterThan, -float.MaxValue, float.NegativeInfinity, true)]
    [TestCase(Condition.FloatGreaterThan, float.PositiveInfinity, float.MaxValue, true)]
    [TestCase(Condition.FloatGreaterThan, float.PositiveInfinity, float.NegativeInfinity, true)]
    [TestCase(Condition.FloatGreaterThan, float.NegativeInfinity, float.PositiveInfinity, false)]
    [TestCase(Condition.FloatGreaterThan, float.PositiveInfinity, float.PositiveInfinity, false)]
    [TestCase(Condition.FloatGreaterThan, float.NaN, 1.0f, false)]
    [TestCase(Condition.FloatGreaterThan, 1.0f, float.NaN, false)]
    [TestCase(Condition.FloatGreaterThan, float.NaN, float.NaN, false)]
    [TestCase(Condition.FloatGreaterThan, float.PositiveInfinity, float.NaN, false)]
    // fnan/finf only inspect a as such b is set to values that would flip the result if it were read
    [TestCase(Condition.FloatNaN, float.NaN, 0.0f, true)]
    [TestCase(Condition.FloatNaN, -float.NaN, 0.0f, true)]
    [TestCase(Condition.FloatNaN, float.NaN, 1.0f, true)]
    [TestCase(Condition.FloatNaN, 1.0f, float.NaN, false)]
    [TestCase(Condition.FloatNaN, 0.0f, 0.0f, false)]
    [TestCase(Condition.FloatNaN, -0.0f, 0.0f, false)]
    [TestCase(Condition.FloatNaN, float.PositiveInfinity, 0.0f, false)]
    [TestCase(Condition.FloatNaN, float.NegativeInfinity, 0.0f, false)]
    [TestCase(Condition.FloatNaN, float.MaxValue, 0.0f, false)]
    [TestCase(Condition.FloatNaN, float.Epsilon, 0.0f, false)]
    [TestCase(Condition.FloatInf, float.PositiveInfinity, 0.0f, true)]
    [TestCase(Condition.FloatInf, float.NegativeInfinity, 0.0f, true)]
    [TestCase(Condition.FloatInf, float.PositiveInfinity, float.NaN, true)]
    [TestCase(Condition.FloatInf, 1.0f, float.PositiveInfinity, false)]
    [TestCase(Condition.FloatInf, float.NaN, 0.0f, false)]
    [TestCase(Condition.FloatInf, float.MaxValue, 0.0f, false)]
    [TestCase(Condition.FloatInf, -float.MaxValue, 0.0f, false)]
    [TestCase(Condition.FloatInf, 0.0f, 0.0f, false)]
    [TestCase(Condition.FloatInf, 1.0f, 0.0f, false)]
    public void FloatCompareOperationsWork(Condition condition, float a, float b, bool expected)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new Compare(condition, a, b));
        program.Emit(new Exit());
        
        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());
        
        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");
        
        Assert.That(emulator.ComparisonResult, Is.EqualTo(expected ? 1 : 0));
    }

    [Test]
    [TestCase(0)]
    [TestCase(-1)]
    [TestCase(1)]
    [TestCase(int.MinValue)]
    [TestCase(int.MaxValue)]
    [TestCase(0x55555555)]
    [TestCase(unchecked((int)0xAAAAAAAA))]
    [TestCase(0x12345678)]
    [TestCase(unchecked((int)0xF0F0F0F0))]
    public void NotWorks(int input)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new Not(Register.a0, input));
        program.Emit(new Exit());
        
        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());
        
        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");
        
        Assert.That(emulator.GetRegisterS(Register.a0), Is.EqualTo(~input));
    }

    [Test]
    [TestCase(0, 0)]
    [TestCase(-1, 0)]
    [TestCase(0, -1)]
    [TestCase(-1, -1)]
    [TestCase(-1, 0x12345678)]
    [TestCase(0x12345678, -1)]
    [TestCase(0x12345678, 0x12345678)]
    [TestCase(0x55555555, unchecked((int)0xAAAAAAAA))]
    [TestCase(unchecked((int)0xAAAAAAAA), unchecked((int)0xAAAAAAAA))]
    [TestCase(0x55555555, 0x55555555)]
    [TestCase(int.MinValue, int.MaxValue)]
    [TestCase(int.MinValue, -1)]
    [TestCase(int.MinValue, int.MinValue)]
    [TestCase(int.MaxValue, 1)]
    [TestCase(0x0F0F0F0F, 0x00FF00FF)]
    [TestCase(1, 2)]
    [TestCase(unchecked((int)0xF0F0F0F0), 0x0F0F0F0F)]
    public void AndWorks(int a, int b)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new And(Register.a0, a, b));
        program.Emit(new Exit());
        
        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());
        
        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");
        
        Assert.That(emulator.GetRegisterS(Register.a0), Is.EqualTo(a & b));
    }

    [Test]
    [TestCase(0, 0)]
    [TestCase(-1, 0)]
    [TestCase(0, -1)]
    [TestCase(-1, -1)]
    [TestCase(-1, 0x12345678)]
    [TestCase(0x12345678, -1)]
    [TestCase(0x12345678, 0x12345678)]
    [TestCase(0x55555555, unchecked((int)0xAAAAAAAA))]
    [TestCase(unchecked((int)0xAAAAAAAA), unchecked((int)0xAAAAAAAA))]
    [TestCase(0x55555555, 0x55555555)]
    [TestCase(int.MinValue, int.MaxValue)]
    [TestCase(int.MinValue, -1)]
    [TestCase(int.MinValue, int.MinValue)]
    [TestCase(int.MaxValue, 1)]
    [TestCase(0x0F0F0F0F, 0x00FF00FF)]
    [TestCase(1, 2)]
    [TestCase(unchecked((int)0xF0F0F0F0), 0x0F0F0F0F)]
    public void OrWorks(int a, int b)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new Or(Register.a0, a, b));
        program.Emit(new Exit());
        
        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());
        
        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");
        
        Assert.That(emulator.GetRegisterS(Register.a0), Is.EqualTo(a | b));
    }

    [Test]
    [TestCase(0, 0)]
    [TestCase(-1, 0)]
    [TestCase(0, -1)]
    [TestCase(-1, -1)]
    [TestCase(-1, 0x12345678)]
    [TestCase(0x12345678, -1)]
    [TestCase(0x12345678, 0x12345678)]
    [TestCase(0x55555555, unchecked((int)0xAAAAAAAA))]
    [TestCase(unchecked((int)0xAAAAAAAA), unchecked((int)0xAAAAAAAA))]
    [TestCase(0x55555555, 0x55555555)]
    [TestCase(int.MinValue, int.MaxValue)]
    [TestCase(int.MinValue, -1)]
    [TestCase(int.MinValue, int.MinValue)]
    [TestCase(int.MaxValue, 1)]
    [TestCase(0x0F0F0F0F, 0x00FF00FF)]
    [TestCase(1, 2)]
    [TestCase(unchecked((int)0xF0F0F0F0), 0x0F0F0F0F)]
    public void XorWorks(int a, int b)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new Xor(Register.a0, a, b));
        program.Emit(new Exit());
        
        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());
        
        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");
        
        Assert.That(emulator.GetRegisterS(Register.a0), Is.EqualTo(a ^ b));
    }
    
    [Test]
    [TestCase(1, 0)]
    [TestCase(1, 1)]
    [TestCase(1, 4)]
    [TestCase(1, 15)]
    [TestCase(1, 16)]
    [TestCase(1, 31)]
    [TestCase(1, 32)]
    [TestCase(1, 33)]
    [TestCase(1, 63)]
    [TestCase(1, 100)]
    [TestCase(1, -1)]
    [TestCase(1, -31)]
    [TestCase(1, -32)]
    [TestCase(-1, 0)]
    [TestCase(-1, 1)]
    [TestCase(-1, 4)]
    [TestCase(-1, 15)]
    [TestCase(-1, 16)]
    [TestCase(-1, 31)]
    [TestCase(-1, 32)]
    [TestCase(-1, 33)]
    [TestCase(-1, 63)]
    [TestCase(-1, 100)]
    [TestCase(-1, -1)]
    [TestCase(-1, -31)]
    [TestCase(-1, -32)]
    [TestCase(int.MinValue, 0)]
    [TestCase(int.MinValue, 1)]
    [TestCase(int.MinValue, 4)]
    [TestCase(int.MinValue, 15)]
    [TestCase(int.MinValue, 16)]
    [TestCase(int.MinValue, 31)]
    [TestCase(int.MinValue, 32)]
    [TestCase(int.MinValue, 33)]
    [TestCase(int.MinValue, 63)]
    [TestCase(int.MinValue, 100)]
    [TestCase(int.MinValue, -1)]
    [TestCase(int.MinValue, -31)]
    [TestCase(int.MinValue, -32)]
    [TestCase(int.MaxValue, 0)]
    [TestCase(int.MaxValue, 1)]
    [TestCase(int.MaxValue, 4)]
    [TestCase(int.MaxValue, 15)]
    [TestCase(int.MaxValue, 16)]
    [TestCase(int.MaxValue, 31)]
    [TestCase(int.MaxValue, 32)]
    [TestCase(int.MaxValue, 33)]
    [TestCase(int.MaxValue, 63)]
    [TestCase(int.MaxValue, 100)]
    [TestCase(int.MaxValue, -1)]
    [TestCase(int.MaxValue, -31)]
    [TestCase(int.MaxValue, -32)]
    [TestCase(0x12345678, 0)]
    [TestCase(0x12345678, 1)]
    [TestCase(0x12345678, 4)]
    [TestCase(0x12345678, 15)]
    [TestCase(0x12345678, 16)]
    [TestCase(0x12345678, 31)]
    [TestCase(0x12345678, 32)]
    [TestCase(0x12345678, 33)]
    [TestCase(0x12345678, 63)]
    [TestCase(0x12345678, 100)]
    [TestCase(0x12345678, -1)]
    [TestCase(0x12345678, -31)]
    [TestCase(0x12345678, -32)]
    [TestCase(unchecked((int)0x80000001), 0)]
    [TestCase(unchecked((int)0x80000001), 1)]
    [TestCase(unchecked((int)0x80000001), 4)]
    [TestCase(unchecked((int)0x80000001), 15)]
    [TestCase(unchecked((int)0x80000001), 16)]
    [TestCase(unchecked((int)0x80000001), 31)]
    [TestCase(unchecked((int)0x80000001), 32)]
    [TestCase(unchecked((int)0x80000001), 33)]
    [TestCase(unchecked((int)0x80000001), 63)]
    [TestCase(unchecked((int)0x80000001), 100)]
    [TestCase(unchecked((int)0x80000001), -1)]
    [TestCase(unchecked((int)0x80000001), -31)]
    [TestCase(unchecked((int)0x80000001), -32)]
    [TestCase(-8, 1)]
    [TestCase(-7, 1)]
    [TestCase(-1, 1)]
    [TestCase(-2, 1)]
    [TestCase(0, 31)]
    public void ArithmeticRightShiftWorks(int value, int shift)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new ArithmeticRightShift(Register.a0, value, shift));
        program.Emit(new Exit());
        
        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());
        
        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");
        
        Assert.That(emulator.GetRegisterS(Register.a0), Is.EqualTo(value >> shift));
    }
    
    [Test]
    [TestCase(1, 0)]
    [TestCase(1, 1)]
    [TestCase(1, 4)]
    [TestCase(1, 15)]
    [TestCase(1, 16)]
    [TestCase(1, 31)]
    [TestCase(1, 32)]
    [TestCase(1, 33)]
    [TestCase(1, 63)]
    [TestCase(1, 100)]
    [TestCase(1, -1)]
    [TestCase(1, -31)]
    [TestCase(1, -32)]
    [TestCase(-1, 0)]
    [TestCase(-1, 1)]
    [TestCase(-1, 4)]
    [TestCase(-1, 15)]
    [TestCase(-1, 16)]
    [TestCase(-1, 31)]
    [TestCase(-1, 32)]
    [TestCase(-1, 33)]
    [TestCase(-1, 63)]
    [TestCase(-1, 100)]
    [TestCase(-1, -1)]
    [TestCase(-1, -31)]
    [TestCase(-1, -32)]
    [TestCase(int.MinValue, 0)]
    [TestCase(int.MinValue, 1)]
    [TestCase(int.MinValue, 4)]
    [TestCase(int.MinValue, 15)]
    [TestCase(int.MinValue, 16)]
    [TestCase(int.MinValue, 31)]
    [TestCase(int.MinValue, 32)]
    [TestCase(int.MinValue, 33)]
    [TestCase(int.MinValue, 63)]
    [TestCase(int.MinValue, 100)]
    [TestCase(int.MinValue, -1)]
    [TestCase(int.MinValue, -31)]
    [TestCase(int.MinValue, -32)]
    [TestCase(int.MaxValue, 0)]
    [TestCase(int.MaxValue, 1)]
    [TestCase(int.MaxValue, 4)]
    [TestCase(int.MaxValue, 15)]
    [TestCase(int.MaxValue, 16)]
    [TestCase(int.MaxValue, 31)]
    [TestCase(int.MaxValue, 32)]
    [TestCase(int.MaxValue, 33)]
    [TestCase(int.MaxValue, 63)]
    [TestCase(int.MaxValue, 100)]
    [TestCase(int.MaxValue, -1)]
    [TestCase(int.MaxValue, -31)]
    [TestCase(int.MaxValue, -32)]
    [TestCase(0x12345678, 0)]
    [TestCase(0x12345678, 1)]
    [TestCase(0x12345678, 4)]
    [TestCase(0x12345678, 15)]
    [TestCase(0x12345678, 16)]
    [TestCase(0x12345678, 31)]
    [TestCase(0x12345678, 32)]
    [TestCase(0x12345678, 33)]
    [TestCase(0x12345678, 63)]
    [TestCase(0x12345678, 100)]
    [TestCase(0x12345678, -1)]
    [TestCase(0x12345678, -31)]
    [TestCase(0x12345678, -32)]
    [TestCase(unchecked((int)0x80000001), 0)]
    [TestCase(unchecked((int)0x80000001), 1)]
    [TestCase(unchecked((int)0x80000001), 4)]
    [TestCase(unchecked((int)0x80000001), 15)]
    [TestCase(unchecked((int)0x80000001), 16)]
    [TestCase(unchecked((int)0x80000001), 31)]
    [TestCase(unchecked((int)0x80000001), 32)]
    [TestCase(unchecked((int)0x80000001), 33)]
    [TestCase(unchecked((int)0x80000001), 63)]
    [TestCase(unchecked((int)0x80000001), 100)]
    [TestCase(unchecked((int)0x80000001), -1)]
    [TestCase(unchecked((int)0x80000001), -31)]
    [TestCase(unchecked((int)0x80000001), -32)]
    [TestCase(0x40000000, 1)]
    [TestCase(0, 31)]
    public void LogicalLeftShiftWorks(int value, int shift)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new LogicalLeftShift(Register.a0, value, shift));
        program.Emit(new Exit());
        
        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());
        
        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");
        
        Assert.That(emulator.GetRegisterS(Register.a0), Is.EqualTo(value << shift));
    }

    [Test]
    [TestCase(1u, 0)]
    [TestCase(1u, 1)]
    [TestCase(1u, 4)]
    [TestCase(1u, 15)]
    [TestCase(1u, 16)]
    [TestCase(1u, 31)]
    [TestCase(1u, 32)]
    [TestCase(1u, 33)]
    [TestCase(1u, 63)]
    [TestCase(1u, 100)]
    [TestCase(1u, -1)]
    [TestCase(1u, -31)]
    [TestCase(1u, -32)]
    [TestCase(uint.MaxValue, 0)]
    [TestCase(uint.MaxValue, 1)]
    [TestCase(uint.MaxValue, 4)]
    [TestCase(uint.MaxValue, 15)]
    [TestCase(uint.MaxValue, 16)]
    [TestCase(uint.MaxValue, 31)]
    [TestCase(uint.MaxValue, 32)]
    [TestCase(uint.MaxValue, 33)]
    [TestCase(uint.MaxValue, 63)]
    [TestCase(uint.MaxValue, 100)]
    [TestCase(uint.MaxValue, -1)]
    [TestCase(uint.MaxValue, -31)]
    [TestCase(uint.MaxValue, -32)]
    [TestCase(0x80000000u, 0)]
    [TestCase(0x80000000u, 1)]
    [TestCase(0x80000000u, 4)]
    [TestCase(0x80000000u, 15)]
    [TestCase(0x80000000u, 16)]
    [TestCase(0x80000000u, 31)]
    [TestCase(0x80000000u, 32)]
    [TestCase(0x80000000u, 33)]
    [TestCase(0x80000000u, 63)]
    [TestCase(0x80000000u, 100)]
    [TestCase(0x80000000u, -1)]
    [TestCase(0x80000000u, -31)]
    [TestCase(0x80000000u, -32)]
    [TestCase(0x7FFFFFFFu, 0)]
    [TestCase(0x7FFFFFFFu, 1)]
    [TestCase(0x7FFFFFFFu, 4)]
    [TestCase(0x7FFFFFFFu, 15)]
    [TestCase(0x7FFFFFFFu, 16)]
    [TestCase(0x7FFFFFFFu, 31)]
    [TestCase(0x7FFFFFFFu, 32)]
    [TestCase(0x7FFFFFFFu, 33)]
    [TestCase(0x7FFFFFFFu, 63)]
    [TestCase(0x7FFFFFFFu, 100)]
    [TestCase(0x7FFFFFFFu, -1)]
    [TestCase(0x7FFFFFFFu, -31)]
    [TestCase(0x7FFFFFFFu, -32)]
    [TestCase(0x12345678u, 0)]
    [TestCase(0x12345678u, 1)]
    [TestCase(0x12345678u, 4)]
    [TestCase(0x12345678u, 15)]
    [TestCase(0x12345678u, 16)]
    [TestCase(0x12345678u, 31)]
    [TestCase(0x12345678u, 32)]
    [TestCase(0x12345678u, 33)]
    [TestCase(0x12345678u, 63)]
    [TestCase(0x12345678u, 100)]
    [TestCase(0x12345678u, -1)]
    [TestCase(0x12345678u, -31)]
    [TestCase(0x12345678u, -32)]
    [TestCase(0x80000001u, 0)]
    [TestCase(0x80000001u, 1)]
    [TestCase(0x80000001u, 4)]
    [TestCase(0x80000001u, 15)]
    [TestCase(0x80000001u, 16)]
    [TestCase(0x80000001u, 31)]
    [TestCase(0x80000001u, 32)]
    [TestCase(0x80000001u, 33)]
    [TestCase(0x80000001u, 63)]
    [TestCase(0x80000001u, 100)]
    [TestCase(0x80000001u, -1)]
    [TestCase(0x80000001u, -31)]
    [TestCase(0x80000001u, -32)]
    [TestCase(0u, 31)]
    public void LogicalRightShiftWorks(uint value, int shift)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new LogicalRightShift(Register.a0, value, shift));
        program.Emit(new Exit());
        
        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());
        
        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");
        
        Assert.That(emulator.GetRegisterU(Register.a0), Is.EqualTo(value >> shift));
    }

    [Test]
    [TestCase(1, 0)]
    [TestCase(1, 1)]
    [TestCase(1, 4)]
    [TestCase(1, 15)]
    [TestCase(1, 16)]
    [TestCase(1, 31)]
    [TestCase(1, 32)]
    [TestCase(1, 33)]
    [TestCase(1, 63)]
    [TestCase(1, 100)]
    [TestCase(1, -1)]
    [TestCase(1, -31)]
    [TestCase(1, -32)]
    [TestCase(-1, 0)]
    [TestCase(-1, 1)]
    [TestCase(-1, 4)]
    [TestCase(-1, 15)]
    [TestCase(-1, 16)]
    [TestCase(-1, 31)]
    [TestCase(-1, 32)]
    [TestCase(-1, 33)]
    [TestCase(-1, 63)]
    [TestCase(-1, 100)]
    [TestCase(-1, -1)]
    [TestCase(-1, -31)]
    [TestCase(-1, -32)]
    [TestCase(int.MinValue, 0)]
    [TestCase(int.MinValue, 1)]
    [TestCase(int.MinValue, 4)]
    [TestCase(int.MinValue, 15)]
    [TestCase(int.MinValue, 16)]
    [TestCase(int.MinValue, 31)]
    [TestCase(int.MinValue, 32)]
    [TestCase(int.MinValue, 33)]
    [TestCase(int.MinValue, 63)]
    [TestCase(int.MinValue, 100)]
    [TestCase(int.MinValue, -1)]
    [TestCase(int.MinValue, -31)]
    [TestCase(int.MinValue, -32)]
    [TestCase(int.MaxValue, 0)]
    [TestCase(int.MaxValue, 1)]
    [TestCase(int.MaxValue, 4)]
    [TestCase(int.MaxValue, 15)]
    [TestCase(int.MaxValue, 16)]
    [TestCase(int.MaxValue, 31)]
    [TestCase(int.MaxValue, 32)]
    [TestCase(int.MaxValue, 33)]
    [TestCase(int.MaxValue, 63)]
    [TestCase(int.MaxValue, 100)]
    [TestCase(int.MaxValue, -1)]
    [TestCase(int.MaxValue, -31)]
    [TestCase(int.MaxValue, -32)]
    [TestCase(0x12345678, 0)]
    [TestCase(0x12345678, 1)]
    [TestCase(0x12345678, 4)]
    [TestCase(0x12345678, 15)]
    [TestCase(0x12345678, 16)]
    [TestCase(0x12345678, 31)]
    [TestCase(0x12345678, 32)]
    [TestCase(0x12345678, 33)]
    [TestCase(0x12345678, 63)]
    [TestCase(0x12345678, 100)]
    [TestCase(0x12345678, -1)]
    [TestCase(0x12345678, -31)]
    [TestCase(0x12345678, -32)]
    [TestCase(unchecked((int)0x80000001), 0)]
    [TestCase(unchecked((int)0x80000001), 1)]
    [TestCase(unchecked((int)0x80000001), 4)]
    [TestCase(unchecked((int)0x80000001), 15)]
    [TestCase(unchecked((int)0x80000001), 16)]
    [TestCase(unchecked((int)0x80000001), 31)]
    [TestCase(unchecked((int)0x80000001), 32)]
    [TestCase(unchecked((int)0x80000001), 33)]
    [TestCase(unchecked((int)0x80000001), 63)]
    [TestCase(unchecked((int)0x80000001), 100)]
    [TestCase(unchecked((int)0x80000001), -1)]
    [TestCase(unchecked((int)0x80000001), -31)]
    [TestCase(unchecked((int)0x80000001), -32)]
    [TestCase(0, 17)]
    public void RotateLeftWorks(int value, int shift)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new RotateLeft(Register.a0, value, shift));
        program.Emit(new Exit());
        
        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());
        
        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");
        
        Assert.That(emulator.GetRegisterS(Register.a0), Is.EqualTo(int.RotateLeft(value, shift)));
    }

    [Test]
    [TestCase(1, 0)]
    [TestCase(1, 1)]
    [TestCase(1, 4)]
    [TestCase(1, 15)]
    [TestCase(1, 16)]
    [TestCase(1, 31)]
    [TestCase(1, 32)]
    [TestCase(1, 33)]
    [TestCase(1, 63)]
    [TestCase(1, 100)]
    [TestCase(1, -1)]
    [TestCase(1, -31)]
    [TestCase(1, -32)]
    [TestCase(-1, 0)]
    [TestCase(-1, 1)]
    [TestCase(-1, 4)]
    [TestCase(-1, 15)]
    [TestCase(-1, 16)]
    [TestCase(-1, 31)]
    [TestCase(-1, 32)]
    [TestCase(-1, 33)]
    [TestCase(-1, 63)]
    [TestCase(-1, 100)]
    [TestCase(-1, -1)]
    [TestCase(-1, -31)]
    [TestCase(-1, -32)]
    [TestCase(int.MinValue, 0)]
    [TestCase(int.MinValue, 1)]
    [TestCase(int.MinValue, 4)]
    [TestCase(int.MinValue, 15)]
    [TestCase(int.MinValue, 16)]
    [TestCase(int.MinValue, 31)]
    [TestCase(int.MinValue, 32)]
    [TestCase(int.MinValue, 33)]
    [TestCase(int.MinValue, 63)]
    [TestCase(int.MinValue, 100)]
    [TestCase(int.MinValue, -1)]
    [TestCase(int.MinValue, -31)]
    [TestCase(int.MinValue, -32)]
    [TestCase(int.MaxValue, 0)]
    [TestCase(int.MaxValue, 1)]
    [TestCase(int.MaxValue, 4)]
    [TestCase(int.MaxValue, 15)]
    [TestCase(int.MaxValue, 16)]
    [TestCase(int.MaxValue, 31)]
    [TestCase(int.MaxValue, 32)]
    [TestCase(int.MaxValue, 33)]
    [TestCase(int.MaxValue, 63)]
    [TestCase(int.MaxValue, 100)]
    [TestCase(int.MaxValue, -1)]
    [TestCase(int.MaxValue, -31)]
    [TestCase(int.MaxValue, -32)]
    [TestCase(0x12345678, 0)]
    [TestCase(0x12345678, 1)]
    [TestCase(0x12345678, 4)]
    [TestCase(0x12345678, 15)]
    [TestCase(0x12345678, 16)]
    [TestCase(0x12345678, 31)]
    [TestCase(0x12345678, 32)]
    [TestCase(0x12345678, 33)]
    [TestCase(0x12345678, 63)]
    [TestCase(0x12345678, 100)]
    [TestCase(0x12345678, -1)]
    [TestCase(0x12345678, -31)]
    [TestCase(0x12345678, -32)]
    [TestCase(unchecked((int)0x80000001), 0)]
    [TestCase(unchecked((int)0x80000001), 1)]
    [TestCase(unchecked((int)0x80000001), 4)]
    [TestCase(unchecked((int)0x80000001), 15)]
    [TestCase(unchecked((int)0x80000001), 16)]
    [TestCase(unchecked((int)0x80000001), 31)]
    [TestCase(unchecked((int)0x80000001), 32)]
    [TestCase(unchecked((int)0x80000001), 33)]
    [TestCase(unchecked((int)0x80000001), 63)]
    [TestCase(unchecked((int)0x80000001), 100)]
    [TestCase(unchecked((int)0x80000001), -1)]
    [TestCase(unchecked((int)0x80000001), -31)]
    [TestCase(unchecked((int)0x80000001), -32)]
    [TestCase(0, 17)]
    public void RotateRightWorks(int value, int shift)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new RotateRight(Register.a0, value, shift));
        program.Emit(new Exit());

        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());

        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");
        
        Assert.That(emulator.GetRegisterS(Register.a0), Is.EqualTo(int.RotateRight(value, shift)));
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

    [Test]
    [TestCase(0u)]
    [TestCase(1u)]
    [TestCase(2u)]
    [TestCase(0x80000000u)]
    [TestCase(0x40000000u)]
    [TestCase(uint.MaxValue)]
    [TestCase(0x7FFFFFFFu)]
    [TestCase(0xFFFFFFFEu)]
    [TestCase(0x80000001u)]
    [TestCase(0x55555555u)]
    [TestCase(0xAAAAAAAAu)]
    [TestCase(0x12345678u)]
    [TestCase(0x0000FFFFu)]
    [TestCase(0xFFFF0000u)]
    [TestCase(0x00010000u)]
    [TestCase(0x00008000u)]
    [TestCase(0x0F0F0F0Fu)]
    [TestCase(0x000000F0u)]
    public void ReverseBitsWorks(uint value)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new ReverseBits(Register.a0, value));
        program.Emit(new Exit());
        
        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());
        
        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");
        
        Assert.That(emulator.GetRegisterU(Register.a0), Is.EqualTo(ReverseBits(value)));
    }

    [Test]
    [TestCase(0u)]
    [TestCase(1u)]
    [TestCase(2u)]
    [TestCase(0x80000000u)]
    [TestCase(0x40000000u)]
    [TestCase(uint.MaxValue)]
    [TestCase(0x7FFFFFFFu)]
    [TestCase(0xFFFFFFFEu)]
    [TestCase(0x80000001u)]
    [TestCase(0x55555555u)]
    [TestCase(0xAAAAAAAAu)]
    [TestCase(0x12345678u)]
    [TestCase(0x0000FFFFu)]
    [TestCase(0xFFFF0000u)]
    [TestCase(0x00010000u)]
    [TestCase(0x00008000u)]
    [TestCase(0x0F0F0F0Fu)]
    [TestCase(0x000000F0u)]
    public void PopulationCountWorks(uint value)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new PopulationCount(Register.a0, value));
        program.Emit(new Exit());
        
        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());
        
        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");
        
        Assert.That(emulator.GetRegisterU(Register.a0), Is.EqualTo(uint.PopCount(value)));
    }
    
    [Test]
    [TestCase(0u)]
    [TestCase(1u)]
    [TestCase(2u)]
    [TestCase(0x80000000u)]
    [TestCase(0x40000000u)]
    [TestCase(uint.MaxValue)]
    [TestCase(0x7FFFFFFFu)]
    [TestCase(0xFFFFFFFEu)]
    [TestCase(0x80000001u)]
    [TestCase(0x55555555u)]
    [TestCase(0xAAAAAAAAu)]
    [TestCase(0x12345678u)]
    [TestCase(0x0000FFFFu)]
    [TestCase(0xFFFF0000u)]
    [TestCase(0x00010000u)]
    [TestCase(0x00008000u)]
    [TestCase(0x0F0F0F0Fu)]
    [TestCase(0x000000F0u)]
    public void CountLeadingZerosWorks(uint value)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new CountLeadingZeros(Register.a0, value));
        program.Emit(new Exit());
        
        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());
        
        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");
        
        Assert.That(emulator.GetRegisterU(Register.a0), Is.EqualTo(uint.LeadingZeroCount(value)));
    }

    [Test]
    [TestCase(0u)]
    [TestCase(1u)]
    [TestCase(2u)]
    [TestCase(0x80000000u)]
    [TestCase(0x40000000u)]
    [TestCase(uint.MaxValue)]
    [TestCase(0x7FFFFFFFu)]
    [TestCase(0xFFFFFFFEu)]
    [TestCase(0x80000001u)]
    [TestCase(0x55555555u)]
    [TestCase(0xAAAAAAAAu)]
    [TestCase(0x12345678u)]
    [TestCase(0x0000FFFFu)]
    [TestCase(0xFFFF0000u)]
    [TestCase(0x00010000u)]
    [TestCase(0x00008000u)]
    [TestCase(0x0F0F0F0Fu)]
    [TestCase(0x000000F0u)]
    public void CountTrailingZerosWorks(uint value)
    {
        var program = new Program();
        program.Mark("main");
        
        program.Emit(new CountTrailingZeros(Register.a0, value));
        program.Emit(new Exit());
        
        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());
        
        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");
        
        Assert.That(emulator.GetRegisterU(Register.a0), Is.EqualTo(uint.TrailingZeroCount(value)));
    }

    
    
    [Test]
    [TestCase(0x61, 4, 3, -2)]
    [TestCase(0x61, 0, 1, -1)]
    [TestCase(0x61, 1, 4, 0)]
    [TestCase(0x61, 5, 2, -1)]
    [TestCase(0x7F, 0, 8, 0x7F)]
    [TestCase(0x80, 0, 8, -128)]
    [TestCase(0xFF, 0, 8, -1)]
    [TestCase(0x100, 0, 8, 0)]
    [TestCase(0x12345678, 4, 8, 0x67)]
    [TestCase(0x12345678, 8, 8, 0x56)]
    [TestCase(0x12345678, 16, 16, 0x1234)]
    [TestCase(unchecked((int)0xABCD0000), 16, 16, -21555)]
    [TestCase(0xF000, 12, 4, -1)]
    [TestCase(0x7000, 12, 4, 7)]
    [TestCase(0x8000, 15, 1, -1)]
    [TestCase(0x8000, 14, 2, -2)]
    [TestCase(int.MinValue, 31, 1, -1)]
    [TestCase(int.MaxValue, 31, 1, 0)]
    [TestCase(int.MaxValue, 0, 31, -1)]
    [TestCase(int.MaxValue, 1, 31, 0x3FFFFFFF)]
    [TestCase(int.MinValue, 0, 32, int.MinValue)]
    [TestCase(int.MaxValue, 0, 32, int.MaxValue)]
    [TestCase(0x12345678, 0, 32, 0x12345678)]
    [TestCase(-1, 0, 32, -1)]
    [TestCase(-1, 5, 7, -1)]
    [TestCase(0, 3, 10, 0)]
    [TestCase(-1, 0, 1, -1)]
    [TestCase(0x40000000, 30, 2, 1)]
    public void SignedBitfieldExtractWorks(int value, int offset, int width, int expected)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new SignedBitfieldExtract(Register.a0, value, offset, width));
        program.Emit(new Exit());
        
        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());
        
        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");
        
        Assert.That(emulator.GetRegisterS(Register.a0), Is.EqualTo(expected));
    }
    
    [Test]
    [TestCase(0x61u, 4u, 3u, 6u)]
    [TestCase(0x61u, 0u, 1u, 1u)]
    [TestCase(0x61u, 1u, 4u, 0u)]
    [TestCase(0x61u, 5u, 2u, 3u)]
    [TestCase(0x80u, 0u, 8u, 0x80u)]
    [TestCase(0xFFu, 0u, 8u, 0xFFu)]
    [TestCase(0x100u, 0u, 8u, 0u)]
    [TestCase(0x12345678u, 4u, 8u, 0x67u)]
    [TestCase(0x12345678u, 28u, 4u, 1u)]
    [TestCase(0x12345678u, 16u, 16u, 0x1234u)]
    [TestCase(0xABCD0000u, 16u, 16u, 0xABCDu)]
    [TestCase(0x80000000u, 31u, 1u, 1u)]
    [TestCase(0x7FFFFFFFu, 31u, 1u, 0u)]
    [TestCase(uint.MaxValue, 0u, 31u, 0x7FFFFFFFu)]
    [TestCase(uint.MaxValue, 1u, 31u, 0x7FFFFFFFu)]
    [TestCase(uint.MaxValue, 0u, 32u, uint.MaxValue)]
    [TestCase(0x12345678u, 0u, 32u, 0x12345678u)]
    [TestCase(0x80000000u, 0u, 32u, 0x80000000u)]
    [TestCase(uint.MaxValue, 5u, 7u, 0x7Fu)]
    [TestCase(0u, 3u, 10u, 0u)]
    [TestCase(0x12345678u, 0u, 1u, 0u)]
    [TestCase(0x12345679u, 0u, 1u, 1u)]
    [TestCase(0xC0000000u, 30u, 2u, 3u)]
    public void UnsignedBitfieldExtractWorks(uint value, uint offset, uint width, uint expected)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new UnsignedBitfieldExtract(Register.a0, value, offset, width));
        program.Emit(new Exit());
        
        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());
        
        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");
        
        Assert.That(emulator.GetRegisterU(Register.a0), Is.EqualTo(expected));
    }

    [Test]
    [TestCase(0x3Du, 6u, 5u, 2u, 0x5Du)]
    [TestCase(0u, uint.MaxValue, 0u, 32u, uint.MaxValue)]
    [TestCase(uint.MaxValue, 0u, 0u, 32u, 0u)]
    [TestCase(0x12345678u, 0x9ABCDEF0u, 0u, 32u, 0x9ABCDEF0u)]
    [TestCase(uint.MaxValue, 0u, 4u, 8u, 0xFFFFF00Fu)]
    [TestCase(0u, 0xFFu, 4u, 8u, 0xFF0u)]
    [TestCase(0u, 0x1FFu, 4u, 8u, 0xFF0u)]
    [TestCase(0u, uint.MaxValue, 4u, 8u, 0xFF0u)]
    [TestCase(0x12345678u, 0xAu, 28u, 4u, 0xA2345678u)]
    [TestCase(0x12345678u, 0x1Au, 28u, 4u, 0xA2345678u)]
    [TestCase(0x12345678u, 1u, 31u, 1u, 0x92345678u)]
    [TestCase(0x92345678u, 0u, 31u, 1u, 0x12345678u)]
    [TestCase(0x12345678u, 0u, 0u, 1u, 0x12345678u)]
    [TestCase(0x12345678u, 1u, 0u, 1u, 0x12345679u)]
    [TestCase(0x12345678u, 0xFFFFu, 16u, 16u, 0xFFFF5678u)]
    [TestCase(0x12345678u, 0xFFFFu, 0u, 16u, 0x1234FFFFu)]
    [TestCase(0xAAAAAAAAu, 0x5555u, 8u, 16u, 0xAA5555AAu)]
    [TestCase(0xAAAAAAAAu, uint.MaxValue, 0u, 31u, uint.MaxValue)]
    [TestCase(0xAAAAAAAAu, uint.MaxValue, 1u, 31u, 0xFFFFFFFEu)]
    [TestCase(uint.MaxValue, 0x55u, 12u, 8u, 0xFFF55FFFu)]
    [TestCase(0u, 1u, 15u, 1u, 0x8000u)]
    public void BitfieldInsertWorks(uint start, uint value, uint offset, uint width, uint expected)
    {
        var program = new Program();
        program.Mark("main");
        program.Emit(new Move(Register.a0, start));
        program.Emit(new BitfieldInsert(Register.a0, value, offset, width));
        program.Emit(new Exit());
        
        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());
        
        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");
        
        Assert.That(emulator.GetRegisterU(Register.a0), Is.EqualTo(expected));
    }
    
    [Test]
    [TestCase(0xB1u, 0x25u)]
    [TestCase(0x05u, 0xC1u)]
    [TestCase(0u, 0u)]
    [TestCase(uint.MaxValue, 0u)]
    [TestCase(0u, uint.MaxValue)]
    [TestCase(uint.MaxValue, uint.MaxValue)]
    [TestCase(0x12345678u, uint.MaxValue)]
    [TestCase(0x12345678u, 0u)]
    [TestCase(uint.MaxValue, 0x0000FFFFu)]
    [TestCase(uint.MaxValue, 0xAAAAAAAAu)]
    [TestCase(0x80000000u, 0x80000000u)]
    [TestCase(1u, 0x80000000u)]
    [TestCase(0x80000000u, 1u)]
    [TestCase(1u, 1u)]
    [TestCase(0x12345678u, 0xF0F0F0F0u)]
    [TestCase(0x12345678u, 0x0000FFFFu)]
    [TestCase(0x12345678u, 0xFFFF0000u)]
    [TestCase(0xAAAAAAAAu, 0x55555555u)]
    [TestCase(0xAAAAAAAAu, 0xAAAAAAAAu)]
    [TestCase(0x0000FFFFu, 0xAAAAAAAAu)]
    [TestCase(0x00001234u, 0xFFFF0000u)]
    [TestCase(0x12345678u, 0x0F0F0F0Fu)]
    [TestCase(0xDEADBEEFu, 0x80000001u)]
    [TestCase(0xDEADBEEFu, 0x00FF00FFu)]
    public void ParallelBitExtractWorks(uint value, uint mask)
    {
        Assume.That(Bmi2.IsSupported, "BMI2 is required for the oracle");

        var program = new Program();
        program.Mark("main");
        program.Emit(new ParallelBitExtract(Register.a0, value, mask));
        program.Emit(new Exit());

        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());

        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");

        Assert.That(emulator.GetRegisterU(Register.a0), Is.EqualTo(Bmi2.ParallelBitExtract(value, mask)));
    }

    [Test]
    [TestCase(0xB1u, 0x25u)]
    [TestCase(0x05u, 0xC1u)]
    [TestCase(0u, 0u)]
    [TestCase(uint.MaxValue, 0u)]
    [TestCase(0u, uint.MaxValue)]
    [TestCase(uint.MaxValue, uint.MaxValue)]
    [TestCase(0x12345678u, uint.MaxValue)]
    [TestCase(0x12345678u, 0u)]
    [TestCase(uint.MaxValue, 0x0000FFFFu)]
    [TestCase(uint.MaxValue, 0xAAAAAAAAu)]
    [TestCase(0x80000000u, 0x80000000u)]
    [TestCase(1u, 0x80000000u)]
    [TestCase(0x80000000u, 1u)]
    [TestCase(1u, 1u)]
    [TestCase(0x12345678u, 0xF0F0F0F0u)]
    [TestCase(0x12345678u, 0x0000FFFFu)]
    [TestCase(0x12345678u, 0xFFFF0000u)]
    [TestCase(0xAAAAAAAAu, 0x55555555u)]
    [TestCase(0xAAAAAAAAu, 0xAAAAAAAAu)]
    [TestCase(0x0000FFFFu, 0xAAAAAAAAu)]
    [TestCase(0x00001234u, 0xFFFF0000u)]
    [TestCase(0x12345678u, 0x0F0F0F0Fu)]
    [TestCase(0xDEADBEEFu, 0x80000001u)]
    [TestCase(0xDEADBEEFu, 0x00FF00FFu)]
    public void ParallelBitDepositWorks(uint value, uint mask)
    {
        Assume.That(Bmi2.IsSupported, "BMI2 is required for the oracle");

        var program = new Program();
        program.Mark("main");
        program.Emit(new ParallelBitDeposit(Register.a0, value, mask));
        program.Emit(new Exit());

        Console.WriteLine("--- EMULATED PROGRAM ---");
        Console.WriteLine(program.Dump());

        var emulator = new MnemonimovEmulator(0, program);
        emulator.RunProcess("main");

        Assert.That(emulator.GetRegisterU(Register.a0), Is.EqualTo(Bmi2.ParallelBitDeposit(value, mask)));
    }
}