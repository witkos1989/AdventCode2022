namespace AdventCode2022.Day2;

public sealed class RockPaperScissorsGame
{
    private readonly List<char[]> _data = new();

    public RockPaperScissorsGame()
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

    private static int GetGameResultPointsWithCode(char[] selections)
    {
        int roundResult = selections[1] == 'X' ? 0 :
            selections[1] == 'Y' ? 3 : 6; 

        return RPSComparerWithCode(selections) + roundResult;
    }

    private static int GetGameResultPointsWithoutCode(char[] selections)
    {
        int pointsForSelection = selections[1] - 'W';

        return pointsForSelection + RPSComparerWithoutCode(selections);
    }

    private static int RPSComparerWithCode(char[] selections)
    {
        int points = 0;

        switch(selections[0])
        {
            case 'A':
                points = selections[1] == 'X' ? 3 :
                    selections[1] == 'Y' ? 1 : 2;
                break;
            case 'B':
                points = selections[1] == 'X' ? 1 :
                    selections[1] == 'Y' ? 2 : 3;
                break;
            case 'C':
                points = selections[1] == 'X' ? 2 :
                    selections[1] == 'Y' ? 3 : 1;
                break;
            default:
                break;
        }

        return points;
    }

    private static int RPSComparerWithoutCode(char[] selections)
    {
        int points = 0;

        switch(selections[0])
        {
            case 'A':
                points = selections[1] == 'X' ? 3 :
                    selections[1] == 'Y' ? 6 : 0;
                break;
            case 'B':
                points = selections[1] == 'X' ? 0 :
                    selections[1] == 'Y' ? 3 : 6;
                break;
            case 'C':
                points = selections[1] == 'X' ? 6 :
                    selections[1] == 'Y' ? 0 : 3;
                break;
            default:
                break;
        }

        return points;
    }

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