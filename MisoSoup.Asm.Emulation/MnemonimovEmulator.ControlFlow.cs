namespace MisoSoup.Asm.Emulation;

public partial class MnemonimovEmulator
{
    public void Execute(Exit exit)
    {
        _exited = true;
    }

    public void Execute(Nop nop)
    {
    }


    private int GetJumpAddress(Operand o)
    {
        var b = GetOperandS(o);
        if (o is DefinedOperand def) b += ProgramAddress;
        else b += _instructionSource;
        return b;
    }

    private int GetJumpAddressPa(Operand o) => GetOperandS(o) + ProgramAddress;

    public void Execute(Call cal)
    {
        StackPointer -= 4;
        Memory.Write(StackPointer, ProgramCounter);
        StackPointer -= 4;
        Memory.Write(StackPointer, FramePointer);
        FramePointer = StackPointer;
        ProgramCounter = GetJumpAddress(cal.Operand);
    }

    public void Execute(Return ret)
    {
        FramePointer = Memory.ReadInt(StackPointer);
        StackPointer += 4;
        ProgramCounter = Memory.ReadInt(StackPointer);
        StackPointer += 4;
    }

    public void Execute(CallPa cala)
    {
        StackPointer -= 4;
        Memory.Write(StackPointer, ProgramCounter);
        StackPointer -= 4;
        Memory.Write(StackPointer, FramePointer);
        FramePointer = StackPointer;
        ProgramCounter = GetJumpAddressPa(cala.Operand);
    }

    public void Execute(Jump jmp)
    {
        ProgramCounter = GetJumpAddress(jmp.Operand);
    }

    public void Execute(JumpPa jmpa)
    {
        ProgramCounter = GetJumpAddressPa(jmpa.Operand);
    }

    public void Execute(JumpTrue jtr)
    {
        if (ComparisonResult > 0) ProgramCounter = GetJumpAddress(jtr.Operand);
    }

    public void Execute(JumpTruePa jtra)
    {
        if (ComparisonResult > 0) ProgramCounter = GetJumpAddressPa(jtra.Operand);
    }

    public void Execute(JumpFalse jfs)
    {
        if (ComparisonResult == 0) ProgramCounter = GetJumpAddress(jfs.Operand);
    }

    public void Execute(JumpFalsePa jfsa)
    {
        if (ComparisonResult == 0) ProgramCounter = GetJumpAddressPa(jfsa.Operand);
    }
}