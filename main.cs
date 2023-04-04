using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;

class Program
{
    public static bool Running = true;
    public static void Main(string[] args)
    {
        new NotChip();
        new OrChip();
        new AndChip();
        new XorChip();
        new HaltChip();
        new ReadChip();
        new WriteChip();

        var mainChip = ChipParser.ParseChips(
          System.Text.Encoding.UTF8.GetString(System.IO.File.ReadAllBytes("test.l"))
        );
        Console.WriteLine(mainChip);
        ChipInstance inst = new(mainChip);
        int loops = 0;
        while (Running)
        {
            loops++;
            inst.Parent.Think(inst, new());
            inst.Swap();
        }
        Console.WriteLine(inst);
        Console.WriteLine($"Finished after {loops} ticks!");
    }
}