namespace AdventCode2022.Day9;

public class RopeBridge
{
    private readonly (char, int)[] _data;
    private bool[][] _board;

	public RopeBridge()
	{
        string currentDirectory = PathHelper.
            GetCurrentDirectory("Day9", "RopeBridgeInput.txt");
        StreamReader file = new(currentDirectory);
        IEnumerable<string?> rawData = file.ImportData();

        _data = ProcessData(rawData!).ToArray();

        _board = new bool[400][];

        for (int i = 0; i < _board.Length; i++)
        {
            _board[i] = new bool[_board.Length];
        }
    }

    public int[] Results()
    {
        int[] results = new int[2];

        results[0] = CountPath(_board);

        return results;
    }

    private int CountPath(bool[][] board)
    {
        StartSimulation(_data);

        int sum = 0;
        foreach (bool[] line in board)
        {
            sum += line.Where(x => x.Equals(true)).Count();
        }

        return sum;
    }

    private void StartSimulation((char, int)[] steps)
    {
        int[] head = new int[2] { _board.Length / 2, _board.Length / 2 };
        int[] tail = new int[2] { _board.Length / 2, _board.Length / 2 };

        _board[tail[0]][tail[1]] = true;

        foreach ((char, int) step in steps)
        {
            for (int i = 0; i < step.Item2; i++)
            {
                int[] previousPosition = new int[2] { head[0], head[1] };

                switch (step.Item1)
                {
                    case 'l':                     
                        head[0] -= 1;
                        break;
                    case 'r':
                        head[0] += 1;
                        break;
                    case 'u':
                        head[1] += 1;
                        break;
                    case 'd':
                        head[1] -= 1;
                        break;
                    default:
                        break;
                }

                UpdateTailPosition(head, tail, previousPosition);
            }       
        }
    }

    private void UpdateTailPosition(int[] head, int[] tail, int[] previousPosition)
    {
        if (!IsTailNextToHead(head, tail))
        {
            (tail[0], tail[1]) = (previousPosition[0], previousPosition[1]);

            _board[tail[0]][tail[1]] = true;
        }
    }

    private static bool IsTailNextToHead(int[] headPosition, int[] tailPosition)
    {
        int xDifference = Math.Abs(tailPosition[0] - headPosition[0]);
        int yDifference = Math.Abs(tailPosition[1] - headPosition[1]);

        if (xDifference > 1 || yDifference > 1)
            return false;

        return true;
    }

    private static IEnumerable<(char, int)> ProcessData(IEnumerable<string> input)
    {
        foreach (string line in input)
        {
            string[] step = line.Split(" ");
            char direction = Convert.ToChar(step[0].ToLower());
            int moves = Convert.ToInt32(step[1]);

            yield return (direction, moves);
        }
    }
}