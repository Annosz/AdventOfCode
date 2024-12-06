namespace _2024;

public static class Day6
{
    private static readonly List<(int, int)> Directions =
    [
        (-1, 0), (0, 1), (1, 0), (0, -1),
    ];

    private static List<List<char>> map = [];
    private static HashSet<(int, int)> visited = [];

    private static (int, int) startingPosition;
    private static int startingDirection = 0;

    public static string Solve()
    {
        foreach (string line in File.ReadLines(@".\Input\Day6.txt"))
        {
            if (line.Contains('^'))
            {
                startingPosition = (map.Count + 1, line.IndexOf('^') + 1);
            }

            map.Add([.. ' ' + line + ' ']);
        }

        // Padding
        map.Insert(0, Enumerable.Repeat(' ', map[0].Count).ToList());
        map.Add(Enumerable.Repeat(' ', map[0].Count).ToList());

        FillVisited();

        int circleCount = 0;
        foreach (var addedObstruction in visited.ToList()[1..])
        {
            circleCount += IsCircle(addedObstruction) ? 1 : 0;
        }

        //return visited.Count().ToString();
        return circleCount.ToString();
    }

    private static void FillVisited()
    {
        var direction = startingDirection;
        var position = startingPosition;

        while (OnTheMap(position))
        {
            visited.Add((position.Item1, position.Item2));
            var nextPosition = GetNextPosition(position, direction);
            if (map[nextPosition.Item1][nextPosition.Item2] != '#')
            {
                position = nextPosition;
            }
            else
            {
                direction = (direction + 1) % 4;
            }
        }
    }

    private static bool OnTheMap((int, int) position) => position.Item1 > 0 && position.Item2 > 0
        && position.Item1 < (map.Count - 1) && position.Item2 < (map[0].Count - 1);

    private static (int, int) GetNextPosition((int, int) position, int direction) =>
        (position.Item1 + Directions[direction].Item1, position.Item2 + Directions[direction].Item2);

    private static bool IsCircle((int, int) addedObstruction)
    {
        HashSet<(int, int, int)> visitedWithDirection = new();

        var direction = startingDirection;
        var position = startingPosition;

        while (OnTheMap(position))
        {
            if (visitedWithDirection.Contains((position.Item1, position.Item2, direction)))
            {
                return true;
            }

            visitedWithDirection.Add((position.Item1, position.Item2, direction));
            var nextPosition = GetNextPosition(position, direction);
            if (map[nextPosition.Item1][nextPosition.Item2] != '#' && nextPosition != addedObstruction)
            {
                position = nextPosition;
            }
            else
            {
                direction = (direction + 1) % 4;
            }
        }

        return false;
    }

}