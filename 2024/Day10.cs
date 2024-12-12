namespace _2024;

public static class Day10
{
    public record Point(int X, int Y);

    private static readonly List<List<int>> Map = [];

    private static readonly Dictionary<Point, List<int>> ReachableNines = [];
    private static int GlobalNineId;
    private static int Sum;

    private const bool IsPart1 = false;

    public static string Solve()
    {
        foreach (var line in File.ReadLines(@".\Input\Day10.txt"))
        {
            var heights = line.Select(c => int.Parse(c.ToString())).ToList();
            heights.Insert(0, -1);
            heights.Add(-1);
            Map.Add(heights);
        }

        Map.Insert(0, Enumerable.Repeat(-1, Map[0].Count).ToList());
        Map.Add(Enumerable.Repeat(-1, Map[0].Count).ToList());

        for (var height = 9; height >= 0; height--)
        {
            for (var i = 1; i < Map.Count - 1; i++)
            {
                for (var j = 1; j < Map[i].Count - 1; j++)
                {
                    if (Map[i][j] != height)
                    {
                        continue;
                    }

                    if (height == 9)
                    {
                        ReachableNines[new Point(i, j)] = [GlobalNineId++];
                        continue;
                    }

                    var reachableNines = new List<int>();
                    if (Map[i + 1][j] == height + 1)
                    {
                        reachableNines.AddRange(ReachableNines[new Point(i + 1, j)]);
                    }
                    if (Map[i - 1][j] == height + 1)
                    {
                        reachableNines.AddRange(ReachableNines[new Point(i - 1, j)]);
                    }
                    if (Map[i][j + 1] == height + 1)
                    {
                        reachableNines.AddRange(ReachableNines[new Point(i, j + 1)]);
                    }
                    if (Map[i][j - 1] == height + 1)
                    {
                        reachableNines.AddRange(ReachableNines[new Point(i, j - 1)]);
                    }
                    ReachableNines[new Point(i, j)] = reachableNines;

                    if (height == 0)
                    {
                        Sum += IsPart1
                            ? ReachableNines[new Point(i, j)].Distinct().Count()
                            : ReachableNines[new Point(i, j)].Count();
                    }
                }
            }
        }

        return Sum.ToString();
    }
}