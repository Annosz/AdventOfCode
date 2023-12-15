namespace _2023;

public static class Day15
{
    private static List<string> Texts = new();
    private static Dictionary<int, List<Lens>> Boxes = new();

    public static string Solve()
    {
        // Part 1 one-liner
        Console.WriteLine($"Part 1 one-liner solution: {File.ReadLines(@".\Input\Day15.txt").First().Split(',').Select(CalculateHash).Sum()}");

        Texts = File.ReadLines(@".\Input\Day15.txt").First().Split(',').ToList();

        for (int i = 0; i <= 255; i++)
            Boxes[i] = new List<Lens>();

        foreach (string text in Texts)
        {
            var textParts = text.Split(new[] { '=', '-' }, StringSplitOptions.RemoveEmptyEntries);
            var boxNumber = CalculateHash(textParts[0]);
            if (textParts.Length == 2)
                if (Boxes[boxNumber].Any(l => l.Label == textParts[0]))
                    Boxes[boxNumber].First(l => l.Label == textParts[0]).FocalStrength = int.Parse(textParts[1]);
                else
                    Boxes[boxNumber].Add(new Lens(textParts[0], int.Parse(textParts[1])));
            else
                Boxes[boxNumber].RemoveAll(l => l.Label == textParts[0]);
        }

        long boxSum = 0;
        for (int i = 0; i <= 255; i++)
            boxSum += Boxes[i].Any() ? Boxes[i].Select((e, j) => (i + 1) * (j + 1) * e.FocalStrength).Sum() : 0;

        return boxSum.ToString();
    }

    private static int CalculateHash(string text) => text.Aggregate(0, (sum, e) => (sum + e) * 17 % 256);

    private record Lens(string Label, int FocalStrength)
    {
        public int FocalStrength = FocalStrength;
    };
}