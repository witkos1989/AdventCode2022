using System.Text.RegularExpressions;

namespace AdventCode2022.Day14;

public sealed class RegolithReservoir
{
	private readonly Regex _pointExtraction;
    private readonly IEnumerable<List<int[]>> _data;
    private char[,] _map = new char[0,0];

	public RegolithReservoir()
	{
        string currentDirectory = PathHelper.
            GetCurrentDirectory("Day14", "RegolithReservoirInput.txt");
        StreamReader file = new(currentDirectory);
        IEnumerable<string?> rawData = file.ImportData().ToList();

        _pointExtraction = new("(?<x>[0-9]{1,}),(?<y>[0-9]{1,})",
            RegexOptions.Compiled);

        _data = ProcessData(rawData, _pointExtraction).ToList();
    }

    public int[] Results()
    {
        int[] results = new int[2];

        _map = CaveGenerator(_data);

        results[0] = CountPouredGrainsOfSand(_map, false);

        _map = CaveGenerator(_data, true);

        results[1] = CountPouredGrainsOfSand(_map, true);

        return results;
    }

    public void DrawMap()
    {
        for (int col = 0; col < _map.GetLength(1); col++)
        {
            for (int row = 470; row < _map.GetLength(0); row++)
            {
                Console.Write(_map[row, col]);
            }
            Console.WriteLine();
        }
    }

    private static int CountPouredGrainsOfSand(char[,] map, bool withFloor) =>
        PourSand(map, withFloor);

    private static int PourSand(char[,] map, bool withFloor)
    {
        int grainsCount = 0;
        bool isOverflowing = false;
        bool fullChamber = false;

        for (; ; )
        {
            int[] sandPos = new int[] { 500, 0 };

            while (true)
            {
                if (sandPos[1] + 1 >= map.GetLength(1))
                {
                    isOverflowing = true;

                    break;
                }

                char belowPos = map[sandPos[0], sandPos[1] + 1];

                if (belowPos == '.')
                {
                    sandPos[1] += 1;

                    continue;
                }

                if (belowPos == '#' || belowPos == 'o')
                {
                    char leftPos = map[sandPos[0] - 1, sandPos[1] + 1];

                    if (leftPos == '.')
                    {
                        sandPos[0] -= 1;

                        sandPos[1] += 1;

                        continue;
                    }

                    char rightPos = map[sandPos[0] + 1, sandPos[1] + 1];

                    if (rightPos == '.')
                    {
                        sandPos[0] += 1;

                        sandPos[1] += 1;

                        continue;
                    }

                    map[sandPos[0], sandPos[1]] = 'o';

                    if (sandPos[0] == 500 && sandPos[1] == 0)
                    {
                        fullChamber = true;

                        grainsCount++;
                    }

                    break;
                }
            }

            if (isOverflowing || fullChamber)
                break;

            grainsCount++;
        }

        return grainsCount;
    }

    private static char[,] CaveGenerator(
        IEnumerable<List<int[]>> input,
        bool withFloor = false)
    {
        int[] mapDimensions = CalculateMapSize(input);
        char[,] map = new char[mapDimensions[0], mapDimensions[1]];

        GenerateEmptyMap(map);

        AddRocksToMap(input, map);

        if (withFloor)
            AddFloor(map);

        return map;
    }

    private static void AddFloor(char[,] map)
    {
        for (int xPos = 0; xPos < map.GetLength(0); xPos++)
        {
            map[xPos, map.GetLength(1) - 1] = '#';
        }
    }

    private static void AddRocksToMap(
        IEnumerable<List<int[]>> rocks,
        char[,] map)
    {
        foreach (List<int[]> pathOfRocks in rocks)
        {
            int xPosition = 0;
            int yPosition = 0;

            foreach (int[] line in pathOfRocks)
            {
                if (xPosition == 0 && yPosition == 0)
                {
                    (xPosition, yPosition) = (line[0], line[1]);

                    continue;
                }

                if (line[0] != xPosition)
                {
                    int startPoint = line[0] > xPosition ? xPosition : line[0];
                    int endPoint = line[0] > xPosition ? line[0] : xPosition;

                    for (int rock = startPoint; rock <= endPoint; rock++)
                    {
                        map[rock, yPosition] = '#';
                    }
                }

                if (line[1] != yPosition)
                {
                    int startPoint = line[1] > yPosition ? yPosition : line[1];
                    int endPoint = line[1] > yPosition ? line[1] : yPosition;

                    for (int rock = startPoint; rock <= endPoint; rock++)
                    {
                        map[xPosition, rock] = '#';
                    }
                }

                xPosition = line[0];

                yPosition = line[1];
            }
        }    
    }

    private static void GenerateEmptyMap(char[,] map)
    {
        for (int row = 0; row < map.GetLength(0); row++)
        {
            for (int col = 0; col < map.GetLength(1); col++)
            {
                map[row, col] = '.';
            }
        }
    }

    private static int[] CalculateMapSize(IEnumerable<List<int[]>> input)
    {
        int maxX = 0;
        int maxY = 0;

        foreach (List<int[]> rocks in input)
        {
            foreach (int[] line in rocks)
            {
                maxX = line[0] > maxX ? line[0] : maxX;

                maxY = line[1] > maxY ? line[1] : maxY;
            }
        }

        return new int[] { maxX + maxY, maxY + 3 };
    }

    private static IEnumerable<List<int[]>> ProcessData(
        IEnumerable<string?> data,
        Regex pattern)
    {
        foreach (string? line in data)
        {
            if (string.IsNullOrEmpty(line))
                continue;

            List<int[]> coordinates = new();

            foreach (Match match in (IEnumerable<Match>)pattern.Matches(line))
            {
                int[] point = new int[2];

                point[0] = int.Parse(match.Groups[1].Value);

                point[1] = int.Parse(match.Groups[2].Value);

                coordinates.Add(point);
            }

            yield return coordinates;
        }
    }
}