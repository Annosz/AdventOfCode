namespace _2023;

public static class Day16
{
    private static List<string> Map = new();

    public static string Solve()
    {
        foreach (var line in File.ReadLines(@".\Input\Day16.txt"))
        {
            Map.Add(line);
        }

        // Create the space in we search for the solution
        int OriginalEnergy = 0;
        int MaxEnergy = 0;
        List<Beam> possibleStartingBeams = new();
        for (int i = 0; i < Map.Count; i++)
        {
            possibleStartingBeams.Add(new Beam(new Point(i, 0), new Point(0, 1)));
            possibleStartingBeams.Add(new Beam(new Point(i, Map[0].Length - 1), new Point(0, -1)));
        }
        for (int j = 0; j < Map[0].Length; j++)
        {
            possibleStartingBeams.Add(new Beam(new Point(0, j), new Point(1, 0)));
            possibleStartingBeams.Add(new Beam(new Point(Map.Count - 1, j), new Point(-1, 0)));
        }

        // Simulate every possible beam movement
        foreach (var startingBeam in possibleStartingBeams)
        {
            List<Beam> Beams = new() { startingBeam with { } };
            HashSet<Beam> EveryBeamEver = new() { startingBeam with { } };

            List<Beam> newBeams;
            do
            {
                newBeams = new List<Beam>();

                foreach (var beam in Beams)
                {
                    newBeams.AddRange(MoveBeam(beam));
                }

                newBeams.RemoveAll(b => EveryBeamEver.Contains(b));
                foreach (var beam in newBeams)
                {
                    EveryBeamEver.Add(beam with { });
                }

                Beams = newBeams;
            } while (newBeams.Any());

            int energy = EveryBeamEver.Select(b => b.Point).ToHashSet().Count;
            if (startingBeam == new Beam(new Point(0, 0), new Point(0, 1)))
            {
                OriginalEnergy = energy;
            }
            MaxEnergy = Math.Max(MaxEnergy, energy);
        }

        return $"Part 1: {OriginalEnergy} Part 2: {MaxEnergy}".ToString();
    }

    private static IEnumerable<Beam> MoveBeam(Beam beam)
    {
        List<Beam> newBeams = new();
        switch (Map[beam.Point.X][beam.Point.Y])
        {
            case '.':
                newBeams.Add(beam with { Point = beam.Point with { X = beam.Point.X + beam.Direction.X, Y = beam.Point.Y + beam.Direction.Y } });
                break;

            case '\\':
                if (beam.Direction.X != 0)
                {
                    Point newDirection = beam.Direction with { X = 0, Y = beam.Direction.X };
                    newBeams.Add(beam with { Point = beam.Point with { X = beam.Point.X + newDirection.X, Y = beam.Point.Y + newDirection.Y }, Direction = newDirection });

                }
                if (beam.Direction.Y != 0)
                {
                    Point newDirection = beam.Direction with { X = beam.Direction.Y, Y = 0 };
                    newBeams.Add(beam with { Point = beam.Point with { X = beam.Point.X + newDirection.X, Y = beam.Point.Y + newDirection.Y }, Direction = newDirection });
                }
                break;

            case '/':
                if (beam.Direction.X != 0)
                {
                    Point newDirection = beam.Direction with { X = 0, Y = beam.Direction.X * -1 };
                    newBeams.Add(beam with { Point = beam.Point with { X = beam.Point.X + newDirection.X, Y = beam.Point.Y + newDirection.Y }, Direction = newDirection });
                }
                if (beam.Direction.Y != 0)
                {
                    Point newDirection = beam.Direction with { X = beam.Direction.Y * -1, Y = 0 };
                    newBeams.Add(beam with { Point = beam.Point with { X = beam.Point.X + newDirection.X, Y = beam.Point.Y + newDirection.Y }, Direction = newDirection });
                }
                break;

            case '|':
                if (beam.Direction.X != 0)
                {
                    newBeams.Add(beam with { Point = beam.Point with { X = beam.Point.X + beam.Direction.X, Y = beam.Point.Y + beam.Direction.Y } });
                }
                if (beam.Direction.Y != 0)
                {
                    Point directionA = new(-1, 0);
                    newBeams.Add(beam with { Point = beam.Point with { X = beam.Point.X + directionA.X, Y = beam.Point.Y + directionA.Y }, Direction = directionA });
                    Point directionB = new(1, 0);
                    newBeams.Add(beam with { Point = beam.Point with { X = beam.Point.X + directionB.X, Y = beam.Point.Y + directionB.Y }, Direction = directionB });
                }
                break;

            case '-':
                if (beam.Direction.X != 0)
                {
                    Point directionA = new(0, -1);
                    newBeams.Add(beam with { Point = beam.Point with { X = beam.Point.X + directionA.X, Y = beam.Point.Y + directionA.Y }, Direction = directionA });
                    Point directionB = new(0, 1);
                    newBeams.Add(beam with { Point = beam.Point with { X = beam.Point.X + directionB.X, Y = beam.Point.Y + directionB.Y }, Direction = directionB });
                }
                if (beam.Direction.Y != 0)
                {
                    newBeams.Add(beam with { Point = beam.Point with { X = beam.Point.X + beam.Direction.X, Y = beam.Point.Y + beam.Direction.Y } });
                }
                break;

        }

        newBeams.RemoveAll(b => b.Point.X < 0 || b.Point.Y < 0 || b.Point.X >= Map.Count || b.Point.Y >= Map[0].Length);

        return newBeams;
    }

    public record Point(int X, int Y);
    public record Beam(Point Point, Point Direction);
}