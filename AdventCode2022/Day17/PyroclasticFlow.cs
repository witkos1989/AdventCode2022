namespace AdventCode2022.Day17;

public sealed class PyroclasticFlow
{
    private readonly int _noOfRocks = 2022;
    private readonly string _windFlow;
    private readonly byte[,] _map;
    private readonly IList<byte[,]> _rocks;

    public PyroclasticFlow()
    {
        string currentDirectory = PathHelper.
            GetCurrentDirectory("Day17", "PyroclasticFlowInput.txt");
        StreamReader file = new(currentDirectory);
        _windFlow = file.ImportData().First()!;
        _rocks = GenerateRocks();
        _map = new byte[_noOfRocks * 2, 7];
    }

    public int[] Results()
    {
        int[] results = new int[2];

        results[0] = FallingRocks(_map, _rocks, _windFlow, _noOfRocks);

        return results;
    }

    private static int FallingRocks(
        byte[,] map, IList<byte[,]> rocks, string wind, int noOfRocks)
    {
        int height = 0;
        int windIndex = 0;

        for (int i = 0; i < noOfRocks; i++)
        {
            int x = 2;
            int y = height + 3;
            byte[,] rock = rocks[i % 5];
            bool placed = false;

            while (!placed)
            {
                Push(map, rock, wind[windIndex], y, ref x);

                bool touchingFloor =
                    CollidingWithFloor(map, rock, x, y, ref height);

                if (touchingFloor)
                {
                    AddRockToMap(map, rock, x, y);

                    placed = true;
                }
                else
                {
                    y -= 1;
                }

                windIndex += 1;

                if (windIndex >= wind.Length)
                    windIndex = 0;
            }
        }

        return height;
    }

    private static void Push(
        byte[,] map, byte[,] rock, char wind, int y, ref int x)
    {
        bool touchingWall =
            CollidingWithWall(map.GetLength(1), rock.GetLength(1), wind, x);
        bool touchingRocks =
            CollidingWithRocks(map, rock, wind, x, y);

        if (!touchingWall && !touchingRocks)
            x = wind == '>' ? x + 1 : x - 1;
    }

    private static bool CollidingWithWall(
        int mapWidth, int rockWidth, char wind, int x) =>
        wind == '>' ? x + rockWidth >= mapWidth : x - 1 < 0;

    private static bool CollidingWithRocks(
        byte[,] map, byte[,] rock, char wind, int x, int y)
    {
        byte mapPos;

        for (int i = 0; i < rock.GetLength(0); i++)
        {
            for (int j = 0; j < rock.GetLength(1); j++)
            {
                if (rock[i, j] == 0)
                    continue;

                if (wind == '>')
                {
                    if (x + j + 1 >= map.GetLength(1))
                        continue;

                    mapPos = map[y + i, x + j + 1];
                }
                else
                {
                    if (x == 0)
                        continue;

                    mapPos = map[y + i, x + j - 1];
                }

                if (mapPos == 1)
                    return true;
            }
        }

        return false;
    }

    private static bool CollidingWithFloor(
        byte[,] map, byte[,] rock, int x, int y, ref int height)
    {
        byte below;

        if (y == 0)
        {
            height = rock.GetLength(0);

            return true;
        }

        for (int i = 0; i < rock.GetLength(0); i++)
        {
            for (int j = 0; j < rock.GetLength(1); j++)
            {
                if (rock[i, j] == 0)
                    continue;

                below = map[y + i - 1, x + j];

                if (below == 1)
                {
                    height = Math.Max(height, y + rock.GetLength(0));

                    return true;
                }
            }
        }

        return false;
    }

    private static void AddRockToMap(byte[,] map, byte[,] rock, int x, int y)
    {
        for (int i = 0; i < rock.GetLength(0); i++)
        {
            for (int j = 0; j < rock.GetLength(1); j++)
            {
                if (rock[i, j] == 0)
                    continue;

                map[y + i, x + j] = 1;
            }
        }
    }

    private static IList<byte[,]> GenerateRocks() => new List<byte[,]>
    {
        new byte[1, 4] { { 1, 1, 1, 1 } },
        new byte[3, 3] { { 0, 1, 0 }, { 1, 1, 1 }, { 0, 1, 0 } },
        new byte[3, 3] { { 1, 1, 1 }, { 0, 0, 1 }, { 0, 0, 1 } },
        new byte[4, 1] { { 1 }, { 1 }, { 1 }, { 1 } },
        new byte[2, 2] { { 1, 1 }, { 1, 1 } },
    };
}