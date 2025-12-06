using JetBrains.Annotations;
using Shared.AoC;

namespace _2025;

[UsedImplicitly]
public class Day5 : IAoCTask
{
    public string Solve()
    {
        List<long[]> validRanges = [];
        long freshCount1 = 0;

        var readRanges = true;
        foreach (var line in File.ReadLines(@$".\Input\{GetType().Name}.txt"))
        {
            if (line.ReplaceLineEndings() == string.Empty)
            {
                readRanges = false;
                continue;
            }

            if (readRanges)
            {
                validRanges.Add(line.Split('-').Select(long.Parse).ToArray());
            }
            else
            {
                var id = long.Parse(line);
                freshCount1 += validRanges.Any(r => r[0] <= id && r[1] >= id) ? 1 : 0;
            }
        }

        var mergedRanges = MergeRanges(validRanges);
        var freshCount2 = mergedRanges.Sum(range => range[1] - range[0] + 1);

        return $"Fresh ingredients: {freshCount1}; {freshCount2}";
    }

    private List<long[]> MergeRanges(List<long[]> validRanges)
    {
        var sortedRanges = validRanges.OrderBy(r => r[0]).ToList();
        var mergedRanges = new List<long[]>();
        foreach (var range in sortedRanges)
        {
            if (mergedRanges.Count == 0 || mergedRanges.Last()[1] < range[0] - 1)
            {
                mergedRanges.Add(range);
            }
            else
            {
                mergedRanges.Last()[1] = Math.Max(mergedRanges.Last()[1], range[1]);
            }
        }
        return mergedRanges;
    }
}