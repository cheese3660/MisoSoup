namespace MisoSoup.Asm.Tests;

public class RegisterTests
{
    [Test]
    public void RegistersPrintCorrectly()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(Register.a0.ToString(), Is.EqualTo("a0"));
            Assert.That(Register.s0.ToString(), Is.EqualTo("s0"));
            Assert.That(Register.t0.ToString(), Is.EqualTo("t0"));
            Assert.That(Register.zr.ToString(), Is.EqualTo("zr"));
            Assert.That(Register.cr.ToString(), Is.EqualTo("cr"));
            Assert.That(Register.ea.ToString(), Is.EqualTo("ea"));
            Assert.That(Register.pa.ToString(), Is.EqualTo("pa"));
            Assert.That(Register.ba.ToString(), Is.EqualTo("ba"));
            Assert.That(Register.sp.ToString(), Is.EqualTo("sp"));
            Assert.That(Register.fp.ToString(), Is.EqualTo("fp"));
        }
    }
    
    [Test]
    public void RegisterVectorsPrintCorrectly()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That((Register.a3 + 3).LongString(), Is.EqualTo("a3..a5"));
            Assert.That((Register.a3 + 3).ToString(), Is.EqualTo("a3.."));
            Assert.That((Register.s3 + 3).LongString(), Is.EqualTo("s3..s5"));
            Assert.That((Register.s3 + 3).ToString(), Is.EqualTo("s3.."));
            Assert.That((Register.t3 + 3).LongString(), Is.EqualTo("t3..t5"));
            Assert.That((Register.t3 + 3).ToString(), Is.EqualTo("t3.."));
        }
    }

    [Test]
    public void RegisterVectorCreationWorks()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(Register.a0 + 3, Is.EqualTo(new RegisterVector(RegisterBank.Argument, 0, 3)));
            Assert.That(Register.s15 + 3, Is.EqualTo(new RegisterVector(RegisterBank.Saved, 15, 3)));
            Assert.That(Register.s3 + Register.s4, Is.EqualTo(new RegisterVector(RegisterBank.Saved, 3, 2)));
            Assert.That((Register.a2 + 1) + 1, Is.EqualTo(new RegisterVector(RegisterBank.Argument, 2, 2)));
        }
    }
}