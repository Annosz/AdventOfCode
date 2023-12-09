namespace _2023;

public static class Day9
{
    private static readonly List<List<long>> History = new();

    public static string Solve()
    {
        foreach (var line in File.ReadLines(@".\Input\Day9.txt"))
        {
            // Part 1: without .Reverse()
            History.Add(line.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(n => long.Parse(n)).Reverse().ToList());
        }

        foreach (List<long> historyLine in History)
        {
            List<List<long>> predictionHelpers = new() { historyLine };
            List<long> differences;
            do
            {
                differences = new List<long>();
                for (int i = 1; i < predictionHelpers.Last().Count; i++)
                {
                    differences.Add(predictionHelpers.Last()[i] - predictionHelpers.Last()[i - 1]);
                }
                predictionHelpers.Add(differences);
            } while (!differences.All(d => d == 0));

            predictionHelpers.Last().Add(0);
            for (int i = predictionHelpers.Count - 2; i >= 0; i--)
            {
                predictionHelpers[i].Add(predictionHelpers[i].Last() + predictionHelpers[i + 1].Last());
            }
        }

        return History.Select(h => h.Last()).Sum().ToString();
    }


}