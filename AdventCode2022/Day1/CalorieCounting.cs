namespace AdventCode2022.Day1;

public sealed class CalorieCounting
{
	private readonly List<List<int>> _data = new();

	public CalorieCounting()
	{
        string currentDirectory = Helpers.Helpers.
			GetCurrentDirectory("Day1", "FoodCaloriesDistribution.txt");
		StreamReader file = new(currentDirectory);
        IEnumerable<string?> rawData = file.ImportData();

        _data = ProcessData(rawData).ToList();
	}

	public int[] Solutions()
	{
		int[] results = new int[2];

		results[0] = ElfWithMaximalCalories();

		results[1] = TopThreeElvesWithMaximalCalories();

		return results;
	}

	private int ElfWithMaximalCalories() =>
		SumCalories(_data).Max();

	private int TopThreeElvesWithMaximalCalories() =>
        SumCalories(_data).OrderDescending().ToArray().Take(3).Sum();

	private static IEnumerable<int> SumCalories(List<List<int>> elfFoods)
	{
		foreach (List<int> foods in elfFoods)
		{
			int caloriesSum = foods.Sum();

			yield return caloriesSum;
		}	
	}

	private static IEnumerable<List<int>> ProcessData(IEnumerable<string?> data)
	{
        List<int> elfInventory = new();

        foreach (string? line in data)
		{
			if (string.IsNullOrEmpty(line))
			{
				yield return elfInventory;
				elfInventory = new();
				continue;
			}

            int calories = Convert.ToInt32(line);

			elfInventory.Add(calories);
        }
	}
}