using System.Text.RegularExpressions;

namespace AdventCode2022.Day15;

public sealed class BeaconExclusionZone
{
    private readonly Regex _sensorsExtraction;
    private readonly IEnumerable<int[]> _data;

    public BeaconExclusionZone()
	{
        string currentDirectory = PathHelper.
            GetCurrentDirectory("Day15", "BeaconExclusionZoneInput.txt");
        StreamReader file = new(currentDirectory);
        IEnumerable<string?> rawData = file.ImportData().ToList();

        _sensorsExtraction = new("[a-zA-Z= ]{1,}(?<sensorX>[-0-9]{1,})[\\,a-z =]{1,}(?<sensorY>[-0-9]{1,})[a-z:= ]{1,}(?<beaconX>[-0-9]{1,})[\\,a-z =]{1,}(?<beaconY>[-0-9]{1,})",
            RegexOptions.Compiled);

        _data = ProcessData(rawData, _sensorsExtraction);
    }

    private static IEnumerable<int[]> ProcessData(
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

            int[] sensorAndBeaconPos = new int[] {
                int.Parse(match.Groups[1].Value),
                int.Parse(match.Groups[2].Value),
                int.Parse(match.Groups[3].Value),
                int.Parse(match.Groups[4].Value)
            };

            yield return sensorAndBeaconPos;
        }
    }
}