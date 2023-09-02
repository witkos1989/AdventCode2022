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

    public int[] Results()
    {
        int[] results = new int[2];

        results[0] = KeepAwayGame(_monkeys);

        return results;
    }

    private static int KeepAwayGame(Monkey[] monkeys)
    {
        int[] monkeyInspections = new int[monkeys.Length];

        for (int i = 0; i < 20; i++)
        {
            foreach (Monkey monkey in monkeys)
            {
                int countItems = monkey.CountItems();

                while(countItems > 0)
                {
                    int itemInMonkeyHand = monkey.InspectAndCalculateWorryLevel();

                    int throwTo = monkey.IsDivisible(itemInMonkeyHand) ?
                        monkey.ThrowToIfTrue :
                        monkey.ThrowToIfFalse;

                    monkey.ThrowItemToMonkey(monkeys[throwTo], itemInMonkeyHand);

                    countItems = monkey.CountItems();
                }
            }
        }

        for (int i = 0; i < monkeys.Length; i++)
        {
            monkeyInspections[i] = monkeys[i].NoOfInspections;
        }

        monkeyInspections = monkeyInspections.OrderDescending().ToArray();

        return monkeyInspections[0] * monkeyInspections[1];
    }

    private static IEnumerable<Monkey> ProcessData(IEnumerable<string?> data)
    {
        List<int> items = new();
        Func<int, int?, int> operation = (x, y) => x;
        int? operationParameter = null;
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
                    (Func<int, int?, int>, int?) operationAndParamer =
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

    private static IEnumerable<int> ParseItems(string line)
    {
        string[] items = line.Split(':')[1].Trim().Split(", ");

        foreach (string item in items)
        {
            yield return Convert.ToInt32(item);
        }
    }

    private static (Func<int, int?, int>, int?)
        GetOperationAndSecondParameter(string line)
    {
        (char, int?) data = GenerateOperationData(line);

        Func<int, int?, int> monkeyOperation = data switch
        {
            { } when data.Item1 == '+' => data.Item2 is null ?
                                ((x, y) => x + x) :
                                ((x, y) => x + (int)y!),
            { } when data.Item1 == '*' => data.Item2 is null ?
                                ((x, y) => x * x) :
                                ((x, y) => x * (int)y!),
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
        private readonly List<int> Items;
        public Func<int, int?, int> Operation { get; }
        public int? OperationParam { get; }
        public int DivisibleBy { get; } 
        public int ThrowToIfTrue { get; }
        public int ThrowToIfFalse { get; }
        public int NoOfInspections { get; private set; }

        public Monkey(
            List<int> items,
            Func<int, int?, int> operation,
            int? operationParam,
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

        public int CountItems() =>
            Items.Count;

        public int InspectAndCalculateWorryLevel()
        {
            NoOfInspections++;

            int item = Items.First();

            Items.Remove(item);

            return Operation.Invoke(item, OperationParam) / 3;
        }

        public bool IsDivisible(int item) =>
            item % DivisibleBy == 0;

        public void ThrowItemToMonkey(Monkey monkey, int item) =>
            monkey.AddItem(item);

        private void AddItem(int item) =>
            Items.Add(item);
    }
}