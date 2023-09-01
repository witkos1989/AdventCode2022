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

                    itemInMonkeyHand = monkey.GetBoredOfItem(itemInMonkeyHand);

                    countItems = monkey.CountItems();

                    int throwTo = monkey.IsDivisible(itemInMonkeyHand) ?
                        monkey.ThrowToIfTrue :
                        monkey.ThrowToIfFalse;

                    monkey.ThrowItemToMonkey(monkeys[throwTo], itemInMonkeyHand);
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
        string operation = "";
        int divideBy = 0;
        int ifTrue = 0;

        foreach (string? line in data)
        {
            switch (line)
            {
                case { } when line.Contains("Starting items:"):
                    items = new();
                    items.AddRange(GetItems(line).ToList());
                    break;
                case { } when line.Contains("Operation:"):
                    operation = line.Split(':')[1].Trim().Split('=')[1].Trim();
                    break;
                case { } when line.Contains("Test:"):
                    divideBy = ExtractNumber(line);
                    break;
                case { } when line.Contains("If true:"):
                    ifTrue = ExtractNumber(line);
                    break;
                case { } when line.Contains("If false:"):
                    int ifFalse = ExtractNumber(line);
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
        private readonly List<int> Items;
        public string Operation { get; }
        public int DivisibleBy { get; } 
        public int ThrowToIfTrue { get; }
        public int ThrowToIfFalse { get; }
        public int NoOfInspections { get; private set; }
        private readonly Dictionary<char, Func<int, int, int>> OperationResult = new()
        {
            { '+', (x, y) => x + y },
            { '-', (x, y) => x - y },
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
            NoOfInspections = 0;
        }

        public int CountItems() =>
            Items.Count;

        public List<int> GetItems() =>
            Items;

        public int InspectAndCalculateWorryLevel()
        {
            NoOfInspections++;

            int item = Items.First();
            (int, char) calculations = CalculateOperationFromString(item);

            Items.Remove(item);

            return OperationResult[calculations.Item2].
                Invoke(item, calculations.Item1);
        }

        public int GetBoredOfItem(int item) =>
            (int)Math.Floor((double)(item / 3));

        public bool IsDivisible(int item) =>
            item % DivisibleBy == 0;

        public void ThrowItemToMonkey(Monkey monkey, int item) =>
            monkey.AddItem(item);

        private (int, char) CalculateOperationFromString(int item)
        {
            string[] calculationItems = Operation.Split(" ");
            char operand = Convert.ToChar(calculationItems.
                First(c => c == "+" || c == "-" || c == "*"));
            int number = calculationItems.Last().Contains("old") ?
                item :
                Convert.ToInt32(calculationItems.Last());

            return (number, operand);
        }

        private void AddItem(int item) =>
            Items.Add(item);
    }
}