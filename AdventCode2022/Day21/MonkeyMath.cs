using System.Text.RegularExpressions;

namespace AdventCode2022.Day21;

public sealed class MonkeyMath
{
    private readonly Regex _pattern;
    private readonly Dictionary<string, Monkey> _monkeys;

    public MonkeyMath()
    {
        string currentDirectory = PathHelper.
            GetCurrentDirectory("Day21", "MonkeyMathInput.txt");
        StreamReader file = new(currentDirectory);
        IEnumerable<string?> rawData = file.ImportData();

        _pattern = new("([a-z]{1,}): ([0-9]{1,}|([a-z]{1,}) ([+-/*]) ([a-z]{1,}))",
            RegexOptions.Compiled);

        _monkeys = ProcessData(rawData, _pattern).ToDictionary(k => k.Name);
    }

    public long[] Results()
    {
        long[] results = new long[2];

        results[0] = (long)FindNumberDFS(_monkeys, "root");

        results[1] = (long)FindHumanNumber(_monkeys);

        return results;
    }

    private static decimal FindHumanNumber(Dictionary<string, Monkey> monkeys)
    {
        Monkey root = monkeys["root"];
        Monkey human = monkeys["humn"];
        bool isHumanOnLeftSide = FindHumanBranch(monkeys, monkeys[root.Left!]);
        decimal min = 0;
        decimal max = long.MaxValue;

        human.Number = 1;

        decimal valueOnOne = isHumanOnLeftSide ?
            FindNumberDFS(monkeys, root.Left!) :
            FindNumberDFS(monkeys, root.Right!);

        human.Number = 10;

        decimal valueOnTen = isHumanOnLeftSide ?
            FindNumberDFS(monkeys, root.Left!) :
            FindNumberDFS(monkeys, root.Right!);

        human.Number = min + (max - min) / 2;

        bool isIncreasing = valueOnTen > valueOnOne;
        decimal left = FindNumberDFS(monkeys, root.Left!);
        decimal right = FindNumberDFS(monkeys, root.Right!);

        while (left != right)
        {
            decimal humanSide = isHumanOnLeftSide ? left : right;
            decimal otherSide = isHumanOnLeftSide ? right : left;

            if (humanSide > otherSide)
            {
                if (isIncreasing)
                    max = min + (max - min) / 2;
                else
                    min += (max - min) / 2;
            }
            else
            {
                if (isIncreasing)
                    min += (max - min) / 2;
                else
                    max = min + (max - min) / 2;
            }

            human.Number = min + (max - min) / 2;

            left = FindNumberDFS(monkeys, root.Left!);

            right = FindNumberDFS(monkeys, root.Right!);
        }

        return Math.Round((decimal)human.Number);
    }

    private static bool FindHumanBranch(
    Dictionary<string, Monkey> monkeys,
    Monkey monkey)
    {
        if (monkey.Name == "humn")
            return true;

        if (monkey.Number is not null)
            return false;

        bool leftBranch = FindHumanBranch(monkeys, monkeys[monkey.Left!]);
        bool rightBranch = FindHumanBranch(monkeys, monkeys[monkey.Right!]);

        return leftBranch || rightBranch;
    }

    private static decimal FindNumberDFS(
        Dictionary<string, Monkey> monkeys,
        string monkeyName)
    {
        Monkey monkey = monkeys[monkeyName];

        if (monkey.Number is not null)
            return (decimal)monkey.Number;

        decimal leftNumber = FindNumberDFS(monkeys, monkey.Left!);
        decimal rightNumber = FindNumberDFS(monkeys, monkey.Right!);

        return DoOperation(leftNumber, rightNumber, monkey.Operation);
    }

    private static decimal DoOperation(
        decimal left,
        decimal right,
        char? operation) =>
        operation switch
        {
            '+' => left + right,
            '-' => left - right,
            '/' => left / right,
            '*' => left * right,
            _ => 0
        };

    private static IEnumerable<Monkey> ProcessData(
        IEnumerable<string?> data,
        Regex pattern)
    {
        foreach (string? line in data)
        {
            if (string.IsNullOrEmpty(line))
                continue;

            Match? match = pattern.Match(line);

            if (match is null)
                continue;

            if (!string.IsNullOrEmpty(match.Groups[3].Value))
            {
                Monkey monkey = new(
                    match.Groups[1].Value,
                    match.Groups[3].Value,
                    match.Groups[5].Value,
                    match.Groups[4].Value[0]);

                yield return monkey;
            }
            else
            {
                Monkey monkey = new(
                    match.Groups[1].Value,
                    long.Parse(match.Groups[2].Value));

                yield return monkey;
            }
        }
    }

    private record Monkey
    {
        public string Name;
        public decimal? Number;
        public string? Left;
        public string? Right;
        public char? Operation;

        public Monkey(string name, decimal number) =>
            (Name, Number) = (name, number);

        public Monkey(string name, string left, string right, char operation) =>
            (Name, Left, Right, Operation) = (name, left, right, operation);
    }
}