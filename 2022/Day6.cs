namespace _2022;

public static class Day6
{
    public static string Solve()
    {
        int index;
        var line = File.ReadAllText(@".\Input\Day6.txt");

        for (index = 14; index < line.Length; index++)
        {
            var asd = line[(index - 14)..index];
            if (line[(index - 14)..index].ToHashSet().Count == 14)
                break;
        }

        return index.ToString();
    }
}