namespace _2023;

public static class Day14
{
    private static List<string> Map = new();

    public static string Solve()
    {
        foreach (var line in File.ReadLines(@".\Input\Day14.txt"))
        {
            Map.Add(line);
        }

        List<List<string>> cachedMaps = new();
        int stillNeeded = 0;
        for (int i = 1; i <= 1000000000; i++)
        {
            DoFull(RollNorth);
            DoFull(RollWest);
            DoFull(RollSouth);
            DoFull(RollEast);

            var cachedCycle = cachedMaps.FindIndex(m => Enumerable.SequenceEqual(Map, m)) + 1;
            if (cachedCycle != 0)
            {
                stillNeeded = (1000000000 - cachedCycle) % (i - cachedCycle);
                break;
            }
            cachedMaps.Add(Map.Select(e => new string(e)).ToList());
        }

        for (int i = 1; i <= stillNeeded; i++)
        {
            DoFull(RollNorth);
            DoFull(RollWest);
            DoFull(RollSouth);
            DoFull(RollEast);
        }

        return CalculateMapSum().ToString();
    }

    private static long CalculateMapSum()
    {
        long sum = 0;
        for (int i = 0; i < Map.Count; i++)
            for (int j = 0; j < Map[i].Length; j++)
                if (Map[i][j] == 'O')
                    sum += Map.Count - i;

        return sum;
    }

    private static void DoFull(Action<Change> action)
    {
        Change change;
        do
        {
            change = new() { Happened = false };
            action(change);

        } while (change.Happened);
    }

    private static void RollNorth(Change change)
    {
        for (int i = 1; i < Map.Count; i++)
        {
            for (int j = 0; j < Map[i].Length; j++)
            {
                if (Map[i][j] == 'O' && Map[i - 1][j] == '.')
                {
                    Roll(i, j, new(-1, 0));
                    change.Happened = true;
                }
            }
        }
    }

    private static void RollEast(Change change)
    {
        for (int i = 0; i < Map.Count; i++)
        {
            for (int j = Map[i].Length - 2; j >= 0; j--)
            {
                if (Map[i][j] == 'O' && Map[i][j + 1] == '.')
                {
                    Roll(i, j, new(0, 1));
                    change.Happened = true;
                }
            }
        }
    }

    private static void RollSouth(Change change)
    {
        for (int i = Map.Count - 2; i >= 0; i--)
        {
            for (int j = 0; j < Map[i].Length; j++)
            {
                if (Map[i][j] == 'O' && Map[i + 1][j] == '.')
                {
                    Roll(i, j, new(1, 0));
                    change.Happened = true;
                }
            }
        }
    }

    private static void RollWest(Change change)
    {
        for (int i = 0; i < Map.Count; i++)
        {
            for (int j = 1; j < Map[i].Length; j++)
            {
                if (Map[i][j] == 'O' && Map[i][j - 1] == '.')
                {
                    Roll(i, j, new(0, -1));
                    change.Happened = true;
                }
            }
        }
    }

    private static void Roll(int i, int j, Direction dir)
    {
        Map[i] = Map[i].Remove(j, 1).Insert(j, ".");
        Map[i + dir.X] = Map[i + dir.X].Remove(j + dir.Y, 1).Insert(j + dir.Y, "O");
    }

    public record Direction(int X, int Y);

    public class Change
    {
        public bool Happened { get; set; }
    };
}