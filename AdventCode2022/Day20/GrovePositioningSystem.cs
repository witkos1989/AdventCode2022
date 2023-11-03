using System.Text.RegularExpressions;

namespace AdventCode2022.Day20;

public sealed class GrovePositioningSystem
{
    private readonly Regex _numbers;
    private readonly IEnumerable<(int, int)> _sequence;
    public GrovePositioningSystem()
	{
        string currentDirectory = PathHelper.
            GetCurrentDirectory("Day20", "GrovePositioningSystemInput.txt");
        StreamReader file = new(currentDirectory);
        IEnumerable<string?> rawData = file.ImportData();

        _numbers = new("[-0-9]{1,}", RegexOptions.Compiled);

        _sequence = ProcessData(rawData, _numbers);
    }

    public int[] Results()
    {
        List<(int, int)> numbers = _sequence.ToList();
        int[] results = new int[2];

        MixingFile(numbers);

        results[0] = GroveCoordinates(numbers);

        return results;
    }

    private static int GroveCoordinates(List<(int, int)> sequence)
    {
        int fileLength = sequence.Count;
        int firstCoord = GetValueAtIndex(sequence, fileLength, 1000);
        int secondCoord = GetValueAtIndex(sequence, fileLength, 2000);
        int thirdCoord = GetValueAtIndex(sequence, fileLength, 3000);

        return firstCoord + secondCoord + thirdCoord;
    }

    private static void MixingFile(List<(int Index, int Value)> sequence)
    {
        int fileLength = sequence.Count - 1;

        for (int i = 0; i <= fileLength; i++)
        { 
            (int Index, int Value) number =
                sequence.First(item => item.Index == i);

            if (number.Value == 0)
                continue;

            int index = sequence.IndexOf(number);

            index += number.Value;

            while (index <= 0)
                index = fileLength - Math.Abs(index);

            while (index > fileLength)
                index -= fileLength;

            sequence.Remove(number);

            sequence.Insert(index, number);
        }
    }

    private static int GetValueAtIndex(
        List<(int Index, int Value)> data,
        int dataLength,
        int index)
    {
        var zero = data.Where(i => i.Value == 0);
        (int, int) zeroValue = data.First(i => i.Value == 0);
        int startPos = data.IndexOf(zeroValue);

        index += startPos;

        while (index > dataLength)
            index -= dataLength;

        int value = data.ElementAt(index).Value;

        return value;
    }

    private static IEnumerable<(int, int)> ProcessData(
        IEnumerable<string?> data,
        Regex pattern)
    {
        int i = 0;
        foreach (string? line in data)
        {
            if (string.IsNullOrEmpty(line))
                continue;

            Match? match = pattern.Match(line);

            if (match is null)
                continue;

            yield return (i, int.Parse(match.Value));

            i++;
        }
    }
}