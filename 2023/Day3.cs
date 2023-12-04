using System.Text.RegularExpressions;

namespace _2023;

public static class Day3
{
    private static List<string> Schemantic = new List<string>();

    private static Dictionary<(int, int), int> GearPeaces = new Dictionary<(int, int), int>();

    private static int EnginePartSum = 0;
    private static int GearRationSum = 0;

    public static string Solve()
    {

        foreach (var line in File.ReadLines(@".\Input\Day3.txt"))
        {
            Schemantic.Add(line);
        }

        for (int i = 0; i < Schemantic.Count; i++)
        {
            var simpleLine = Regex.Matches(Schemantic[i], @"[\p{N}]+").Cast<Match>().Select(x => x.Value).ToArray();
            var numbersInLine = simpleLine.Where(s => int.TryParse(s, out _)).Select(s => int.Parse(s)).ToList();

            int numberBeingChecked = 0;
            bool numberAddedToSum = false;
            for (int j = 0; j < Schemantic[i].Length; j++)
            {
                if (!Char.IsDigit(Schemantic[i][j]))
                {
                    if (j > 0 && Char.IsDigit(Schemantic[i][j - 1]))
                    {
                        numberBeingChecked++;
                        numberAddedToSum = false;
                    }
                    if (Schemantic[i][j] == '.')
                        continue;
                }

                if (Char.IsDigit(Schemantic[i][j]))
                {
                    if ((j > 0 && !Char.IsDigit(Schemantic[i][j - 1]) && Schemantic[i][j - 1] != '.')
                        || (i > 0 && !Char.IsDigit(Schemantic[i - 1][j]) && Schemantic[i - 1][j] != '.')
                        || (j < Schemantic[i].Length - 1 && !Char.IsDigit(Schemantic[i][j + 1]) && Schemantic[i][j + 1] != '.')
                        || (i < Schemantic.Count - 1 && !Char.IsDigit(Schemantic[i + 1][j]) && Schemantic[i + 1][j] != '.')
                        || (i > 0 && j > 0 && !Char.IsDigit(Schemantic[i - 1][j - 1]) && Schemantic[i - 1][j - 1] != '.')
                        || (i > 0 && j < Schemantic[i].Length - 1 && !Char.IsDigit(Schemantic[i - 1][j + 1]) && Schemantic[i - 1][j + 1] != '.')
                        || (i < Schemantic.Count - 1 && j > 0 && !Char.IsDigit(Schemantic[i + 1][j - 1]) && Schemantic[i + 1][j - 1] != '.')
                        || (i < Schemantic.Count - 1 && j < Schemantic[i].Length - 1 && !Char.IsDigit(Schemantic[i + 1][j + 1]) && Schemantic[i + 1][j + 1] != '.'))
                    {
                        if (!numberAddedToSum)
                        {
                            EnginePartSum += numbersInLine[numberBeingChecked];
                            numberAddedToSum = true;

                            // Gear ratio check
                            if (j > 0) CheckGearRatio(i, j - 1, numbersInLine[numberBeingChecked]);
                            if (i > 0) CheckGearRatio(i - 1, j, numbersInLine[numberBeingChecked]);
                            if (j < Schemantic[i].Length - 1) CheckGearRatio(i, j + 1, numbersInLine[numberBeingChecked]);
                            if (i < Schemantic.Count - 1) CheckGearRatio(i + 1, j, numbersInLine[numberBeingChecked]);
                            if (i > 0 && j > 0) CheckGearRatio(i - 1, j - 1, numbersInLine[numberBeingChecked]);
                            if (i > 0 && j < Schemantic[i].Length - 1) CheckGearRatio(i - 1, j + 1, numbersInLine[numberBeingChecked]);
                            if (i < Schemantic.Count - 1 && j > 0) CheckGearRatio(i + 1, j - 1, numbersInLine[numberBeingChecked]);
                            if (i < Schemantic.Count - 1 && j < Schemantic[i].Length - 1) CheckGearRatio(i + 1, j + 1, numbersInLine[numberBeingChecked]);
                        }
                    }
                }
            }
        }

        return GearRationSum.ToString();
    }

    static void CheckGearRatio(int i, int j, int currentNumber)
    {
        if (Schemantic[i][j] == '*')
        {
            if (GearPeaces.TryGetValue((i, j), out int previousGearPart))
            {
                GearRationSum += previousGearPart * currentNumber;
                GearPeaces.Remove((i, j));
            }
            else
            {
                GearPeaces.Add((i, j), currentNumber);
            }
        }

    }

}