namespace _2022;

public static class Day2
{
    private static readonly Dictionary<char, char> Draws = new() { { 'A', 'X' }, { 'B', 'Y' }, { 'C', 'Z' } };
    private static readonly Dictionary<char, char> Wins = new() { { 'A', 'Y' }, { 'B', 'Z' }, { 'C', 'X' } };
    private static readonly Dictionary<char, char> Loses = new() { { 'A', 'Z' }, { 'B', 'X' }, { 'C', 'Y' } };

    private static readonly Dictionary<char, int> SymbolScore = new() { { 'X', 1 }, { 'Y', 2 }, { 'Z', 3 } };
    private static readonly Dictionary<char, int> ResultScore = new() { { 'X', 0 }, { 'Y', 3 }, { 'Z', 6 } };
    public static string Solve()
    {
        int finalScore = 0;

        foreach (string line in File.ReadLines(@".\Input\Day2.txt"))
        {
            char enemy = line[0];
            char us = line[2];

            finalScore += WinScore(us);
            finalScore += PlayScore(enemy, us);
        }

        return finalScore.ToString();
    }

    private static int WinScore_First(char enemy, char us)
    {
        if (Draws[enemy] == us)
            return 3;

        return Wins[enemy] == us ? 6 : 0;
    }

    private static int WinScore(char us)
    {
        return ResultScore[us];
    }

    private static int PlayScore_First(char us)
    {
        return SymbolScore[us];
    }

    private static int PlayScore(char enemy, char us)
    {
        char needToPlay = us switch
        {
            'X' => Loses[enemy],
            'Y' => Draws[enemy],
            _ => Wins[enemy]
        };
        return SymbolScore[needToPlay];
    }
}