namespace AdventCode2022.Day23;

public sealed class UnstableDiffusion
{
    private readonly char[][] _grove;
    private readonly Queue<(int, int)> _directions;

    public UnstableDiffusion()
    {
        string currentDirectory = PathHelper.
            GetCurrentDirectory("Day23", "UnstableDiffusionInput.txt");
        StreamReader file = new(currentDirectory);
        IEnumerable<string?> rawData = file.ImportData();

        _grove = ProcessData(rawData.ToArray()).ToArray();

        _directions = new(new (int, int)[] { (-1, 0), (1, 0), (0, -1), (0, 1) });
    }

    public int[] Results()
    {
        int[] results = new int[2];

        results[0] = CountEmptyGround(_grove, _directions);

        results[1] = RealocateElves(int.MaxValue, _grove, _directions) + 10;

        return results;
    }

    private static int CountEmptyGround(
        char[][] grove,
        Queue<(int, int)> directions)
    {
        int maxX = 0, maxY = 0, minX = grove.Length, minY = grove.Length;
        int sum = 0;

        RealocateElves(10, grove, directions);

        for (int i = 0; i < grove.Length; i++)
        {
            for (int j = 0; j < grove[i].Length; j++)
            {
                if (grove[i][j] == '.')
                    continue;

                maxX = i > maxX ? i : maxX;

                minX = i < minX ? i : minX;

                maxY = j > maxY ? j : maxY;

                minY = j < minY ? j : minY;
            }
        }

        for (int i = minX; i <= maxX; i++)
        {
            for (int j = minY; j <= maxY; j++)
            {
                if (grove[i][j] == '.')
                    sum += 1;
            }
        }

        return sum;
    }

    private static int RealocateElves(
        int steps,
        char[][] grove,
        Queue<(int, int)> directions)
    {
        Dictionary<(int, int), (int, int, bool)> moveTo = new();

        for (int i = 0; i < steps; i++)
        {
            CanMove(grove, directions, moveTo);

            Move(grove, moveTo);

            if (moveTo.Count == 0)
                return i + 1;

            moveTo.Clear();
            
            (int, int) direction = directions.Dequeue();

            directions.Enqueue(direction);
        }

        return -1;
    }

    private static void Move(
        char[][] grove,
        Dictionary<(int, int), (int, int, bool)> moveTo)
    {
        foreach (KeyValuePair<(int X, int Y), (int X, int Y, bool)> pair in moveTo)
        {
            if (pair.Value.Item3)
                continue;

            grove[pair.Key.X][pair.Key.Y] = '#';

            grove[pair.Value.X][pair.Value.Y] = '.';
        }
    }

    private static void CanMove(
        char[][] grove,
        Queue<(int, int)> directions,
        Dictionary<(int, int), (int, int, bool)> moveTo)
    {
        for (int i = 0; i < grove.GetLength(0); i++)
        {
            for (int j = 0; j < grove[i].Length; j++)
            {
                if (grove[i][j] == '.')
                    continue;

                if (CheckAdjacentPositions(i, j, grove))
                    continue;

                foreach ((int X, int Y) direction in directions)
                {
                    if (direction.X == 0)
                    {
                        if (CanMoveWestOrEast(i, j, grove, direction, moveTo))
                            break;
                    }
                    else
                    {
                        if (CanMoveNorthOrSouth(i, j, grove, direction, moveTo))
                            break;
                    }
                }
            } 
        }
    }

    private static bool CheckAdjacentPositions(int x, int y, char[][] grove) =>
        grove[x][y + 1] == '.' && grove[x][y - 1] == '.' &&
        grove[x - 1][y] == '.' && grove[x - 1][y + 1] == '.' &&
        grove[x - 1][y - 1] == '.' && grove[x + 1][y] == '.' &&
        grove[x + 1][y + 1] == '.' && grove[x + 1][y - 1] == '.';

    private static bool CanMoveNorthOrSouth(
        int x,
        int y,
        char[][] grove,
        (int X, int Y) direction,
        Dictionary<(int, int), (int, int, bool)> moveTo)
    {
        if (grove[x + direction.X][y] == '.' &&
            grove[x + direction.X][y - 1] == '.' &&
            grove[x + direction.X][y + 1] == '.')
        {
            if (moveTo.ContainsKey((x + direction.X, y)))
            {
                (int X, int Y, bool) prev = moveTo[(x + direction.X, y)];

                moveTo[(x + direction.X, y)] = (prev.X, prev.Y, true);

                return true;
            }

            moveTo.Add((x + direction.X, y), (x, y, false));

            return true;
        }

        return false;
    }

    private static bool CanMoveWestOrEast(
        int x,
        int y,
        char[][] grove,
        (int X, int Y) direction,
        Dictionary<(int, int), (int, int, bool)> moveTo)
    {
        if (grove[x][y + direction.Y] == '.' &&
            grove[x - 1][y + direction.Y] == '.' &&
            grove[x + 1][y + direction.Y] == '.')
        {
            if (moveTo.ContainsKey((x, y + direction.Y)))
            {
                (int X, int Y, bool) prev = moveTo[(x, y + direction.Y)];

                moveTo[(x, y + direction.Y)] = (prev.X, prev.Y, true);

                return true;
            }

            moveTo.Add((x, y + direction.Y), (x, y, false));

            return true;
        }

        return false;
    }

    private static IEnumerable<char[]> ProcessData(string?[] data)
    {
        int rowLength = data.Length;
        
        for (int i = 0; i < 100; i++)
        {
            yield return Enumerable.Repeat('.', rowLength + 200).ToArray();
        }

        foreach (string? line in data)
        {
            if (string.IsNullOrEmpty(line))
                continue;

            char[] groveLine = Enumerable.Repeat('.', 100).Concat(line).
                Concat(Enumerable.Repeat('.', 100)).ToArray();

            yield return groveLine;
        }

        for (int i = 0; i < 100; i++)
        {
            yield return Enumerable.Repeat('.', rowLength + 200).ToArray();
        }
    }
}