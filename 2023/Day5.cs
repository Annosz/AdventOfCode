using System.Diagnostics;

namespace _2023;

public static class Day5
{
    private static List<Range> ranges = new List<Range>();

    public static string Solve()
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();
        List<Rule> rules = new();
        foreach (var line in File.ReadLines(@".\Input\Day5.txt"))
        {
            if (line.StartsWith("seeds"))
            {
                var lineSplit = line.Remove(0, "seeds: ".Length).Split(" ", StringSplitOptions.RemoveEmptyEntries);
                var parsedRanges = lineSplit.Select(n => long.Parse(n)).ToList();
                // Part 1
                //for (int i = 0; i < parsedRanges.Count; i++)
                //{
                //    ranges.Add(new Range(parsedRanges[i], 1));
                //}

                // Part 2
                for (int i = 0; i < parsedRanges.Count; i += 2)
                {
                    ranges.Add(new Range(parsedRanges[i], parsedRanges[i + 1]));
                }
                continue;
            }

            if (string.IsNullOrEmpty(line))
                continue;

            if (char.IsNumber(line.First()))
            {
                var parsedRule = line.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(n => long.Parse(n)).ToList();
                rules.Add(new Rule(parsedRule[0], parsedRule[1], parsedRule[2]));
            }

            if (!char.IsNumber(line.First()) && rules.Any())
            {
                ApplyRules(rules);

                rules = new List<Rule>();
            }
        }
        ApplyRules(rules);

        sw.Stop();
        return $"{sw.ElapsedMilliseconds} ms: {ranges.Min(r => r.Start)}";
    }

    private static void ApplyRules(List<Rule> rules)
    {
        List<Range> convertedRanges = new();
        while (ranges.Any())
        {
            var range = ranges.First();
            ranges.RemoveAt(0);

            var appliedRule = rules.FirstOrDefault(r => r.SourceRangeStart <= range.Start && range.Start < r.SourceRangeEnd);
            if (appliedRule != null)
            {
                if (appliedRule.SourceRangeEnd > range.End)
                {
                    convertedRanges.Add(range with { Start = range.Start + appliedRule.MappingShift });
                }
                else
                {
                    convertedRanges.Add(new Range(range.Start + appliedRule.MappingShift, appliedRule.SourceRangeEnd - range.Start));
                    ranges.Add(new Range(appliedRule.SourceRangeEnd, range.Length - (appliedRule.SourceRangeEnd - range.Start)));
                }

                continue;
            }

            var closestRule = rules.Where(r => range.Start < r.SourceRangeStart).OrderBy(r => r.SourceRangeStart).FirstOrDefault();
            if (closestRule != null)
            {
                convertedRanges.Add(new Range(range.Start, closestRule.SourceRangeStart - range.Start));
                ranges.Add(new Range(closestRule.SourceRangeStart, range.Length - (closestRule.SourceRangeStart - range.Start)));
                continue;
            }

            convertedRanges.Add(range);
        }

        ranges = convertedRanges;
    }

    public record Rule(long DestinationRangeStart, long SourceRangeStart, long RangeLength)
    {
        public long SourceRangeEnd => SourceRangeStart + RangeLength;
        public long MappingShift => DestinationRangeStart - SourceRangeStart;
    }

    public record Range(long Start, long Length)
    {
        public long End => Start + Length;
    }
}