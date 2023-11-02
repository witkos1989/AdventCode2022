using System.Text.RegularExpressions;

namespace AdventCode2022.Day20;

public sealed class GrovePositioningSystem
{
    private readonly Regex _numbers;
    private readonly Dictionary<int, int> _sequence;
    public GrovePositioningSystem()
	{
        string currentDirectory = PathHelper.
            GetCurrentDirectory("Day20", "GrovePositioningSystemInput.txt");
        StreamReader file = new(currentDirectory);
        IEnumerable<string?> rawData = file.ImportData();

        _numbers = new("[-0-9]{1,}", RegexOptions.Compiled);

        _sequence = ProcessData(rawData, _numbers);
    }

    private static Dictionary<int, int> ProcessData(
        IEnumerable<string?> data,
        Regex pattern)
    {
        Dictionary<int, int> sequence = new();
        int i = 0;
        foreach (string? line in data)
        {
            if (string.IsNullOrEmpty(line))
                continue;

            Match? match = pattern.Match(line);

            if (match is null)
                continue;

            sequence.Add(i, int.Parse(match.Value));

            i++;
        }

        return sequence;
    }
}