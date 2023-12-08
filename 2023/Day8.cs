using Shared;

namespace _2023;

public static class Day8
{
    private static string Directions = "";
    private static readonly Dictionary<string, string[]> Map = new();

    private const string part1Start = "AAA";
    private const string part1End = "ZZZ";
    private const string part2Start = "A";
    private const string part2End = "Z";

    public static string Solve()
    {
        foreach (var line in File.ReadLines(@".\Input\Day8.txt"))
        {
            if (string.IsNullOrEmpty(line))
                continue;

            if (!line.Contains(" = "))
            {
                Directions = new string(line);
                continue;
            }

            var lineSplit = line.Split(" = ", StringSplitOptions.RemoveEmptyEntries);
            var node = lineSplit[0];
            var edges = lineSplit[1].Replace("(", "").Replace(")", "").Split(", ");

            Map.Add(node, edges);
        }

        int stepCount = 0;
        int directionPointer = 0;
        string[] currentNodes = Map.Keys.Where(n => n.EndsWith(part2Start)).ToArray();
        Dictionary<int, List<int>> zStepTimes = new();
        for (int i = 0; i < currentNodes.Length; i++)
        {
            zStepTimes.Add(i, new List<int>());
        }

        // Gather some data, constant patterns should emerge by 10 iterations
        while (!zStepTimes.Values.All(z => z.Count >= 10))
        {
            for (int i = 0; i < currentNodes.Length; i++)
            {
                currentNodes[i] = Map[currentNodes[i]][Directions[directionPointer] == 'L' ? 0 : 1];
                if (currentNodes[i].EndsWith(part2End))
                {
                    zStepTimes[i].Add(stepCount);
                }
            }
            stepCount++;
            directionPointer++;
            directionPointer %= Directions.Length;
        }

        // Find the intervals between arriving at Z
        Dictionary<int, List<int>> zStepTimeDifferences = new();
        for (int i = 0; i < currentNodes.Length; i++)
        {
            zStepTimeDifferences.Add(i, new List<int>());
            zStepTimeDifferences[i].Add(zStepTimes[i].First());
            for (int j = 1; j < zStepTimes[i].Count; j++)
            {
                zStepTimeDifferences[i].Add(zStepTimes[i][j] - zStepTimes[i][j - 1]);
            }
        }

        return MathHelpers.LeastCommonMultiple(zStepTimeDifferences.Values.Select(z => z.Last()).ToArray()).ToString();
    }


}