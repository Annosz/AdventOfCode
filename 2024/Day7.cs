namespace _2024;

public static class Day7
{
    public static string Solve()
    {
        var part1Result = 0L;
        var part2Result = 0L;
        foreach (var line in File.ReadLines(@".\Input\Day7.txt"))
        {
            var split = line.Split(": ");

            var total = long.Parse(split[0]);
            var elements = split[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList();

            part1Result += IsSimpleCombinationPossible(total, elements) ? total : 0;
            part2Result += IsComplexCombinationPossible(total, elements[1..], [elements[0]]) ? total : 0;
        }

        return $"Part 1: {part1Result}, Part 2: {part2Result}";
    }

    private static bool IsSimpleCombinationPossible(long total, List<long> elements)
    {
        if (elements.Count == 1)
            return total == elements.First();

        return IsSimpleCombinationPossible(total - elements.Last(), elements[..^1])
               || (total % elements.Last() == 0 && IsSimpleCombinationPossible(total / elements.Last(), elements[..^1]));
    }

    private static bool IsComplexCombinationPossible(long total, List<long> elements, List<long> possibleTotals)
    {
        if (elements.Count == 0)
            return possibleTotals.Contains(total);

        var newPossibleTotals = possibleTotals.Select(e => e + elements[0]).ToList();
        newPossibleTotals.AddRange(possibleTotals.Select(e => e * elements[0]).ToList());
        newPossibleTotals.AddRange(possibleTotals.Select(e => long.Parse(string.Join("", e, elements[0]))).ToList());

        return IsComplexCombinationPossible(total, elements[1..], newPossibleTotals);
    }
}