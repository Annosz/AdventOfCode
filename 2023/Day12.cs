namespace _2023;

public static class Day12
{
    private static readonly List<(string, int[])> Map = new();

    // 1 for Part 1, 5 for Part 2
    private const int RepeatFactor = 5;

    public static string Solve()
    {
        foreach (var line in File.ReadLines(@".\Input\Day12.txt"))
        {
            var lineSplit = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var lineSplitBegin = string.Join("?", Enumerable.Repeat(lineSplit[0], RepeatFactor));
            var lineSplitEnd = string.Join(",", Enumerable.Repeat(lineSplit[1], RepeatFactor));
            Map.Add((lineSplitBegin, lineSplitEnd.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(n => int.Parse(n)).ToArray()));

            var template = string.Join("?", Enumerable.Repeat(lineSplit[0], RepeatFactor));
            var intervals = string.Join(",", Enumerable.Repeat(lineSplit[1], RepeatFactor)).Split(',', StringSplitOptions.RemoveEmptyEntries).Select(n => int.Parse(n)).ToArray();
            Map.Add((template, intervals));
        }

        long allPossibilities = 0;
        foreach (var (template, intervals) in Map)
        {
            long[,] solutionSpace = new long[template.Length + 1, intervals.Length + 1];

            for (int templateLength = 0; templateLength <= template.Length; templateLength++)
            {
                for (int intervalCount = 0; intervalCount <= intervals.Length; intervalCount++)
                {
                    string currentTempalte = template[..templateLength];

                    if (intervalCount == 0)
                    {
                        solutionSpace[templateLength, intervalCount] = !currentTempalte.Any(e => e == '#') ? 1 : 0;
                        continue;
                    }

                    if (templateLength == 0)
                    {
                        solutionSpace[templateLength, intervalCount] = 0;
                        continue;
                    }

                    int currentInterval = intervals[intervalCount - 1];

                    bool mustFitToEnd = currentTempalte.Last() == '#';
                    bool canFitToEnd = templateLength >= currentInterval
                        && !currentTempalte[^currentInterval..].Any(e => e == '.')
                        && (templateLength == currentInterval || currentTempalte[^(currentInterval + 1)] != '#');
                    solutionSpace[templateLength, intervalCount] =
                        (mustFitToEnd ? 0 : solutionSpace[templateLength - 1, intervalCount])
                        + (canFitToEnd ? (solutionSpace[Math.Max(templateLength - currentInterval - 1, 0), intervalCount - 1]) : 0);
                }
            }

            allPossibilities += solutionSpace[template.Length, intervals.Length];
        }

        return allPossibilities.ToString();
    }
}