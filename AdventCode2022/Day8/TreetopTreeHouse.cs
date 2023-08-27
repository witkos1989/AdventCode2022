namespace AdventCode2022.Day8;

public class TreetopTreeHouse
{
    private readonly int[][] _data;

    public TreetopTreeHouse()
    {
        string currentDirectory = PathHelper.
            GetCurrentDirectory("Day8", "TreetopTreeHouseInput.txt");
        StreamReader file = new(currentDirectory);
        _data = file.ImportInts().ToArray();
    }

    public int[] Results()
    {
        int[] results = new int[2];

        results[0] = CountTreesSeen(_data);

        results[1] = HighestScenicScore(_data);

        return results;
    }

    private static int CountTreesSeen(int[][] forest)
    {
        bool[][] seenFromTop = TreesSeenForward(forest, true);
        bool[][] seenFromLeft = TreesSeenForward(forest, false);
        bool[][] seenFromRight = TreesSeenBackward(forest, false);
        bool[][] seenFromBottom = TreesSeenBackward(forest, true);
        int sum = 0;

        for (int i = 0; i < forest.Length; i++)
        {
            for (int j = 0; j < forest.Length; j++)
            {
                if (seenFromBottom[i][j] || seenFromLeft[i][j] ||
                    seenFromRight[i][j] || seenFromTop[i][j])
                {
                    sum++;
                }
            }
        }

        return sum;
    }

    private static int HighestScenicScore(int[][] forest)
    {
        int highestScore = 0;

        for (int i = 0; i < forest.Length; i++)
        {
            for (int j = 0; j < forest.Length; j++)
            {
                if (forest[i][j] == 9)
                {
                    Console.Write("");
                }
                int up = LookBack(forest[i][j], forest, i, j, true);
                int down = LookForward(forest[i][j], forest, i, j, true);
                int left = LookBack(forest[i][j], forest, i, j, false);
                int right = LookForward(forest[i][j], forest, i, j, false);
                int currentScore = up * left * down * right;
                
                if (highestScore < currentScore)
                    highestScore = currentScore;
            }
        }

        return highestScore;
    }

    private static int LookBack(int tree, int[][] forest, int x, int y, bool lookingUp)
    {
        int sum = 1;

        for (int i = lookingUp ? x - 1 : y - 1; i > 0; i--)
        {
            int currentTree = lookingUp ? forest[i][y] : forest[x][i];
            if (currentTree < tree)
            {
                sum++;
            }
            else
            {
                break;
            }
        }

        return sum;
    }

    private static int LookForward(int tree, int[][] forest, int x, int y, bool lookingDown)
    {
        int sum = 1;

        for (int i = lookingDown ? x + 1 : y + 1; i < forest.Length - 1; i++)
        {
            int currentTree = lookingDown ? forest[i][y] : forest[x][i];
            if (currentTree < tree)
            {
                sum++;
            }
            else
            {
                break;
            }
        }

        return sum;
    }

    private static bool[][] TreesSeenForward(int[][] forest, bool verticalDirection)
    {
        bool[][] seen = new bool[99][];
        int treeHeight = -1;

        for (int i = 0; i < seen.Length; i++)
            seen[i] = new bool[seen.Length];

        for (int i = 0; i < forest.Length; i++)
        {
            for (int j = 0; j < forest.Length; j++)
            {
                int tree = verticalDirection ? forest[j][i] : forest[i][j];

                if (treeHeight == tree)
                    continue;

                if (treeHeight < tree)
                {
                    treeHeight = tree;

                    if (verticalDirection)
                    {
                        seen[j][i] = true;
                    }
                    else
                    {
                        seen[i][j] = true;
                    }
                }

                if (treeHeight == 9)
                    break;
            }

            treeHeight = -1;
        }

        return seen;
    }

    private static bool[][] TreesSeenBackward(int[][] forest, bool verticalDirection)
    {
        bool[][] seen = new bool[99][];
        int treeHeight = -1;

        for (int i = 0; i < seen.Length; i++)
            seen[i] = new bool[seen.Length];

        for (int i = forest.Length - 1; i >= 0; i--)
        {
            for (int j = forest.Length - 1; j >= 0; j--)
            {
                int tree = verticalDirection ? forest[j][i] : forest[i][j];

                if (treeHeight == tree)
                    continue;

                if (treeHeight < tree)
                {
                    treeHeight = tree;

                    if (verticalDirection)
                    {
                        seen[j][i] = true;
                    }
                    else
                    {
                        seen[i][j] = true;
                    }
                }

                if (treeHeight == 9)
                    break;
            }

            treeHeight = -1;
        }

        return seen;
    }
}