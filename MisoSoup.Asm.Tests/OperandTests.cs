using System.Globalization;

namespace MisoSoup.Asm.Tests;

public class OperandTests
{
    [TestCase(0)]
    [TestCase(1)]
    [TestCase(-1)]
    [TestCase(10)]
    [TestCase(16777216)]
    [TestCase(16777217)]
    [TestCase(123456789)]
    [TestCase(999999999)]
    [TestCase(1000000000)]
    [TestCase(-1000000000)]
    [TestCase(1500000000)]
    [TestCase(-1500000000)]
    [TestCase(int.MaxValue)]
    [TestCase(int.MinValue)]
    public void FloatPrintsIntegerWithDot(int x)
    {
        Operand i = (float)x;
        Assert.That(i.ToString(), Contains.Substring("."));
    }

    [Test]
    public void FloatNanUsesConstant()
    {
        Operand i = float.NaN;
        Assert.That(i.ToString(), Is.EqualTo("NAN"));
    }

    [Test]
    public void FloatInfUsesConstant()
    {
        Operand i = float.PositiveInfinity;
        Assert.That(i.ToString(), Is.EqualTo("INF"));
    }

    [Test]
    public void FloatNegativeInfUsesConstant()
    {
        Operand i = float.NegativeInfinity;
        Assert.That(i.ToString(), Is.EqualTo("-INF"));
    }

    // Plain "R" output, no exponent
    [TestCase(0f)]
    [TestCase(-0f)]
    [TestCase(1f)]
    [TestCase(-1f)]
    [TestCase(0.1f)]
    [TestCase(0.3f)]
    [TestCase(1.0000001f)]
    [TestCase(15.3f)]
    [TestCase(0.0001f)]
    [TestCase(123456789f)]
    // Negative exponent, single digit mantissa
    [TestCase(1e-5f)]
    [TestCase(-1e-5f)]
    [TestCase(1e-10f)]
    // Negative exponent, fractional mantissa
    [TestCase(1.5e-5f)]
    [TestCase(-1.5e-7f)]
    [TestCase(1.2345e-7f)]
    [TestCase(-1.2345e-7f)]
    // Subnormals and smallest normal
    [TestCase(float.Epsilon)]
    [TestCase(-float.Epsilon)]
    [TestCase(1.17549435e-38f)]
    // Positive exponent, single digit mantissa
    [TestCase(1e9f)]
    [TestCase(1e15f)]
    [TestCase(-1e15f)]
    [TestCase(1e20f)]
    // Positive exponent, mantissa digits fewer than the exponent
    [TestCase(1.5e15f)]
    [TestCase(-1.5e20f)]
    [TestCase(1.2345678e16f)]
    [TestCase(2.1474836e9f)]
    [TestCase(-2.1474836e9f)]
    [TestCase(1.2345678e9f)]
    [TestCase(-1.2345678e9f)]
    [TestCase(9.876543e9f)]
    // Extremes
    [TestCase(float.MaxValue)]
    [TestCase(float.MinValue)]
    public void FloatRoundTripsProperly(float f)
    {
        Operand i = f;
        Assert.That(float.Parse(i.ToString(), CultureInfo.InvariantCulture), Is.EqualTo(f));
    }
    
    [Test]
    public void TrueBecomesTrue()
    {
        Operand b = true;
        Assert.That(b.ToString(), Is.EqualTo("true"));
    }

    [Test]
    public void FalseBecomesFalse()
    {
        Operand b = false;
        Assert.That(b.ToString(), Is.EqualTo("false"));
    }

    [Test]
    public void IntegerBecomesInteger()
    {
        Operand i = 5;
        Assert.That(i, Is.EqualTo(new IntegerOperand(5)));
    }

    [Test]
    public void FloatBecomesFloat()
    {
        Operand f = 5f;
        Assert.That(f, Is.EqualTo(new FloatOperand(5)));
    }

    [Test]
    public void UintBecomesUnsigned()
    {
        Operand u = 5u;
        Assert.That(u, Is.EqualTo(new UnsignedOperand(5u)));
    }

    [Test]
    public void StringBecomesDefined()
    {
        Operand s = "defineme";
        Assert.That(s, Is.EqualTo(new DefinedOperand("defineme")));
    }

    [Test]
    public void RegisterBecomesRegister()
    {
        Operand r = Register.a5;
        Assert.That(r, Is.EqualTo(new RegisterOperand(Register.a5)));
    }
    
    [Test]
    public void RegisterVectorBecomesVector()
    {
        Operand r = Register.a5 + 3;
        Assert.That(r, Is.EqualTo(new VectorOperand(Register.a5 + 3)));
    }
}