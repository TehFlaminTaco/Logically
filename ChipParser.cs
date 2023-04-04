using System.Text.RegularExpressions;
using System.Collections.Generic;
public class ChipParser
{
    public static Regex ChipRegex = new Regex(@"@(?<ChipName>\w+)\n
(?<IOB>(?:.*:[^:;]*;?\n?)*)
(?<Wires>(?:[^@](?!\n+@))*.?)", RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);
    public static Regex IOBRegex = new Regex(@"(?<Type>\w*):[ \t]*(?<Value>[^:;]*)");
    public static Regex ConnectionRegex = new Regex(@"(?<Chip>\w+)[ \t]*
\((?<InputWires>[^)]*)\)\s*
\((?<OutputWires>[^)]*)\)", RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);
    public static Regex WireNames = new Regex(@"\w+");

    public static ChipDefinition ParseChips(string s)
    {
        ChipDefinition mainByName = null;
        ChipDefinition last = null;
        foreach (Match chipMatch in ChipRegex.Matches(s))
        {
            ChipDefinition chipDef = new(chipMatch.Groups["ChipName"].Value);

            List<string> Inputs = null;
            List<string> Outputs = null;
            List<string> BusLines = null;
            List<List<string>> AllLines = new();
            if (chipMatch.Groups.ContainsKey("IOB"))
            {
                foreach (Match iobMatch in IOBRegex.Matches(chipMatch.Groups["IOB"].Value))
                {
                    string type = iobMatch.Groups["Type"].Value.ToLower();
                    List<string> wires = ParseWireGroup(iobMatch.Groups["Value"].Value);
                    if (type.StartsWith("i")) Inputs = wires;
                    else if (type.StartsWith("o")) Outputs = wires;
                    else if (type.StartsWith("b")) BusLines = wires;
                    else AllLines.Add(wires);
                }
            }
            foreach (var wires in AllLines)
            {
                if (Inputs == null) Inputs = wires;
                else if (Outputs == null) Outputs = wires;
                else if (BusLines == null) BusLines = wires;
            }
            chipDef.Inputs = Inputs ?? new();
            chipDef.Outputs = Outputs ?? new();
            chipDef.Buslines = BusLines ?? new();

            chipDef.Connections = ParseConnections(chipMatch.Groups["Wires"].Value);

            last = chipDef;
            if (mainByName == null && chipDef.Name.ToLower().StartsWith("main")) mainByName = chipDef;
        }

        // Find the Main chip!
        // It'll either be the Last chip defined, or one called "Main" or "main"
        ChipDefinition main = mainByName ?? last;
        if (main is null)
        {
            throw new System.Exception("No chips detected. Nothing to run!");
        }
        return main;
    }

    public static List<string> ParseWireGroup(string s)
    {
        List<string> wires = new();
        foreach (Match wireMatch in WireNames.Matches(s))
        {
            wires.Add(wireMatch.Value);
        }
        return wires;
    }

    public static HashSet<ChipConnection> ParseConnections(string s)
    {
        HashSet<ChipConnection> connections = new();
        foreach (Match connMatch in ConnectionRegex.Matches(s))
        {
            connections.Add(new ChipConnection()
            {
                TargetChip = connMatch.Groups["Chip"].Value,
                InputWires = ParseWireGroup(connMatch.Groups["InputWires"].Value),
                OutputWires = ParseWireGroup(connMatch.Groups["OutputWires"].Value),
            });
        }
        return connections;
    }
}