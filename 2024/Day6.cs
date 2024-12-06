namespace _2024;

public static class Day6
{
    private static readonly List<(int, int)> Directions =
    [
        (-1, 0), (0, 1), (1, 0), (0, -1),
    ];

    private static List<List<char>> map = [];
    private static (int, int) position;
    private static List<List<bool>> visited = [];
    private static int direction = 0;

    public static string Solve()
    {
        foreach (string line in File.ReadLines(@".\Input\Day6.txt"))
        {
            if (line.Contains('^'))
            {
                position = (map.Count, line.IndexOf('^'));
            }

            map.Add([.. line]);
            visited.Add(Enumerable.Repeat(false, line.Length).ToList());
        }

        while (position.Item1 >= 0 && position.Item2 >= 0
            && position.Item1 < map.Count && position.Item2 < map[0].Count)
        {
            try
            {
                if (map[position.Item1 + Directions[direction].Item1][position.Item2 + Directions[direction].Item2] != '#')
                {
                    position = (position.Item1 + Directions[direction].Item1, position.Item2 + Directions[direction].Item2);
                    visited[position.Item1][position.Item2] = true;
                }
                else
                {
                    direction = (direction + 1) % 4;
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                break;
            }
        }

        return visited.Sum(v => v.Count(e => e)).ToString();
    }


}