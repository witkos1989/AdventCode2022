namespace AdventCode2022.Day10;

public class CathodeRayTube
{
    private readonly IEnumerable<(string, int?)> _data;

	public CathodeRayTube()
	{
        string currentDirectory = PathHelper.
            GetCurrentDirectory("Day10", "CathodeRayTubeInput.txt");
        StreamReader file = new(currentDirectory);
        IEnumerable<string?> rawData = file.ImportData();

        _data = ProcessData(rawData!);
    }

    public int[] Results()
    {
        int[] results = new int[2];

        results[0] = CalculateSignals(_data.ToArray());

        return results;
    }

    private static int CalculateSignals((string, int?)[] instructions)
    {
        int signalStrength = 0;
        int steps = 0;
        int register = 1;

        foreach ((string, int?) instruction in instructions)
        {
            switch (instruction.Item1)
            {
                case { } when instruction.Item1.StartsWith("noop"):
                    steps = AddCycles(steps, register, ref signalStrength);
                    break;
                case { } when instruction.Item1.StartsWith("addx"):
                    steps = AddCycles(steps, register, ref signalStrength);
                    steps = AddCycles(steps, register, ref signalStrength);
                    register += (int)instruction.Item2!;
                    break;
            }
        }

        return signalStrength;
    }

    private static int AddCycles(int steps, int register, ref int signalStrength)
    {
        steps += 1;

        if (steps % 40 == 0 + 20 || steps == 20)
        {
            signalStrength += steps * register;
        }

        return steps;
    }

    private static IEnumerable<(string, int?)> ProcessData(IEnumerable<string> data)
    {
        int? value;

        foreach (string line in data)
        {
            string[] instruction = line.Split(" ");

            value = instruction.Length > 1 ? Convert.ToInt32(instruction[1]) : null;

            yield return (instruction[0], value);
        }
    }
}