using System.Text.RegularExpressions;

namespace AdventCode2022.Day19;

public sealed class NotEnoughMinerals
{
    private readonly Regex _resources;
    private readonly IEnumerable<Blueprint> _blueprints;
    private byte _maxGeodes = 0;

    public NotEnoughMinerals()
	{
        string currentDirectory = PathHelper.
            GetCurrentDirectory("Day19", "NotEnoughMineralsInput.txt");
        StreamReader file = new(currentDirectory);
        IEnumerable<string?> rawData = file.ImportData();

        _resources = new("[0-9]{1,}", RegexOptions.Compiled);

        _blueprints = ProcessData(rawData, _resources);
    }

    public int[] Results()
    {
        int[] results = new int[2];

        results[0] = CollectingGeodes(_blueprints, 24);

        return results;
    }

    private int CollectingGeodes(IEnumerable<Blueprint> blueprints, byte time)
    {
        int sum = 0;

        foreach (Blueprint blueprint in blueprints)
        {
            byte maxOre = Math.Max(
            Math.Max(blueprint.OreRobot, blueprint.ClayRobot),
            Math.Max(blueprint.ObsidianRobot[0], blueprint.GeodeRobot[0]));

            GeodesDFS(0, 0, 0, 0, 1, 0, 0, 0, 4, maxOre, blueprint, time);

            sum += blueprint.Number * _maxGeodes;

            _maxGeodes = 0;
        }

        return sum;
    }

    private void GeodesDFS(byte ore, byte clay, byte obsidian, byte geode,
                           byte oRobot, byte cRobot, byte sRobot, byte gRobot,
                           byte build, byte maxOre,
                           Blueprint blueprint, byte time)
    {
        if (time <= 0)
            return;

        ore += oRobot;
        clay += cRobot;
        obsidian += sRobot;
        geode += gRobot;

        _maxGeodes = Math.Max(_maxGeodes, geode);

        switch (build)
        {
            case 0:
                oRobot += 1;
                ore -= blueprint.OreRobot;
                break;
            case 1:
                cRobot += 1;
                ore -= blueprint.ClayRobot;
                break;
            case 2:
                sRobot += 1;
                ore -= blueprint.ObsidianRobot[0];
                clay -= blueprint.ObsidianRobot[1];
                break;
            case 3:
                gRobot += 1;
                ore -= blueprint.GeodeRobot[0];
                obsidian -= blueprint.GeodeRobot[1];
                break;
            default:
                break;
        }

        time -= 1;

        if (clay >= blueprint.GeodeRobot[0] &&
            obsidian >= blueprint.GeodeRobot[1])
        {
            GeodesDFS(ore, clay, obsidian, geode,
                      oRobot, cRobot, sRobot, gRobot,
                      3, maxOre, blueprint, time);

            return;
        }

        if (ore >= blueprint.ObsidianRobot[0] &&
            clay >= blueprint.ObsidianRobot[1] &&
            sRobot < blueprint.GeodeRobot[1])
        {
            GeodesDFS(ore, clay, obsidian, geode,
                      oRobot, cRobot, sRobot, gRobot,
                      2, maxOre, blueprint, time);

            return;
        }

        if (ore >= blueprint.ClayRobot &&
            cRobot < blueprint.ObsidianRobot[1] &&
            cRobot < blueprint.GeodeRobot[1])
        {
            GeodesDFS(ore, clay, obsidian, geode,
                      oRobot, cRobot, sRobot, gRobot,
                      1, maxOre, blueprint, time);
        }

        if (ore >= blueprint.OreRobot &&
            oRobot < maxOre)
        {
            GeodesDFS(ore, clay, obsidian, geode,
                      oRobot, cRobot, sRobot, gRobot,
                      0, maxOre, blueprint, time);

            return;
        }

        GeodesDFS(ore, clay, obsidian, geode,
                      oRobot, cRobot, sRobot, gRobot,
                      4, maxOre, blueprint, time);
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

            byte number = byte.Parse(match[0].Value);
            byte oreRobot = byte.Parse(match[1].Value);
            byte clayRobot = byte.Parse(match[2].Value);
            byte[] obsidianRobot = new byte[]
            {
                byte.Parse(match[3].Value),
                byte.Parse(match[4].Value)
            };
            byte[] geodeRobot = new byte[]
            {
                byte.Parse(match[5].Value),
                byte.Parse(match[6].Value)
            };
            Blueprint blueprint =
                new(number, oreRobot, clayRobot, obsidianRobot, geodeRobot);

            yield return blueprint;
        }
    }

    private record Blueprint
    {
        public byte Number;
        public byte OreRobot;
        public byte ClayRobot;
        public byte[] ObsidianRobot;
        public byte[] GeodeRobot;

        public Blueprint(
            byte number,
            byte oreRobot,
            byte clayRobot,
            byte[] obsidianRobot,
            byte[] geodeRobot)
        {
            (Number, OreRobot, ClayRobot, ObsidianRobot, GeodeRobot) =
                (number, oreRobot, clayRobot, obsidianRobot, geodeRobot);
        }
    }
}