using System.Text;
using System.Text.RegularExpressions;

namespace AdventCode2022.Day5;

public sealed partial class SupplyStacks
{
	private IEnumerable<Stack<char>>? _firstStacks;
    private IEnumerable<List<char>>? _secondStacks;
    private IEnumerable<int[]>? _instructions;

	public SupplyStacks()
	{
        string currentDirectory = PathHelper.
           GetCurrentDirectory("Day5", "SupplyStacksInput.txt");
        StreamReader file = new(currentDirectory);
        IEnumerable<string?> rawData = file.ImportData();

        ProcessData(rawData);
    }

    public string[] Solutions()
    {
        return TopCratesFromStacksAndLists();
    }

    private string[] TopCratesFromStacksAndLists()
    {
        StringBuilder stacks = new();
        StringBuilder lists = new();
        string[] stackAndListResult = new string[2];

        (Stack<char>[], List<char>[]) results = Rearrangement();

        foreach (Stack<char> result in results.Item1)
        {
            stacks.Append(result.Peek());
        }

        foreach (List<char> result in results.Item2)
        {
            lists.Append(result[0]);
        }

        (stackAndListResult[0], stackAndListResult[1]) = (stacks.ToString(), lists.ToString());

        return stackAndListResult;
    }

    private (Stack<char>[], List<char>[]) Rearrangement()
    {
        if (_firstStacks is null && _instructions is null)
            return (Array.Empty<Stack<char>>(), Array.Empty<List<char>>());

        Stack<char>[] stacks = _firstStacks!.ToArray();
        List<char>[] lists = _secondStacks!.ToArray();

        foreach (int[] move in _instructions!)
        {
            for (int i = 0; i < move[0]; i++)
            {
                char crate = stacks[move[1] - 1].Pop();

                stacks[move[2] - 1].Push(crate);
            }

            char[] crates = lists[move[1] - 1].Take(move[0]).ToArray();

            lists[move[1] - 1].RemoveRange(0, move[0]);

            lists[move[2] - 1].InsertRange(0, crates);
        }

        return (stacks, lists);
    }

    private void ProcessData(IEnumerable<string?> data)
    {
        string stackNumberNeedle = " 1   2   3  ";
        int numOfStacks = 0;
        List<string> crates = new();

        foreach (var line in data)
        {
            if (string.IsNullOrEmpty(line))
                continue;

            crates.Add(line);

            if (line.Contains(stackNumberNeedle))
            {
                numOfStacks = DefineNoOfStacks(line);
                break;
            }
        }

        crates.Reverse();

        PutCratesOnStacks(crates, numOfStacks);

        _instructions = AddInscrutions(data);
    }

    private void PutCratesOnStacks(List<string> input, int numOfStacks)
    {
        Stack<char>[] supplyStacks = new Stack<char>[numOfStacks];
        List<char>[] supplyLists = new List<char>[numOfStacks];

        foreach (var line in input)
        {
            for (int i = 0; i < line.Length; i++)
            {
                if (char.IsLetter(line[i]) && char.IsUpper(line[i]))
                {
                    int index = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(i / 4)));

                    if (supplyStacks[index] is null)
                        supplyStacks[index] = new();

                    if (supplyLists[index] is null)
                        supplyLists[index] = new();

                    supplyStacks[index].Push(line[i]);
                    supplyLists[index].Add(line[i]);
                }
            }
        }

        foreach (var list in supplyLists)
        {
            list.Reverse();
        }

        (_firstStacks, _secondStacks) = (supplyStacks, supplyLists);
    }

    private static IEnumerable<int[]> AddInscrutions(IEnumerable<string?> data)
    {
        foreach (var line in data)
        {
            List<int> instructions = new();

            if (string.IsNullOrEmpty(line))
                continue;

            var test = string.Concat(line.Where(char.IsDigit).ToArray());

            string[] numbers = ExtractDigits().Split(line);

            foreach (string number in numbers)
            {
                if (string.IsNullOrEmpty(number))
                    continue;

                instructions.Add(Convert.ToInt32(number));
            }

            yield return instructions.ToArray();
        }
    }

    private static int DefineNoOfStacks(string line)
    {
        int stacksNo = 0;

        foreach (char number in line)
        {
            if (char.IsNumber(number))
                stacksNo = number - 48;
        }

        return stacksNo;
    }

    [GeneratedRegex("\\D+")]
    private static partial Regex ExtractDigits();
}