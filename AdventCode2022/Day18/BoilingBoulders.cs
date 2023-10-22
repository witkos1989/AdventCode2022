namespace AdventCode2022.Day18;

public sealed class BoilingBoulders
{
    private readonly IEnumerable<Point3D> _dropletsPosition;

	public BoilingBoulders()
	{
        string currentDirectory = PathHelper.
            GetCurrentDirectory("Day18", "BoilingBouldersInput.txt");
        StreamReader file = new(currentDirectory);
        IEnumerable<string?> rawData = file.ImportData();
        _dropletsPosition = ProcessData(rawData);
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