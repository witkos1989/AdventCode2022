using System.Text;
using System.Text.RegularExpressions;

namespace AdventCode2022.Day5;

public partial class SupplyStacks
{
	private IEnumerable<Stack<char>>? _firstStacks;
    private IEnumerable<Stack<char>>? _secondStacks;
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
        string[] results = new string[2];

        results[0] = TopCratesUsingStacks();

        results[1] = TopCratesUsingLists();

        return results;
    }

    private string TopCratesUsingLists()
    {
        StringBuilder builder = new();

        List<char>[] results = NewRearrangement();

        foreach (List<char> result in results)
        {
            builder.Append(result[0]);
        }

        return builder.ToString();
    }

    private string TopCratesUsingStacks()
    {
        StringBuilder builder = new();

        Stack<char>[] results = Rearrangement();

        foreach (Stack<char> result in results)
        {
            builder.Append(result.Peek());
        }

        return builder.ToString();
    }

    private List<char>[] NewRearrangement()
    {
        if (_secondStacks is null && _instructions is null)
            return Array.Empty<List<char>>();

        Stack<char>[] stackArray = _secondStacks!.ToArray();
        List<char>[] stacks = new List<char>[stackArray.Length];

        for (int i = 0; i < stackArray.Length; i++)
        {
            stacks[i] = stackArray[i].ToList();
        }

        foreach (int[] move in _instructions!)
        {
            char[] crates = stacks[move[1] - 1].Take(move[0]).ToArray();

            stacks[move[1] - 1].RemoveRange(0, move[0]);

            stacks[move[2] - 1].InsertRange(0, crates);
        }

        return stacks;
    }

    private Stack<char>[] Rearrangement()
    {
        if (_firstStacks is null && _instructions is null)
            return Array.Empty<Stack<char>>();

        Stack<char>[] stacks = _firstStacks!.ToArray();

        foreach (int[] move in _instructions!)
        {
            for (int i = 0; i < move[0]; i++)
            {
                char crate = stacks[move[1] - 1].Pop();

                stacks[move[2] - 1].Push(crate);
            }
        }

        return stacks;
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

        _firstStacks = PutCratesOnStacks(crates, numOfStacks);

        _secondStacks = PutCratesOnStacks(crates, numOfStacks);

        _instructions = AddInscrutions(data);
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

    private static Stack<char>[] PutCratesOnStacks(List<string> input, int numOfStacks)
    {
        Stack<char>[] supplyStacks = new Stack<char>[numOfStacks];

        foreach (var line in input)
        {
            for (int i = 0; i < line.Length; i++)
            {
                if (char.IsLetter(line[i]) && char.IsUpper(line[i]))
                {
                    int index = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(i / 4)));

                    if (supplyStacks[index] is null)
                        supplyStacks[index] = new();

                    supplyStacks[index].Push(line[i]);
                }
            }
        }

        return supplyStacks;
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