﻿namespace AdventCode2022.Day1;

public sealed class ElfExpedition
{
	private readonly List<List<int>> _data = new();

	public ElfExpedition()
	{
		string currentDirectory = Path.GetDirectoryName(
		Path.GetDirectoryName(
			Path.GetDirectoryName(
				Directory.GetCurrentDirectory())))!;
		string archiveFolder = Path.Combine(currentDirectory, "Day1");
		StreamReader file = new(archiveFolder + "/FoodCaloriesDistribution.txt");

		_data = ImportData(file).ToList();
	}

	public int[] Solutions()
	{
		int[] results = new int[2];

		results[0] = ElfWithMaximalCalories();

		results[1] = TopThreeElvesWithMaximalCalories();

		return results;
	}

	private int ElfWithMaximalCalories()
	{
		IEnumerable<int> summedCalories = SumCalories(_data);

		return summedCalories.Max();
	}

	private int TopThreeElvesWithMaximalCalories()
	{
		IEnumerable<int> summedCalories = SumCalories(_data).
		OrderDescending();

		return summedCalories.Take(3).Sum();
	}

	private static IEnumerable<int> SumCalories(List<List<int>> elfFoods)
	{
		foreach (List<int> foods in elfFoods)
		{
			int caloriesSum = foods.Sum();

			yield return caloriesSum;
		}	
	}

	private static IEnumerable<List<int>> ImportData(StreamReader stream)
	{
		bool endOfFile = false;

		while (!endOfFile)
		{
			List<int> elfInventory = new();

			for (; ; )
			{
                if (stream.EndOfStream)
                    endOfFile = true;

                string? caloriesString = stream.ReadLine();

				if (string.IsNullOrEmpty(caloriesString))
					break;

				int calories = Convert.ToInt32(caloriesString);

				elfInventory.Add(calories);
			}

			yield return elfInventory;
		}
	}
}