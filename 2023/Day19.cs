using System.Data;

namespace _2023;

public static class Day19
{
    private static readonly Dictionary<string, List<Instruction>> Instructions = new();
    private static readonly Dictionary<char, int> PropertyMap = new() { { 'x', 0 }, { 'm', 1 }, { 'a', 2 }, { 's', 3 } };
    private static readonly List<Part> Parts = new();

    public static string Solve()
    {
        bool readingInstructions = true;
        foreach (var line in File.ReadLines(@".\Input\Day19.txt"))
        {
            if (string.IsNullOrEmpty(line))
            {
                readingInstructions = false;
                continue;
            }

            if (readingInstructions)
            {
                var lineSplit = line.Split('{', StringSplitOptions.RemoveEmptyEntries);
                string name = lineSplit[0];
                lineSplit = lineSplit[1][0..^1].Split(",", StringSplitOptions.RemoveEmptyEntries);
                foreach (var element in lineSplit)
                {
                    if (!Instructions.ContainsKey(name))
                        Instructions.Add(name, new List<Instruction>());

                    if (!element.Contains(":"))
                    {
                        Instructions[name].Add(new Instruction('-', false, -1, element));
                        continue;
                    }

                    char property = element[0];
                    bool relation = element[1] == '<';
                    int number = int.Parse(element[2..].Split(":")[0]);
                    string destination = element[2..].Split(":")[1];

                    Instructions[name].Add(new Instruction(property, relation, number, destination));
                }
            }
            else
            {
                var lineSplit = line[1..^1].Split(",", StringSplitOptions.RemoveEmptyEntries);
                var numbers = lineSplit.Select(e => int.Parse(e.Split("=", StringSplitOptions.RemoveEmptyEntries)[1])).ToList();
                Parts.Add(new Part(numbers[0], numbers[1], numbers[2], numbers[3]));
            }
        }

        long sum = 0;
        string node = "in";
        List<Range> ranges = new() { new Range(1, 4000), new Range(1, 4000), new Range(1, 4000), new Range(1, 4000) };
        NavigateNode(node, ranges, ref sum);

        return sum.ToString();
    }

    private static void NavigateNode(string node, List<Range> ranges, ref long sum)
    {
        if (ranges[0].End <= ranges[0].Start || ranges[1].End <= ranges[1].Start || ranges[2].End <= ranges[2].Start || ranges[3].End <= ranges[3].Start)
        {
            return;
        }

        if (node == "A")
        {
            sum += (ranges[0].End - ranges[0].Start + (long)1) * (ranges[1].End - ranges[1].Start + (long)1) * (ranges[2].End - ranges[2].Start + (long)1) * (ranges[3].End - ranges[3].Start + (long)1);
            return;
        }

        if (node == "R")
        {
            return;
        }

        foreach (var instruction in Instructions[node])
        {
            if (instruction.Property == '-')
            {
                NavigateNode(instruction.Desitantion, new List<Range>(ranges), ref sum);
                continue;
            }

            if (instruction.LessThan)
            {
                Range currentPropertyRange = ranges[PropertyMap[instruction.Property]];

                var newRanges = new List<Range>(ranges);
                newRanges[PropertyMap[instruction.Property]] = new Range(currentPropertyRange.Start, Math.Min(currentPropertyRange.End, instruction.Number - 1));
                NavigateNode(instruction.Desitantion, newRanges, ref sum);

                ranges[PropertyMap[instruction.Property]] = new Range(Math.Max(currentPropertyRange.Start, instruction.Number), currentPropertyRange.End);
            }
            else
            {
                Range currentPropertyRange = ranges[PropertyMap[instruction.Property]];

                var newRanges = new List<Range>(ranges);
                newRanges[PropertyMap[instruction.Property]] = new Range(Math.Max(currentPropertyRange.Start, instruction.Number + 1), currentPropertyRange.End);
                NavigateNode(instruction.Desitantion, newRanges, ref sum);

                ranges[PropertyMap[instruction.Property]] = new Range(currentPropertyRange.Start, Math.Min(currentPropertyRange.End, instruction.Number));
            }
        }
    }

    private struct Range
    {
        public int Start { get; set; }
        public int End { get; set; }
        public Range(int start, int end)
        {
            Start = start;
            End = end;
        }
    }

    private record Part(int X, int M, int A, int S);
    private record Instruction(char Property, bool LessThan, int Number, string Desitantion);
}