namespace _2023;

public static class Day11
{
    // 2 for Part 1, 1000000 for Part 2
    public const long Multiplier = 1000000;

    private static readonly List<string> Map = new();
    private static readonly List<int> EmptyRows = new();
    private static readonly List<int> EmptyColumns = new();

    public static string Solve()
    {
        foreach (var line in File.ReadLines(@".\Input\Day11.txt"))
            Map.Add(line);

        // Rows
        for (int i = 0; i < Map.Count; i++)
            if (Map[i].All(x => x == '.'))
                EmptyRows.Add(i);

        // Columns
        for (int j = 0; j < Map[0].Length; j++)
        {
            bool isColumnEmpty = true;
            for (int i = 0; i < Map.Count; i++)
                if (Map[i][j] != '.')
                    isColumnEmpty = false;
            if (isColumnEmpty)
                EmptyColumns.Add(j);
        }

        // Find all galaxies
        List<Point> allGalaxies = new();
        for (int i = 0; i < Map.Count; i++)
            for (int j = 0; j < Map[0].Length; j++)
                if (Map[i][j] == '#')
                    allGalaxies.Add(new Point(i, j));

        // Find all combinations
        var combinations = allGalaxies.SelectMany((x, i) => allGalaxies.Skip(i + 1), (x, y) => Tuple.Create(x, y)).ToList();

        // Calculate distance sums
        long sum = 0;
        foreach (var (a, b) in combinations)
        {
            // Row distance
            sum += Math.Abs(a.X - b.X) + EmptyRows.Where(i => Math.Min(a.X, b.X) < i && i < Math.Max(a.X, b.X)).Count() * (Multiplier - 1);
            // Column distance
            sum += Math.Abs(a.Y - b.Y) + EmptyColumns.Where(i => Math.Min(a.Y, b.Y) < i && i < Math.Max(a.Y, b.Y)).Count() * (Multiplier - 1);
        }

        return sum.ToString();
    }

    public record Point(int X, int Y);
}