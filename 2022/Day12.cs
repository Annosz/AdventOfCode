namespace _2022;

public class Day12
{
    public static string Solve()
    {
        List<List<char>> map = new();
        List<List<int>> distanceMap = new();
        Queue<(Coordinate, int)> nextPoints = new();
        Coordinate endPoint = new(0, 0);

        var lines = File.ReadLines(@".\Input\Day12.txt").ToArray();
        for (int i = 0; i < lines.Length; i++)
        {
            map.Add(new List<char>());
            distanceMap.Add(new List<int>());
            for (int j = 0; j < lines[i].Length; j++)
            {
                if (lines[i][j] == 'S')
                {
                    nextPoints.Enqueue((new Coordinate(i, j), 0));
                    map[i].Add('a');
                    distanceMap[i].Add(-1);
                    continue;
                }
                if (lines[i][j] == 'E')
                {
                    endPoint = new Coordinate(i, j);
                    map[i].Add('z');
                    distanceMap[i].Add(-1);
                    continue;
                }

                map[i].Add(lines[i][j]);
                distanceMap[i].Add(-1);
            }
        }

        while (nextPoints.Count > 0)
        {
            var (currentPoint, distance) = nextPoints.Dequeue();

            // For part 2
            if (map[currentPoint.X][currentPoint.Y] == 'a')
                distance = 0;

            if (distanceMap[currentPoint.X][currentPoint.Y] != -1 &&
                distanceMap[currentPoint.X][currentPoint.Y] <= distance)
            {
                continue;
            }

            distanceMap[currentPoint.X][currentPoint.Y] = distance;

            if (CheckNextPoint(currentPoint, currentPoint with { X = currentPoint.X + 1 }, map))
                nextPoints.Enqueue((currentPoint with { X = currentPoint.X + 1 }, distance + 1));
            if (CheckNextPoint(currentPoint, currentPoint with { X = currentPoint.X - 1 }, map))
                nextPoints.Enqueue((currentPoint with { X = currentPoint.X - 1 }, distance + 1));
            if (CheckNextPoint(currentPoint, currentPoint with { Y = currentPoint.Y + 1 }, map))
                nextPoints.Enqueue((currentPoint with { Y = currentPoint.Y + 1 }, distance + 1));
            if (CheckNextPoint(currentPoint, currentPoint with { Y = currentPoint.Y - 1 }, map))
                nextPoints.Enqueue((currentPoint with { Y = currentPoint.Y - 1 }, distance + 1));
        }

        return distanceMap[endPoint.X][endPoint.Y].ToString();
    }

    private static bool CheckNextPoint(Coordinate currentPoint, Coordinate nextPoint, List<List<char>> map)
    {
        if (nextPoint.X < 0 || nextPoint.Y < 0)
            return false;
        if (nextPoint.X >= map.Count || nextPoint.Y >= map[0].Count)
            return false;
        return map[nextPoint.X][nextPoint.Y] <= map[currentPoint.X][currentPoint.Y] + 1;
    }

    public record Coordinate(int X, int Y);
}