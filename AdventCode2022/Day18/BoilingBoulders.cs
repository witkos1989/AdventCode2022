namespace AdventCode2022.Day18;

public sealed class BoilingBoulders
{
    private readonly IDictionary<(int, int, int), Point3D> _cubesPosition;

	public BoilingBoulders()
	{
        string currentDirectory = PathHelper.
            GetCurrentDirectory("Day18", "BoilingBouldersInput.txt");
        StreamReader file = new(currentDirectory);
        IEnumerable<string?> rawData = file.ImportData();
        _cubesPosition = ProcessData(rawData).
            ToDictionary(d => (d.X, d.Y, d.Z));
    }

    public int[] Results()
    {
        int[] results = new int[2];

        results[0] = SurfaceArea(_cubesPosition);

        return results;
    }

    private static int SurfaceArea(IDictionary<(int, int, int), Point3D> cubes)
    {
        int sum = 0;

        foreach (Point3D cube in cubes.Values)
        {
            List<Point3D> connected = new();

            for (int i = -1; i <= 1; i += 2)
            {
                if (cubes.ContainsKey((cube.X + i, cube.Y, cube.Z)))
                    connected.Add(cubes[(cube.X + i, cube.Y, cube.Z)]);

                if (cubes.ContainsKey((cube.X, cube.Y + i, cube.Z)))
                    connected.Add(cubes[(cube.X, cube.Y + i, cube.Z)]);

                if (cubes.ContainsKey((cube.X, cube.Y, cube.Z + i)))
                    connected.Add(cubes[(cube.X, cube.Y, cube.Z + i)]);
            }

            sum += 6 - connected.Count;
        }

        return sum;
    }

    private static IEnumerable<Point3D> ProcessData(IEnumerable<string?> data)
    {
        foreach (string? line in data)
        {
            if (string.IsNullOrEmpty(line))
                continue;

            string[] points = line.Split(',');

            yield return new Point3D()
            {
                X = int.Parse(points[0]),
                Y = int.Parse(points[1]),
                Z = int.Parse(points[2]),
            };
        }
    }

    private record Point3D
    {
        public int X;
        public int Y;
        public int Z;
    }
}