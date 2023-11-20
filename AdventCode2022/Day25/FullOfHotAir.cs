namespace AdventCode2022.Day25;

public sealed class FullOfHotAir
{
    private readonly string[] _snafuArray;

	public FullOfHotAir()
	{
        string currentDirectory = PathHelper.
            GetCurrentDirectory("Day25", "FullOfHotAirInput.txt");
        StreamReader file = new(currentDirectory);
        _snafuArray = file.ImportData().ToArray()!;
    }

    public string Result() => ToSnafu(SumInput(_snafuArray));

    private static long SumInput(string[] snafuArray) =>
        snafuArray.Select(ToDecimal).Sum();

    private static string ToSnafu(long @decimal)
    {
        List<long> listOfFives = new() { 1 };
        long powerOfFive = 1;

        while (@decimal * 2 > powerOfFive * 5)
            listOfFives.Add(powerOfFive *= 5);

        return CalculateSnafuNumber(listOfFives, @decimal);
    }

    private static string CalculateSnafuNumber(List<long> fives, long @decimal)
    {
        byte snafuIndex = 0;
        Dictionary<(char, byte), long> cache = new(); 
        char[] snafus = Enumerable.Repeat('2', fives.Count).ToArray();
        long value = ToDecimal(new string(snafus));

        cache.Add((snafus[snafuIndex], snafuIndex), value);

        while (value != @decimal)
        {
            snafus[snafuIndex] =
                ChangeValue(snafus[snafuIndex], value <= @decimal);

            if (cache.ContainsKey((snafus[snafuIndex], snafuIndex)))
                snafuIndex += 1;

            cache.Add((snafus[snafuIndex], snafuIndex), value);

            value = ToDecimal(new string(snafus));
        }

        return new string(snafus);
    }

    private static long ToDecimal(string snafu) =>
        snafu.Select((s, i) =>
        CalculateResult(s, (long)Math.Pow(5, snafu.Length - i - 1))).Sum();

    private static long CalculateResult(char snafuSign, long powerOfFive) =>
        snafuSign switch
        {
            '2' => powerOfFive * 2,
            '1' => powerOfFive,
            '-' => powerOfFive * -1,
            '=' => powerOfFive * -2,
             _  => 0
        };

    private static char ChangeValue(char snafuSign, bool increase) =>
        snafuSign switch
        {
            '2' => increase ? '2' : '1',
            '1' => increase ? '2' : '0',
            '0' => increase ? '1' : '-',
            '-' => increase ? '0' : '=',
            '=' => increase ? '-' : '=',
            _ => ' '
        };
}