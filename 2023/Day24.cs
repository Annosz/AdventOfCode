namespace _2023;

public static class Day24
{
    public static List<MathematicLine> MathematicLines = new();
    public static List<VelocityLine> VelocityLines = new();

    public const double LowerLimit = 200000000000000;
    public const double UpperLimit = 400000000000000;

    public static string Solve()
    {
        foreach (var line in File.ReadLines(@".\Input\Day24.txt"))
        {
            var lineSplit = line.Split(" @ ", StringSplitOptions.RemoveEmptyEntries);
            var position = lineSplit[0].Split(", ", StringSplitOptions.RemoveEmptyEntries).Select(e => double.Parse(e)).ToArray();
            var velocity = lineSplit[1].Split(", ", StringSplitOptions.RemoveEmptyEntries).Select(e => double.Parse(e)).ToArray();

            var point1 = new Point(position[0], position[1]);
            var point2 = new Point(position[0] + velocity[0], position[1] + velocity[1]);

            double A = point2.Y - point1.Y;
            double B = point1.X - point2.X;
            double C = A * point1.X + B * point1.Y;

            MathematicLines.Add(new MathematicLine(A, B, C));
            VelocityLines.Add(new VelocityLine(position[0], position[1], velocity[0], velocity[1]));
        }

        long intersectionCount = 0;
        for (var i = 1; i < MathematicLines.Count; i++)
            for (var j = 0; j < i; j++)
            {
                var intersection = GetIntersection(MathematicLines[i], MathematicLines[j]);
                if (IntersectionIsWithinBoundary(intersection) && IntersectionIsInFuture(i, j, intersection))
                    intersectionCount++;
            }

        return $"Part 1: {intersectionCount}, Part 2: {intersectionCount}";
    }

    public static Point GetIntersection(MathematicLine first, MathematicLine second)
    {
        double delta = first.A * second.B - second.A * first.B;

        if (delta == 0)
            return new Point(-1, -1);

        double x = (second.B * first.C - first.B * second.C) / delta;
        double y = (first.A * second.C - second.A * first.C) / delta;

        return new Point(x, y);
    }

    private static bool IntersectionIsInFuture(int i, int j, Point intersection) =>
        !(VelocityLines[i].X < intersection.X && VelocityLines[i].VX < 0) && !(VelocityLines[i].X > intersection.X && VelocityLines[i].VX > 0)
            && !(VelocityLines[i].Y < intersection.Y && VelocityLines[i].VY < 0) && !(VelocityLines[i].Y > intersection.Y && VelocityLines[i].VY > 0)
            && !(VelocityLines[j].X < intersection.X && VelocityLines[j].VX < 0) && !(VelocityLines[j].X > intersection.X && VelocityLines[j].VX > 0)
            && !(VelocityLines[j].Y < intersection.Y && VelocityLines[j].VY < 0) && !(VelocityLines[j].Y > intersection.Y && VelocityLines[j].VY > 0);

    private static bool IntersectionIsWithinBoundary(Point intersection) =>
        intersection.X >= LowerLimit && intersection.X <= UpperLimit && intersection.Y >= LowerLimit && intersection.Y <= UpperLimit;

    public record Point(double X, double Y);
    public record MathematicLine(double A, double B, double C);
    public record VelocityLine(double X, double Y, double VX, double VY);
}