using System.Text.RegularExpressions;

namespace AdventCode2022.Day16;

public sealed class ProboscideaVolcanium
{
    private readonly Regex _valvePattern;
    private readonly IEnumerable<Valve> _valves;

    public ProboscideaVolcanium()
    {
        string currentDirectory = PathHelper.
            GetCurrentDirectory("Day16", "ProboscideaVolcaniumInput.txt");
        StreamReader file = new(currentDirectory);
        IEnumerable<string?> rawData = file.ImportData().ToList();

        _valvePattern = new("[A-Za-z ]{1,}(?<name>[A-Z]{2})[a-z= ]{1,}(?<flow>[0-9]{1,})[a-z; ]{1,}(?<leadsTo>[A-Z, ]{1,})",
            RegexOptions.Compiled);

        _valves = ProcessData(rawData, _valvePattern).ToList();
    }

    private static IEnumerable<Valve> ProcessData(
        IEnumerable<string?> data,
        Regex pattern)
    {
        foreach (string? line in data)
        {
            if (string.IsNullOrEmpty(line))
                continue;

            Match? match = pattern.Matches(line).FirstOrDefault();

            if (match is null)
                continue;

            string name = match.Groups[1].Value;
            int flowRate = int.Parse(match.Groups[2].Value);
            string[] leadsTo = match.Groups[3].Value.Split(", ");
            Valve valve = new(name, flowRate, leadsTo);

            yield return valve;
        }
    }

    private record Valve
    {
        public string Name { get; }
        public int FlowRate { get; }
        public string[] LeadsTo { get; }

        public Valve(string name, int flowRate, string[] leadsTo)
        {
            Name = name;
            FlowRate = flowRate;
            LeadsTo = leadsTo;
        }
    }
}