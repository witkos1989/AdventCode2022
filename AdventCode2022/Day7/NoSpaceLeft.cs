namespace AdventCode2022.Day7;

public sealed class NoSpaceLeft
{
    private readonly TreeNode _data;

    public NoSpaceLeft()
    {
        string currentDirectory = PathHelper.
            GetCurrentDirectory("Day7", "NoSpaceLeftInput.txt");
        StreamReader file = new(currentDirectory);
        IEnumerable<string?> rawData = file.ImportData();

        _data = new TreeNode("/", true);

        ProcessData(rawData.ToArray()!, _data);
    }

    public int[] ShowResult()
    {
        int[] results = new int[2];

        results[0] = CountDirectioriesSize(_data);

        results[1] = FindSmallestDirectoryToSaveEnoughSpace(_data);

        return results;
    }

    private static int FindSmallestDirectoryToSaveEnoughSpace(TreeNode parent)
    {
        int overallSize = (int)parent.Size!;
        int smallestDirectory = overallSize;

        smallestDirectory = FindSmallestRecursion(parent, (int)parent.Size, smallestDirectory);

        return smallestDirectory;
    }

    private static int FindSmallestRecursion(TreeNode parent, int overallSize, int smallestSize)
    {
        TreeNode[] children = parent.Children.Where(c => c.IsDirectory).ToArray();

        foreach (TreeNode child in children)
        {
            int substractionSize = overallSize - (int)child.Size!;

            if (substractionSize < 40000000 && smallestSize > (int)child.Size!)
            {
                smallestSize = (int)child.Size;

                int checkSizeOfChildren = FindSmallestRecursion(child, overallSize, (int)child.Size!);

                if (checkSizeOfChildren < smallestSize)
                {
                    smallestSize = checkSizeOfChildren;
                }
            }
        }

        return smallestSize;
    }

    private static int CountDirectioriesSize(TreeNode parent)
    {
        int sum = 0;

        foreach (TreeNode child in parent.Children)
        {
            if (child.IsDirectory)
            {
                if (child.Size <= 100000)
                    sum += (int)child.Size;

                sum += CountDirectioriesSize(child);
            }
        }

        return sum;
    }

    private static void ProcessData(string[] data, TreeNode parent)
    {
        TreeNode node = parent;
        bool isListingOn = false;

        foreach (var line in data)
        {
            switch (line)
            {
                case { } when line.StartsWith("$ cd"):
                    isListingOn = false;
                    node = SwitchToNode(line, node);
                    break;
                case { } when line.StartsWith("$ ls"):
                    isListingOn = true;
                    break;
                case { } when line.StartsWith("dir"):
                    if (isListingOn)
                        AddChildren(line, node, false);
                    break;
                case { } when char.IsDigit(line[0]):
                    if (isListingOn)
                        AddChildren(line, node, true);
                    break;
            };
        }

        parent.CountSize();
    }

    private static void AddChildren(string line, TreeNode parent, bool isFile)
    {
        string[] fileData = line.Split(" ");

        TreeNode node = isFile ?
            new(fileData[1], false, Convert.ToInt32(fileData[0]), parent) :
            new(fileData[1], true, null, parent);

        parent.Add(node);
    }

    private static TreeNode SwitchToNode(string line, TreeNode node)
    {
        string nodeName = line["$ cd ".Length..];

        if (nodeName.Equals(".."))
            return node.Parent!;

        TreeNode? newNode = node.Contains(nodeName);

        if (newNode is null)
        {
            return node;
        }

        return newNode;
    }

    private record TreeNode
    {
        public string Name { get; }
        public int? Size { get; private set; }
        public bool IsDirectory { get; }
        public List<TreeNode> Children { get; }
        public TreeNode? Parent { get; }

        public TreeNode(string name, bool isDirectory, int? size = null, TreeNode? parent = null)
        {
            Name = name;
            IsDirectory = isDirectory;
            Size = size;
            Children = new();
            Parent = parent;
        }

        public void Add(TreeNode node) =>
            Children.Add(node);

        public int CountSize()
        {
            int sum = 0;

            foreach (TreeNode child in Children)
            {
                if (child.IsDirectory)
                {
                    sum += child.CountSize();
                }
                else
                {
                    sum += (int)(child.Size is null ? 0 : child.Size);
                }
            }

            Size = sum;

            return sum;
        }

        public TreeNode? Contains(string name)
        {
            TreeNode? node = Children.LastOrDefault(c => c.Name.Equals(name));

            if (node is null)
            {
                foreach (TreeNode child in Children.Where(c => c.IsDirectory))
                {
                    return node = child.Contains(name);
                }
            }

            return node;
        }
    }
}