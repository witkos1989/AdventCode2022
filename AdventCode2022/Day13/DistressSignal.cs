namespace AdventCode2022.Day13;

public sealed class DistressSignal
{
	public DistressSignal()
	{
        string currentDirectory = PathHelper.
            GetCurrentDirectory("Day13", "TestInput.txt");
        StreamReader file = new(currentDirectory);
        IEnumerable<string?> rawData = file.ImportData();

        List<List<object>[]> data = ProcessData(rawData).ToList(); 
    }

    private static IEnumerable<List<object>[]> ProcessData(IEnumerable<string?> data)
    {
        List<object>[] lists = new List<object>[2];
        int noOfLine = 0;

        foreach (string? line in data)
        {
            int nestedArraysCount = 0;

            if (string.IsNullOrEmpty(line))
            {
                noOfLine = 0;

                continue;
            }

            ObjectTree mainList = new() { Children = new() };
            ObjectTree tempList = new() { Children = new() };

            string trimmedLine = line![1..^1];

            for (int i = 0; i < trimmedLine.Length; i++)
            {
                switch (trimmedLine[i])
                {
                    case { } when char.IsDigit(trimmedLine[i]):
                        int value = (int)trimmedLine[i] - 48;

                        if (i + 1 < trimmedLine.Length && char.IsDigit(trimmedLine[i + 1]))
                        {
                            value = (value * 10) + (int)trimmedLine[i + 1] - 48;
                            i++;
                        }

                        ObjectTree node = new() { Value = value };

                        if (nestedArraysCount == 0)
                        {
                            node.Parent = mainList;
                            mainList.Children.Add(node);
                        }
                        else
                        {
                            node.Parent = tempList;
                            tempList.Children!.Add(node);
                        }
                        break;

                    case { } when trimmedLine[i] == '[':
                        nestedArraysCount++;

                        ObjectTree list = new() { Children = new() };

                        if (nestedArraysCount == 1)
                        {
                            list.Parent = mainList;
                            mainList.Children.Add(tempList);
                        }
                        else
                        {
                            list.Parent = tempList;

                            tempList.Children!.Add(list);

                            tempList = list;
                        }
                        break;

                    case { } when trimmedLine[i] == ']':
                        nestedArraysCount--;
                        if (nestedArraysCount == 0)
                        {
                            tempList = new() { Children = new() };
                        }
                        else
                        {
                            tempList = tempList.Parent!;
                        }
                        break;
                }
            }

            lists[noOfLine] = mainList.AsList();
            
            noOfLine++;

            if (noOfLine == 2)
            {
                yield return lists;

                lists = new List<object>[2];
            }
        }
    }

    private sealed record ObjectTree
    {
        public ObjectTree? Parent;
        public int? Value;
        public List<ObjectTree>? Children;

        public List<object> AsList()
        {
            List<object> result = new();

            if (Children is null)
                return result;

            foreach (ObjectTree child in Children)
            {
                if (child.Value is not null)
                    result.Add(child.Value);

                if (child.Children is not null)
                    result.Add(child.AsList());
            }

            return result;
        }
    }
}