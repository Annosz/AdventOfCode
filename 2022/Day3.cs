namespace _2022;

public static class Day3
{
    public static string Solve()
    {
        List<char> wrongItemTypes = new();

        Dictionary<char, int> itemTypeOccurrenceInGroup = new();
        List<char> groupBadges = new();
        int currentLine = 0;

        foreach (string line in File.ReadLines(@".\Input\Day3.txt"))
        {
            currentLine++;

            // First task
            var firstPocketTypes = line[0..(line.Length / 2)].ToHashSet();

            wrongItemTypes.Add(line[(line.Length / 2)..^0].First(c => firstPocketTypes.Contains(c)));

            // Second task
            foreach (char itemType in line.ToHashSet())
            {
                if (itemTypeOccurrenceInGroup.ContainsKey(itemType))
                    itemTypeOccurrenceInGroup[itemType]++;
                else itemTypeOccurrenceInGroup[itemType] = 1;
            }

            if (currentLine % 3 == 0)
            {
                groupBadges.Add(itemTypeOccurrenceInGroup.First(kv => kv.Value == 3).Key);
                itemTypeOccurrenceInGroup.Clear();
            }
        }

        //return wrongItems.Select(c => char.IsUpper(c) ? c - 38 : c - 96).Sum().ToString();
        return groupBadges.Select(c => char.IsUpper(c) ? c - 38 : c - 96).Sum().ToString();
    }
}