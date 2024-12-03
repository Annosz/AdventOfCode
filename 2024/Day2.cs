namespace _2024;

public static class Day2
{
    public static string Solve()
    {
        var correctReports = 0;

        foreach (var line in File.ReadLines(@".\Input\Day2.txt"))
        {
            var rawReport = line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();

            var dampenedReport = rawReport.Select((_, i) =>
            {
                var reportCopy = new List<int>(rawReport);
                reportCopy.RemoveAt(i);
                return reportCopy;
            }).ToList();

            var isAnyCorrect = dampenedReport.Any(report =>
            {
                var differences = report.Zip(report.Skip(1), (x, y) => y - x).ToList();
                return Math.Min(NegativeDifferenceErrors(differences), PositiveDifferenceErrors(differences)) +
                    SmallDifferenceErrors(differences) + LargeDifferenceErrors(differences) == 0;
            });

            correctReports += isAnyCorrect ? 1 : 0;
        }

        return correctReports.ToString();
    }

    private static int LargeDifferenceErrors(List<int> differences) => differences.Count(e => Math.Abs(e) > 3);

    private static int SmallDifferenceErrors(List<int> differences) => differences.Count(e => Math.Abs(e) < 1);

    private static int PositiveDifferenceErrors(List<int> differences) => Math.Abs(differences.Count(e => e > 0) - differences.Count);

    private static int NegativeDifferenceErrors(List<int> differences) => Math.Abs(differences.Count(e => e < 0) - differences.Count);
}