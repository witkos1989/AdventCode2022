namespace AdventCode2022.Day9;

public sealed class RopeBridge
{
    private readonly (char, int)[] _data;
    private bool[][]? _board;

	public RopeBridge()
	{
        string currentDirectory = PathHelper.
            GetCurrentDirectory("Day9", "RopeBridgeInput.txt");
        StreamReader file = new(currentDirectory);
        IEnumerable<string?> rawData = file.ImportData();

        _data = ProcessData(rawData!).ToArray();

        InitBoard();
    }

    public int[] Results()
    {
        int[] results = new int[2];

        results[0] = CountPath(_board!, 2);

        InitBoard();

        results[1] = CountPath(_board!, 10);

        return results;
    }

    private void InitBoard()
    {
        _board = new bool[400][];

        for (int i = 0; i < _board.Length; i++)
        {
            _board[i] = new bool[_board.Length];
        }
    }

    private int CountPath(bool[][] board, int knotsNumber)
    {
        StartSimulation(_data, knotsNumber);

        int sum = 0;
        foreach (bool[] line in board)
        {
            sum += line.Where(x => x.Equals(true)).Count();
        }

        return sum;
    }

    private void StartSimulation((char, int)[] steps, int noOfKnots)
    {
        int[,] knots = new int[noOfKnots, 2];

        for (int i = 0; i < noOfKnots; i++)
        {
            knots[i, 0] = _board!.Length / 2;
            knots[i, 1] = _board!.Length / 2;
        }

        _board![knots[0, 0]][knots[0, 1]] = true;

        foreach ((char, int) step in steps)
        {
            for (int i = 0; i < step.Item2; i++)
            {
                switch (step.Item1)
                {
                    case 'l':
                        knots[0, 0] -= 1;
                        break;
                    case 'r':
                        knots[0, 0] += 1;
                        break;
                    case 'u':
                        knots[0, 1] += 1;
                        break;
                    case 'd':
                        knots[0, 1] -= 1;
                        break;
                    default:
                        break;
                }

                UpdateKnotsPosition(knots, noOfKnots);
            }       
        }
    }

    private void UpdateKnotsPosition(int[,] knots, int noOfKnots)
    {
        int[] head = new int[2] { knots[0, 0], knots[0, 1] };

        for (int i = 1; i < noOfKnots; i++)
        {
            int[] tail = new int[2] { knots[i, 0], knots[i, 1] };

            if (!IsTailNextToHead(head, tail))
            {
                int moveX = tail[0] == head[0] ? 0 :
                    (head[0] - tail[0]) / Math.Abs(head[0] - tail[0]);
                int moveY = tail[1] == head[1] ? 0 :
                    (head[1] - tail[1]) / Math.Abs(head[1] - tail[1]);

                knots[i, 0] += moveX;

                knots[i, 1] += moveY;

                if (i == noOfKnots - 1)
                {
                    _board![knots[i, 0]][knots[i, 1]] = true;
                }
            }

            head = new int[2] { knots[i, 0], knots[i, 1] };
        }
    }

    private static bool IsTailNextToHead(int[] headPosition, int[] tailPosition)
    {
        int xDifference = Math.Abs(tailPosition[0] - headPosition[0]);
        int yDifference = Math.Abs(tailPosition[1] - headPosition[1]);

        return !(xDifference > 1 || yDifference > 1);
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