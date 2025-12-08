using JetBrains.Annotations;
using Shared.AoC;

namespace _2025;

[UsedImplicitly]
public class Day7 : IAoCTask
{
    public string Solve()
    {
        List<List<char>> input = [];
        List<List<long>> timelines = [];
        var totalSplits = 0L;

        foreach (var line in File.ReadLines(@$".\Input\{GetType().Name}.txt"))
        {
            input.Add(line.ToList());
            timelines.Add(Enumerable.Repeat(0L, line.Length).ToList());
        }

        for (var i = 0; i < input[0].Count; i++)
        {
            if (input[0][i] == 'S')
            {
                timelines[0][i] = 1;
            }
        }

        for (var i = 1; i < input.Count; i++)
        {
            for (var j = 0; j < input[i].Count; j++)
            {
                if ((input[i][j] == '.' || input[i][j] == '|') && (input[i - 1][j] == '|' || input[i - 1][j] == 'S'))
                {
                    input[i][j] = '|';
                    timelines[i][j] += timelines[i - 1][j];
                }

                if (input[i][j] == '^' && (input[i - 1][j] == '|' || input[i - 1][j] == 'S'))
                {
                    input[i][j - 1] = '|';
                    input[i][j + 1] = '|';
                    timelines[i][j - 1] += timelines[i - 1][j];
                    timelines[i][j + 1] += timelines[i - 1][j];
                    totalSplits++;
                }
            }
        }

        // Write out the final state for visualization
        //for (var i = 0; i < input.Count; i++)
        //{
        //    Console.WriteLine(string.Join("", input[i]) + "    " + string.Join("", timelines[i]));
        //}

        return $"Total splits: {totalSplits}; Timelines: {timelines.Last().Sum()}";
    }
}