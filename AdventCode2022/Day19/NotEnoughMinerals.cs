using System.Text.RegularExpressions;

namespace AdventCode2022.Day19;

public sealed class NotEnoughMinerals
{
    private readonly Regex _resources;
    private readonly IEnumerable<Blueprint> _blueprints;

	public NotEnoughMinerals()
	{
        string currentDirectory = PathHelper.
            GetCurrentDirectory("Day19", "NotEnoughMineralsInput.txt");
        StreamReader file = new(currentDirectory);
        IEnumerable<string?> rawData = file.ImportData();

        _resources = new("[0-9]{1,}", RegexOptions.Compiled);

        _blueprints = ProcessData(rawData, _resources);
    }

    private static IEnumerable<Blueprint> ProcessData(
        IEnumerable<string?> data,
        Regex pattern)
    {
        foreach (string? line in data)
        {
            if (string.IsNullOrEmpty(line))
                continue;

            MatchCollection? match = pattern.Matches(line);

            if (match is null)
                continue;

            int number = int.Parse(match[0].Value);
            int oreRobot = int.Parse(match[1].Value);
            int clayRobot = int.Parse(match[2].Value);
            int[] obsidianRobot = new int[]
            {
                int.Parse(match[3].Value),
                int.Parse(match[4].Value)
            };
            int[] geodeRobot = new int[]
            {
                int.Parse(match[5].Value),
                int.Parse(match[6].Value)
            };
            Blueprint blueprint =
                new(number, oreRobot, clayRobot, obsidianRobot, geodeRobot);

            yield return blueprint;
        }
    }

    private record Blueprint
    {
        public int Number;
        public int OreRobot;
        public int ClayRobot;
        public int[] ObsidianRobot;
        public int[] GeodeRobot;

        public Blueprint(
            int number,
            int ore,
            int clay,
            int[] obsidian,
            int[] geode)
        {
            (Number, OreRobot, ClayRobot, ObsidianRobot, GeodeRobot) =
                (number, ore, clay, obsidian, geode);
        }
    }
}