using System.Text.RegularExpressions;

namespace AdventCode2022.Day22;

public sealed class MonkeyMap
{
    public IEnumerable<char[]> _map;
    public IEnumerable<(int, char)> _steps;
    private readonly Regex _pattern;

    public MonkeyMap()
    {
        string currentDirectory = PathHelper.
            GetCurrentDirectory("Day22", "MonkeyMapInput.txt");
        StreamReader file = new(currentDirectory);
        IEnumerable<string?> rawData = file.ImportData();

        _pattern = new("[0-9]{1,}|[RL]{1}", RegexOptions.Compiled);

        _map = ProcessMap(rawData);

        _steps = ProcessSteps(rawData.Last()!, _pattern);
    }

    private static IEnumerable<char[]> ProcessMap(IEnumerable<string?> data)
    {
        foreach (string? line in data)
        {
            if (string.IsNullOrEmpty(line))
                break;

            char[] mapLine = new char[line.Length];

            for (int i = 0; i < line.Length; i++)
            {
                mapLine[i] = line[i];
            }

            yield return mapLine;
        }
    }

    private static IEnumerable<(int, char)> ProcessSteps(string path, Regex pattern)
    {
        MatchCollection? matches = pattern.Matches(path);

        for (int i = 0; i < matches.Count; i += 2)
        {
            int steps = int.Parse(matches[i].Value);
            char direction = ' ';

            if (i + 1 < matches.Count)
                direction = matches[i + 1].Value.First();

            yield return (steps, direction);
        }

    }
}