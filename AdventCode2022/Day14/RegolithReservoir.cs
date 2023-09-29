using System.Text.RegularExpressions;

namespace AdventCode2022.Day14;

public sealed class RegolithReservoir
{
	private readonly Regex _pointExtraction;
    private readonly IEnumerable<List<int[]>> _data;

	public RegolithReservoir()
	{
        string currentDirectory = PathHelper.
            GetCurrentDirectory("Day14", "RegolithReservoirInput.txt");
        StreamReader file = new(currentDirectory);
        IEnumerable<string?> rawData = file.ImportData().ToList();

        _pointExtraction = new("(?<x>[0-9]{1,}),(?<y>[0-9]{1,})", RegexOptions.Compiled);

        _data = ProcessData(rawData, _pointExtraction).ToList();
	}

    private static IEnumerable<List<int[]>> ProcessData(IEnumerable<string?> data, Regex pattern)
    {
        foreach (string? line in data)
        {
            if (string.IsNullOrEmpty(line))
                continue;

            List<int[]> coordinates = new();

            foreach (Match match in (IEnumerable<Match>)pattern.Matches(line))
            {
                int[] point = new int[2];

                point[0] = int.Parse(match.Groups[1].Value);

                point[1] = int.Parse(match.Groups[2].Value);

                coordinates.Add(point);
            }

            yield return coordinates;
        }
    }
}