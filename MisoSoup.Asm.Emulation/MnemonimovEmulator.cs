using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace MisoSoup.Asm.Emulation;

public partial class MnemonimovEmulator
{
    // So how do we want to do the data structure for this, how are we going to implement reading at the end too to
    // validate the results

    public const int ReservedSpace = 256;
    
    private readonly int[] _argumentRegisters = new int[16];
    private readonly int[] _temporaryRegisters = new int[16];
    private readonly int[] _savedRegisters = new int[32];
    private readonly int[] _specialRegisters = new int[8];
    
    public int Zero => _specialRegisters[0];

    public int ComparisonResult
    {
        get => _specialRegisters[1];
        set => _specialRegisters[1] = value;
    }

    public int EffectiveAddress
    {
        get => _argumentRegisters[2];
        set => _argumentRegisters[2] = value;
    }
    
    public int ProgramAddress => _specialRegisters[3];
    public int BackBufferAddress => _specialRegisters[4];

    public int StackPointer
    {
        get => _argumentRegisters[5];
        set => _argumentRegisters[5] = value;
    }

    public int FramePointer
    {
        get => _argumentRegisters[6];
        set => _argumentRegisters[6] = value;
    }

    public int ProgramCounter
    {
        get => _argumentRegisters[7];
        set => _argumentRegisters[7] = value;
    }

    private static readonly bool[] WriteableSpecialRegisters = [false, true, true, false, false, true, true, true];

    public MachineMemory Memory;
    
    public MnemonimovEmulator(int stackSize, Program emulatedProgram)
    {
        _specialRegisters[3] = stackSize + ReservedSpace;
        StackPointer = stackSize + ReservedSpace;
        // Give a fake back buffer address to make stuff make sense
        _specialRegisters[4] = ReservedSpace;
        Memory = new MachineMemory(stackSize, emulatedProgram);
    }

    public int GetRegisterS(Register register) =>
        register.Bank switch
        {
            RegisterBank.Temporary => _temporaryRegisters[register.Number],
            RegisterBank.Argument => _argumentRegisters[register.Number],
            RegisterBank.Saved => _savedRegisters[register.Number],
            RegisterBank.Special => _specialRegisters[register.Number],
            _ => throw new ArgumentOutOfRangeException()
        };

    public void SetRegisterS(Register register, int value)
    {
        switch (register.Bank)
        {
            case RegisterBank.Temporary:
                _temporaryRegisters[register.Number] = value;
                break;
            case RegisterBank.Argument:
                _argumentRegisters[register.Number] = value;
                break;
            case RegisterBank.Saved:
                _savedRegisters[register.Number] = value;
                break;
            case RegisterBank.Special:
                if (WriteableSpecialRegisters[register.Number])
                {
                    _specialRegisters[register.Number] = value;
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public uint GetRegisterU(Register register) => Unsafe.BitCast<int, uint>(GetRegisterS(register));

    public void SetRegisterU(Register register, uint value) => SetRegisterS(register, Unsafe.BitCast<uint, int>(value));
    
    public float GetRegisterF(Register register) => Unsafe.BitCast<int, float>(GetRegisterS(register));
    
    public void SetRegisterF(ref Register register, float value) => SetRegisterS(register, Unsafe.BitCast<float, int>(value));

    public List<int> GetRegisterVectorS(RegisterVector registers)
    {
        return registers.Bank switch
        {
            RegisterBank.Temporary => [.. _temporaryRegisters[registers.Start .. (registers.Start + registers.Count)]],
            RegisterBank.Argument => [.. _argumentRegisters[registers.Start .. (registers.Start + registers.Count)]],
            RegisterBank.Saved => [.. _savedRegisters[registers.Start .. (registers.Start + registers.Count)]],
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public List<uint> GetRegisterVectorU(RegisterVector registers) =>
        [.. GetRegisterVectorS(registers).Select(Unsafe.BitCast<int, uint>)];

    public List<float> GetRegisterVectorF(RegisterVector registers) =>
        [.. GetRegisterVectorS(registers).Select(Unsafe.BitCast<int, float>)];

    private bool _exited;
    private int _instructionSource;
    
    // This is how the machine actually runs a process
    public void RunProcess(string processName)
    {
        ProgramCounter = Memory.ResolveLabel(processName);
        _exited = false;
        while (!_exited)
        {
            var instruction = Memory.ReadInstruction(ProgramCounter);
            _instructionSource = ProgramCounter;
            ProgramCounter += 4;
            Dispatch(instruction);
        }
    }

    private static Dictionary<Type, Action<MnemonimovEmulator, Instruction>> _instructions = [];

    static MnemonimovEmulator()
    {
        foreach (var method in typeof(MnemonimovEmulator).GetMethods().Where(m => m.Name == "Execute" && m.GetParameters().Length == 1))
        {
            // _instructions[method.GetParameters()[0].ParameterType] =
            //     method.CreateDelegate<Action<MnemonimovEmulator, Instruction>>();
            var paramType = method.GetParameters()[0].ParameterType;
            
            var emuParam = Expression.Parameter(typeof(MnemonimovEmulator), "emu");
            var instParam = Expression.Parameter(typeof(Instruction), "inst");
            
            var castInst = Expression.Convert(instParam, paramType);
            var callExpr = Expression.Call(emuParam, method, castInst);

            var lambda = Expression.Lambda<Action<MnemonimovEmulator, Instruction>>(callExpr, emuParam, instParam);

            _instructions[paramType] = lambda.Compile();
        }
    }

    private void Dispatch(Instruction instruction)
    {
        _instructions[instruction.GetType()](this, instruction);
    }
}