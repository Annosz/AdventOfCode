using JetBrains.Annotations;
using Shared.AoC;

namespace _2025;

[UsedImplicitly]
public class Day6 : IAoCTask
{
    public string Solve()
    {
        List<List<char>> text = [];

        List<List<long>> numbers = [[]];
        List<char> operators = [];
        var sum = 0L;

        foreach (var line in File.ReadLines(@$".\Input\{GetType().Name}.txt"))
        {
            text.Add(line.ToList());
        }

        var equation = 0;
        for (var j = 0; j < text[0].Count; j++)
        {
            var number = string.Empty;
            for (var i = 0; i < text.Count; i++)
            {
                if (text[i][j] == ' ')
                {
                    continue;
                }
                if (text[i][j] == '+' || text[i][j] == '*')
                {
                    operators.Add(text[i][j]);
                    continue;
                }

                number += text[i][j];
            }

            if (number == string.Empty)
            {
                numbers.Add([]);
                equation++;
                continue;
            }

            numbers[equation].Add(long.Parse(number));
        }

        for (var i = 0; i < numbers.Count; i++)
        {
            switch (operators[i])
            {
                case '+':
                    sum += numbers[i].Sum();
                    break;
                case '*':
                    sum += numbers[i].Aggregate(1L, (acc, val) => acc * val);
                    break;
            }
        }

        return $"Sum: {sum}";
    }
}