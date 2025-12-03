namespace _2025;

public static class Day3
{
    public static string Solve()
    {
        const int N = 12;
        var batteryPower = 0L;

        foreach (var line in File.ReadLines(@$".\Input\{nameof(Day3)}.txt"))
        {
            var lineVoltage = new char[N];

            for (int i = 0; i < line.Length; i++)
            {
                for (int j = Math.Max(0, N - (line.Length - i)); j < N; j++)
                {
                    if (lineVoltage[j] < line[i])
                    {
                        lineVoltage[j] = line[i];
                        for (int k = j + 1; k < N; k++)
                        {
                            lineVoltage[k] = '0';
                        }

                        break;
                    }
                }
            }

            batteryPower += long.Parse(string.Concat(lineVoltage));
        }

        return $"Battery power: {batteryPower}";
    }
}