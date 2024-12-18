using MathNet.Numerics.LinearAlgebra;
using System.Text.RegularExpressions;

namespace _2024;

public static class Day13
{
    public record Vector(long X, long Y);

    public record Task(Vector A, Vector B, Vector Target);

    private static List<Task> Tasks = [];

    public static string Solve()
    {
        var Addition = 10000000000000L;

        var pattern = @"X[+=](\d+), Y[+=](\d+)";
        var lines = File.ReadAllLines(@".\Input\Day13.txt");
        for (var i = 0; i < lines.Length; i++)
        {
            if (lines[i] == "")
            {
                continue;
            }

            Match matchA = Regex.Match(lines[i++], pattern);
            var pointA = new Vector(int.Parse(matchA.Groups[1].Value), int.Parse(matchA.Groups[2].Value));
            Match matchB = Regex.Match(lines[i++], pattern);
            var pointB = new Vector(int.Parse(matchB.Groups[1].Value), int.Parse(matchB.Groups[2].Value));
            Match matchT = Regex.Match(lines[i++], pattern);
            var pointT = new Vector(int.Parse(matchT.Groups[1].Value) + Addition, int.Parse(matchT.Groups[2].Value) + Addition);

            Tasks.Add(new Task(pointA, pointB, pointT));
        }

        var sum = 0L;
        foreach (var task in Tasks)
        {
            var A = Matrix<Double>.Build.DenseOfArray(new Double[,]
            {
                {task.A.X, task.B.X},
                {task.A.Y, task.B.Y}
            });
            var b = Vector<Double>.Build.Dense(new Double[] { task.Target.X, task.Target.Y });
            var x = A.Solve(b);

            var APress = (long)Math.Round(x[0]);
            var BPress = (long)Math.Round(x[1]);
            if (APress * task.A.X + BPress * task.B.X == task.Target.X && APress * task.A.Y + BPress * task.B.Y == task.Target.Y)
            {
                sum += 3 * APress + BPress;
            }
        }

        return $"Result is: {sum}";
    }
}