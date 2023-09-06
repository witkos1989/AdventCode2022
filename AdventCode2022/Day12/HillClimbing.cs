namespace AdventCode2022.Day12;

public class HillClimbing
{
    private readonly char[][] _data;
    private readonly (int[], int)[][][] _pathList;

    public HillClimbing()
    {
        string currentDirectory = PathHelper.
            GetCurrentDirectory("Day12", "HillClimbingInput.txt");
        StreamReader file = new(currentDirectory);
        IEnumerable<string?> rawData = file.ImportData();

        _data = ProcessData(rawData!).ToArray();

        _pathList = GeneratePaths(_data).ToArray();
    }

    public int[] Results()
    {
        int[] results = new int[2];

        results[0] = FindShortestPath(_data, _pathList);

        return results;
    }

    private static int FindShortestPath(
        char[][] map,
        (int[], int)[][][] pathList)
    {
        int[] start = FindPosition(map, 'S');
        int[] end = FindPosition(map, 'E');

        int[][] path = DijkstraPath(
            pathList,
            start,
            end,
            map.Length,
            map[0].Length);

        return path.Length - 1;
    }

    private static int[][] DijkstraPath(
        (int[], int)[][][] paths,
        int[] start,
        int[] end,
        int lengthX,
        int lengthY)
    {
        IMinHeap<NextEdge> distancesHeap = new MinHeap<NextEdge>();
        bool[][] visited = GenerateSeenMap(lengthX, lengthY).
            ToArray();
        int[][] distances = GenerateArrayForDistances(lengthX, lengthY).
            ToArray();
        int[][][] previous = GenerateArrayForPreviousLocations(lengthX,lengthY).
            ToArray();

        distances[start[0]][start[1]] = 0;

        distancesHeap.Insert(new NextEdge(start, 0));

        while (IsUnvisited(visited, distances))
        {
            NextEdge? current = distancesHeap.Delete();

            if (current is null)
            {
                continue;
            }

            visited[current.Index[0]][current.Index[1]] = true;

            (int[], int)[] adjs = paths[current.Index[0]][current.Index[1]];

            for (int i = 0; i < adjs.Length; i++)
            {
                (int[] To, int Weight) = adjs[i];

                if (visited[To[0]][To[1]])
                {
                    continue;
                }

                int distance = distances[current.Index[0]][current.Index[1]] +
                    Weight;

                var nextNode = new NextEdge(To, Weight);

                if (!distancesHeap.Contains(nextNode))
                {
                    distancesHeap.Insert(nextNode);
                }

                if (distance < distances[To[0]][To[1]])
                {
                    distancesHeap.Update(nextNode, new NextEdge(To, distance));

                    distances[To[0]][To[1]] = distance;

                    previous[To[0]][To[1]] = current.Index;
                }
            }
        }

        IList<int[]> result = new List<int[]>();
        int[] temp = end;

        while (previous[temp[0]][temp[1]][0] != -1)
        {
            result.Add(temp);

            temp = previous[temp[0]][temp[1]];
        }

        result.Add(start);

        return result.Reverse().ToArray();
    }

    private static bool IsUnvisited(bool[][] visited, int[][] distances)
    {
        bool unvisited = false;

        for (int i = 0; i < visited.Length; i++)
        {
            for (int j = 0; j < visited[0].Length; j++)
            {
                if (!visited[i][j] && distances[i][j] < int.MaxValue)
                    unvisited = true;
            }
        }

        return unvisited;
    }

    private static IEnumerable<(int[], int)[][]> GeneratePaths(char[][] map)
    {

        for (int i = 0; i < map.Length; i++)
        {
            List<(int[], int)[]> edges = new();

            for (int j = 0; j < map[i].Length; j++)
            {
                List<(int[], int)> edge = new();

                char height = map[i][j] == 'S' ?
                    'a' : map[i][j] == 'E' ?
                    'z' : map[i][j];

                if (j + 1 < map[0].Length &&
                    IsAbleToClimb(height, map[i][j + 1]))
                    edge.Add((new int[2] { i, j + 1 }, 1));

                if (i + 1 < map.Length &&
                    IsAbleToClimb(height, map[i + 1][j]))
                    edge.Add((new int[2] { i + 1, j }, 1));

                if (j - 1 >= 0 &&
                    IsAbleToClimb(height, map[i][j - 1]))
                    edge.Add((new int[2] { i, j - 1 }, 1));

                if (i - 1 >= 0 &&
                    IsAbleToClimb(height, map[i - 1][j]))
                    edge.Add((new int[2] { i - 1, j }, 1));

                edges.Add(edge.ToArray());
            }

            yield return edges.ToArray();
        }
    }

    private static bool IsAbleToClimb(char current, char next) => next switch
    {
        'S' => true,
        'E' => current == 'z',
        _ => next - current < 2
    };

    private static int[] FindPosition(char[][] map, char search)
    {
        int[] searchedPosition = new int[2] { 0, 0 };

        for (int i = 0; i < map.Length; i++)
        {
            for (int j = 0; j < map[i].Length; j++)
            {
                if (map[i][j] == search)
                {
                    searchedPosition = new int[2] { i, j };
                    break;
                }
            }
        }

        return searchedPosition;
    }

    private static IEnumerable<bool[]> GenerateSeenMap(
        int lengthX,
        int lengthY)
    {
        for (int i = 0; i < lengthX; i++)
        {
            yield return new bool[lengthY];
        }
    }

    private static IEnumerable<int[]> GenerateArrayForDistances(
        int lengthX,
        int lengthY)
    {
        for (int i = 0; i < lengthX; i++)
        {
            yield return new int[lengthY].Select(value =>
                value = int.MaxValue).ToArray();
        }
    }

    private static IEnumerable<int[][]> GenerateArrayForPreviousLocations(
        int lengthX,
        int lengthY)
    {
        for (int i = 0; i < lengthX; i++)
        {
            yield return new int[lengthY][].Select(value =>
                value = new int[2] { -1, -1 }).ToArray();
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

file record NextEdge : IComparable<NextEdge>
{
    internal int[] Index;
    internal int Weight;

    internal NextEdge(int[] index, int weight) =>
        (Index, Weight) = (index, weight);

    public int CompareTo(NextEdge? obj) =>
        obj is null ? 1 : Weight.CompareTo(obj.Weight);
}