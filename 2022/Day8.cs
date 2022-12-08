namespace _2022;

public static class Day8
{
    public static List<List<int>> Forest = new List<List<int>>();
    public static List<List<bool>> Visible = new List<List<bool>>();
    public static List<List<int>> ScenicScore = new List<List<int>>();

    public static string Solve()
    {
        foreach (string line in File.ReadLines(@".\Input\Day8.txt"))
        {
            Forest.Add(line.Chunk(1).Select(c => int.Parse(c)).ToList());
            Visible.Add(Forest[0].Select(_ => false).ToList());
            ScenicScore.Add(Forest[0].Select(_ => 0).ToList());
        }

        FillVisibility();

        FillScenicScore();

        return ScenicScore.Max(r => r.Max(t => t)).ToString();
    }

    private static void FillScenicScore()
    {
        for (int i = 0; i < Forest.Count; i++)
        {
            for (int j = 0; j < Forest[i].Count; j++)
            {
                int scenicIMinus = 0;
                for (int iSceinic = i - 1; iSceinic >= 0; iSceinic--)
                {
                    scenicIMinus++;
                    if (Forest[iSceinic][j] >= Forest[i][j])
                    {
                        break;
                    }
                }

                int scenicIPlus = 0;
                for (int iSceinic = i + 1; iSceinic < Forest.Count; iSceinic++)
                {
                    scenicIPlus++;
                    if (Forest[iSceinic][j] >= Forest[i][j])
                    {
                        break;
                    }
                }

                int scenicJPlus = 0;
                for (int jSceinic = j + 1; jSceinic < Forest[i].Count; jSceinic++)
                {
                    scenicJPlus++;
                    if (Forest[i][jSceinic] >= Forest[i][j])
                    {
                        break;
                    }
                }

                int scenicJMinus = 0;
                for (int jSceinic = j - 1; jSceinic >= 0; jSceinic--)
                {
                    scenicJMinus++;
                    if (Forest[i][jSceinic] >= Forest[i][j])
                    {
                        break;
                    }
                }

                ScenicScore[i][j] = scenicIMinus * scenicIPlus * scenicJPlus * scenicJMinus;
            }
        }
    }

    private static void FillVisibility()
    {
        for (int i = 0; i < Forest.Count; i++)
        {
            for (int j = 0; j < Forest[i].Count; j++)
            {
                if (i == 0 || j == 0 || i == Forest.Count - 1 || j == Forest.Count - 1)
                {
                    Visible[i][j] = true;
                    continue;
                }

                // check visibility by I
                int maxTree = 0;
                for (int iVisibility = 0; iVisibility < i; iVisibility++)
                {
                    maxTree = Forest[iVisibility][j] > maxTree ? Forest[iVisibility][j] : maxTree;
                }

                if (maxTree < Forest[i][j])
                {
                    Visible[i][j] = true;
                }

                // check visibility by J
                maxTree = 0;
                for (int jVisibility = 0; jVisibility < j; jVisibility++)
                {
                    maxTree = Forest[i][jVisibility] > maxTree ? Forest[i][jVisibility] : maxTree;
                }

                if (maxTree < Forest[i][j])
                {
                    Visible[i][j] = true;
                }
            }
        }

        for (int i = Forest.Count - 1; i >= 0; i--)
        {
            for (int j = Forest[i].Count - 1; j >= 0; j--)
            {
                if (i == 0 || j == 0 || i == Forest.Count - 1 || j == Forest.Count - 1)
                {
                    Visible[i][j] = true;
                    continue;
                }

                // check visibility by I
                int maxTree = 0;
                for (int iVisibility = Forest.Count - 1; iVisibility > i; iVisibility--)
                {
                    maxTree = Forest[iVisibility][j] > maxTree ? Forest[iVisibility][j] : maxTree;
                }

                if (maxTree < Forest[i][j])
                {
                    Visible[i][j] = true;
                }

                // check visibility by J
                maxTree = 0;
                for (int jVisibility = Forest[i].Count - 1; jVisibility > j; jVisibility--)
                {
                    maxTree = Forest[i][jVisibility] > maxTree ? Forest[i][jVisibility] : maxTree;
                }

                if (maxTree < Forest[i][j])
                {
                    Visible[i][j] = true;
                }
            }
        }
    }
}