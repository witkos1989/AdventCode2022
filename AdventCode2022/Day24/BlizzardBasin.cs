namespace AdventCode2022.Day24;

public sealed class BlizzardBasin
{
    private readonly Valley[] _blizzards;

    public BlizzardBasin()
    {
        string currentDirectory = PathHelper.
            GetCurrentDirectory("Day24", "BlizzardBasinInput.txt");
        StreamReader file = new(currentDirectory);
        IEnumerable<string?> rawData = file.ImportData();

        _blizzards = ProcessData(rawData.ToArray()).ToArray();
    }

    public int[] Results()
    {
        int[] results = new int[2];

        results[0] = FastestPath(_blizzards);

        return results;
    }

    private static int FastestPath(Valley[] blizzards)
    {
        (int X, int Y) = blizzards.First().Position;
        int maxX = blizzards.Last().Position.X - 1;
        int maxY = blizzards.Last().Position.Y;
        int fastestTime = int.MaxValue;
        int time = 1;

        while (fastestTime == int.MaxValue)
        {
            PathBFS(X + 1, Y, maxX, maxY, time, ref fastestTime, blizzards);

            time += 1;
        }
        return fastestTime;
    }

    private static int PathBFS(
        int x,
        int y,
        int maxX,
        int maxY,
        int time,
        ref int fTime,
        Valley[] blizzards)
    {
        Queue<(int, int, int)> stack = new();
        Dictionary<(int, int, int), (int, int, int)> seen = new();

        stack.Enqueue((x, y, time));

        while (stack.Count > 0)
        {
            (int X, int Y, int Time) = stack.Dequeue();

            if (X == maxX && Y == maxY)
            {
                fTime = Math.Min(Time + 1, fTime);

                return fTime;
            }

            if (seen.ContainsKey((X, Y, Time)))
                continue;

            seen.Add((X, Y, Time), (X, Y, Time));

            if (Y + 1 <= maxY &&
                !CheckIfBlizzardIsInPosition(
                    X, Y + 1, maxX, maxY, Time + 1, blizzards))
                stack.Enqueue((X, Y + 1, Time + 1));

            if (X + 1 <= maxX &&
                !CheckIfBlizzardIsInPosition(
                    X + 1, Y, maxX, maxY, Time + 1, blizzards))
                stack.Enqueue((X + 1, Y, Time + 1));

            if (!CheckIfBlizzardIsInPosition(
                X, Y, maxX, maxY, Time + 1, blizzards))
                stack.Enqueue((X, Y, Time + 1));

            if (Y - 1 >= 1 &&
                !CheckIfBlizzardIsInPosition(
                    X, Y - 1, maxX, maxY, Time + 1, blizzards))
                stack.Enqueue((X, Y - 1, Time + 1));

            if (X - 1 >= 1 &&
                !CheckIfBlizzardIsInPosition(
                    X - 1, Y, maxX, maxY, Time + 1, blizzards))
                stack.Enqueue((X - 1, Y, Time + 1));
        }

        return fTime;
    }

    private static bool CheckIfBlizzardIsInPosition(
        int x,
        int y,
        int maxX,
        int maxY,
        int time,
        Valley[] blizzards)
    {
        int newX = 0, newY = 0;

        foreach (Valley blizzard in blizzards)
        {
            if (blizzard.BlizzardDirection.X != 0)
            {
                newX = CalculatePositionOnMap(
                    time,
                    maxX,
                    blizzard.BlizzardDirection.X,
                    blizzard.Position.X);
                newY = blizzard.Position.Y;
            }

            if (blizzard.BlizzardDirection.Y != 0)
            {
                newY = CalculatePositionOnMap(
                    time,
                    maxY,
                    blizzard.BlizzardDirection.Y,
                    blizzard.Position.Y);
                newX = blizzard.Position.X;
            }

            if (x == newX && y == newY)
                return true;
        }

        return false;
    }

    private static int CalculatePositionOnMap(
        int time,
        int mapSize,
        int direction,
        int prevPosition)
    {
        int offset = time * direction;

        int position = prevPosition + offset;

        while (position < 1)
            position = mapSize + position;

        while (position > mapSize)
            position -= mapSize;

        return position;
    }

    private static IEnumerable<Valley> ProcessData(
        string?[] data)
    {
        for (int i = 0; i < data.Length; i++)
        {
            if (string.IsNullOrEmpty(data[i]))
                continue;

            for (int j = 0; j < data[i]!.Length; j++)
            {
                switch (data[i]![j])
                {
                    case '.':
                        if (i == 0 || i == data.Length - 1)
                            yield return new Valley((i, j), (0, 0));
                        break;
                    case '^':
                        yield return new Valley((i, j), (-1, 0));
                        break;
                    case 'v':
                        yield return new Valley((i, j), (1, 0));
                        break;
                    case '<':
                        yield return new Valley((i, j), (0, -1));
                        break;
                    case '>':
                        yield return new Valley((i, j), (0, 1));
                        break;
                }
            }
        }
    }

    private record Valley
    {
        public (int X, int Y) Position;
        public (int X, int Y) BlizzardDirection;

        public Valley((int, int) position, (int, int) blizzard) =>
            (Position, BlizzardDirection) = (position, blizzard);
    }
}