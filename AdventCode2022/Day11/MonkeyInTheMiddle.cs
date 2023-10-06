namespace AdventCode2022.Day11;

public sealed class MonkeyInTheMiddle
{
    private readonly Monkey[] _monkeys;

    public MonkeyInTheMiddle()
    {
        string currentDirectory = PathHelper.
            GetCurrentDirectory("Day11", "MonkeyInTheMiddleInput.txt");
        StreamReader file = new(currentDirectory);
        IEnumerable<string?> rawData = file.ImportData();

        _monkeys = ProcessData(rawData).ToArray();
    }

    public long Results(byte partNo) => 
        partNo == 1 ?
        KeepAwayGame(_monkeys, false) :
        KeepAwayGame(_monkeys, true);
    
    private static long KeepAwayGame(Monkey[] monkeys, bool selfWorryLevel)
    {
        long[] monkeyInspections = new long[monkeys.Length];
        long mod = CalculateCommonDivision(monkeys);
        int noOfIterations = selfWorryLevel ? 10000 : 20;

        for (int i = 0; i < noOfIterations; i++)
        {
            foreach (Monkey monkey in monkeys)
            {
                for (int item = 0; item < monkey.Items.Count; item++)
                {
                    long itemInMonkeyHand = monkey.Items[item];

                    long worryLevel = selfWorryLevel ? monkey.
                        CalculateWorryLevelByModulo(itemInMonkeyHand, mod) :
                        monkey.CalculateWorryLevelByDivision(itemInMonkeyHand);

                    int throwTo = worryLevel % monkey.DivisibleBy == 0 ?
                        monkey.ThrowToIfTrue :
                        monkey.ThrowToIfFalse;

                    monkeys[throwTo].Items.Add(worryLevel);

                    monkey.NoOfInspections++;
                }

                monkey.Items.Clear();
            }
        }

        for (int i = 0; i < monkeys.Length; i++)
        {
            monkeyInspections[i] = monkeys[i].NoOfInspections;
        }

        monkeyInspections = monkeyInspections.OrderDescending().ToArray();

        return monkeyInspections[0] * monkeyInspections[1];
    }

    private static long CalculateCommonDivision(Monkey[] monkeys) =>
        monkeys.
        Select(m => m.DivisibleBy).
        Aggregate(1, (x, y) => x * y);

    private static IEnumerable<Monkey> ProcessData(IEnumerable<string?> data)
    {
        List<long> items = new();
        Func<long, long?, long> operation = (x, y) => x;
        long? operationParameter = null;
        int divideBy = 0;
        int ifTrue = 0;

        foreach (string? line in data)
        {
            switch (line)
            {
                case { } when line.Contains("Starting items:"):
                    items = new();
                    items.AddRange(ParseItems(line).ToList());
                    break;
                case { } when line.Contains("Operation:"):
                    (Func<long, long?, long>, long?) operationAndParamer =
                        GetOperationAndSecondParameter(line);
                    operation = operationAndParamer.Item1;
                    operationParameter = operationAndParamer.Item2;
                    break;
                case { } when line.Contains("Test:"):
                    divideBy = ExtractNumber(line);
                    break;
                case { } when line.Contains("If true:"):
                    ifTrue = ExtractNumber(line);
                    break;
                case { } when line.Contains("If false:"):
                    int ifFalse = ExtractNumber(line);
                    yield return new Monkey(
                        items,
                        operation,
                        operationParameter,
                        divideBy,
                        ifTrue,
                        ifFalse);
                    break;
               default:
                    break;
            }
        }
    }

    private static IEnumerable<long> ParseItems(string line)
    {
        string[] items = line.Split(':')[1].Trim().Split(", ");

        foreach (string item in items)
        {
            yield return Convert.ToInt64(item);
        }
    }

    private static (Func<long, long?, long>, long?)
        GetOperationAndSecondParameter(string line)
    {
        (char, long?) data = GenerateOperationData(line);

        Func<long, long?, long> monkeyOperation = data switch
        {
            { } when data.Item1 == '+' => data.Item2 is null ?
                                ((x, y) => x + x) :
                                ((x, y) => x + (long)y!),
            { } when data.Item1 == '*' => data.Item2 is null ?
                                ((x, y) => x * x) :
                                ((x, y) => x * (long)y!),
            _ => (x, y) => x,
        };

        return (monkeyOperation, data.Item2);
    }

    private static (char, int?) GenerateOperationData(string line)
    {
        string operation = line.Split(':')[1].Trim().Split('=')[1].Trim();

        string[] calculationItems = operation.Split(" ");
        char operand = Convert.ToChar(calculationItems.
            First(c => c == "+" || c == "*"));
        int? number = calculationItems.Last().Contains("old") ?
            null :
            Convert.ToInt32(calculationItems.Last());

        return (operand, number);
    }

    private static int ExtractNumber(string line) =>
        Convert.ToInt32(FindNumberInArrayOfStrings(SplitString(line)));

    private static string FindNumberInArrayOfStrings(string[] array) =>
        array.First(s => int.TryParse(s, out _));

    private static string[] SplitString(string line) =>
        line.Split(':')[1].Trim().Split(' ');

    private record Monkey
    {
        public List<long> Items { get; }
        public Func<long, long?, long> Operation { get; }
        public long? OperationParam { get; }
        public int DivisibleBy { get; } 
        public int ThrowToIfTrue { get; }
        public int ThrowToIfFalse { get; }
        public long NoOfInspections { get; set; }

        public Monkey(
            List<long> items,
            Func<long, long?, long> operation,
            long? operationParam,
            int divisibleBy,
            int throwToIfTrue,
            int throwToIfFalse)
        {
            Items = items;
            Operation = operation;
            OperationParam = operationParam;
            DivisibleBy = divisibleBy;
            ThrowToIfTrue = throwToIfTrue;
            ThrowToIfFalse = throwToIfFalse;
            NoOfInspections = 0;
        }

        public long CalculateWorryLevelByModulo(long item, long modulo) =>
            Operation.Invoke(item, OperationParam) % modulo;

        public long CalculateWorryLevelByDivision(long item) =>
            Operation.Invoke(item, OperationParam) / 3;
    }
}