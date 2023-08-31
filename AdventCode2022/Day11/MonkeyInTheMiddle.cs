namespace AdventCode2022.Day11;

public sealed class MonkeyInTheMiddle
{
    private Monkey[] _monkeys;

    public MonkeyInTheMiddle()
    {
        string currentDirectory = PathHelper.
            GetCurrentDirectory("Day11", "MonkeyInTheMiddleInput.txt");
        StreamReader file = new(currentDirectory);
        IEnumerable<string?> rawData = file.ImportData();

        _monkeys = ProcessData(rawData).ToArray();

    }

    private static IEnumerable<Monkey> ProcessData(IEnumerable<string?> data)
    {
        List<int> items = new();
        string operation = "";
        int divideBy = 0;
        int ifTrue = 0;
        int ifFalse = 0;

        foreach (string? line in data)
        {
            switch (line)
            {
                case { } when line.Contains("Starting items:"):
                    items = new();
                    items.AddRange(GetItems(line).ToList());
                    break;
                case { } when line.Contains("Operation:"):
                    operation = line.Split(':')[1].Trim();
                    break;
                case { } when line.Contains("Test:"):
                    divideBy = ExtractNumber(line);
                    break;
                case { } when line.Contains("If true:"):
                    ifTrue = ExtractNumber(line);
                    break;
                case { } when line.Contains("If false:"):
                    ifFalse = ExtractNumber(line);
                    yield return new Monkey(items, operation, divideBy, ifTrue, ifFalse);
                    break;
               default:
                    break;
            }
        }
    }

    private static IEnumerable<int> GetItems(string line)
    {
        string[] items = line.Split(':')[1].Trim().Split(", ");

        foreach (string item in items)
        {
            yield return Convert.ToInt32(item);
        }
    }

    private static int ExtractNumber(string line) =>
        Convert.ToInt32(FindNumberInArrayOfStrings(SplitString(line)));

    private static string FindNumberInArrayOfStrings(string[] array) =>
        array.First(s => int.TryParse(s, out _));

    private static string[] SplitString(string line) =>
        line.Split(':')[1].Trim().Split(' ');

    private record Monkey
    {
        public List<int> Items { get; private set; }
        public string Operation { get; }
        public int DivisibleBy { get; } 
        public int ThrowToIfTrue { get; }
        public int ThrowToIfFalse { get; }
        public int NoOfInspections { get; private set; }
        private readonly Dictionary<char, Func<int, int, int>> OperationResult = new()
        {
            { 'x', (x, y) => x + y },
            { '-', (x, y) => x-y },
            { '*', (x, y) => x * y }
        };

        public Monkey(
            List<int> items,
            string operation,
            int divisibleBy,
            int throwToIfTrue,
            int throwToIfFalse)
        {
            Items = items;
            Operation = operation;
            DivisibleBy = divisibleBy;
            ThrowToIfTrue = throwToIfTrue;
            ThrowToIfFalse = throwToIfFalse;
        }
    }
}