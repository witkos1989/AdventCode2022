namespace AdventCode2022.Day3;

public sealed class RucksackReorganization
{
    private readonly List<string> _data = new();

    public RucksackReorganization()
	{
        string currentDirectory = Path.GetDirectoryName(
            Path.GetDirectoryName(
                Path.GetDirectoryName(
                    Directory.GetCurrentDirectory())))!;
        string archiveFolder = Path.Combine(currentDirectory, "Day3");
        StreamReader file = new(archiveFolder + "/BackpackSupplies.txt");

        _data = ImportData(file).ToList();
    }

    public int[] Solutions()
    {
        int[] results = new int[2];

        results[0] = CountCommonProducts(true);

        results[1] = CountCommonProducts(false);

        return results;
    }

    private int CountCommonProducts(bool checkingCompartments)
    {
        IEnumerable<string[]> splittedData = checkingCompartments ?
            SplitInHalf(_data) :
            GroupByThree(_data);
        IEnumerable<char[]> sameProducts = FindCommonsInArray(splittedData);

        return SumAllProducts(sameProducts);
    }

    private static int SumAllProducts(IEnumerable<char[]> commonProducts)
    {
        int sum = 0;

        foreach (char[] products in commonProducts)
        {
            foreach (char product in products)
            {
                sum += FoodPriority(product);
            }
        }

        return sum;
    }

    private static int FoodPriority(char product) => product switch
    {
        >= 'a' and <= 'z' => (int)product - 96,
        >= 'A' and <= 'Z' => (int)product - 38,
        _ => 0
    };

    private static IEnumerable<char[]> FindCommonsInArray(IEnumerable<string[]> input)
    {
        foreach (string[] backpacks in input)
        {
            IEnumerable<char> intersect = backpacks[0].Intersect(backpacks[1]);

            for (int i = 2; i < backpacks.Length; i++)
            {
                intersect = intersect.Intersect(backpacks[i]);
            }

            yield return intersect.ToArray();
        }
    }

    private static IEnumerable<string[]> GroupByThree(IEnumerable<string> data)
    {
        List<string> threeBackpacks = new();
        foreach (string item in data)
        {
            threeBackpacks.Add(item);

            if (threeBackpacks.Count == 3)
            {
                yield return threeBackpacks.ToArray();

                threeBackpacks.Clear();
            }
        }
    }

    private static IEnumerable<string[]> SplitInHalf(IEnumerable<string> data)
    {
        foreach (string line in data)
        {
            string[] products = new string[2];

            products[0] = line[..(int)(line.Length / 2)];

            products[1] = line.Substring((int)(line.Length / 2), (int)(line.Length / 2));

            yield return products;
        }
    }

    private static IEnumerable<string> ImportData(StreamReader stream)
    {
        for (; ; )
        {
            if (stream.EndOfStream)
                break;

            string? line = stream.ReadLine();

            if (string.IsNullOrEmpty(line))
                continue;

            yield return line;
        }
    }
}