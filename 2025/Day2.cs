namespace _2025;

public static class Day2
{
    public static string Solve()
    {
        var invalidProductIdSum1 = 0L;
        var invalidProductIdSum2 = 0L;
        var line = File.ReadLines(@".\Input\Day2.txt").First();

        var ranges = line.Split(',');
        foreach (var range in ranges)
        {
            var rangeEdges = range.Split('-').Select(long.Parse).ToArray();
            for (var productId = rangeEdges[0]; productId <= rangeEdges[1]; productId++)
            {
                var productIdStr = productId.ToString();
                for (int i = 1; i <= productIdStr.Length / 2; i++)
                {
                    if (RepeatString(productIdStr[..i], productIdStr.Length / i) == productIdStr)
                    {
                        invalidProductIdSum1 += i * 2 == productIdStr.Length ? 0 : productId;
                        invalidProductIdSum2 += productId;
                        break;
                    }
                }
            }
        }

        return $"Part 1: {invalidProductIdSum1}, Part 2: {invalidProductIdSum2}";
    }

    private static string RepeatString(string str, int n)
    {
        return string.Concat(Enumerable.Repeat(str, n));
    }
}