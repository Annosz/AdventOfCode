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
            var paddedLine = "." + line + ".";
            Schemantic.Add(paddedLine);
        }
        string padding = new(Enumerable.Repeat('.', Schemantic.First().Length).ToArray());
        Schemantic.Insert(0, padding);
        Schemantic.Add(padding);

        for (int i = 1; i < Schemantic.Count - 1; i++)
        {
            var simpleLine = Regex.Matches(Schemantic[i], @"[\p{N}]+").Cast<Match>().Select(x => x.Value).ToArray();
            var numbersInLine = simpleLine.Where(s => int.TryParse(s, out _)).Select(s => int.Parse(s)).ToList();

            int numberBeingChecked = 0;
            bool numberAddedToSum = false;
            for (int j = 1; j < Schemantic[i].Length - 1; j++)
            {
                if (!Char.IsDigit(Schemantic[i][j]))
                {
                    if (Char.IsDigit(Schemantic[i][j - 1]))
                    {
                        numberBeingChecked++;
                        numberAddedToSum = false;
                    }
                    if (Schemantic[i][j] == '.')
                        continue;
                }

                if (Char.IsDigit(Schemantic[i][j]))
                {
                    if (IsSpecialCharater(i, j - 1) || IsSpecialCharater(i - 1, j) || IsSpecialCharater(i, j + 1) || IsSpecialCharater(i + 1, j)
                        || IsSpecialCharater(i - 1, j - 1) || IsSpecialCharater(i - 1, j + 1) || IsSpecialCharater(i + 1, j - 1) || IsSpecialCharater(i + 1, j + 1))
                    {
                        if (!numberAddedToSum)
                        {
                            numberAddedToSum = true;

                            // Part 1
                            EnginePartSum += numbersInLine[numberBeingChecked];

                            // Part 2
                            CheckGearRatio(i, j - 1, numbersInLine[numberBeingChecked]);
                            CheckGearRatio(i - 1, j, numbersInLine[numberBeingChecked]);
                            CheckGearRatio(i, j + 1, numbersInLine[numberBeingChecked]);
                            CheckGearRatio(i + 1, j, numbersInLine[numberBeingChecked]);
                            CheckGearRatio(i - 1, j - 1, numbersInLine[numberBeingChecked]);
                            CheckGearRatio(i - 1, j + 1, numbersInLine[numberBeingChecked]);
                            CheckGearRatio(i + 1, j - 1, numbersInLine[numberBeingChecked]);
                            CheckGearRatio(i + 1, j + 1, numbersInLine[numberBeingChecked]);
                        }
                    }
                }
            }
        }

        return EnginePartSum.ToString() + " " + GearRationSum.ToString();
    }

    private static bool IsSpecialCharater(int i, int j) => !Char.IsDigit(Schemantic[i][j]) && Schemantic[i][j] != '.';

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