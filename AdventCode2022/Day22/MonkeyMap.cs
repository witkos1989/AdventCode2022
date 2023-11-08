using System.Text.RegularExpressions;

namespace AdventCode2022.Day22;

public sealed class MonkeyMap
{
    public char[][] _map;
    public (int, char)[] _steps;
    private readonly Regex _pattern;

    public MonkeyMap()
    {
        string currentDirectory = PathHelper.
            GetCurrentDirectory("Day22", "MonkeyMapInput.txt");
        StreamReader file = new(currentDirectory);
        IEnumerable<string?> rawData = file.ImportData();

        _pattern = new("[0-9]{1,}|[RL]{1}", RegexOptions.Compiled);

        _map = ProcessMap(rawData).ToArray();

        _steps = ProcessSteps(rawData.Last()!, _pattern).ToArray();
    }

    public int[] Results()
    {
        int[] results = new int[2];

        results[0] = GoPath(_map, _steps);

        return results;
    }

    private static int GoPath(char[][] map, (int, char)[] steps)
    {
        int[] position = new int[2] { 0, Array.IndexOf(map[0], '.') };
        char[] directions = new char[4] { 'R', 'D', 'L', 'U' };
        int currentDir = 0;

        for (int i = 0; i < steps.Length; i++)
        {
            for (int j = 0; j < steps[i].Item1; j++)
            {
                char nextPosition = '.';

                switch (directions[currentDir])
                {
                    case 'R':
                        nextPosition = TakeStep(map, position, 0, 1);
                        break;
                    case 'L':
                        nextPosition = TakeStep(map, position, 0, -1);
                        break;
                    case 'D':
                        nextPosition = TakeStep(map, position, 1, 1);
                        break;
                    case 'U':
                        nextPosition = TakeStep(map, position, 1, -1);
                        break;
                }

                if (nextPosition == '#')
                    break;
            }
            if (steps[i].Item2 != ' ')
                currentDir = ChangeDir(directions, currentDir, steps[i].Item2);
        }

        int sum = (position[0] + 1) * 1000 + (position[1] + 1) * 4 + currentDir;

        return sum;
    }

    private static char TakeStep(
        char[][] map,
        int[] position,
        int direction,
        int move)
    {
        char nextPosition;
        int nextMove;

        if (direction == 0)
        {
            nextMove = MoveHorizontally(map, position, move);

            nextPosition = map[position[0]][nextMove];

            if (nextPosition == '#')
                return nextPosition;

            position[1] = nextMove;
        }
        else
        {
            nextMove = MoveVertically(map, position, move);

            nextPosition = map[nextMove][position[1]];

            if (nextPosition == '#')
                return nextPosition;

            position[0] = nextMove;
        }

        return nextPosition;
    }

    private static int MoveHorizontally(char[][] map, int[] position, int move)
    {
        if (position[1] + move >= map[position[0]].Length)
        {
            for (int i = 0; i < map[position[0]].Length; i++)
            {
                if (map[position[0]][i] == '.' || map[position[0]][i] == '#')
                    return i;
            }
        }

        if (position[1] + move < 0 ||
            map[position[0]][position[1] + move] == ' ')
            return map[position[0]].Length - 1;

        return position[1] + move;
    }

    private static int MoveVertically(char[][] map, int[] position, int move)
    {
        if (position[0] + move >= map.Length ||
            position[0] + move < 0 ||
            position[1] >= map[position[0] + move].Length ||
            map[position[0] + move][position[1]] == ' ')
        {
            if (move > 0)
            {
                for (int i = 0; i < map.Length; i++)
                {
                    if (position[1] >= map[i].Length)
                        continue;

                    if (map[i][position[1]] == '.' ||
                        map[i][position[1]] == '#')
                        return i;
                }
            }
            else
            {
                for (int i = map.Length - 1; i >= 0; i--)
                {
                    if (position[1] >= map[i].Length)
                        continue;

                    if (map[i][position[1]] == '.' ||
                        map[i][position[1]] == '#')
                        return i;
                }
            }
        }

        return position[0] + move;
    }

    private static int ChangeDir(char[] directions, int current, char turn) =>
        turn == 'R' ?
        current + 1 >= directions.Length ? 0 : current += 1 :
        current - 1 < 0 ? directions.Length - 1 : current -= 1;

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

    private static IEnumerable<(int, char)> ProcessSteps(
        string path,
        Regex pattern)
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