namespace _2024;

public static class Day1
{
    public static string Solve()
    {
        List<int> listA = [];
        List<int> listB = [];

        foreach (var line in File.ReadLines(@".\Input\Day1.txt"))
        {
            var numbers = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            listA.Add(int.Parse(numbers[0]));
            listB.Add(int.Parse(numbers[1]));
        }

        listA.Sort();
        listB.Sort();

        var part1Sum = listA.Zip(listB, (a, b) => Math.Abs(a - b)).Sum();

        long part2Sum = listA.Aggregate<int, long>(0, (current, num) => current + listB.Count(b => b == num) * num);

        return part2Sum.ToString();
    }
}