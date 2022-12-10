namespace _2022;

public static class Day10
{
    public static string Solve()
    {
        List<int> timeline = new List<int> { 1 };

        int t = 1;
        foreach (string line in File.ReadLines(@".\Input\Day10.txt"))
        {
            if (timeline.Count <= t)
            {
                timeline.Add(timeline[t - 1]);
            }

            if (line == "noop")
            {
                t++;
                continue;
            }

            timeline.Add(timeline[t]);
            timeline.Add(timeline[t] + int.Parse(line.Split(" ")[1]));
            t += 2;
        }

        int signalStrength = 0;
        for (int i = 20; i <= timeline.Count; i += 40)
        {
            signalStrength += i * timeline[i];
        }

        for (int i = 1; i <= 240; i++)
        {
            int drawnPixel = i % 40 - 1;
            int[] spite = { timeline[i] - 1, timeline[i], timeline[i] + 1 };
            Console.Write(spite.Contains(drawnPixel) ? "#" : ".");

            if (i % 40 == 0)
            {
                Console.WriteLine();
            }
        }

        Console.WriteLine();
        return signalStrength.ToString();
    }
}