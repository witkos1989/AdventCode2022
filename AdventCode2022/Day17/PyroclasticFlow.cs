using System.Collections.Generic;
using System.Text;

namespace AdventCode2022.Day17;

public class PyroclasticFlow
{
    private readonly int _rockCount = 2022;
    private readonly string _windFlow;
    private readonly string[] _map;
    private readonly IList<string[]> _rockShapes;

    public PyroclasticFlow()
    {
        string currentDirectory = PathHelper.
            GetCurrentDirectory("Day17", "PyroclasticFlowInput.txt");
        StreamReader file = new(currentDirectory);
        _windFlow = file.ImportData().First()!;
        _rockShapes = GenerateRockShapes();
        _map = GenerateMap(_rockCount * 4).ToArray();
    }

    public int[] Results()
    {
        int[] results = new int[2];

        results[0] = FallingRocks(_map, _rockShapes, _windFlow, _rockCount);

        DrawMap(_map.Reverse());

        return results;
    }
    
    private static int FallingRocks(
        string[] map,
        IList<string[]> rocks,
        string windFlow,
        int rockCount)
    {
        int towerHeight = 0;
        int windFlowIndex = 0;

        for (int i = 0; i < rockCount; i++)
        {
            int horPosition = 2;
            string[] rock = rocks[i % 5];
            int rockPosition = towerHeight + 4;
            bool rockPlaced = false;

            while (!rockPlaced)
            {
                char windDirection = windFlow[windFlowIndex];

                PushRock(
                    rock,
                    map,
                    rockPosition,
                    windDirection,
                    ref horPosition);

                bool canGoDown = ColliderChecker(
                    rock,
                    map,
                    rockPosition,
                    horPosition,
                    horPosition,
                    ref towerHeight);

                if (canGoDown)
                {
                    rockPosition -= 1;
                }
                else
                {
                    AddRockToMap(rock, map, rockPosition, horPosition);

                    rockPlaced = true;
                }

                windFlowIndex += 1;

                if (windFlowIndex >= windFlow.Length)
                    windFlowIndex = 0;
            }
        }

        return towerHeight;
    }

    private static void PushRock(
        string[] rock,
        string[] map,
        int rockPosition,
        char windDirection,
        ref int horizontalPosition)
    {
        int height = 0;

        if (windDirection == '>')
        {
            if (horizontalPosition + rock[0].Length + 1 <= 7 &&
                ColliderChecker(
                    rock,
                    map,
                    rockPosition,
                    horizontalPosition,
                    horizontalPosition + 1,
                    ref height))
                horizontalPosition += 1;
        }
        else
        {
            if (horizontalPosition - 1 >= 0 &&
                ColliderChecker(
                    rock,
                    map,
                    rockPosition,
                    horizontalPosition,
                    horizontalPosition - 1,
                    ref height))
                    horizontalPosition -= 1;
        }
    }

    private static bool ColliderChecker(
        string[] rock,
        string[] map,
        int rockPosition,
        int currPosition,
        int nextPosition,
        ref int towerHeight)
    {
        for (int i = rock.Length - 1; i >= 0; i--)
        {
            string currentLevel = map[rockPosition + i];
            string levelBelow = map[rockPosition + i - 1];

            for (int j = 0; j < rock[i].Length; j++)
            {
                if (rock[i][j] == '.')
                    continue;

                if (currPosition == nextPosition)
                {
                    char positionBelow = levelBelow[j + 1 + currPosition];

                    if (positionBelow == '#' || positionBelow == '-')
                    {
                        towerHeight = Math.Max(
                            towerHeight,
                            rockPosition + rock.Length - 1);

                        return false;
                    }
                }
                else
                {
                    if (nextPosition > currPosition)
                    {
                        if (currentLevel[currPosition + j + 2] == '#')
                            return false;
                    }
                    else
                    {
                        if (currentLevel[currPosition + j] == '#')
                            return false;
                    }
                }
            }
        }

        return true;
    }

    private static void AddRockToMap(
        string[] rock,
        string[] map,
        int rockPosition,
        int horizontalPosition)
    {
        for (int i = rock.Length - 1; i >= 0; i--)
        {
            StringBuilder builder = new();

            for (int j = 0; j < map[0].Length; j++)
            {
                if (j < horizontalPosition + 1 ||
                    j > horizontalPosition + rock[i].Length)
                    builder.Append(map[i + rockPosition][j]);
                else
                    builder.Append(rock[i][j - horizontalPosition - 1]);
            }

            map[i + rockPosition] = builder.ToString();
        }
    }

    private static void DrawMap(IEnumerable<string> map)
    {
        foreach (string line in map)
            Console.WriteLine(line);
    }

    private static IEnumerable<string> GenerateMap(int size)
    {
        for (int i = 0; i < size; i++)
        {
            if (i == 0)
                yield return "+-------+";

            yield return "|.......|";
        }
    }

    private static IList<string[]> GenerateRockShapes() => new List<string[]>
    {
        new string[1] { "####" },
        new string[3] { ".#.", "###", ".#." },
        new string[3] { "###", "..#", "..#" },
        new string[4] { "#", "#", "#", "#" },
        new string[2] { "##", "##" }
    };
}