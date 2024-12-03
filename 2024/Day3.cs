using System.Text.RegularExpressions;

namespace _2024;

public static class Day3
{
    public static string Solve()
    {
        long sum = 0;
        bool enabled = true;
        string pattern = @"mul\((\d{1,3}),(\d{1,3})\)|do\(\)|don't\(\)";
        foreach (var line in File.ReadLines(@".\Input\Day3.txt"))
        {
            foreach (Match match in Regex.Matches(line, pattern))
            {
                switch (match.Value)
                {
                    case "do()":
                        enabled = true;
                        continue;
                    case "don't()":
                        enabled = false;
                        continue;
                    default:
                        if (enabled)
                            sum += long.Parse(match.Groups[1].Value) * long.Parse(match.Groups[2].Value);
                        break;
                }
            }
        }
        return sum.ToString();
    }
}