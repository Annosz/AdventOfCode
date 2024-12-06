namespace _2024;

public static class Day6
{
    private static readonly List<(int, int)> Directions =
    [
        (-1, 0), (0, 1), (1, 0), (0, -1),
    ];

    private static List<List<char>> map = [];
    private static (int, int) position;
    private static HashSet<(int, int)> visited = new();
    private static int direction = 0;

    public static string Solve()
    {
        foreach (string line in File.ReadLines(@".\Input\Day6.txt"))
        {
            if (line.Contains('^'))
            {
                position = (map.Count + 1, line.IndexOf('^') + 1);
            }

            map.Add([.. ' ' + line + ' ']);
        }

        // Padding
        map.Insert(0, Enumerable.Repeat(' ', map[0].Count).ToList());
        map.Add(Enumerable.Repeat(' ', map[0].Count).ToList());

        while (position.Item1 > 0 && position.Item2 > 0
            && position.Item1 < (map.Count - 1) && position.Item2 < (map[0].Count - 1))
        {
            visited.Add((position.Item1, position.Item2));
            if (map[position.Item1 + Directions[direction].Item1][position.Item2 + Directions[direction].Item2] != '#')
            {
                position = (position.Item1 + Directions[direction].Item1, position.Item2 + Directions[direction].Item2);
            }
            else
            {
                direction = (direction + 1) % 4;
            }
        }

        return visited.Count().ToString();
    }


}