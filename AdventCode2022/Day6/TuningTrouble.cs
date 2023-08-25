using System.Text;

namespace AdventCode2022.Day6;

public sealed class TuningTrouble
{
	private readonly string _data;

	public TuningTrouble()
	{
        string currentDirectory = PathHelper.
            GetCurrentDirectory("Day6", "TuningTroubleInput.txt");
        StreamReader file = new(currentDirectory);
        IEnumerable<string?> rawData = file.ImportData();

        _data = ProcessData(rawData!);
    }

    public int[] Solutions()
    {
        int[] results = new int[2];

        results[0] = FindingMarker(4);

        results[1] = FindingMarker(14);

        return results;
    }

    private int FindingMarker(int lengthOfPacket)
    {
        StringBuilder failedMarker = new();
        StringBuilder marker = new();
        marker.Append(_data.AsSpan(0, lengthOfPacket));

        for (int i = lengthOfPacket; i < _data.Length; i++)
        {
            bool isCorrectMarker = marker.ToString().GroupBy(x => x).Any(c => c.Count() > 1);

            if (!isCorrectMarker)
                break;

            failedMarker.Append(marker[0]);

            marker.Remove(0, 1);

            marker.Append(_data[i]);
        }

        return failedMarker.Length + marker.Length;
    }

    private static string ProcessData(IEnumerable<string> data)
    {
        string result = "";

        foreach (var line in data)
        {
            result += line;
        }

        return result;
    }
}