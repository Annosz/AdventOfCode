using System.Text.RegularExpressions;

namespace _2024;

public static class Day14
{
    private const bool IsPart1 = false;
    private static int[] Nd = [11, 101];
    private static int[] Md = [7, 103];
    private static int N => Nd[IsPart1 ? 0 : 1];
    private static int M => Md[IsPart1 ? 0 : 1];

    private record Point(int X, int Y);

    private class Robot
    {
        public Point Position { get; set; }
        public Point Speed { get; set; }

        public void Move()
        {
            var X = Position.X + Speed.X;
            var Y = Position.Y + Speed.Y;
            if (X >= N) X %= N;
            if (Y >= M) Y %= M;
            if (X < 0) X += N;
            if (Y < 0) Y += M;
            Position = new Point(X, Y);
        }
    }

    private static readonly List<Robot> Robots = [];

    public static string Solve()
    {
        const string pattern = @"p=(-?\d+),(-?\d+) v=(-?\d+),(-?\d+)";
        foreach (var line in File.ReadLines(@$".\{(IsPart1 ? "Sample" : "Input")}\Day14.txt"))
        {
            var match = Regex.Match(line, pattern);
            if (match.Success)
            {
                Robots.Add(new Robot
                {
                    Position = new Point(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value)),
                    Speed = new Point(int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value))
                });
            }
        }

        long t = 0;
        var part1Result = Part1Calculation(ref t);
        Part2Search(ref t);

        return $"Part 1 result: {part1Result}";
    }

    private static int Part1Calculation(ref long t)
    {
        while (t < 100)
        {
            t++;
            MoveAllRobots();
        }

        return Robots.Count(r => r.Position.X < Math.Floor(N / 2d) && r.Position.Y < Math.Floor(M / 2d))
            * Robots.Count(r => r.Position.X < Math.Floor(N / 2d) && r.Position.Y > Math.Floor(M / 2d))
            * Robots.Count(r => r.Position.X > Math.Floor(N / 2d) && r.Position.Y < Math.Floor(M / 2d))
            * Robots.Count(r => r.Position.X > Math.Floor(N / 2d) && r.Position.Y > Math.Floor(M / 2d));
    }

    private static void MoveAllRobots()
    {
        foreach (var robot in Robots)
            robot.Move();
    }

    private static void Part2Search(ref long t)
    {
        var lastReadKey = ' ';
        do
        {
            t++;
            MoveAllRobots();

            if (!PossibleChristmasTree())
                continue;

            // Print out for visual check
            Console.Clear();
            for (var j = 0; j < M; j++)
            {
                for (var i = 0; i < N; i++)
                {
                    Console.Write(Robots.Any(r => r.Position.X == i && r.Position.Y == j) ? '#' : '.');
                }
                Console.WriteLine();
            }
            Console.WriteLine($"Possible part 2 result: {t}");
            Console.WriteLine($"Press 's' if you want to stop searching.");
            lastReadKey = Console.ReadKey().KeyChar;
        } while (lastReadKey != 's');

        Console.WriteLine();
    }

    public static bool PossibleChristmasTree()
    {
        var straightLines = 0;
        foreach (var robot in Robots)
        {
            if (Robots.Count(r => r.Position != robot.Position && Math.Abs(robot.Position.X - r.Position.X) <= 5 && robot.Position.Y == r.Position.Y) >= 10)
                straightLines++;
        }
        return straightLines >= 8;
    }
}