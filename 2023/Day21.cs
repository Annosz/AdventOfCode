namespace _2023;

public static class Day21
{
    private static List<string> Map = new();
    private static List<List<int>> Distances = new();
    private static HashSet<Point> InQueue = new();

    public static List<Point> Directions = new() { new(1, 0), new(0, 1), new(-1, 0), new(0, -1) };

    public static string Solve()
    {
        foreach (var line in File.ReadLines(@".\Input\Day21.txt"))
        {
            Map.Add(line);
        }

        Point startingPoint = new Point(0, 0);
        for (int i = 0; i < Map.Count; i++)
            for (int j = 0; j < Map[0].Length; j++)
                if (Map[i][j] == 'S')
                    startingPoint = new Point(i, j);

        for (int i = 0; i < Map.Count; i++)
            Distances.Add(Enumerable.Repeat(-1, Map[0].Length).ToList());


        PriorityQueue<Point, int> priorityQueue = new();
        priorityQueue.Enqueue(startingPoint, 0);

        while (priorityQueue.TryDequeue(out Point point, out int distance))
        {
            InQueue.Remove(point);

            if (distance > 64)
                break;

            if (Distances[point.X][point.Y] == -1)
                Distances[point.X][point.Y] = distance;

            foreach (var direction in Directions)
            {
                Point nextPoint = point with
                {
                    X = point.X + direction.X,
                    Y = point.Y + direction.Y,
                };

                if (nextPoint.X < 0 || nextPoint.Y < 0 || nextPoint.X > Map.Count - 1 || nextPoint.Y > Map[0].Length - 1
                    || Map[nextPoint.X][nextPoint.Y] == '#' || Distances[nextPoint.X][nextPoint.Y] != -1
                    || InQueue.Contains(nextPoint))
                    continue;

                priorityQueue.Enqueue(nextPoint, distance + 1);
                InQueue.Add(nextPoint);
            }
        }

        return Distances.Sum(l => l.Count(e => e % 2 == 0)).ToString();
    }

    public record Point(int X, int Y);
}