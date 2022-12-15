namespace _2022;

public static class Day15
{
    public static string Solve()
    {
        List<List<char>> forbiddenMap = new List<List<char>>();
        List<(Coordinate, int)> sensors = new List<(Coordinate, int)>();
        HashSet<Coordinate> beacons = new HashSet<Coordinate>();

        int smallestX = int.MaxValue;
        int smallestY = int.MaxValue;
        int largestX = int.MinValue;
        int largestY = int.MinValue;
        int largestManhattan = int.MinValue;
        foreach (var line in File.ReadLines(@".\Input\Day15.txt"))
        {
            var lineWorkingCopy = line.Remove(0, "Sensor at x=".Length);
            var lineSplit = lineWorkingCopy.Split(',', 2);
            var sensorX = int.Parse(lineSplit[0]);
            smallestX = sensorX < smallestX ? sensorX : smallestX;
            largestX = sensorX > largestX ? sensorX : largestX;
            lineWorkingCopy = lineSplit[1];

            lineWorkingCopy = lineWorkingCopy.Remove(0, " y=".Length);
            lineSplit = lineWorkingCopy.Split(':', 2);
            var sensorY = int.Parse(lineSplit[0]);
            smallestY = sensorY < smallestY ? sensorY : smallestY;
            largestY = sensorY > largestY ? sensorY : largestY;
            lineWorkingCopy = lineSplit[1];


            lineWorkingCopy = lineWorkingCopy.Remove(0, " closest beacon is at x=".Length);
            lineSplit = lineWorkingCopy.Split(',', 2);
            var beaconX = int.Parse(lineSplit[0]);
            lineWorkingCopy = lineSplit[1];

            lineWorkingCopy = lineWorkingCopy.Remove(0, " y=".Length);
            var beaconY = int.Parse(lineWorkingCopy);

            var manhattanDistance = Math.Abs(sensorX - beaconX) + Math.Abs(sensorY - beaconY);
            largestManhattan = largestManhattan < manhattanDistance ? manhattanDistance : largestManhattan;
            sensors.Add((new(sensorX, sensorY), manhattanDistance));
            beacons.Add(new(beaconX, beaconY));
        }

        int offset = Math.Max(Math.Max(Math.Abs(smallestX - largestManhattan), Math.Abs(smallestY - largestManhattan)), largestManhattan);

        for (int i = 0; i < largestX + 2 * offset + 1; i++)
        {
            forbiddenMap.Add(Enumerable.Repeat('.', largestY + 2 * offset + 1).ToList());
        }

        foreach (var coordinate in beacons)
        {
            forbiddenMap[coordinate.X + offset][coordinate.Y + offset] = 'B';
        }

        foreach (var (coordinate, manhattan) in sensors)
        {
            forbiddenMap[coordinate.X + offset][coordinate.Y + offset] = 'S';

            for (int i = coordinate.X - manhattan; i <= coordinate.X + manhattan; i++)
            {
                for (int j = coordinate.Y - manhattan; j <= coordinate.Y + manhattan; j++)
                {
                    if (forbiddenMap[i + offset][j + offset] == '.'
                        && Math.Abs(coordinate.X - i) + Math.Abs(coordinate.Y - j) <= manhattan)
                    {
                        forbiddenMap[i + offset][j + offset] = '#';
                    }
                }
            }
        }

        Visualize(forbiddenMap);

        Console.WriteLine();
        Console.WriteLine(forbiddenMap.Select(l => l[10 + offset]).ToArray());

        return forbiddenMap.Select(l => l[10 + offset]).Count(c => c == '#').ToString();
    }

    private static void Visualize(List<List<char>> forbiddenMap)
    {
        for (int j = 0; j < forbiddenMap[0].Count; j++)
        {
            for (int i = 0; i < forbiddenMap.Count; i++)
            {
                Console.Write(forbiddenMap[i][j]);
            }

            Console.WriteLine();
        }
    }

    private record Coordinate(int X, int Y);
}