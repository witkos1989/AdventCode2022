using AdventCode2022.Day1;
using AdventCode2022.Day2;
using AdventCode2022.Day3;

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

Console.ReadKey();