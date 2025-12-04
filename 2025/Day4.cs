using JetBrains.Annotations;
using Shared.AoC;

namespace _2025;

[UsedImplicitly]
public class Day4 : IAoCTask
{
    public string Solve()
    {
        List<List<char>> map = [];

        var input = File.ReadLines(@$".\Input\{GetType().Name}.txt").ToList();
        map.Add(Enumerable.Repeat('.', input[0].Length + 2).ToList());
        map.AddRange(input.Select(line => $".{line}.".ToList()));
        map.Add(Enumerable.Repeat('.', input[0].Length + 2).ToList());

        var removedPapers = 0;
        int newlyRemovedPapers;
        do
        {
            newlyRemovedPapers = 0;
            for (var i = 1; i < map.Count; i++)
            {
                for (var j = 1; j < map[i].Count; j++)
                {
                    if (map[i][j] != '@')
                    {
                        continue;
                    }

                    var neighbourPapers = 0;
                    for (var di = -1; di <= 1; di++)
                    {
                        for (var dj = -1; dj <= 1; dj++)
                        {
                            if (map[i + di][j + dj] == '@' && !(di == 0 && dj == 0))
                            {
                                neighbourPapers++;
                            }
                        }
                    }
                    if (neighbourPapers < 4)
                    {
                        map[i][j] = '.';
                        newlyRemovedPapers++;
                    }
                }
            }
            removedPapers += newlyRemovedPapers;
        } while (newlyRemovedPapers > 0);


        return $"Removed papers: {removedPapers}";
    }
}