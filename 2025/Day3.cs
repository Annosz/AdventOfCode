using JetBrains.Annotations;
using Shared.AoC;

namespace _2025;

[UsedImplicitly]
public class Day3 : IAoCTask
{
    private const int N = 12;

    public string Solve()
    {
        var batteryPower = 0L;

        foreach (var line in File.ReadLines(@$".\Input\{GetType().Name}.txt"))
        {
            var lineVoltage = new char[N];

            for (var i = 0; i < line.Length; i++)
            {
                for (var j = Math.Max(0, N - (line.Length - i)); j < N; j++)
                {
                    if (lineVoltage[j] < line[i])
                    {
                        lineVoltage[j] = line[i];
                        for (var k = j + 1; k < N; k++)
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