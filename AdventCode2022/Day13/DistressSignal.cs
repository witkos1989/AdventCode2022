namespace AdventCode2022.Day13;

public sealed class DistressSignal
{
    private readonly IEnumerable<List<object>[]> _data;

    public DistressSignal()
	{
        string currentDirectory = PathHelper.
            GetCurrentDirectory("Day13", "DistressSignalInput.txt");
        StreamReader file = new(currentDirectory);
        IEnumerable<string?> rawData = file.ImportData();

        _data = ProcessData(rawData);
    }

    public int[] Results()
    {
        int[] results = new int[2];

        results[0] = CountSumOfValidIndices(_data);

        return results;
    }

    private static int CountSumOfValidIndices(IEnumerable<List<object>[]> input)
    {
        int sum = 0;
        List<bool?> validityOfInputs = CheckInputOrders(input).ToList();

        for (int i = 1; i <= validityOfInputs.Count; i++)
        {
            if (validityOfInputs[i - 1] is not null && (bool)validityOfInputs[i - 1]!)
                sum += i;
        }

        return sum;
    }

    private static IEnumerable<bool?> CheckInputOrders(IEnumerable<List<object>[]> input) =>
        input.Select(lists => ListComparer(lists[0], lists[1]));


    private static bool? ListComparer(List<object> leftList, List<object> rightList)
    {
        bool? isInTheRightOrder = null;
        int longerListCount = leftList.Count > rightList.Count ?
            leftList.Count :
            rightList.Count;

        for (int i = 0; i < longerListCount; i++)
        {
            if (i >= leftList.Count)
            {
                isInTheRightOrder = true;

                break;
            }

            if (i >= rightList.Count)
            {
                isInTheRightOrder = false;

                break;
            }

            if (leftList[i] is int leftVal && rightList[i] is int rightVal)
            {
                int leftSideValue = leftVal;
                int rightSideValue = rightVal;

                if (leftSideValue > rightSideValue)
                    isInTheRightOrder = false;

                if (leftSideValue < rightSideValue)
                    isInTheRightOrder = true;
            }

            if (leftList[i] is List<object> leftSubList)
            {
                if (rightList[i] is List<object> rightSubList)
                {
                    isInTheRightOrder =
                        ListComparer(leftSubList, rightSubList);
                }
                else
                {
                    List<object> newList = new() { (int)rightList[i] };

                    isInTheRightOrder =
                        ListComparer(leftSubList, newList);
                }
            }
            else if (rightList[i] is List<object> rightSmallerList)
            {
                if (leftList[i] is List<object> leftSmallerList)
                {
                    isInTheRightOrder =
                        ListComparer(leftSmallerList, rightSmallerList);
                }
                else
                {
                    List<object> newList = new() { (int)leftList[i] };

                    isInTheRightOrder =
                        ListComparer(newList, rightSmallerList);
                }
            }

            if (isInTheRightOrder is not null)
                break;
        }

        return isInTheRightOrder;
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