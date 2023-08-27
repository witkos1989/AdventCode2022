namespace AdventCode2022.Extensions;

public static class Extensions
{
    public static IEnumerable<string?> ImportData(this StreamReader stream)
    {
        while (!stream.EndOfStream)
        {
            string? line = stream.ReadLine();

            yield return line;
        }
    }

    public static IEnumerable<int[]> ImportInts(this StreamReader stream)
    {
        while (!stream.EndOfStream)
        {
            string? line = stream.ReadLine();

            if (string.IsNullOrEmpty(line))
                continue;

            int[] lineOfInts = new int[line.Length];

            for (int i = 0; i < line.Length; i++)
            {
                lineOfInts[i] = line[i] - 48;
            }

            yield return lineOfInts;
        }
    }
}