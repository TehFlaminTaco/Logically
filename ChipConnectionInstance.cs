using System.Collections.Generic;

namespace Logically;

public class ChipConnectionInstance
{
    public ChipInstance Chip;
    public ChipConnection Parent;



    public void Think(ChipInstance chip)
    {
        List<bool> inputs = new();
        for (var i = 0; i < Parent.InputWires.Count; i++)
        {
            var inLow = Parent.InputWires[i].ToLower();
            if (inLow == "high" || inLow == "h" || inLow == "1")
                inputs.Add(true);
            else if (inLow == "low" || inLow == "l" || inLow == "0")
                inputs.Add(false);
            else
                inputs.Add(chip.WireStates[Parent.InputWires[i]]);
        }
        List<bool> outputs = Chip.Parent.Think(Chip, inputs);
        for (var i = 0; i < System.Math.Min(outputs.Count, Parent.OutputWires.Count); i++)
        {
            if (Parent.OutputWires[i] == "_") continue;
            chip.NewWireStates[Parent.OutputWires[i]] = outputs[i];
        }
    }
}