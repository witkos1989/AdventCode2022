namespace AdventCode2022.Day12;

public class HillClimbing
{
    private readonly char[][] _data;
    public HillClimbing()
    {
        string currentDirectory = PathHelper.
            GetCurrentDirectory("Day12", "TestInput.txt");
        StreamReader file = new(currentDirectory);
        IEnumerable<string?> rawData = file.ImportData();

        _data = ProcessData(rawData!).ToArray();

        FindShortestPath(_data);
    }

    private static int FindShortestPath(char[][] map)
    {
        bool[][] seen = GenerateSeenMap(map).ToArray();

        int[] start = FindStartPosition(map);

        map[start[0]][start[1]] = 'a';

        int steps = -1;
        int result = 0;

        ShortestPath(seen, map, start[0], start[1], 'a', steps, ref result);

        return result;
    }

    private static void ShortestPath(bool[][] seen, char[][] map, int posX, int posY, char level, int steps, ref int result)
    {
        if (posY >= map[0].Length || posX >= map.Length || posY < 0 || posX < 0)
            return;

        if (seen[posX][posY])
            return;

        steps++;
        

        char levelOnMap = map[posX][posY];

        if (levelOnMap == 'E' && level == 'z')
        {
            result = steps;
        }

        char maplevel = map[posX][posY];
        int height = maplevel - level;

        if (height > 1)
        {
            seen[posX][posY] = false;
            return;
        }

        seen[posX][posY] = true;

        Console.WriteLine("X: {0}; Y: {1}; Level: {2}; Steps: {3}; Height: {4}", posX, posY, level, steps, height);

        ShortestPath(seen, map, posX, posY + 1, map[posX][posY], steps, ref result);

        ShortestPath(seen, map, posX + 1, posY, map[posX][posY], steps, ref result);

        ShortestPath(seen, map, posX, posY - 1, map[posX][posY], steps, ref result);

        ShortestPath(seen, map, posX - 1, posY, map[posX][posY], steps, ref result);
    }

    private static int[] FindStartPosition(char[][] map)
    {
        int[] startPosition = new int[2] { 0, 0 };

        for (int i = 0; i < map.Length; i++)
        {
            for (int j = 0; j < map[i].Length; j++)
            {
                if (map[i][j] == 'S')
                {
                    startPosition = new int[2] { i, j };
                    break;
                }
            }
        }

        return startPosition;
    }

    private static IEnumerable<bool[]> GenerateSeenMap(char[][] map)
    {
        for (int i = 0; i < map.Length; i++)
        {
            yield return new bool[map[i].Length];
        }
    }

    private static IEnumerable<char[]> ProcessData(IEnumerable<string> data)
    {
        foreach (string line in data)
        {
            yield return line.ToArray();
        }
    }
}