namespace _2023;

public static class Day2
{
    public static string Solve()
    {
        int sumOfValidGames = 0;
        int sumOfGamePowers = 0;

        foreach (var line in File.ReadLines(@".\Input\Day2.txt"))
        {
            var lineSplit = line.Split(':');

            var gameNumber = Int32.Parse(lineSplit[0].Replace("Game ", ""));

            lineSplit = lineSplit[1].Replace(';', ',').Split(",");

            (bool validGame, int gamePower) = AnalyzeGame(lineSplit);

            if (validGame)
                sumOfValidGames += gameNumber;
            sumOfGamePowers += gamePower;
        }

        return sumOfGamePowers.ToString();
    }

    private static (bool, int) AnalyzeGame(string[] lineSplit)
    {
        bool validGame = true;

        int minRed = 0;
        int minGreen = 0;
        int minBlue = 0;
        foreach (var segment in lineSplit)
        {
            var pullSplit = segment.Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

            switch (pullSplit[1])
            {
                case "red":
                    minRed = Math.Max(minRed, Int32.Parse(pullSplit[0]));
                    if (Int32.Parse(pullSplit[0]) > 12)
                        validGame = false;
                    break;
                case "green":
                    minGreen = Math.Max(minGreen, Int32.Parse(pullSplit[0]));
                    if (Int32.Parse(pullSplit[0]) > 13)
                        validGame = false;
                    break;
                case "blue":
                    minBlue = Math.Max(minBlue, Int32.Parse(pullSplit[0]));
                    if (Int32.Parse(pullSplit[0]) > 14)
                        validGame = false;
                    break;
                default:
                    break;
            }
        }

        return (validGame, minRed * minBlue * minGreen);
    }
}