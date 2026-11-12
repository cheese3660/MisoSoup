namespace MisoSoup.Asm.Tests;

public class ProgramTests
{
    [Test]
    public void SingularLabelEmitsProperly()
    {
        var program = new Program();
        program.Mark("test");
        Assert.That(program.Dump(), Is.EqualTo("test:" + Environment.NewLine));
    }
}