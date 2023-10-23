namespace AdventCode2022.Day18;

public sealed class BoilingBoulders
{
    private readonly IDictionary<Point3D, Point3D> _cubesPosition;

    public BoilingBoulders()
    {
        string currentDirectory = PathHelper.
            GetCurrentDirectory("Day18", "BoilingBouldersInput.txt");
        StreamReader file = new(currentDirectory);
        IEnumerable<string?> rawData = file.ImportData();
        _cubesPosition = ProcessData(rawData).
            ToDictionary(d => d);
    }

    public int[] Results()
    {
        int[] results = new int[2];

        results[0] = SurfaceArea(_cubesPosition);

        results[1] = ExteriorSurfaceArea(_cubesPosition);

        return results;
    }

    private static int ExteriorSurfaceArea(
        IDictionary<Point3D, Point3D> cubes)
    {
        int sum = 0;
        int max = Math.Max(cubes.Values.Max(p => p.X),
            Math.Max(cubes.Values.Max(p => p.Y), cubes.Values.Max(p => p.Z)));
        int min = Math.Min(cubes.Values.Min(p => p.X),
            Math.Min(cubes.Values.Min(p => p.Y), cubes.Values.Min(p => p.Z)));

        foreach (Point3D cube in cubes.Values)
        {
            for (int i = -1; i <= 1; i += 2)
            {
                sum = IsExternal(cubes, new Point3D(cube.X + 1, cube.Y, cube.Z),
                    max, min) ?
                    sum + 1 :
                    sum;
                sum = IsExternal(cubes, new Point3D(cube.X, cube.Y + 1, cube.Z),
                    max, min) ?
                    sum + 1 :
                    sum;
                sum = IsExternal(cubes, new Point3D(cube.X, cube.Y, cube.Z + 1),
                    max, min) ?
                    sum + 1 :
                    sum;
            }
        }

        return sum;
    }

    private static bool IsExternal(
        IDictionary<Point3D, Point3D> cubes,
        Point3D cube,
        int max,
        int min)
    {
        Stack<Point3D> stack = new();
        Dictionary<Point3D, Point3D> seen = new();

        stack.Push(cube);

        while (stack.Count > 0)
        {
            Point3D point = stack.Pop();

            if (cubes.ContainsKey(point))
                continue;

            if (point.X >= max || point.X <= min ||
                point.Y >= max || point.Y <= min ||
                point.Z >= max || point.Z <= min)
                return true;

            if (seen.ContainsKey(point))
                continue;

            seen.Add(point, point);

            stack.Push(new Point3D(point.X + 1, point.Y, point.Z));

            stack.Push(new Point3D(point.X - 1, point.Y, point.Z));

            stack.Push(new Point3D(point.X, point.Y + 1, point.Z));

            stack.Push(new Point3D(point.X, point.Y - 1, point.Z));

            stack.Push(new Point3D(point.X, point.Y, point.Z + 1));

            stack.Push(new Point3D(point.X, point.Y, point.Z - 1));
        }

        return false;
    }

    private static int SurfaceArea(IDictionary<Point3D, Point3D> cubes) =>
        cubes.Select(cube => CountCubeOpenArea(cubes, cube.Value)).Sum();

    private static int CountCubeOpenArea(
        IDictionary<Point3D, Point3D> cubes, Point3D cube)
    {
        List<Point3D> connected = new();

        for (int i = -1; i <= 1; i += 2)
        {
            if (cubes.ContainsKey(new Point3D(cube.X + i, cube.Y, cube.Z)))
                connected.Add(cubes[new Point3D(cube.X + i, cube.Y, cube.Z)]);

            if (cubes.ContainsKey(new Point3D(cube.X, cube.Y + i, cube.Z)))
                connected.Add(cubes[new Point3D(cube.X, cube.Y + i, cube.Z)]);

            if (cubes.ContainsKey(new Point3D(cube.X, cube.Y, cube.Z + i)))
                connected.Add(cubes[new Point3D(cube.X, cube.Y, cube.Z + i)]);
        }

        return 6 - connected.Count;
    }

    private static IEnumerable<Point3D> ProcessData(IEnumerable<string?> data)
    {
        foreach (string? line in data)
        {
            if (string.IsNullOrEmpty(line))
                continue;

            string[] points = line.Split(',');

            yield return new Point3D(
                int.Parse(points[0]),
                int.Parse(points[1]),
                int.Parse(points[2]));
        }
    }

    private record Point3D
    {
        public int X;
        public int Y;
        public int Z;

        public Point3D(int x, int y, int z)
        {
            (X, Y, Z) = (x, y, z);
        }
    }
}