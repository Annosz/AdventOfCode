namespace _2023;

public static class Day6
{
    private static List<long> Times = new List<long>();
    private static List<long> Distances = new List<long>();

    private static long MarginOfError = 1;

    public static string Solve()
    {

        foreach (var line in File.ReadLines(@".\Input\Day6.txt"))
        {
            if (line.StartsWith("Time"))
            {
                var lineSplit = line.Remove(0, "Time: ".Length).Split(" ", StringSplitOptions.RemoveEmptyEntries);
                // Part 1
                //Times = lineSplit.Select(long.Parse).ToList();
                // Part 2
                Times = new List<long>() { long.Parse(string.Join("", lineSplit)) };
            }
            if (line.StartsWith("Distance"))
            {
                var lineSplit = line.Remove(0, "Distance: ".Length).Split(" ", StringSplitOptions.RemoveEmptyEntries);
                // Part 1
                //Distances = lineSplit.Select(long.Parse).ToList();
                // Part 2
                Distances = new List<long>() { long.Parse(string.Join("", lineSplit)) };
            }
        }

        for (var i = 0; i < Times.Count; i++)
        {
            long possibilityCount = 0;
            for (int j = 0; j < Times[i]; j++)
            {
                if (j * (Times[i] - j) > Distances[i])
                    possibilityCount++;
            }

            MarginOfError *= possibilityCount;
        }

        return MarginOfError.ToString();
    }
}