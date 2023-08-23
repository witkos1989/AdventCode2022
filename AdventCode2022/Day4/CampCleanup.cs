namespace AdventCode2022.Day4;

public sealed class CampCleanup
{
    private readonly List<List<int[]>> _data;

    public CampCleanup()
    {
        string currentDirectory = PathHelper.
            GetCurrentDirectory("Day4", "CampCleanupInput.txt");
        StreamReader file = new(currentDirectory);
        IEnumerable<string?> rawData = file.ImportData();

        _data = ProcessData(rawData).ToList();
    }

    public int[] Solutions()
    {
        int[] results = new int[2];

        results[0] = CountPositiveAreaCoverages();

        results[1] = CountPositiveAreaOverlaps();

        return results;
    }

    private int CountPositiveAreaCoverages() =>
        CheckAreaCoverage(_data).Count(x => x);

    private int CountPositiveAreaOverlaps() =>
        CheckAreaOverlap(_data).Count(x => x);

    private static IEnumerable<bool> CheckAreaCoverage(List<List<int[]>> data)
    {
        foreach (List<int[]> assignmentPairs in data)
        {
            int[] firstArea = assignmentPairs[0];
            int[] secondArea = assignmentPairs[1];

            if ((firstArea[0] >= secondArea[0] && firstArea[1] <= secondArea[1]) ||
                (secondArea[0] >= firstArea[0] && secondArea[1] <= firstArea[1]))
                yield return true;

            yield return false;
        }
    }

    private static IEnumerable<bool> CheckAreaOverlap(List<List<int[]>> data)
    {
        foreach (List<int[]> assignmentPairs in data)
        {
            int[] firstArea = assignmentPairs[0];
            int[] secondArea = assignmentPairs[1];

            if ((firstArea[0] >= secondArea[0] && firstArea[0] <= secondArea[1]) ||
                (firstArea[1] >= secondArea[0] && firstArea[1] <= secondArea[1]) ||
                (secondArea[0] >= firstArea[0] && secondArea[0] <= firstArea[1]) ||
                (secondArea[1] >= firstArea[0] && secondArea[1] <= firstArea[1]))
                yield return true;

            yield return false;
        }
    }

    private static IEnumerable<List<int[]>> ProcessData(IEnumerable<string?> data)
    {
        foreach (string? input in data)
        {
            List<int[]> coveredAreas = new();

            if (string.IsNullOrEmpty(input))
                continue;

            string[] areas = input.Split(",");

            foreach (string area in areas)
            {
                int from = Convert.ToInt32(area.Split("-").First());
                int to = Convert.ToInt32(area.Split("-").Last());

                coveredAreas.Add(new int[] { from, to });
            }

            yield return coveredAreas;
        }
    }
}