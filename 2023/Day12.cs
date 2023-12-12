namespace _2023;

public static class Day12
{
    private static List<(string, int[])> Map = new();
    private const int RepeatFactor = 5;

    public static string Solve()
    {
        foreach (var line in File.ReadLines(@".\Input\Day12.txt"))
        {
            var lineSplit = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var lineSplitBegin = string.Join("?", Enumerable.Repeat(lineSplit[0], RepeatFactor));
            var lineSplitEnd = string.Join(",", Enumerable.Repeat(lineSplit[1], RepeatFactor));
            Map.Add((lineSplitBegin, lineSplitEnd.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(n => int.Parse(n)).ToArray()));
        }

        long allPossibilities = 0;
        int count = 1;
        foreach (var (mapLine, intervals) in Map)
        {
            Console.WriteLine($"{count}: {mapLine}");
            GenerateAllPossibilities(mapLine, intervals, ref allPossibilities);
            count++;
        }

        return allPossibilities.ToString();
    }

    private static void GenerateAllPossibilities(string mapLine, int[] intervals, ref long allPossibilities)
    {
        if (!mapLine.Any() && intervals.Any())
            return;

        if (!intervals.Any())
        {
            if (!mapLine.Contains('#'))
                allPossibilities++;
            return;
        }

        if (mapLine.Length < (intervals.Sum() + intervals.Count() - 1))
            return;

        var segmentLengths = mapLine.Split('.', StringSplitOptions.RemoveEmptyEntries).Select(e => e.Length);
        if (segmentLengths.Sum() < intervals.Sum())
            return;

        int longestInterval = intervals.Max();
        if (segmentLengths.Sum(l => Math.Floor(l / (decimal)longestInterval)) < intervals.Where(e => e == longestInterval).Count())
            return;

        switch (mapLine.First())
        {
            case '.':
                GenerateAllPossibilities(mapLine.Remove(0, 1), intervals, ref allPossibilities);
                break;
            case '?':
                GenerateAllPossibilities(mapLine.Remove(0, 1), intervals, ref allPossibilities);
                if (mapLine.Length >= intervals[0] && !mapLine.Substring(0, intervals[0]).Contains('.') && (mapLine.Length == intervals[0] || mapLine[intervals[0]] != '#'))
                    GenerateAllPossibilities(mapLine.Remove(0, Math.Min(mapLine.Length, intervals[0] + 1)), intervals[1..], ref allPossibilities);
                break;
            case '#':
                if (mapLine.Length >= intervals[0] && !mapLine.Substring(0, intervals[0]).Contains('.') && (mapLine.Length == intervals[0] || mapLine[intervals[0]] != '#'))
                    GenerateAllPossibilities(mapLine.Remove(0, Math.Min(mapLine.Length, intervals[0] + 1)), intervals[1..], ref allPossibilities);
                break;
        }
    }
}