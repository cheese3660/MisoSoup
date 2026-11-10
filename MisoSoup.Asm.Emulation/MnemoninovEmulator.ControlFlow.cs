namespace MisoSoup.Asm.Emulation;

public partial class MnemonimovEmulator
{
    public void Execute(Exit exit)
    {
        _exited = true;
    }
}