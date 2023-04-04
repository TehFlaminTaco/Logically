using System.Collections.Generic;

namespace Logically;

public struct ChipConnection
{
    public string TargetChip;
    public List<string> InputWires;
    public List<string> OutputWires;
}