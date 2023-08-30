namespace AdventCode2022.Day10;

public class CathodeRayTube
{
    private readonly IEnumerable<(string, int?)> _data;
    private readonly char[,] _crtScreen;

	public CathodeRayTube()
	{
        string currentDirectory = PathHelper.
            GetCurrentDirectory("Day10", "CathodeRayTubeInput.txt");
        StreamReader file = new(currentDirectory);
        IEnumerable<string?> rawData = file.ImportData();

        _data = ProcessData(rawData!);
        _crtScreen = new char[40,6];
    }

    public int Result()
    {
        return CalculateSignals(_data.ToArray());
    }

    public void GenerateResultOnScreen()
    {
        Console.WriteLine("Result shown on the screen according to signal:");

        for (int i = 0; i < _crtScreen.Length; i++)
        {
            int column = i % 40;
            int row = (int)Math.Floor((decimal)(i / 40));

            if (i > 1 && column == 0)
            {
                Console.WriteLine();
            }

            Console.Write(_crtScreen[column, row]);
        }
        Console.WriteLine();
    }

    private int CalculateSignals((string, int?)[] instructions)
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

    private int AddCycles(int steps, int register, ref int signalStrength)
    {
        DrawOnCrt(steps, register);

        steps += 1;

        if (steps % 40 == 0 + 20)
        {
            signalStrength += steps * register;
        }

        return steps;
    }

    private void DrawOnCrt(int steps, int register)
    {
        int column = steps % 40;
        int row = (int)Math.Floor((decimal)(steps / 40));
        int[] registerFields = GenerateSignalFields(register);
        bool drawSpriteOnScreen = CheckSpritePosition(registerFields, column);

        _crtScreen[column, row] = drawSpriteOnScreen ? '#' : '.';
    }

    private static bool CheckSpritePosition(int[] fields, int column)
    {
        foreach (int field in fields)
        {
            if (field == column)
            {
                return true;
            }
        }

        return false;
    }

    private static int[] GenerateSignalFields(int register) => register switch
        {
            -1 => new int[1] { register + 1 },
            0 => new int[2] { register, register + 1 },
            39 => new int[2] { register - 1, register },
            40 => new int[1] { register - 1 },
            _ => new int[3] { register - 1, register, register + 1 },
        };

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