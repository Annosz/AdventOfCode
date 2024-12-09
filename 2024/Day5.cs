namespace _2024;

public static class Day5
{
    private static bool IsPart1 = false;

    private static readonly Dictionary<int, List<int>> ComesBefore = new();
    private static readonly Dictionary<int, List<int>> ComesAfter = new();

    private static readonly List<List<int>> Manuals = [];

    public static string Solve()
    {
        var readingRules = true;
        foreach (var line in File.ReadLines(@".\Input\Day5.txt"))
        {
            if (line.Length == 0)
            {
                readingRules = false;
                continue;
            }

            switch (readingRules)
            {
                case true:
                    {
                        var rulePair = line.Split('|').Select(int.Parse).ToArray();
                        if (!ComesBefore.ContainsKey(rulePair[0]))
                        {
                            ComesBefore[rulePair[0]] = [];
                        }
                        ComesBefore[rulePair[0]].Add(rulePair[1]);

                        if (!ComesAfter.ContainsKey(rulePair[1]))
                        {
                            ComesAfter[rulePair[1]] = [];
                        }
                        ComesAfter[rulePair[1]].Add(rulePair[0]);
                        break;
                    }
                case false:
                    Manuals.Add(line.Split(',').Select(int.Parse).ToList());
                    break;
            }
        }

        var sum = 0L;
        foreach (var manual in Manuals)
        {
            var middleNumber = FindMiddleNumber(manual);
            if (IsPart1 ? IsCorrect(manual) : !IsCorrect(manual))
            {
                sum += middleNumber;
            }
        }

        return sum.ToString();
    }

    private static int FindMiddleNumber(List<int> manual)
    {
        foreach (var page in manual)
        {
            if (ComesAfter[page].Intersect(manual).Count() == ComesBefore[page].Intersect(manual).Count())
                return page;
        }

        return -1;
    }

    private static bool IsCorrect(List<int> manual)
    {
        for (var i = 0; i < manual.Count; i++)
        {
            var current = manual[i];

            var before = manual.GetRange(0, i);
            foreach (var b in before)
            {
                if (ComesBefore.ContainsKey(current) && ComesBefore[current].Contains(b))
                {
                    return false;
                }
            }

            var after = manual.GetRange(i + 1, manual.Count - i - 1);
            foreach (var a in after)
            {
                if (ComesAfter.ContainsKey(current) && ComesAfter[current].Contains(a))
                {
                    return false;
                }
            }
        }

        return true;
    }
}