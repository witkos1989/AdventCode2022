using System.Text.RegularExpressions;

namespace AdventCode2022.Day21;

public sealed class MonkeyMath
{
	private readonly Regex _pattern;
    private readonly Dictionary<string, Monkey> _monkeyYells;

	public MonkeyMath()
	{
        string currentDirectory = PathHelper.
            GetCurrentDirectory("Day21", "MonkeyMathInput.txt");
        StreamReader file = new(currentDirectory);
        IEnumerable<string?> rawData = file.ImportData();

        _pattern = new("([a-z]{1,}): ([0-9]{1,}|([a-z]{1,}) ([+-/*]) ([a-z]{1,}))",
			RegexOptions.Compiled);

        _monkeyYells = ProcessData(rawData, _pattern).ToDictionary(k => k.Name);
    }

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
                    int.Parse(match.Groups[2].Value));

                yield return monkey;
            }
        }
    }

    private record Monkey
    {
        public string Name;
        public int? Number;
        public string? Left;
        public string? Right;
        public char? Operation;

        public Monkey(string name, int number) =>
            (Name, Number) = (name, number);

        public Monkey(string name, string left, string right, char operation) =>
            (Name, Left, Right, Operation) = (name, left, right, operation);
    }
}