namespace AdventCode2022.Day2;

public sealed class RockPaperScissors
{
    private readonly List<char[]> _data = new();

    public RockPaperScissors()
    {
        string currentDirectory = Path.GetDirectoryName(
            Path.GetDirectoryName(
                Path.GetDirectoryName(
                    Directory.GetCurrentDirectory())))!;
        string archiveFolder = Path.Combine(currentDirectory, "Day2");
        StreamReader file = new(archiveFolder + "/RockPaperScissorsGamePlan.txt");

        _data = ImportData(file).ToList();
    }

    public int[] Solutions()
    {
        int[] results = new int[2];

        results[0] = SumOfPointsInFirstRound();

        results[1] = SumOfPointsInSecondRound();

        return results;
    }

    private int SumOfPointsInSecondRound()
    {
        int sum = 0;

        foreach (char[] game in _data)
        {
            sum += GetGameResultPointsWithCode(game);
        }

        return sum;
    }

    private int SumOfPointsInFirstRound()
    {
        int sum = 0;

        foreach (char[] game in _data)
        {
            sum += GetGameResultPointsWithoutCode(game);
        }

        return sum;
    }

    private static int GetGameResultPointsWithCode(char[] selections) =>
        selections switch
        {
            [_, 'X'] => RPSComparerWithCode(selections) + 0,
            [_, 'Y'] => RPSComparerWithCode(selections) + 3,
            [_, 'Z'] => RPSComparerWithCode(selections) + 6,
            _ => 0
        };

    private static int GetGameResultPointsWithoutCode(char[] selections) =>
        (selections[1] - 'W') + RPSComparerWithoutCode(selections);
    

    private static int RPSComparerWithCode(char[] selections) =>
        selections switch
        {
            ['A', 'Y'] or ['B', 'X'] or ['C', 'Z'] => 1,
            ['A', 'Z'] or ['B', 'Y'] or ['C', 'X'] => 2,
            ['A', 'X'] or ['B', 'Z'] or ['C', 'Y'] => 3,
            _ => 0
        };

    private static int RPSComparerWithoutCode(char[] selections) =>
        selections switch
        {
            ['A', 'Z'] or ['B', 'X'] or ['C', 'Y'] => 0,
            ['A', 'X'] or ['B', 'Y'] or ['C', 'Z'] => 3,
            ['A', 'Y'] or ['B', 'Z'] or ['C', 'X'] => 6,
            _ => 0
        };

    private static IEnumerable<char[]> ImportData(StreamReader stream)
    {
        for (; ; )
        {
            if (stream.EndOfStream)
                break;

            string? line = stream.ReadLine();

            if (string.IsNullOrEmpty(line))
                continue;

            char[] selections = line.Replace(" ", "").ToCharArray();

            yield return selections;
        }
    }
}