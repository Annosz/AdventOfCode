namespace _2023;

public static class Day1
{


    public static string Solve()
    {
        int sum = 0;

        foreach (var line in File.ReadLines(@".\Input\Day1.txt"))
        {
            // Part 1
            //string cleanLine = line;

            string cleanLine = Part2DataCleaning(line);

            while (!char.IsNumber(cleanLine.First()))
            {
                cleanLine = new string(cleanLine.Skip(1).ToArray());
            }

            while (!char.IsNumber(cleanLine.Last()))
            {
                cleanLine = new string(cleanLine.SkipLast(1).ToArray());
            }

            sum += int.Parse(cleanLine.First().ToString() + cleanLine.Last().ToString());
        }

        return sum.ToString();
    }

    private static string Part2DataCleaning(string line)
    {
        line = line.Replace("one", "one1one");
        line = line.Replace("two", "two2two");
        line = line.Replace("three", "three3three");
        line = line.Replace("four", "four4four");
        line = line.Replace("five", "five5five");
        line = line.Replace("six", "six6six");
        line = line.Replace("seven", "seven7seven");
        line = line.Replace("eight", "eight8eight");
        line = line.Replace("nine", "nine9nine");
        return line;
    }
}