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

    public int[] Results()
    {
        int[] results = new int[2];

        results[0] = CountPositionsOfBeaconAbsence(_data, 2000000);

        results[1] = FindDistressSignal(_data);

        return results;
    }

    private static int FindDistressSignal(IEnumerable<int[]> data)
    {
        int firstLine = 0, secondLine = 0;
        List<int[]> signalSidesList = GenerateSensorsSignalSides(data).ToList();

        for (int i = 0; i < signalSidesList.Count; i++)
        {
            for (int j = i + 1; j < signalSidesList.Count; j++)
            {
                int firstSide = signalSidesList[i][0];
                int secondSide = signalSidesList[j][0];

                if (Math.Abs(firstSide - secondSide) == 2)
                    firstLine = Math.Min(firstSide, secondSide) + 1;

                firstSide = signalSidesList[i][1];
                secondSide = signalSidesList[j][1];

                if (Math.Abs(firstSide - secondSide) == 2)
                    secondLine = Math.Min(firstSide, secondSide) + 1;
            }
        }

        int x = (firstLine + secondLine) / 2;
        int y = (secondLine - firstLine) / 2;

        var result = x * 4000000 + y;

        return result;
    }

    private static IEnumerable<int[]> GenerateSensorsSignalSides(
        IEnumerable<int[]> data)
    {
        foreach (int[] positions in data)
        {
            int signalStrength = Math.Abs(positions[0] - positions[2]) +
                Math.Abs(positions[1] - positions[3]);

            yield return new int[] { positions[0] - positions[1] - signalStrength,
                positions[0] + positions[1] - signalStrength };

            yield return new int[] { positions[0] - positions[1] + signalStrength,
                positions[0] + positions[1] + signalStrength };
        }
    }

    private static int CountPositionsOfBeaconAbsence(
        IEnumerable<int[]> data,
        int y)
    {
        List<int[]> intervals = new();
        int result = 0;

        foreach (int[] positions in data)
        {
            int signalStrength = Math.Abs(positions[0] - positions[2]) +
                Math.Abs(positions[1] - positions[3]);

            int exceedY = signalStrength - Math.Abs(positions[1] - y);

            if (exceedY <= 0)
                continue;

            intervals.Add(new int[] { positions[0] - exceedY,
                positions[0] + exceedY });
        }

        int minX = intervals.Count > 0 ? intervals.Min(i => i[0]) : 0;
        int maxX = intervals.Count > 0 ? intervals.Max(i => i[1]) : 0;

        List<int> beaconsOnY = data.
            Where(b => b[3] == y).
            Select(b => b[2]).
            Distinct().
            ToList();

        List<int> sensorsOnY = data.
            Where(b => b[1] == y).
            Select(b => b[0]).
            Distinct().
            ToList();

        for (int x = minX; x <= maxX + 1; x++)
        {
            if (beaconsOnY.Contains(x) || sensorsOnY.Contains(x))
                continue;

            foreach (int[] interval in intervals)
            {
                if (x <= interval[1] && x >= interval[0])
                {
                    result++;
                    break;
                }
            }
        }

        return result;
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