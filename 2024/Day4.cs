namespace _2024;

public static class Day4
{
    private static readonly List<(int, int)> Part1Directions =
    [
        (0, 1), (1, 0), (1, 1), (1, -1),
        (0, -1), (-1, 0), (-1, -1), (-1, 1)
    ];

    private static readonly List<List<(int, int)>> Part2Directions =
    [
        [(-1, -1), (-1, 1), (1, 1), (1, -1)],
        [(-1, 1), (1, 1), (1, -1), (-1, -1)],
        [(1, 1), (1, -1), (-1, -1), (-1, 1)],
        [(1, -1), (-1, -1), (-1, 1), (1, 1)],
    ];

    public static string Solve()
    {
        List<List<char>> wordSearch = [];
        wordSearch.AddRange(File.ReadLines(@".\Input\Day4.txt").Select(line => ("...." + line + "....").ToList()));

        var paddingLine = new string('.', wordSearch[0].Count);
        for (var i = 0; i < 4; i++)
        {
            wordSearch.Add(paddingLine.ToList());
            wordSearch.Insert(0, paddingLine.ToList());
        }

        var part1Sum = 0;
        var part2Sum = 0;
        for (var i = 4; i < wordSearch.Count - 4; i++)
        {
            for (var j = 4; j < wordSearch[i].Count - 4; j++)
            {
                part1Sum += Part1Search(wordSearch, i, j);

                part2Sum += Part2Search(wordSearch, i, j);
            }
        }

        return $"Part 1: {part1Sum}, Part 2: {part2Sum}";
    }

    private static int Part1Search(List<List<char>> wordSearch, int i, int j)
    {
        var sum = 0;
        foreach (var direction in Part1Directions)
        {
            var word = string.Empty;
            for (var k = 0; k < 4; k++)
            {
                word += wordSearch[i + k * direction.Item1][j + k * direction.Item2].ToString();
            }
            sum += word == "XMAS" ? 1 : 0;
        }
        return sum;
    }

    private static int Part2Search(List<List<char>> wordSearch, int i, int j)
    {
        var sum = 0;
        foreach (var direction in Part2Directions)
        {
            sum += wordSearch[i][j] == 'A'
                   && wordSearch[i + direction[0].Item1][j + direction[0].Item2] == 'M' && wordSearch[i + direction[1].Item1][j + direction[1].Item2] == 'M'
                   && wordSearch[i + direction[2].Item1][j + direction[2].Item2] == 'S' && wordSearch[i + direction[3].Item1][j + direction[3].Item2] == 'S' ? 1 : 0;
        }

        return sum;
    }
}