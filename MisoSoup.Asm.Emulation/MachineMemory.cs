namespace MisoSoup.Asm.Emulation;

public class MachineMemory
{
    public byte[] Memory;

    // This is the main thing used when you do Emulator.Run(Process Name)
    public Dictionary<string, int> GlobalLabels = [];

    // The first integer here is where the attached global label goes, and the second the actual memory location
    public Dictionary<string, List<(int, int)>> LocalLabels = [];

    // The integer here is the actual exact location of the label
    public Dictionary<string, List<int>> ReusableLabels = [];

    public Program Program;

    private int _pa;
    public MachineMemory(int stackSize, Program program)
    {
        List<byte> newMemory = [.. new byte[stackSize + MnemonimovEmulator.ReservedSpace]];
        _pa = newMemory.Count;
        var lastGlobalIndex = -1;
        var lastGlobalLabel = "";

        List<(int, Embed)> embeds = [];
        for (var i = 0; i <= program.Instructions.Count; i++)
        {
            var pc = newMemory.Count;
            if (program.Labels.TryGetValue(i, out var labels))
            {
                foreach (var label in labels)
                {
                    if (label.StartsWith('@'))
                    {
                        if (ReusableLabels.TryGetValue(label, out var reusableLabel))
                        {
                            reusableLabel.Add(pc);
                        }
                        else
                        {
                            ReusableLabels[label] = [pc];
                        }
                    }
                    else if (label.StartsWith('.'))
                    {
                        var combined = lastGlobalLabel + label;
                        GlobalLabels[combined] = pc;
                        if (LocalLabels.TryGetValue(label, out var localLabels))
                        {
                            localLabels.Add((lastGlobalIndex, pc));
                        }
                        else
                        {
                            LocalLabels[label] = [(lastGlobalIndex, pc)];
                        }
                    }
                    else
                    {
                        lastGlobalIndex = pc;
                        lastGlobalLabel = label;
                        GlobalLabels[label] = pc;
                    }
                }
            }

            if (i == program.Instructions.Count) break;

            switch (program.Instructions[i])
            {
                case Embed embed:
                    // Now this is where we have to do some fun things
                    embeds.Add((pc, embed));
                    newMemory.AddRange(embed.Type switch
                    {
                        MisaType.Uint8 => new byte[embed.Operands.Length],
                        MisaType.Uint16 => new byte[embed.Operands.Length * 2],
                        MisaType.Uint32 => new byte[embed.Operands.Length * 4],
                        MisaType.Int8 => new byte[embed.Operands.Length],
                        MisaType.Int16 => new byte[embed.Operands.Length * 2],
                        MisaType.Int32 => new byte[embed.Operands.Length * 4],
                        MisaType.Float32 => new byte[embed.Operands.Length * 4],
                        MisaType.File => throw new Exception("File embedding is not supported yet in emulation."),
                        MisaType.String => new byte[embed.Operands.OfType<StringOperand>()
                            .Select(x => x.Value.Length + 1).Sum()],
                        _ => throw new ArgumentOutOfRangeException()
                    });
                    break;
                case Reserve reserve:
                    newMemory.AddRange(reserve.Type switch
                    {
                        MisaType.Uint8 => new byte[reserve.Count],
                        MisaType.Uint16 => new byte[reserve.Count * 2],
                        MisaType.Uint32 => new byte[reserve.Count * 4],
                        MisaType.Int8 => new byte[reserve.Count],
                        MisaType.Int16 => new byte[reserve.Count * 2],
                        MisaType.Int32 => new byte[reserve.Count * 4],
                        MisaType.Float32 => new byte[reserve.Count * 4],
                        _ => throw new ArgumentOutOfRangeException()
                    });
                    break;
                default:
                    newMemory.AddRange(EndiannessUtility.GetBytes(i));
                    break;
            }
        }

        // Now that all the labels have been resolved we need to resolve the embeds
        foreach (var (offset, embed) in embeds)
        {
            var off = offset;
            foreach (var operand in embed.Operands)
            {
                switch (embed.Type)
                {
                    case MisaType.Uint8:
                        switch (operand)
                        {
                            case DefinedOperand definedOperand:
                            {
                                var definition = ResolveLabel(off, definedOperand.Label);
                                newMemory[off] = (byte)definition;
                                break;
                            }
                            case IntegerOperand integerOperand:
                                newMemory[off] = (byte)integerOperand.Value;
                                break;
                            default:
                                throw new Exception("Unsupported embedded operand");
                        }

                        off += 1;
                        break;
                    case MisaType.Uint16:
                        switch (operand)
                        {
                            case DefinedOperand definedOperand:
                            {
                                var definition = (ushort)ResolveLabel(off, definedOperand.Label);
                                var bytes = EndiannessUtility.GetBytes((short)definition);
                                newMemory[off] = bytes[0];
                                newMemory[off + 1] = bytes[1];
                                break;
                            }
                            case IntegerOperand integerOperand:
                            {
                                var bytes = EndiannessUtility.GetBytes((short)(ushort)integerOperand.Value);
                                newMemory[off] = bytes[0];
                                newMemory[off + 1] = bytes[1];
                                break;
                            }
                            default:
                                throw new Exception("Unsupported embedded operand");
                        }

                        off += 2;
                        break;
                    case MisaType.Uint32:
                    case MisaType.Int32:
                        switch (operand)
                        {
                            case DefinedOperand definedOperand:
                            {
                                var definition = ResolveLabel(off, definedOperand.Label);
                                var bytes = EndiannessUtility.GetBytes(definition);
                                newMemory[off] = bytes[0];
                                newMemory[off + 1] = bytes[1];
                                newMemory[off + 2] = bytes[2];
                                newMemory[off + 3] = bytes[3];
                                break;
                            }
                            case IntegerOperand integerOperand:
                            {
                                var bytes = EndiannessUtility.GetBytes(integerOperand.Value);
                                newMemory[off] = bytes[0];
                                newMemory[off + 1] = bytes[1];
                                newMemory[off + 2] = bytes[2];
                                newMemory[off + 3] = bytes[3];
                                break;
                            }
                            default:
                                throw new Exception("Unsupported embedded operand");
                        }

                        off += 4;
                        break;
                    case MisaType.Int8:
                        switch (operand)
                        {
                            case DefinedOperand definedOperand:
                            {
                                var definition = ResolveLabel(off, definedOperand.Label);
                                newMemory[off] = (byte)(sbyte)definition;
                                break;
                            }
                            case IntegerOperand integerOperand:
                                newMemory[off] = (byte)(sbyte)integerOperand.Value;
                                break;
                            default:
                                throw new Exception("Unsupported embedded operand");
                        }

                        off += 1;
                        break;
                    case MisaType.Int16:
                        switch (operand)
                        {
                            case DefinedOperand definedOperand:
                            {
                                var definition = (short)ResolveLabel(off, definedOperand.Label);
                                var bytes = EndiannessUtility.GetBytes(definition);
                                newMemory[off] = bytes[0];
                                newMemory[off + 1] = bytes[1];
                                break;
                            }
                            case IntegerOperand integerOperand:
                            {
                                var bytes = EndiannessUtility.GetBytes((short)integerOperand.Value);
                                newMemory[off] = bytes[0];
                                newMemory[off + 1] = bytes[1];
                                break;
                            }
                            default:
                                throw new Exception("Unsupported embedded operand");
                        }

                        off += 2;
                        break;
                    case MisaType.Float32:
                        switch (operand)
                        {
                            case DefinedOperand definedOperand:
                            {
                                var definition = ResolveLabel(off, definedOperand.Label);
                                var bytes = EndiannessUtility.GetBytes(definition);
                                newMemory[off] = bytes[0];
                                newMemory[off + 1] = bytes[1];
                                newMemory[off + 2] = bytes[2];
                                newMemory[off + 3] = bytes[3];
                                break;
                            }
                            case IntegerOperand integerOperand:
                            {
                                var bytes = EndiannessUtility.GetBytes((float)integerOperand.Value);
                                newMemory[off] = bytes[0];
                                newMemory[off + 1] = bytes[1];
                                newMemory[off + 2] = bytes[2];
                                newMemory[off + 3] = bytes[3];
                                break;
                            }
                            case FloatOperand floatOperand:
                            {
                                var bytes = EndiannessUtility.GetBytes(floatOperand.Value);
                                newMemory[off] = bytes[0];
                                newMemory[off + 1] = bytes[1];
                                newMemory[off + 2] = bytes[2];
                                newMemory[off + 3] = bytes[3];
                                break;
                            }
                            default:
                                throw new Exception("Unsupported embedded operand");
                        }

                        off += 4;
                        break;
                    case MisaType.String:
                        if (operand is not StringOperand stringOperand)
                            throw new Exception("Unsupported embedded operand");
                        foreach (var c in stringOperand.Value)
                        {
                            newMemory[off] = (byte)c;
                            off += 1;
                        }

                        newMemory[off++] = 0;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        Memory = [.. newMemory];
        Program = program;
    }

    public int ResolveLabel(int sourceAddress, string label)
    {
        if (Constants.TryGetConstantValue(label) is { } result) return result;

        if (label == "$")
        {
            return sourceAddress - _pa;
        }

        if (label.StartsWith('@'))
        {
            var reusableLabels = ReusableLabels[label[..^1]];
            // Now we need to get the closest before and after
            var closestPrior = 0;
            var closestAfter = int.MaxValue;

            foreach (var lab in reusableLabels)
            {
                if (lab <= sourceAddress && lab > closestPrior) closestPrior = lab;
                if (lab > sourceAddress && lab < closestAfter) closestAfter = lab;
            }

            return label[^1] == '+' ? closestAfter - _pa : closestPrior - _pa;
        }

        if (!label.StartsWith('.')) return GlobalLabels[label] - _pa;

        var localLabels = LocalLabels[label];
        var closestGlobalIndex = 0;
        var closestLabel = 0;
        foreach (var (glob, loc) in localLabels)
        {
            if (glob <= sourceAddress && glob > closestGlobalIndex)
            {
                closestGlobalIndex = glob;
                closestLabel = loc;
            }
        }

        return closestLabel - _pa;
    }

    public int ResolveLabel(string label) =>
        Constants.TryGetConstantValue(label) is { } result ? result : GlobalLabels[label];

    public Instruction ResolveInstruction(int instRef) => Program.Instructions[instRef];

    public byte ReadByte(int absolute) => Memory[absolute];
    public sbyte ReadSbyte(int absolute) => (sbyte)Memory[absolute];
    public short ReadShort(int absolute) => EndiannessUtility.ShortFromBytes(Memory[absolute .. (absolute + 2)]);
    public ushort ReadUShort(int absolute) => (ushort)ReadShort(absolute);
    public int ReadInt(int absolute) => EndiannessUtility.IntFromBytes(Memory[absolute..(absolute + 4)]);
    public uint ReadUInt(int absolute) => (uint)ReadInt(absolute);
    public float ReadFloat(int absolute) => EndiannessUtility.FloatFromBytes(Memory[absolute..(absolute + 4)]);
    public Instruction ReadInstruction(int absolute) => ResolveInstruction(ReadInt(absolute));

    public void Write(int absolute, byte value) => Memory[absolute] = value;
    public void Write(int absolute, sbyte value) => Memory[absolute] = unchecked((byte)value);

    public void Write(int absolute, short value)
    {
        var bytes = EndiannessUtility.GetBytes(value);
        Memory[absolute] = bytes[0];
        Memory[absolute + 1] = bytes[1];
    }

    public void Write(int absolute, ushort value) => Write(absolute, (short)value);

    public void Write(int absolute, int value)
    {
        var bytes = EndiannessUtility.GetBytes(value);
        Memory[absolute] = bytes[0];
        Memory[absolute + 1] = bytes[1];
        Memory[absolute + 2] = bytes[2];
        Memory[absolute + 3] = bytes[3];
    }

    public void Write(int absolute, uint value) => Write(absolute, (int)value);

    public void Write(int absolute, float value)
    {
        var bytes = EndiannessUtility.GetBytes(value);
        Memory[absolute] = bytes[0];
        Memory[absolute + 1] = bytes[1];
        Memory[absolute + 2] = bytes[2];
        Memory[absolute + 3] = bytes[3];
    }
}