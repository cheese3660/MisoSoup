namespace MisoSoup.Asm.Emulation;

public partial class MnemonimovEmulator
{
    public void Execute(Load load)
    {
        var address = GetOperandS(load.Address) + ProgramAddress;
        switch (load.Type)
        {
            case MisaType.Uint8:
                SetRegister(load.Destination, (uint)Memory.ReadByte(address));
                break;
            case MisaType.Uint16:
                SetRegister(load.Destination, (uint)Memory.ReadUShort(address));
                break;
            case MisaType.Uint32:
                SetRegister(load.Destination, Memory.ReadUInt(address));
                break;
            case MisaType.Int8:
                SetRegister(load.Destination, Memory.ReadSbyte(address));
                break;
            case MisaType.Int16:
                SetRegister(load.Destination, Memory.ReadShort(address));
                break;
            case MisaType.Int32:
                SetRegister(load.Destination, Memory.ReadInt(address));
                break;
            case MisaType.Float32:
                SetRegister(load.Destination, Memory.ReadFloat(address));
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void Execute(Store store)
    {
        var address = GetOperandS(store.Address) + ProgramAddress;
        switch (store.Type)
        {
            case MisaType.Uint8:
                Memory.Write(address, (byte)GetOperandU(store.Value));
                break;
            case MisaType.Uint16:
                Memory.Write(address, (ushort)GetOperandU(store.Value));
                break;
            case MisaType.Uint32:
                Memory.Write(address, GetOperandU(store.Value));
                break;
            case MisaType.Int8:
                Memory.Write(address, (sbyte)GetOperandS(store.Value));
                break;
            case MisaType.Int16:
                Memory.Write(address, (short)GetOperandS(store.Value));
                break;
            case MisaType.Int32:
                Memory.Write(address, GetOperandS(store.Value));
                break;
            case MisaType.Float32:
                Memory.Write(address, GetOperandF(store.Value));
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void Execute(ComputeEffectiveAddress cea)
    {
        var baseAddress = GetOperandS(cea.BaseAddress) + ProgramAddress;
        var offset = GetOperandS(cea.Index) * GetOperandS(cea.Scale);
        EffectiveAddress = baseAddress + offset;
    }

    public void Execute(LoadEffective lde)
    {
        var address = EffectiveAddress + GetOperandS(lde.Displacement);
        switch (lde.Type)
        {
            case MisaType.Uint8:
                SetRegister(lde.Destination, (uint)Memory.ReadByte(address));
                break;
            case MisaType.Uint16:
                SetRegister(lde.Destination, (uint)Memory.ReadUShort(address));
                break;
            case MisaType.Uint32:
                SetRegister(lde.Destination, Memory.ReadUInt(address));
                break;
            case MisaType.Int8:
                SetRegister(lde.Destination, Memory.ReadSbyte(address));
                break;
            case MisaType.Int16:
                SetRegister(lde.Destination, Memory.ReadShort(address));
                break;
            case MisaType.Int32:
                SetRegister(lde.Destination, Memory.ReadInt(address));
                break;
            case MisaType.Float32:
                SetRegister(lde.Destination, Memory.ReadFloat(address));
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void Execute(StoreEffective ste)
    {
        var address = EffectiveAddress + GetOperandS(ste.Displacement);
        switch (ste.Type)
        {
            case MisaType.Uint8:
                Memory.Write(address, (byte)GetOperandU(ste.Value));
                break;
            case MisaType.Uint16:
                Memory.Write(address, (ushort)GetOperandU(ste.Value));
                break;
            case MisaType.Uint32:
                Memory.Write(address, GetOperandU(ste.Value));
                break;
            case MisaType.Int8:
                Memory.Write(address, (sbyte)GetOperandS(ste.Value));
                break;
            case MisaType.Int16:
                Memory.Write(address, (short)GetOperandS(ste.Value));
                break;
            case MisaType.Int32:
                Memory.Write(address, GetOperandS(ste.Value));
                break;
            case MisaType.Float32:
                Memory.Write(address, GetOperandF(ste.Value));
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void Execute(ToProgramRelative tpr)
    {
        SetRegister(tpr.Dest, GetOperandS(tpr.Address) - ProgramAddress);
    }

    public void Execute(ToProgramAbsolute tpa)
    {
        SetRegister(tpa.Dest, GetOperandS(tpa.Address) + ProgramAddress);
    }

    public void Execute(Push psh)
    {
        StackPointer -= 4;
        Memory.Write(StackPointer, GetOperandS(psh.Value));
    }

    public void Execute(Pop pop)
    {
        SetRegister(pop.Dest, Memory.ReadInt(StackPointer));
        StackPointer += 4;
    }

    public void Execute(Move mov)
    {
        SetRegister(mov.Destination, GetOperandS(mov.Source));
    }

    public void Execute(ConditionalMove mvc)
    {
        if (ComparisonResult != 0) SetRegister(mvc.Destination, GetOperandS(mvc.Source));
    }

    public void Execute(Select sel)
    {
        SetRegister(sel.Destination, ComparisonResult != 0 ? GetOperandS(sel.IfTrue) : GetOperandS(sel.IfFalse));
    }

    public void Execute(Swap swp)
    {
        var b = GetRegisterS(swp.B);
        SetRegister(swp.B, GetRegisterS(swp.A));
        SetRegister(swp.A, b);
    }
}