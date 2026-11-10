using System.Text;

namespace MisoSoup.Asm;

public class Program
{
    public List<Instruction> Instructions = [];
    // public List<List<string>> Labels = [];
    
    public Dictionary<int, List<string>> Labels = [];
    
    public void Mark(string label)
    {
        if (Labels.TryGetValue(Instructions.Count, out var list))
        {
            list.Add(label);
        }
        else
        {
            Labels[Instructions.Count] = [label];
        }
    }

    public void Emit(Instruction instruction)
    {
        Instructions.Add(instruction);
    }

    public string Dump()
    {
        StringBuilder sb = new();
        for (var i = 0; i < Instructions.Count; i++)
        {
            if (Labels.TryGetValue(i, out var labels))
            {
                foreach (var label in labels)
                {
                    sb.AppendLine($"{label}:");
                }
            }

            sb.AppendLine($"    {Instructions[i].Text}");
        }

        if (Labels.TryGetValue(Instructions.Count, out var list))
        {
            foreach (var label in list)
            {
                sb.AppendLine($"{label}:");
            }
        }
        
        return sb.ToString();
    }
}