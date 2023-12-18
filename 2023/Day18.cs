namespace _2023;

public static class Day18
{
    private static readonly List<Instruction> Instructions = new();

    private const bool IsPart2 = true;

    public static string Solve()
    {
        foreach (var line in File.ReadLines(@".\Input\Day18.txt"))
        {
            var lineSplit = line.Split(' ');
            Instructions.Add(new(lineSplit[0][0], int.Parse(lineSplit[1]), new string(lineSplit[2].Skip(2).SkipLast(1).ToArray())));
        }

        // Shoelace formula for polygonal area
        var lastPosition = new Point(0, 0);
        var currentPosition = new Point(0, 0);
        long edge = 0;
        long sum1 = 0;
        long sum2 = 0;
        foreach (var instruction in Instructions)
        {
            currentPosition = currentPosition with
            {
                X = currentPosition.X + instruction.GetDeirection().X * instruction.GetCount(),
                Y = currentPosition.Y + instruction.GetDeirection().Y * instruction.GetCount()
            };
            edge += instruction.GetCount();
            sum1 += lastPosition.X * currentPosition.Y;
            sum2 += currentPosition.X * lastPosition.Y;
            lastPosition = currentPosition;
        }

        return (Math.Abs(sum1 - sum2) / 2 + edge / 2 + 1).ToString();
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