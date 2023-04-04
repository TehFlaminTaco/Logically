using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logically;

public class ChipDefinition
{
    public static readonly Dictionary<string, ChipDefinition> AllChips = new();

    public string Name;

    public List<string> Inputs = new();
    public List<string> Outputs = new();
    public List<string> Buslines = new();

    public HashSet<ChipConnection> Connections = new();

    public ChipDefinition(string name)
    {
        this.Name = name;
        AllChips.Add(name, this);
    }

    public virtual List<bool> Think(ChipInstance instance, List<bool> inputs)
    {
        for (var i = 0; i < System.Math.Min(inputs.Count, Inputs.Count); i++)
        {
            instance.WireStates[Inputs[i]] = inputs[i];
        }
        instance.Connections.ToList().ForEach(conn =>
        {
            conn.Think(instance);
        });
        List<bool> outputs = new();
        for (var i = 0; i < Outputs.Count; i++)
        {
            outputs.Add(instance.NewWireStates[ChipInstance.RemoveHigh(Outputs[i])]);
        }
        return outputs;
    }

    public override string ToString()
    {
        StringBuilder sb = new();
        sb.Append('@'); sb.AppendLine(Name);
        sb.Append("I:"); sb.AppendLine(System.String.Join(", ", this.Inputs));
        sb.Append("O:"); sb.AppendLine(System.String.Join(", ", this.Outputs));
        sb.Append("B:"); sb.AppendLine(System.String.Join(", ", this.Buslines));
        this.Connections.ToList().ForEach(c =>
        {
            sb.Append(c.TargetChip);
            sb.Append(" (");
            sb.Append(System.String.Join(", ", c.InputWires));
            sb.Append(") (");
            sb.Append(System.String.Join(", ", c.OutputWires));
            sb.AppendLine(")");
        });
        return sb.ToString();
    }
}

public class NotChip : ChipDefinition
{
    public NotChip() : base("NOT") { }
    public override List<bool> Think(ChipInstance instance, List<bool> inputs)
    {
        return inputs.Select(c => !c).ToList();
    }
}
public class OrChip : ChipDefinition
{
    public OrChip() : base("OR") { }
    public override List<bool> Think(ChipInstance instance, List<bool> inputs)
    {
        return new List<bool>(new bool[] { inputs.Any(c => c) });
    }
}
public class AndChip : ChipDefinition
{
    public AndChip() : base("AND") { }
    public override List<bool> Think(ChipInstance instance, List<bool> inputs)
    {
        return new List<bool>(new bool[] { inputs.All(c => c) });
    }
}
public class XorChip : ChipDefinition
{
    public XorChip() : base("XOR") { }
    public override List<bool> Think(ChipInstance instance, List<bool> inputs)
    {
        return new List<bool>(new bool[] { inputs.Count(c => c) % 2 == 1 });
    }
}
public class HaltChip : ChipDefinition
{
    public HaltChip() : base("HALT") { }
    public override List<bool> Think(ChipInstance instance, List<bool> inputs)
    {
        if (inputs.Count > 0 && inputs[0]) Program.Running = false;
        return inputs;
    }
}
public class ReadChip : ChipDefinition
{
    public ReadChip() : base("READ")
    {
        this.Buslines.Add("clock");
    }
    public override List<bool> Think(ChipInstance instance, List<bool> inputs)
    {
        List<bool> outputs = new();
        instance.NewWireStates["clock"] = false;
        if (inputs.Count > 0 && inputs[0])
        {
            instance.NewWireStates["clock"] = true;
            if (!instance.WireStates["clock"])
            {
                int b = System.Console.Read();
                if (b < 0)
                {
                    outputs.Add(true);
                }
                else
                {
                    outputs.Add(false);
                    outputs.Add((b & 1) == 1); b >>= 1;
                    outputs.Add((b & 1) == 1); b >>= 1;
                    outputs.Add((b & 1) == 1); b >>= 1;
                    outputs.Add((b & 1) == 1); b >>= 1;
                    outputs.Add((b & 1) == 1); b >>= 1;
                    outputs.Add((b & 1) == 1); b >>= 1;
                    outputs.Add((b & 1) == 1); b >>= 1;
                    outputs.Add((b & 1) == 1); b >>= 1;
                }
            }
        }
        return outputs;
    }
}
public class WriteChip : ChipDefinition
{
    public WriteChip() : base("WRITE")
    {
        this.Buslines.Add("clock");
    }
    public override List<bool> Think(ChipInstance instance, List<bool> inputs)
    {
        List<bool> outputs = new();
        instance.NewWireStates["clock"] = false;
        if (inputs.Count > 0 && inputs[0])
        {
            instance.NewWireStates["clock"] = true;
            if (!instance.WireStates["clock"])
            {
                int b = 0;
                b <<= 1; b |= (inputs.Count > 7 && inputs[8]) ? 1 : 0;
                b <<= 1; b |= (inputs.Count > 6 && inputs[7]) ? 1 : 0;
                b <<= 1; b |= (inputs.Count > 5 && inputs[6]) ? 1 : 0;
                b <<= 1; b |= (inputs.Count > 4 && inputs[5]) ? 1 : 0;
                b <<= 1; b |= (inputs.Count > 3 && inputs[4]) ? 1 : 0;
                b <<= 1; b |= (inputs.Count > 2 && inputs[3]) ? 1 : 0;
                b <<= 1; b |= (inputs.Count > 1 && inputs[2]) ? 1 : 0;
                b <<= 1; b |= (inputs.Count > 0 && inputs[1]) ? 1 : 0;
                System.Console.Write((char)b);
            }
        }
        return outputs;
    }
}

public class CopyChip : ChipDefinition
{
    public CopyChip() : base("COPY") { }
    public override List<bool> Think(ChipInstance instance, List<bool> inputs)
    {
        return inputs.ToList();
    }
}

public class CellChip : ChipDefinition
{
    public CellChip() : base("CELL") { }
    public override List<bool> Think(ChipInstance instance, List<bool> inputs)
    {
        for (var i = 1; i < inputs.Count; i++)
        {
            if (!instance.WireStates.ContainsKey("d" + i))
            {
                instance.WireStates["d" + i] = false;
                instance.NewWireStates["d" + i] = false;
            }
        }
        if (inputs.Count > 0 && inputs[0])
        {
            for (var i = 1; i < inputs.Count; i++)
            {
                instance.NewWireStates["d" + i] = inputs[i];
            }
        }
        List<bool> outputs = new();
        int j = 1;
        while (instance.NewWireStates.ContainsKey("d" + j))
        {
            outputs.Add(instance.NewWireStates["d" + j++]);
        }
        return outputs;
    }

}