using System.Collections.Generic;
public class ChipConnectionInstance
{
    public ChipInstance Chip;
    public ChipConnection Parent;



    public void Think(ChipInstance chip)
    {
        List<bool> inputs = new();
        for (var i = 0; i < Parent.InputWires.Count; i++)
        {
            if (Parent.InputWires[i].ToLower() == "high" || Parent.InputWires[i].ToLower() == "h")
                inputs.Add(true);
            else if (Parent.InputWires[i].ToLower() == "low" || Parent.InputWires[i].ToLower() == "l")
                inputs.Add(false);
            else
                inputs.Add(chip.WireStates[Parent.InputWires[i]]);
        }
        List<bool> outputs = Chip.Parent.Think(Chip, inputs);
        for (var i = 0; i < System.Math.Min(outputs.Count, Parent.OutputWires.Count); i++)
        {
            chip.NewWireStates[Parent.OutputWires[i]] = outputs[i];
        }
    }
}