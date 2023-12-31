﻿using AdventCode2022.Day1;
using AdventCode2022.Day2;
using AdventCode2022.Day3;
using AdventCode2022.Day4;
using AdventCode2022.Day5;
using AdventCode2022.Day6;
using AdventCode2022.Day7;
using AdventCode2022.Day8;
using AdventCode2022.Day9;
using AdventCode2022.Day10;
using AdventCode2022.Day11;
using AdventCode2022.Day12;
using AdventCode2022.Day13;
using AdventCode2022.Day14;
using AdventCode2022.Day15;
using AdventCode2022.Day16;
using AdventCode2022.Day17;
using AdventCode2022.Day18;
using AdventCode2022.Day19;
using AdventCode2022.Day20;
using AdventCode2022.Day21;
using AdventCode2022.Day22;
using AdventCode2022.Day23;
using AdventCode2022.Day24;
using AdventCode2022.Day25;

Console.Write("Which day of advent of code puzzle would you like to run: ");

string? output = Console.ReadLine();

int day;
int[] results;
long[] longResults;

if (!int.TryParse(output, out day))
    return;

switch (day)
{
    case 1:
        CalorieCounting elfExpedition = new();

        results = elfExpedition.Solutions();

        Console.WriteLine("Day 1 results: " +
            "Elf with maximal calories in backpack = {0}; " +
            "Top three elves with maximal calories in backpack = {1}",
            results[0],
            results[1]);
        break;
    case 2:
        RockPaperScissors RPSGame = new();

        results = RPSGame.Solutions();

        Console.WriteLine("Day 2 results: " +
            "Game score without knowing the instructions = {0}; " +
            "Game score knowing the instructions = {1}",
            results[0],
            results[1]);
        break;
    case 3:
        RucksackReorganization reorganization = new();

        results = reorganization.Solutions();

        Console.WriteLine("Day 3 results: " +
            "Sum of product priorities found in both compartments in backpack = {0}; " +
            "Sum of product priorities found in each three-Elf group = {1}",
            results[0],
            results[1]);
        break;
    case 4:
        CampCleanup campCleanup = new();

        results = campCleanup.Solutions();

        Console.WriteLine("Day 4 results: " +
            "Assignments that fully covers one another = {0}; " +
            "Assignments that overlaps one another = {1}",
            results[0],
            results[1]);
        break;
    case 5:
        SupplyStacks supplyStacks = new();

        string[] supplyResults = supplyStacks.Solutions();

        Console.WriteLine("Day 5 results: " +
            "Crates on top of each stack after rearrangement = {0}; " +
            "Crates on top of each stack after rearrangement using new crane = {1}",
            supplyResults[0],
            supplyResults[1]);
        break;
    case 6:
        TuningTrouble tuningTrouble = new();

        results = tuningTrouble.Solutions();

        Console.WriteLine("Day 6 results: " +
            "Start-of-packet marker found after {0} characters; " +
            "Start-of-message marker found after {1} characters",
            results[0],
            results[1]);
        break;
    case 7:
        NoSpaceLeft noSpaceLeft = new();

        results = noSpaceLeft.ShowResult();

        Console.WriteLine("Day 7 results: " +
            "Total size of directories with size under 100000 = {0}; " +
            "Smallest directory size that needs to be deleted to free enough memory = {1}",
            results[0],
            results[1]);
        break;
    case 8:
        TreetopTreeHouse treetopTreeHouse = new();

        results = treetopTreeHouse.Results();

        Console.WriteLine("Day 8 results: " +
            "Tree coverage in the forest = {0}; " +
            "Tree with highest scenic score = {1}",
            results[0],
            results[1]);
        break;
    case 9:
        RopeBridge ropeBridge = new();

        results = ropeBridge.Results();

        Console.WriteLine("Day 9 results: " +
            "Positions visited by the tail at least once with two knots = {0}; " +
            "Positions visited by the tail at least once with ten knots = {1}",
            results[0],
            results[1]);
        break;
    case 10:
        CathodeRayTube cathodeRayTube = new();

        int signalStrength = cathodeRayTube.Result();

        Console.Write("Day 10 results: " +
            "Sum of six signal strengths = {0}; ",
            signalStrength);

        cathodeRayTube.GenerateResultOnScreen();
        break;
    case 11:
        MonkeyInTheMiddle monkeyInTheMiddle = new();
        long firstResult = monkeyInTheMiddle.Results(1);

        monkeyInTheMiddle = new();

        long secondResult = monkeyInTheMiddle.Results(2);

        Console.WriteLine("Day 11 results: " +
            "Level of monkey business with fixed worry level = {0}; " +
            "Level of monkey business with calculated worry level = {1}",
            firstResult,
            secondResult);
        break;
    case 12:
        HillClimbing hillClimbing = new();

        results = hillClimbing.Results();

        Console.WriteLine("Day 12 results: " +
            "Shortest path to get to the top position on the map = {0}; " +
            "Shortest path to get to the top from any lowest point = {1}",
            results[0],
            results[1]);
        break;
    case 13:
        DistressSignal distressSignal = new();

        results = distressSignal.Results();

        Console.WriteLine("Day 13 results: " +
            "Sum of valid indices = {0}; " +
            "Decoder key for the distress signal = {1}",
            results[0],
            results[1]);
        break;
    case 14:
        RegolithReservoir regolithReservoir = new();

        results = regolithReservoir.Results();

        Console.WriteLine("Day 14 results: " +
            "Units of sand came to rest before overflowing = {0}; " +
            "Units of sand came to rest before filling cave = {1}",
            results[0],
            results[1]);
        break;
    case 15:
        BeaconExclusionZone beaconExclusionZone = new();

        results = beaconExclusionZone.Results();

        Console.WriteLine("Day 15 results: " +
            "Positions that doesn't contain beacon in selected row = {0}; " +
            "Tuning frequency of distress signal = {1}",
            results[0],
            results[1]);
        break;
    case 16:
        ProboscideaVolcanium proboscideaVolcanium = new();

        results = proboscideaVolcanium.Results();

        Console.WriteLine("Day 16 results: " +
            "Most pressure that can be realeased = {0}; ",
            results[0]);
        break;
    case 17:
        PyroclasticFlow pyroclasticFlow = new();

        results = pyroclasticFlow.Results();

        Console.WriteLine("Day 17 results: " +
            "Height of tower after 2022 rocks have fallen = {0}; ",
            results[0]);
        break;
    case 18:
        BoilingBoulders boilingBoulders = new();

        results = boilingBoulders.Results();

        Console.WriteLine("Day 18 results: " +
            "Surface area of scanned lava droplets = {0}; " +
            "Exterior surface ares of scanned lava droplets = {1}",
            results[0],
            results[1]);
        break;
    case 19:
        NotEnoughMinerals notEnoughMinerals = new();

        results = notEnoughMinerals.Results();

        Console.WriteLine("Day 19 results: " +
            "Quality level of all of the blueprints = {0}; " +
            "Largest number of geodes using three first blueprints = {1}",
            results[0],
            results[1]);
        break;
    case 20:
        GrovePositioningSystem grovePositioningSystem = new();

        longResults = grovePositioningSystem.Results();

        Console.WriteLine("Day 20 results: " +
            "Sum of numbers that create grove coordinates = {0}; " +
            "Sum of numbers that create grove coordinates using decryption key = {1}",
            longResults[0],
            longResults[1]);
        break;
    case 21:
        MonkeyMath monkeyMath = new();

        longResults = monkeyMath.Results();

        Console.WriteLine("Day 21 results: " +
            "Root monkey yells = {0}; " +
            "I should yell = {1}",
            longResults[0],
            longResults[1]);
        break;
    case 22:
        MonkeyMap monkeyMap = new();

        results = monkeyMap.Results();

        Console.WriteLine("Day 22 results: " +
            "Final password for first puzzle = {0}; " +
            "Final password for second puzzle = {1}",
            results[0],
            results[1]);
        break;
    case 23:
        UnstableDiffusion unstableDiffusion = new();

        results = unstableDiffusion.Results();

        Console.WriteLine("Day 23 results: " +
            "Empty ground tiles in the grove = {0}; " +
            "First round where noone moves = {1}",
            results[0],
            results[1]);
        break;
    case 24:
        BlizzardBasin blizzardBasin = new();

        results = blizzardBasin.Results();

        Console.WriteLine("Day 24 results: " +
            "Fastest time to reach the goal and avoid blizzards = {0}; " +
            "Fastest time to reach the goal, go back and reach the goal once again = {1}",
            results[0],
            results[1]);
        break;
    case 25:
        FullOfHotAir fullOfHotAir = new();

        string stringResult = fullOfHotAir.Result();

        Console.WriteLine("Day 25 results: " +
            "SNAFU number supplied to Bob's console = {0}",
            stringResult);
        break;
    default:
        break;
}

Console.ReadKey();