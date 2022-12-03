namespace _2022
{
    public static class Day1
    {
        public static string Solve()
        {
            int actualElf = 0;
            int[] maximalElves = { 0, 0, 0 };

            foreach (string line in System.IO.File.ReadLines(@".\Input\Day1.txt"))
            {
                if (int.TryParse(line, out int value))
                {
                    actualElf += value;
                }
                else
                {
                    if (actualElf > maximalElves[0])
                    {
                        maximalElves[2] = maximalElves[1];
                        maximalElves[1] = maximalElves[0];
                        maximalElves[0] = actualElf;
                    }
                    else if (actualElf > maximalElves[1])
                    {
                        maximalElves[2] = maximalElves[1];
                        maximalElves[1] = actualElf;
                    }
                    else if (actualElf > maximalElves[2])
                    {
                        maximalElves[2] = actualElf;
                    }

                    actualElf = 0;
                }
            }

            return maximalElves.Sum().ToString();
        }
    }
}
