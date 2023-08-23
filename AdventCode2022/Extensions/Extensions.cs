namespace AdventCode2022.Extensions;

public static class Extensions
{
    public static IEnumerable<string?> ImportData(this StreamReader stream)
    {
        for (; ; )
        {
            if (stream.EndOfStream)
                break;

            string? line = stream.ReadLine();

            yield return line;
        }
    }
}