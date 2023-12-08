namespace _2023;

public static class Day8
{
    private static string Directions = "";
    private static Dictionary<string, string[]> Map = new Dictionary<string, string[]>();

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

        long stepCount = 0;
        int directionPointer = 0;
        string[] currentNodes = Map.Keys.Where(n => n.EndsWith("AAA")).ToArray();
        while (!currentNodes.All(n => n.EndsWith("ZZZ")))
        {
            for (int i = 0; i < currentNodes.Length; i++)
            {
                currentNodes[i] = Map[currentNodes[i]][Directions[directionPointer] == 'L' ? 0 : 1];
            }
            stepCount++;
            directionPointer++;
            directionPointer %= Directions.Length;
        }

        return stepCount.ToString();
    }
}