namespace _2023;

public static class Day4
{
    private static double Points = 0;
    private static Dictionary<int, long> ScratchCards = new Dictionary<int, long>();

    public static string Solve()
    {
        foreach (var line in File.ReadLines(@".\Input\Day4.txt"))
        {
            var lineSplit = line.Split(':');
            int gameNumber = int.Parse(lineSplit[0].Split(" ", StringSplitOptions.RemoveEmptyEntries)[1]);
            if (!ScratchCards.ContainsKey(gameNumber))
                ScratchCards[gameNumber] = 1;

            lineSplit = lineSplit[1].Split("|");

            var winningNumbers = lineSplit[0].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(n => int.Parse(n));
            var mygNumbers = lineSplit[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(n => int.Parse(n));

            var commonElementCount = winningNumbers.Intersect(mygNumbers).Count();
            Points += (commonElementCount >= 1 ? Math.Pow(2, commonElementCount - 1) : 0);
            for (int i = 1; i <= commonElementCount; i++)
            {
                if (!ScratchCards.ContainsKey(gameNumber + i))
                    ScratchCards[gameNumber + i] = 1;

                ScratchCards[gameNumber + i] += ScratchCards[gameNumber];
            }
        }

        return ScratchCards.Values.Sum().ToString();
    }
}