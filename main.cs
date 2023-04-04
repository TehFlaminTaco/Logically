using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Logically;

class Program
{
    public static bool Running = true;
    public static void Main(string[] args)
    {
        _ = new NotChip();
        _ = new OrChip();
        _ = new AndChip();
        _ = new XorChip();
        _ = new HaltChip();
        _ = new ReadChip();
        _ = new WriteChip();
        _ = new CopyChip();
        _ = new CellChip();

        var mainChip = ChipParser.ParseChips(
          System.Text.Encoding.UTF8.GetString(System.IO.File.ReadAllBytes("test.l"))
        );
        Console.Error.WriteLine(mainChip);
        ChipInstance inst = new(mainChip);
        int loops = 0;
        while (Running)
        {
            loops++;
            if (loops > 6000) break;
            inst.Swap();
            inst.Parent.Think(inst, new());
        }
        Console.Error.WriteLine(inst);
        Console.Error.WriteLine($"Finished after {loops} ticks!");
    }
}