namespace _2022;

public static class Day5
{
    public static string Solve()
    {
        bool readingInitSection = true;
        List<List<char>> containers = new List<List<char>>();

        foreach (string line in File.ReadLines(@".\Input\Day5.txt"))
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                readingInitSection = false;
                containers.ForEach(c => c.Reverse());
                continue;
            }

            // Initializing the containers from starting data
            if (readingInitSection)
            {
                var elementsToPlace = line.Chunk(4).Select(s => s[1]).ToList();

                // Initialize containers
                if (!containers.Any())
                {
                    containers.AddRange(elementsToPlace.Select(_ => new List<char>()));
                }

                // Add elements to container
                for (int i = 0; i < elementsToPlace.Count; i++)
                {
                    if (!string.IsNullOrWhiteSpace(elementsToPlace[i].ToString()) && !int.TryParse(elementsToPlace[i].ToString(), out _))
                    {
                        containers[i].Add(elementsToPlace[i]);
                    }
                }
            }
            // Moving data
            else
            {
                var splits = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                var count = int.Parse(splits[1]);
                var from = int.Parse(splits[3]) - 1;
                var to = int.Parse(splits[5]) - 1;

                // CrateMover 9000
                //for (int i = 0; i < count; i++)
                //{
                //    var movedElement = containers[from].Last();
                //    containers[from].RemoveAt(containers[from].Count - 1);
                //    containers[to].Add(movedElement);
                //}

                // CrateMover 9001
                var movedElement = containers[from].TakeLast(count).ToList();
                containers[from].RemoveRange(containers[from].Count - count, count);
                containers[to].AddRange(movedElement);
            }
        }

        return string.Concat(containers.Select(c => c.Last()));
    }
}