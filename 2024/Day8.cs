using Shared;

namespace _2024;

public static class Day8
{
    public record Point(int X, int Y);

    private static readonly Dictionary<char, List<Point>> Towers = new();
    private static readonly HashSet<Point> InterferencePoints = new();

    private const bool Part1 = false;

    public static string Solve()
    {
        var N = 0;
        foreach (var line in File.ReadLines(@".\Input\Day8.txt"))
        {
            for (var i = 0; i < line.Length; i++)
            {
                if (line[i] != '.')
                {
                    if (!Towers.ContainsKey(line[i]))
                    {
                        Towers[line[i]] = new List<Point>();
                    }

                    Towers[line[i]].Add(new Point(N, i));
                }
            }

            N++;
        }

        foreach (var towerName in Towers.Keys)
        {
            foreach (var pair in Towers[towerName].FindAllPairs())
            {
                var directionA = new Point(pair.Item1.X - pair.Item2.X, pair.Item1.Y - pair.Item2.Y);
                var directionB = new Point(pair.Item2.X - pair.Item1.X, pair.Item2.Y - pair.Item1.Y);

                if (Part1)
                {
                    InterferencePoints.Add(new Point(pair.Item1.X + directionA.X, pair.Item1.Y + directionA.Y));
                    InterferencePoints.Add(new Point(pair.Item2.X + directionB.X, pair.Item2.Y + directionB.Y));
                }
                else
                {
                    for (var i = 0; i < N; i++)
                    {
                        InterferencePoints.Add(new Point(pair.Item1.X + directionA.X * i, pair.Item1.Y + directionA.Y * i));
                        InterferencePoints.Add(new Point(pair.Item2.X + directionB.X * i, pair.Item2.Y + directionB.Y * i));
                    }
                }
            }
        }

        InterferencePoints.RemoveWhere(p => p.X < 0 || p.Y < 0 || p.X >= N || p.Y >= N);

        return InterferencePoints.Count.ToString();
    }
}