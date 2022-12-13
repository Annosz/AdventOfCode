namespace _2022;

public static class Day13
{
    private const string Divider1 = "[[2]]";
    private const string Divider2 = "[[6]]";

    private static int _score;

    public static string Solve()
    {
        SignalComparer comparer = new();

        List<string> lines = File.ReadLines(@".\Input\Day13.txt").Where(l => !string.IsNullOrWhiteSpace(l)).ToList();
        //Replace 10s with values that will make ASCII comparison work
        lines = lines.Select(l => l.Replace("10", "X")).ToList();

        int lineIndex = 1;
        for (int i = 0; i < lines.Count; i += 2)
        {
            if (comparer.Compare(lines[i], lines[i + 1]) == -1)
            {
                _score += lineIndex;
            }

            lineIndex++;
        }

        lines.Add(Divider1);
        lines.Add(Divider2);
        lines = lines.OrderBy(l => l, comparer).ToList();

        int divider1Index = lines.IndexOf(Divider1) + 1;
        int divider2Index = lines.IndexOf(Divider2) + 1;

        return (divider1Index * divider2Index).ToString();
    }

    private class SignalComparer : IComparer<string>
    {
        public int Compare(string left, string right)
        {
            int leftIndex = 0;
            int rightIndex = 0;

            while (leftIndex < left.Length && rightIndex < right.Length)
            {
                // One element list
                if (left[leftIndex] == '[' && (int.TryParse(right[rightIndex].ToString(), out _) || right[rightIndex] == 'X'))
                {
                    right = right.Insert(rightIndex + 1, "]");
                    right = right.Insert(rightIndex, "[");
                    continue;
                }
                if (right[rightIndex] == '[' && (int.TryParse(left[leftIndex].ToString(), out _) || left[leftIndex] == 'X'))
                {
                    left = left.Insert(leftIndex + 1, "]");
                    left = left.Insert(leftIndex, "[");
                    continue;
                }

                // Regular whitespace characters
                if (left[leftIndex] == '[' && right[rightIndex] == '[')
                {
                    leftIndex++;
                    rightIndex++;
                    continue;
                }

                if (left[leftIndex] == ']' && right[rightIndex] == ']')
                {
                    leftIndex++;
                    rightIndex++;
                    continue;
                }

                if (left[leftIndex] == ',' && right[rightIndex] == ',')
                {
                    leftIndex++;
                    rightIndex++;
                    continue;
                }

                // Finishing a list
                if (left[leftIndex] == ']')
                {
                    return -1;
                }
                if (right[rightIndex] == ']')
                {
                    return 1;
                }

                // The basic comparison
                if (left[leftIndex] < right[rightIndex])
                {
                    return -1;
                }
                if (left[leftIndex] > right[rightIndex])
                {
                    return 1;
                }
                if (left[leftIndex] == right[rightIndex])
                {
                    leftIndex++;
                    rightIndex++;
                    continue;
                }
            }

            return 1;
        }
    }
}