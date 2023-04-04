using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logically;

public class ChipInstance
{
    public ChipDefinition Parent;
    public Dictionary<string, bool> WireStates = new();
    public Dictionary<string, bool> NewWireStates = new();

    public HashSet<ChipConnectionInstance> Connections = new();

    public ChipInstance(ChipDefinition parent)
    {
        Parent = parent;
        parent.Inputs.ForEach(c => NewWireStates[RemoveHigh(c)] = StartHigh(c));
        parent.Outputs.ForEach(c => NewWireStates[RemoveHigh(c)] = StartHigh(c));
        parent.Buslines.ForEach(c => NewWireStates[RemoveHigh(c)] = StartHigh(c));
        parent.Connections.ToList().ForEach(c => Connections.Add(new ChipConnectionInstance()
        {
            Chip = new ChipInstance(ChipDefinition.AllChips[c.TargetChip]),
            Parent = c
        }));
        Swap();
    }

    private static bool StartHigh(string name)
    {
        return name.EndsWith("_HIGH");
    }
    public static string RemoveHigh(string name)
    {
        if (StartHigh(name)) return name.Replace("_HIGH", "");
        return name;
    }

    public void Swap()
    {
        foreach (var kv in NewWireStates)
        {
            WireStates[kv.Key] = kv.Value;
        }
        foreach (var chipConnInst in Connections)
        {
            chipConnInst.Chip.Swap();
        }
    }
    public override string ToString()
    {
        StringBuilder sb = new();
        sb.Append('@');
        sb.AppendLine(Parent.Name);
        foreach (var kv in this.NewWireStates)
        {
            sb.Append(kv.Key);
            sb.Append(": ");
            sb.Append(this.WireStates[kv.Key] ? "HIGH" : "LOW");
            sb.Append(" -> ");
            sb.AppendLine(kv.Value ? "HIGH" : "LOW");
        }
        return sb.ToString();
    }
}