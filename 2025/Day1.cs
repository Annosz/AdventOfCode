namespace _2025;

public static class Day1
{
    public static string Solve()
    {
        var totalZeros = 0;
        var dial = 50;

        foreach (var line in File.ReadLines(@".\Input\Day1.txt"))
        {
            var startsFromZero = dial == 0;

            var rotationCount = int.Parse(line[1..].ReplaceLineEndings());
            if (rotationCount >= 100)
            {
                totalZeros += rotationCount / 100;
                rotationCount %= 100;
            }

            switch (line[0])
            {
                case 'R':
                    dial += rotationCount;
                    break;
                case 'L':
                    dial -= rotationCount;
                    break;
                default:
                    throw new InvalidDataException("Not a valid instruction");
            }

            switch (dial)
            {
                case 0:
                    totalZeros++;
                    break;
                case >= 100:
                    totalZeros++;
                    dial -= 100;
                    break;
                case < 0:
                    totalZeros += startsFromZero ? 0 : 1;
                    dial += 100;
                    break;
            }
        }

        return totalZeros.ToString();
    }
}