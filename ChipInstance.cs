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
        parent.Inputs.ForEach(c => NewWireStates[c] = false);
        parent.Outputs.ForEach(c => NewWireStates[c] = false);
        parent.Buslines.ForEach(c => NewWireStates[c] = false);
        parent.Connections.ToList().ForEach(c => Connections.Add(new ChipConnectionInstance()
        {
            Chip = new ChipInstance(ChipDefinition.AllChips[c.TargetChip]),
            Parent = c
        }));
        Swap();
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
            sb.AppendLine(kv.Value ? "HIGH" : "LOW");
        }
        return sb.ToString();
    }
}