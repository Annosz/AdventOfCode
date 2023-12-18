namespace _2023;

public static class Day18
{
    private static readonly List<Point> DigCells = new();
    private static readonly HashSet<Point> Trench = new();

    private static readonly List<Instruction> Instructions = new();

    private const bool IsPart2 = true;

    public static string Solve()
    {
        foreach (var line in File.ReadLines(@".\Input\Day18.txt"))
        {
            var lineSplit = line.Split(' ');
            Instructions.Add(new(lineSplit[0][0], int.Parse(lineSplit[1]), new string(lineSplit[2].Skip(2).SkipLast(1).ToArray())));
        }

        long lowestX = 0;
        long lowestY = 0;
        var currentPosition = new Point(0, 0);
        DigCells.Add(currentPosition);
        foreach (var instruction in Instructions)
        {
            for (var i = 0; i < instruction.GetCount(); i++)
            {
                currentPosition = currentPosition with { X = currentPosition.X + instruction.GetDeirection().X, Y = currentPosition.Y + instruction.GetDeirection().Y };
                DigCells.Add(currentPosition);

                lowestX = Math.Min(lowestX, currentPosition.X);
                lowestY = Math.Min(lowestY, currentPosition.Y);
            }
        }

        for (int i = 0; i < DigCells.Count; i++)
        {
            Trench.Add(DigCells[i] with { X = DigCells[i].X + (lowestX * -1), Y = DigCells[i].Y + (lowestY * -1) });
        }

        Queue<Point> fillQueue = new Queue<Point>();
        HashSet<Point> fillQueueContains = new HashSet<Point>();
        fillQueue.Enqueue(new Point(235, 235));
        while (fillQueue.TryDequeue(out var currentFill))
        {
            Trench.Add(currentFill);

            Point nextElement = currentFill with { X = currentFill.X + 1 };
            if (!Trench.Contains(nextElement) && !fillQueueContains.Contains(nextElement))
            {
                fillQueue.Enqueue(nextElement);
                fillQueueContains.Add(nextElement);
            }
            nextElement = currentFill with { X = currentFill.X - 1 };
            if (!Trench.Contains(nextElement) && !fillQueueContains.Contains(nextElement))
            {
                fillQueue.Enqueue(nextElement);
                fillQueueContains.Add(nextElement);
            }
            nextElement = currentFill with { Y = currentFill.Y + 1 };
            if (!Trench.Contains(nextElement) && !fillQueueContains.Contains(nextElement))
            {
                fillQueue.Enqueue(nextElement);
                fillQueueContains.Add(nextElement);
            }
            nextElement = currentFill with { Y = currentFill.Y - 1 };
            if (!Trench.Contains(nextElement) && !fillQueueContains.Contains(nextElement))
            {
                fillQueue.Enqueue(nextElement);
                fillQueueContains.Add(nextElement);
            }
        }

        return Trench.Count().ToString();
    }

    private record Point(long X, long Y);
    private record Instruction(char Direction, long Count, string Color)
    {
        public long GetCount() => IsPart2 ? Convert.ToInt64(new string(Color.Take(5).ToArray()), 16) : Count;

        public Point GetDeirection() => IsPart2 ?
            Color[5] switch
            {
                '3' => new Point(-1, 0),
                '1' => new Point(1, 0),
                '2' => new Point(0, -1),
                '0' => new Point(0, 1),
                _ => new Point(0, 0),
            }
            : Direction switch
            {
                'U' => new Point(-1, 0),
                'D' => new Point(1, 0),
                'L' => new Point(0, -1),
                'R' => new Point(0, 1),
                _ => new Point(0, 0),
            };
    }
}