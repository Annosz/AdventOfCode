namespace _2022;

public static class Day4
{
    private static int fullyContainsCount = 0;
    private static int overlapsCount = 0;

    public static string Solve()
    {
        foreach (string line in File.ReadLines(@".\Input\Day4.txt"))
        {
            var sectionStrings = line.Split(',');
            var section1 = Array.ConvertAll(sectionStrings[0].Split('-'), int.Parse);
            var section2 = Array.ConvertAll(sectionStrings[1].Split('-'), int.Parse);

            fullyContainsCount += FullyContains(section1[0], section1[1], section2[0], section2[1]) ? 1 : 0;
            overlapsCount += Overlaps(section1[0], section1[1], section2[0], section2[1]) ? 1 : 0;
        }

        return overlapsCount.ToString();
    }

    private static bool FullyContains(int firstStart, int firstEnd, int secondStart, int secondEnd)
        => (firstStart >= secondStart && firstEnd <= secondEnd) || (secondStart >= firstStart && secondEnd <= firstEnd);

    private static bool Overlaps(int firstStart, int firstEnd, int secondStart, int secondEnd)
        => firstStart <= secondEnd && secondStart <= firstEnd;
}