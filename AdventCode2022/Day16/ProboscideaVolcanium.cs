using System.Text.RegularExpressions;

namespace AdventCode2022.Day16;

public sealed class ProboscideaVolcanium
{
    private readonly Regex _valvePattern;
    private readonly IDictionary<string, Valve> _valves;
    private readonly Dictionary<(string, int), int> _cachedValues;

    public ProboscideaVolcanium()
    {
        string currentDirectory = PathHelper.
            GetCurrentDirectory("Day16", "ProboscideaVolcaniumInput.txt");
        StreamReader file = new(currentDirectory);
        IEnumerable<string?> rawData = file.ImportData().ToList();

        _cachedValues = new();

        _valvePattern = new("[A-Za-z ]{1,}(?<name>[A-Z]{2})[a-z= ]{1,}(?<flow>[0-9]{1,})[a-z; ]{1,}(?<leadsTo>[A-Z, ]{1,})",
            RegexOptions.Compiled);

        _valves = ProcessData(rawData, _valvePattern).ToDictionary(v => v.Name);

        foreach (Valve valve in _valves.Values)
        {
            valve.FillValveLeadsToDictionary(_valves);
        }    
    }

    public int[] Results()
    {
        int[] results = new int[2];

        results[0] = CalculateFlowRate(
            _valves["AA"],
            new Dictionary<string, Valve>(),
            30);

        return results;
    }

    private int CalculateFlowRate(Valve current,
        Dictionary<string, Valve> opened,
        int minutes)
    {
        if (minutes <= 0)
            return 0;

        List<string> keyList = opened.Keys.ToList();

        keyList.Add(current.Name);

        (string, int) key = (String.Join(String.Empty, keyList), minutes);

        if (_cachedValues.ContainsKey(key))
            return _cachedValues[key];

        int flowRate = 0;

        Dictionary<string, Valve> openedValves = opened.
            ToDictionary(k => k.Key, v => v.Value);

        if (!opened.ContainsKey(current.Name) && current.FlowRate > 0)
        {
            int pressure = current.FlowRate * (minutes - 1);

            openedValves.Add(current.Name, current);

            int maxFlow = pressure +
                CalculateFlowRate(current, openedValves, minutes - 1);

            flowRate = Math.Max(maxFlow, flowRate);
        }

        foreach (Valve valve in current.Valves.Values)
        {
            int maxFlow = CalculateFlowRate(valve, openedValves, minutes - 1);

            flowRate = Math.Max(maxFlow, flowRate);
        }

        _cachedValues.Add(key, flowRate);

        return flowRate;
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
            List<string> leadsTo = match.Groups[3].Value.Split(", ").ToList();
            Valve valve = new(name, flowRate, leadsTo);

            yield return valve;
        }
    }

    private record Valve
    {
        public string Name { get; }
        public int FlowRate { get; }
        public Dictionary<string, Valve> Valves { get; }
        private readonly List<string> LeadsTo;

        public Valve(
            string name,
            int flowRate,
            List<string> leadsTo)
        {
            Name = name;
            FlowRate = flowRate;
            LeadsTo = leadsTo;
            Valves = new();
        }

        public void FillValveLeadsToDictionary(IDictionary<string, Valve> valves)
        {
            foreach (string valveName in LeadsTo)
            {
                Valves.Add(valveName, valves[valveName]);
            }
        }
    }
}