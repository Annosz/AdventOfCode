namespace _2023;

public static class Day17
{
    private const int MinDistance = 4;
    private const int MaxDistance = 10;

    public static List<Direction> Directions = new() { new(1, 0), new(0, 1), new(-1, 0), new(0, -1) };
    public static Dictionary<Direction, int> DirectionAxis = new() { { new(1, 0), 0 }, { new(0, 1), 1 }, { new(-1, 0), 2 }, { new(0, -1), 3 } };
    public static Dictionary<Direction, string> DirectionString = new() { { new(1, 0), "↓" }, { new(0, 1), "→" }, { new(-1, 0), "↑" }, { new(0, -1), "←" } };

    private static readonly List<List<int>> Map = new();
    private static readonly List<List<int>>[][] Distances = new List<List<int>>[][] {
        new List<List<int>>[MaxDistance], new List<List<int>>[MaxDistance], new List<List<int>>[MaxDistance], new List<List<int>>[MaxDistance]
    };

    public static string Solve()
    {
        for (int i = 0; i < 4; i++)
            for (int j = 0; j < MaxDistance; j++)
                Distances[i][j] = new();

        foreach (var line in File.ReadLines(@".\Input\Day17.txt"))
        {
            Map.Add(line.Select(c => int.Parse(c.ToString())).ToList());
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < MaxDistance; j++)
                    Distances[i][j].Add(Enumerable.Repeat(-1, line.Length).ToList());
        }

        PriorityQueue<Point, int> priorityQueue = new();
        Distances[DirectionAxis[new(1, 0)]][0][1][0] = Map[1][0];
        priorityQueue.Enqueue(new Point(1, 0, 0, new(1, 0), DirectionString[new(1, 0)]), Map[1][0]);
        Distances[DirectionAxis[new(0, 1)]][0][0][1] = Map[0][1];
        priorityQueue.Enqueue(new Point(0, 1, 0, new(0, 1), DirectionString[new(0, 1)]), Map[0][1]);

        while (priorityQueue.TryDequeue(out Point point, out int distance))
        {
            if (point.X == Map.Count - 1 && point.Y == Map[0].Count - 1 && point.Level >= MinDistance - 1)
                return distance.ToString();

            foreach (var direction in Directions)
            {
                if (direction != point.Direction && DirectionAxis[direction] == DirectionAxis[point.Direction])
                    continue;

                if (DirectionAxis[direction] != DirectionAxis[point.Direction] && point.Level < MinDistance - 1)
                    continue;

                Point nextPoint = point with
                {
                    X = point.X + direction.X,
                    Y = point.Y + direction.Y,
                    Level = direction == point.Direction ? point.Level + 1 : 0,
                    Direction = direction with { },
                    History = point.History + DirectionString[direction]
                };
                if (nextPoint.X < 0 || nextPoint.Y < 0 || nextPoint.X > Map.Count - 1 || nextPoint.Y > Map[0].Count - 1 || nextPoint.Level >= MaxDistance
                    || (Distances[DirectionAxis[nextPoint.Direction]][nextPoint.Level][nextPoint.X][nextPoint.Y] != -1 && Distances[DirectionAxis[nextPoint.Direction]][nextPoint.Level][nextPoint.X][nextPoint.Y] <= distance + Map[nextPoint.X][nextPoint.Y]))
                    continue;

                Distances[DirectionAxis[nextPoint.Direction]][nextPoint.Level][nextPoint.X][nextPoint.Y] = distance + Map[nextPoint.X][nextPoint.Y];
                priorityQueue.Enqueue(nextPoint, distance + Map[nextPoint.X][nextPoint.Y]);
            }
        }

        return "";
    }

    public record Direction(int X, int Y);
    public record Point(int X, int Y, int Level, Direction Direction, string History);
}