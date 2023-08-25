using AdventCode2022.Day1;
using AdventCode2022.Day2;
using AdventCode2022.Day3;
using AdventCode2022.Day4;
using AdventCode2022.Day5;
using AdventCode2022.Day6;
using AdventCode2022.Day7;

CalorieCounting elfExpedition = new();

int[] results = elfExpedition.Solutions();

Console.WriteLine(
    "Day 1 results: Elf with maximal calories in backpack = {0}; " +
    "Top three elves with maximal calories in backpack = {1}",
    results[0],
    results[1]);

RockPaperScissors RPSGame = new();

results = RPSGame.Solutions();

Console.WriteLine(
    "Day 2 results: Game score without knowing the instructions = {0}; " +
    "Game score knowing the instructions = {1}",
    results[0],
    results[1]);

RucksackReorganization reorganization = new();

results = reorganization.Solutions();

Console.WriteLine(
    "Day 3 results: Sum of product priorities found in both compartments in backpack = {0}; " +
    "Sum of product priorities found in each three-Elf group = {1}",
    results[0],
    results[1]);

CampCleanup campCleanup = new();

results = campCleanup.Solutions();

Console.WriteLine(
    "Day 4 results: Assignments that fully covers one another = {0}; " +
    "Assignments that overlaps one another = {1}",
    results[0],
    results[1]);

SupplyStacks supplyStacks = new();

string[] supplyResults = supplyStacks.Solutions();

Console.WriteLine(
    "Day 5 results: Crates on top of each stack after rearrangement = {0}; " +
    "Crates on top of each stack after rearrangement using new crane = {1}",
    supplyResults[0],
    supplyResults[1]);

TuningTrouble tuningTrouble = new();

results = tuningTrouble.Solutions();

Console.WriteLine(
    "Day 6 results: Start-of-packet marker found after {0} characters; " +
    "Start-of-message marker found after {1} characters",
    results[0],
    results[1]);

NoSpaceLeft noSpaceLeft = new();

Console.ReadKey();