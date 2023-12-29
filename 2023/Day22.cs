namespace _2023;

public static class Day22
{
    private static List<Brick> Bricks = new();

    private static List<int>[] SupportedBy;
    private static List<int>[] Supporting;
    private static int[,,] Space;

    public static string Solve()
    {
        int maxX = 0;
        int maxY = 0;
        int maxZ = 0;
        int brickIndex = 0;
        foreach (var line in File.ReadLines(@".\Input\Day22.txt"))
        {
            var lineSplit = line.Split('~');
            var posA = lineSplit[0].Split(',').Select(e => int.Parse(e)).ToList();
            var posB = lineSplit[1].Split(',').Select(e => int.Parse(e)).ToList();

            List<Point> points = new();
            for (int i = posA[0]; i <= posB[0]; i++)
                for (int j = posA[1]; j <= posB[1]; j++)
                    for (int k = posA[2]; k <= posB[2]; k++)
                    {
                        points.Add(new Point(i, j, k));
                        maxX = Math.Max(maxX, i);
                        maxY = Math.Max(maxY, j);
                        maxZ = Math.Max(maxZ, k);
                    }

            Bricks.Add(new Brick(brickIndex, points));
            brickIndex++;
        }

        // Bricks must be ordered on Z axis to simulate fall
        Bricks = Bricks.OrderBy(b => b.Points.Min(e => e.Z)).ToList();

        // Initialize solution space
        Space = new int[maxX + 1, maxY + 1, maxZ + 1];
        for (int i = 0; i <= maxX; i++)
            for (int j = 0; j <= maxY; j++)
                for (int k = 0; k <= maxZ; k++)
                    Space[i, j, k] = -1;

        // Initialize Supported by array
        SupportedBy = new List<int>[Bricks.Count];
        for (int i = 0; i < Bricks.Count; i++)
            SupportedBy[i] = new List<int>();

        // Simulate fall
        foreach (var brick in Bricks)
        {
            while (!brick.Points.Any(b => b.Z == 1) && brick.Points.All(e => Space[e.X, e.Y, e.Z - 1] == -1))
            {
                brick.Points = brick.Points.Select(e => e with { Z = e.Z - 1 }).ToList();
            }

            SupportedBy[brick.Index] = brick.Points.Select(e => Space[e.X, e.Y, e.Z - 1]).Where(e => e != -1).Distinct().ToList();
            // Add ground as an extra supporter
            if (brick.Points.Any(b => b.Z == 1))
                SupportedBy[brick.Index].Add(-1);

            foreach (Point p in brick.Points)
                Space[p.X, p.Y, p.Z] = brick.Index;
        }

        // Initialize and calculate Supporting array
        Supporting = new List<int>[Bricks.Count];
        for (int i = 0; i < Bricks.Count; i++)
            Supporting[i] = new List<int>();
        for (int i = 0; i < Bricks.Count; i++)
        {
            foreach (var supporter in SupportedBy[i])
            {
                if (supporter == -1)
                    continue;

                Supporting[supporter].Add(i);
            }
        }

        // Part 1 solution
        var nonSingleSupporterCount = Supporting.Where(s => s.All(e => SupportedBy[e].Count > 1)).Count();

        // Part 2 solution
        int chainReactionCount = 0;
        foreach (var brick in Bricks)
        {
            int previousWouldFallCount = 0;
            HashSet<int> wouldFall = Supporting[brick.Index].Where(e => SupportedBy[e].Count == 1).ToHashSet();
            while (previousWouldFallCount != wouldFall.Count)
            {
                var moreWouldFalls = Bricks.Where(b => SupportedBy[b.Index].All(e => wouldFall.Contains(e))).Select(b => b.Index);
                foreach (var moreWouldFall in moreWouldFalls)
                    wouldFall.Add(moreWouldFall);

                previousWouldFallCount = wouldFall.Count;
            }

            chainReactionCount += wouldFall.Count;
        }

        return $"Part 1: {nonSingleSupporterCount}, Part 2: {chainReactionCount}";
    }

    public class Brick
    {
        public int Index;
        public List<Point> Points;

        public Brick(int index, List<Point> points)
        {
            Index = index;
            Points = points;
        }
    }
    public record Point(int X, int Y, int Z);
}