namespace _2022;

public static class Day8
{
    public static List<List<int>> Forest = new List<List<int>>();
    public static List<List<bool>> Visible = new List<List<bool>>();
    public static List<List<int>> HiddenByInLine = new List<List<int>>();
    public static List<List<int>> HiddenByInColumn = new List<List<int>>();

    public static string Solve()
    {
        foreach (string line in File.ReadLines(@".\Input\Day8.txt"))
        {
            Forest.Add(line.Chunk(1).Select(c => int.Parse(c)).ToList());
            Visible.Add(Forest[0].Select(_ => false).ToList());
            HiddenByInLine.Add(Forest[0].Select(_ => 0).ToList());
            HiddenByInColumn.Add(Forest[0].Select(_ => 0).ToList());
        }

        for (int i = 0; i < Forest.Count; i++)
        {
            for (int j = 0; j < Forest[i].Count; j++)
            {
                if (i == 0 || j == 0 || i == Forest.Count - 1 || j == Forest.Count - 1)
                {
                    Visible[i][j] = true;
                    HiddenByInLine[i][j] = 0;
                    HiddenByInColumn[i][j] = 0;
                    continue;
                }

                if (VisibleInColumn(i, j, i - 1, j) || VisibleInLine(i, j, i, j - 1))
                {
                    Visible[i][j] = true;
                    HiddenByInLine[i][j] = 0;
                    HiddenByInColumn[i][j] = 0;
                    continue;
                }

                HiddenByInLine[i][j] = Math.Max(HiddenByInLine[i][j - 1], Forest[i][j - 1]);
                HiddenByInColumn[i][j] = Math.Max(HiddenByInLine[i - 1][j], Forest[i - 1][j]);
            }
        }

        for (int i = Forest.Count - 1; i >= 0; i--)
        {
            for (int j = Forest[i].Count - 1; j >= 0; j--)
            {
                if (i == 0 || j == 0 || i == Forest.Count - 1 || j == Forest.Count - 1)
                {
                    Visible[i][j] = true;
                    HiddenByInLine[i][j] = 0;
                    HiddenByInColumn[i][j] = 0;
                    continue;
                }

                if (VisibleInColumn(i, j, i + 1, j) || VisibleInLine(i, j, i, j + 1))
                {
                    Visible[i][j] = true;
                    HiddenByInLine[i][j] = 0;
                    HiddenByInColumn[i][j] = 0;
                    continue;
                }

                HiddenByInLine[i][j] = Math.Max(HiddenByInLine[i][j + 1], Forest[i][j + 1]);
                HiddenByInColumn[i][j] = Math.Max(HiddenByInLine[i + 1][j], Forest[i + 1][j]);
            }
        }

        return Visible.Sum(r => r.Count(t => t)).ToString();
    }

    private static bool VisibleInLine(int thisI, int thisJ, int thatI, int thatJ)
    {
        return (Visible[thatI][thatJ] && Forest[thatI][thatJ] < Forest[thisI][thisJ]) || (!Visible[thatI][thatJ] && HiddenByInLine[thatI][thatJ] < Forest[thisI][thisJ]);
    }

    private static bool VisibleInColumn(int thisI, int thisJ, int thatI, int thatJ)
    {
        return (Visible[thatI][thatJ] && Forest[thatI][thatJ] < Forest[thisI][thisJ]) || (!Visible[thatI][thatJ] && HiddenByInColumn[thatI][thatJ] < Forest[thisI][thisJ]);
    }
}