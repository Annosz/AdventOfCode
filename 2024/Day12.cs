using System.Collections.Concurrent;

namespace _2024;

public static class Day12
{
    private const int ImageEnhancementMultiplier = 2;

    private static readonly List<(int, int)> Directions =
    [
        (0, 1), (1, 0), (0, -1), (-1, 0)
    ];

    private record Point(int X, int Y);
    private class AreaMeasures(int area, int perimeter, int sides)
    {
        public int Area { get; set; } = area;
        public int Perimeter { get; set; } = perimeter;
        public int Sides { get; set; } = sides;
    }

    private static readonly List<List<char>> Map = [];
    private static readonly HashSet<Point> ColoredPoints = [];
    private static readonly Dictionary<Point, AreaMeasures> AreasByCorner = [];

    public static string Solve()
    {
        foreach (var line in File.ReadLines(@".\Input\Day12.txt"))
        {
            var enhancedLine = string.Join("", line.Select(c => string.Join("", Enumerable.Repeat(c, ImageEnhancementMultiplier))));
            for (var i = 0; i < ImageEnhancementMultiplier; i++)
            {
                Map.Add((" " + enhancedLine + " ").Select(c => c).ToList());
            }
        }

        Map.Insert(0, Enumerable.Repeat(' ', Map[0].Count).ToList());
        Map.Add(Enumerable.Repeat(' ', Map[0].Count).ToList());

        foreach (var row in Map)
        {
            Console.WriteLine(string.Join("", row));
        }

        for (var i = 1; i < Map.Count - 1; i++)
        {
            for (var j = 1; j < Map[i].Count - 1; j++)
            {
                var point = new Point(i, j);
                if (!ColoredPoints.Contains(point))
                {
                    AreasByCorner.Add(point, new AreaMeasures(0, 0, 0));

                    FloodFill(point);
                }
            }
        }

        return $"Part 1: {AreasByCorner.Sum(a => a.Value.Area / Math.Pow(ImageEnhancementMultiplier, 2) * a.Value.Perimeter / ImageEnhancementMultiplier)}, " +
               $"Part 2: {AreasByCorner.Sum(a => a.Value.Area / Math.Pow(ImageEnhancementMultiplier, 2) * a.Value.Sides)}";
    }

    private static void FloodFill(Point cornerPoint)
    {
        ConcurrentDictionary<Point, int> pointOccurrences = [];
        Queue<Point> coloringQueue = [];

        coloringQueue.Enqueue(cornerPoint);
        while (coloringQueue.Count > 0)
        {
            var currentPoint = coloringQueue.Dequeue();
            ColoredPoints.Add(currentPoint);

            AreasByCorner[cornerPoint].Area++;

            foreach (var (dx, dy) in Directions)
            {
                var nextPoint = new Point(currentPoint.X + dx, currentPoint.Y + dy);

                if (Map[nextPoint.X][nextPoint.Y] == Map[cornerPoint.X][cornerPoint.Y] && !coloringQueue.Contains(nextPoint) && !ColoredPoints.Contains(nextPoint))
                {
                    coloringQueue.Enqueue(nextPoint);
                    continue;
                }

                if (Map[nextPoint.X][nextPoint.Y] != Map[cornerPoint.X][cornerPoint.Y])
                {
                    AreasByCorner[cornerPoint].Perimeter++;

                    pointOccurrences.AddOrUpdate(currentPoint, 1, (_, count) => count + 1);
                    pointOccurrences.AddOrUpdate(nextPoint, 1, (_, count) => count + 1);
                }
            }
        }

        AreasByCorner[cornerPoint].Sides = pointOccurrences.Values.Sum(c => c - 1) - MagicNumber(cornerPoint, pointOccurrences);
    }

    // Calculates a magic number for the case of an X appearing in the grid.
    // A very good example is the following:
    //.....
    // .AAA.
    // .A.A.
    // .AA..
    // .A.A.
    // .AAA.
    // .....
    // Expected answer for part 1 is 1202, for part 2 is 452
    private static int MagicNumber(Point cornerPoint, ConcurrentDictionary<Point, int> pointOccurrences) =>
        pointOccurrences.Count(kv => kv.Value == 2
            && pointOccurrences[new Point(kv.Key.X + 1, kv.Key.Y)] == 2 && Map[kv.Key.X + 1][kv.Key.Y] != Map[cornerPoint.X][cornerPoint.Y]
            && pointOccurrences[new Point(kv.Key.X + 1, kv.Key.Y + 1)] == 2 && Map[kv.Key.X + 1][kv.Key.Y + 1] == Map[cornerPoint.X][cornerPoint.Y]
            && pointOccurrences[new Point(kv.Key.X, kv.Key.Y + 1)] == 2 && Map[kv.Key.X][kv.Key.Y + 1] != Map[cornerPoint.X][cornerPoint.Y])
        + pointOccurrences.Count(kv => kv.Value == 2
            && pointOccurrences[new Point(kv.Key.X - 1, kv.Key.Y)] == 2 && Map[kv.Key.X - 1][kv.Key.Y] != Map[cornerPoint.X][cornerPoint.Y]
            && pointOccurrences[new Point(kv.Key.X - 1, kv.Key.Y + 1)] == 2 && Map[kv.Key.X - 1][kv.Key.Y + 1] == Map[cornerPoint.X][cornerPoint.Y]
            && pointOccurrences[new Point(kv.Key.X, kv.Key.Y + 1)] == 2 && Map[kv.Key.X][kv.Key.Y + 1] != Map[cornerPoint.X][cornerPoint.Y])
        + pointOccurrences.Count(kv => kv.Value == 2
            && pointOccurrences[new Point(kv.Key.X + 1, kv.Key.Y)] == 2 && Map[kv.Key.X + 1][kv.Key.Y] != Map[cornerPoint.X][cornerPoint.Y]
            && pointOccurrences[new Point(kv.Key.X + 1, kv.Key.Y - 1)] == 2 && Map[kv.Key.X + 1][kv.Key.Y - 1] == Map[cornerPoint.X][cornerPoint.Y]
            && pointOccurrences[new Point(kv.Key.X, kv.Key.Y - 1)] == 2 && Map[kv.Key.X][kv.Key.Y - 1] != Map[cornerPoint.X][cornerPoint.Y])
        + pointOccurrences.Count(kv => kv.Value == 2
            && pointOccurrences[new Point(kv.Key.X - 1, kv.Key.Y)] == 2 && Map[kv.Key.X - 1][kv.Key.Y] != Map[cornerPoint.X][cornerPoint.Y]
            && pointOccurrences[new Point(kv.Key.X - 1, kv.Key.Y - 1)] == 2 && Map[kv.Key.X - 1][kv.Key.Y - 1] == Map[cornerPoint.X][cornerPoint.Y]
            && pointOccurrences[new Point(kv.Key.X, kv.Key.Y - 1)] == 2 && Map[kv.Key.X][kv.Key.Y - 1] != Map[cornerPoint.X][cornerPoint.Y]);
}