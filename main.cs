using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.IO;

namespace Logically;

enum InputMode
{
    States,
    Bytes,
    Hex
}

enum OutputMode
{
    States,
    Bytes,
    Hex,
    Quiet
}

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
        _ = new RandomChip();

        string file = null;
        List<string> inputStrings = new();
        Dictionary<char, string> flags = new();
        for (var i = 0; i < args.Length; i++)
        {
            string a = args[i];
            if (a.StartsWith("/"))
            {
                if (a.Length > 1)
                {
                    flags.Add(a[1], a[2..]);
                }
            }
            else
            {
                if (file == null)
                {
                    file = a;
                }
                else
                {
                    inputStrings.Add(a);
                }
            }
        }
        InputMode im = InputMode.States;
        OutputMode om = OutputMode.States;
        if (flags.ContainsKey('i'))
        {
            if (flags['i'].ToLower().StartsWith("b")) im = InputMode.Bytes;
            else if (flags['i'].ToLower().StartsWith("h")) im = InputMode.Hex;
        }
        if (flags.ContainsKey('o'))
        {
            if (flags['o'].ToLower().StartsWith("b")) om = OutputMode.Bytes;
            else if (flags['o'].ToLower().StartsWith("h")) om = OutputMode.Hex;
            else if (flags['o'].ToLower().StartsWith("q")) om = OutputMode.Quiet;
        }

        int loopMax = -1;
        if (flags.ContainsKey('l')) int.TryParse(flags['l'], out loopMax);

        if (file is null)
        {
            Console.WriteLine("Usage: logically [file] (inputs...)");
            return;
        }
        if (!File.Exists(file))
        {
            Console.WriteLine($"Couldn't find specified file! \"{file}\"");
            return;
        }
        List<bool> inputs = new();
        for (var i = 0; i < inputStrings.Count; i++)
        {
            string l = inputStrings[i].ToLower();
            switch (im)
            {
                case InputMode.States:
                    {
                        foreach (char c in l)
                        {
                            if (c == '1' || c == 'h') inputs.Add(true);
                            else if (c == '0' || c == 'l') inputs.Add(false);
                        }
                    }
                    break;
                case InputMode.Bytes:
                    {
                        if (byte.TryParse(l, out var b))
                        {
                            inputs.Add((b & 1) == 1); b >>= 1;
                            inputs.Add((b & 1) == 1); b >>= 1;
                            inputs.Add((b & 1) == 1); b >>= 1;
                            inputs.Add((b & 1) == 1); b >>= 1;
                            inputs.Add((b & 1) == 1); b >>= 1;
                            inputs.Add((b & 1) == 1); b >>= 1;
                            inputs.Add((b & 1) == 1); b >>= 1;
                            inputs.Add((b & 1) == 1); b >>= 1;
                        }
                    }
                    break;
                case InputMode.Hex:
                    {
                        foreach (char c in l)
                        {
                            int n = "0123456789abcdef".IndexOf(c);
                            if (n >= 0)
                            {
                                inputs.Add((n & 1) == 1); n >>= 1;
                                inputs.Add((n & 1) == 1); n >>= 1;
                                inputs.Add((n & 1) == 1); n >>= 1;
                                inputs.Add((n & 1) == 1); n >>= 1;
                            }
                        }
                    }
                    break;
            }
        }

        var mainChip = ChipParser.ParseChips(
          System.Text.Encoding.UTF8.GetString(File.ReadAllBytes(file))
        );
        if (flags.ContainsKey('d')) Console.Error.WriteLine(mainChip);
        ChipInstance inst = new(mainChip);
        int loops = 0;
        List<bool> last = new();
        while (Running)
        {
            loops++;
            inst.Swap();
            last = inst.Parent.Think(inst, inputs);
            if (inst.Satisfied()) break;
            if (loops >= loopMax && loopMax > 0) break;
        }
        switch (om)
        {
            case OutputMode.States:
                {
                    Console.Write(String.Join("", last.Select(c => c ? "1" : "0")));
                }
                break;
            case OutputMode.Bytes:
                {
                    for (var i = 0; i < last.Count; i += 8)
                    {
                        byte b = 0;
                        if (i + 7 < last.Count) { b <<= 1; b = (byte)(b | (last[i + 7] ? 1 : 0)); }
                        if (i + 6 < last.Count) { b <<= 1; b = (byte)(b | (last[i + 6] ? 1 : 0)); }
                        if (i + 5 < last.Count) { b <<= 1; b = (byte)(b | (last[i + 5] ? 1 : 0)); }
                        if (i + 4 < last.Count) { b <<= 1; b = (byte)(b | (last[i + 4] ? 1 : 0)); }
                        if (i + 3 < last.Count) { b <<= 1; b = (byte)(b | (last[i + 3] ? 1 : 0)); }
                        if (i + 2 < last.Count) { b <<= 1; b = (byte)(b | (last[i + 2] ? 1 : 0)); }
                        if (i + 1 < last.Count) { b <<= 1; b = (byte)(b | (last[i + 1] ? 1 : 0)); }
                        if (i + 0 < last.Count) { b <<= 1; b = (byte)(b | (last[i + 0] ? 1 : 0)); }

                        if (i > 0) Console.Write(" ");
                        Console.Write(b);
                    }
                }
                break;
            case OutputMode.Hex:
                {
                    for (var i = 0; i < last.Count; i += 8)
                    {
                        byte b = 0;
                        if (i + 3 < last.Count) { b <<= 1; b = (byte)(b | (last[i + 3] ? 1 : 0)); }
                        if (i + 2 < last.Count) { b <<= 1; b = (byte)(b | (last[i + 2] ? 1 : 0)); }
                        if (i + 1 < last.Count) { b <<= 1; b = (byte)(b | (last[i + 1] ? 1 : 0)); }
                        if (i + 0 < last.Count) { b <<= 1; b = (byte)(b | (last[i + 0] ? 1 : 0)); }
                        Console.Write("0123456789ABCDEF"[b]);
                    }
                }
                break;
        }
        if (flags.ContainsKey('d')) Console.Error.WriteLine(inst);
        if (flags.ContainsKey('d')) Console.Error.WriteLine($"Finished after {loops} ticks!");
    }
}