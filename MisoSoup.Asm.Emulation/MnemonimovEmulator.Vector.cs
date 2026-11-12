namespace MisoSoup.Asm.Emulation;

public partial class MnemonimovEmulator
{
    public void Execute(VectorPush vpsh)
    {
        var v = GetRegisterVectorS(vpsh.Range);
        foreach (var value in v)
        {
            StackPointer -= 4;
            Memory.Write(StackPointer, value);
        }
    }

    public void Execute(VectorPop vpop)
    {
        var results = new int[vpop.Range.Count];
        for (var i = vpop.Range.Count - 1; i >= 0; i--)
        {
            results[i] = Memory.ReadInt(StackPointer);
            StackPointer += 4;
        }

        SetRegister(vpop.Range, results);
    }

    public void Execute(VectorMove vmov)
    {
        var count = vmov.Destination.Count;
        SetRegister(vmov.Destination, GetOperandVectorS(vmov.Source, count));
    }

    public void Execute(VectorFloatAdd vfadd)
    {
        var count = vfadd.Destination.Count;
        SetRegister(vfadd.Destination,
            [.. GetOperandVectorF(vfadd.A, count).Zip(GetOperandVectorF(vfadd.B, count), (a, b) => a + b)]);
    }

    public void Execute(VectorFloatSubtract vfsub)
    {
        var count = vfsub.Destination.Count;
        SetRegister(vfsub.Destination,
            [.. GetOperandVectorF(vfsub.A, count).Zip(GetOperandVectorF(vfsub.B, count), (a, b) => a - b)]);
    }

    public void Execute(VectorFloatMultiply vfmul)
    {
        var count = vfmul.Destination.Count;
        SetRegister(vfmul.Destination,
            [.. GetOperandVectorF(vfmul.A, count).Zip(GetOperandVectorF(vfmul.B, count), (a, b) => a * b)]);
    }

    public void Execute(VectorFloatFusedMultiplyAdd vffma)
    {
        var count = vffma.Destination.Count;
        SetRegister(vffma.Destination,
        [
            .. GetOperandVectorF(vffma.A, count)
                .Zip(GetOperandVectorF(vffma.B, count), GetOperandVectorF(vffma.C, count))
                .Select(((float a, float b, float c) t) => float.FusedMultiplyAdd(t.a, t.b, t.c))
        ]);
    }

    public void Execute(VectorFloatDivide vfdiv)
    {
        var count = vfdiv.Destination.Count;
        SetRegister(vfdiv.Destination,
            [.. GetOperandVectorF(vfdiv.A, count).Zip(GetOperandVectorF(vfdiv.B, count), (a, b) => a / b)]);
    }
}