using System.Text.RegularExpressions;

namespace AdventCode2022.Day20;

public sealed class GrovePositioningSystem
{
    private readonly Regex _numbers;
    private readonly IEnumerable<(long, long)> _sequence;
    public GrovePositioningSystem()
	{
        string currentDirectory = PathHelper.
            GetCurrentDirectory("Day20", "GrovePositioningSystemInput.txt");
        StreamReader file = new(currentDirectory);
        IEnumerable<string?> rawData = file.ImportData();

        _numbers = new("[-0-9]{1,}", RegexOptions.Compiled);

        _sequence = ProcessData(rawData, _numbers);
    }

    public long[] Results()
    {
        long[] results = new long[2];
        List<(long, long)> numbers = _sequence.ToList();
        List<(long, long)> secondNumbers =
            MultiplyByDecriptionKey(numbers).ToList();

        MixingFile(numbers);

        results[0] = GroveCoordinates(numbers);

        MixFileTenTimes(secondNumbers);

        results[1] = GroveCoordinates(secondNumbers);

        return results;
    }

    private static long GroveCoordinates(List<(long, long)> sequence)
    {
        long fileLength = sequence.Count;
        long firstCoord = GetValueAtIndex(sequence, fileLength, 1000);
        long secondCoord = GetValueAtIndex(sequence, fileLength, 2000);
        long thirdCoord = GetValueAtIndex(sequence, fileLength, 3000);

        return firstCoord + secondCoord + thirdCoord;
    }

    private static void MixFileTenTimes(List<(long Index, long Value)> sequence)
    {
        for (int i = 0; i < 10; i++)
        {
            MixingFile(sequence);
        }
    }

    private static void MixingFile(List<(long Index, long Value)> sequence)
    {
        long fileLength = sequence.Count - 1;

        for (int i = 0; i <= fileLength; i++)
        { 
            (long Index, long Value) number =
                sequence.First(item => item.Index == i);

            if (number.Value == 0)
                continue;

            long index = sequence.IndexOf(number);

            index += number.Value;

            index %= fileLength;

            if (index <= 0)
                index = fileLength - Math.Abs(index);

            sequence.Remove(number);

            sequence.Insert((int)index, number);
        }
    }

    private static long GetValueAtIndex(
        List<(long Index, long Value)> data,
        long dataLength,
        long index)
    {
        (long, long) zeroValue = data.First(i => i.Value == 0);
        int startPos = data.IndexOf(zeroValue);

        index += startPos;

        while (index > dataLength)
            index -= dataLength;

        long value = data.ElementAt((int)index).Value;

        return value;
    }

    private static IEnumerable<(long, long)> MultiplyByDecriptionKey(
        IEnumerable<(long, long)> data)
    {
        foreach ((long Index, long Value) number in data)
        {
            long multipliedValue = number.Value * 811589153;

            yield return (number.Index, multipliedValue); 
        }
    }

    private static IEnumerable<(long, long)> ProcessData(
        IEnumerable<string?> data,
        Regex pattern)
    {
        long i = 0;
        foreach (string? line in data)
        {
            if (string.IsNullOrEmpty(line))
                continue;

            Match? match = pattern.Match(line);

            if (match is null)
                continue;

            yield return (i, long.Parse(match.Value));

            i++;
        }
    }
}