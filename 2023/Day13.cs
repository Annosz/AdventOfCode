using Shared;

namespace _2023;

public static class Day13
{
    public readonly static List<List<string>> Patterns = new List<List<string>>();

    private const int FaultTolerance = 1;

    public static string Solve()
    {
        List<string> singlePattern = new();
        foreach (var line in File.ReadLines(@".\Input\Day13.txt"))
        {
            if (string.IsNullOrEmpty(line))
            {
                Patterns.Add(singlePattern);
                singlePattern = new();
                continue;
            }
            singlePattern.Add(line);
        }
        Patterns.Add(singlePattern);

        long solution = 0;
        foreach (var pattern in Patterns)
        {
            FindMirror(pattern, 100, ref solution);
            var patternArray = pattern.Select(e => e.ToArray()).ToArray();
            var rotatedPatternArray = ArrayHelpers.RotateArrayCounterClockwise(patternArray.To2DArray());
            var rotatedPatternJaggedArray = rotatedPatternArray.ToJaggedArray();
            var rotatedPattern = rotatedPatternJaggedArray.Select(e => new string(e)).ToList();
            FindMirror(rotatedPattern, 1, ref solution);
        }

        return solution.ToString();
    }

    private static void FindMirror(List<string> pattern, int multiplier, ref long solution)
    {
        for (int i = 0; i < pattern.Count - 1; i++)
        {
            int possibleMirror = i;
            int distanceFromMirror = 0;
            int j = 0;
            while (i - j >= 0 && i + j + 1 < pattern.Count && possibleMirror != -1)
            {
                int currentDistance = StringHelpers.CalcLevenshteinDistance(pattern[i - j], pattern[i + j + 1]);
                if (currentDistance + distanceFromMirror > FaultTolerance)
                {
                    possibleMirror = -1;
                }
                distanceFromMirror += currentDistance;
                j++;
            }
            if (possibleMirror != -1 && distanceFromMirror == FaultTolerance)
            {
                solution += multiplier * (possibleMirror + 1);
            }
        }
    }
}