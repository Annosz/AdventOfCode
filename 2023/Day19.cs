using System.Data;

namespace _2023;

public static class Day19
{
    private static readonly List<Part> Parts = new List<Part>();
    private static readonly Dictionary<string, List<Instruction>> Instructions = new Dictionary<string, List<Instruction>>();

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
        foreach (var part in Parts)
        {
            string station = "in";
            string nextStation = "";
            while (true)
            {
                foreach (var instruction in Instructions[station])
                {
                    if (instruction.Property == '-')
                    {
                        nextStation = instruction.Desitantion;
                        break;
                    }

                    int value = (int)part.GetType().GetProperty(char.ToUpper(instruction.Property).ToString()).GetValue(part, null);
                    if (instruction.Relation ? value < instruction.Number : value > instruction.Number)
                    {
                        nextStation = instruction.Desitantion;
                        break;
                    }
                }

                if (nextStation == "A")
                {
                    sum += part.X + part.M + part.A + part.S;
                    break;
                }
                if (nextStation == "R")
                {
                    break;
                }

                station = nextStation;
                nextStation = "";
            }
        }

        return sum.ToString();
    }

    private record Part(int X, int M, int A, int S);
    private record Instruction(char Property, bool Relation, int Number, string Desitantion)
    {

    }
}