namespace _2022;

public static class Day14
{
    private const bool SECOND_PART = true;

    public static string Solve()
    {
        List<Coordinate> elements = new List<Coordinate>();

        foreach (var line in File.ReadLines(@".\Input\Day14.txt"))
        {
            var coordinateStrings = line.Split(" -> ");
            for (int i = 0; i < coordinateStrings.Length; i++)
            {
                var coordinateSplit = coordinateStrings[i].Split(",");
                Coordinate currentCoordinate = new Coordinate(int.Parse(coordinateSplit[0]), int.Parse(coordinateSplit[1]));

                if (i != 0)
                {
                    Coordinate previousCoordinate = elements[^1];

                    if (previousCoordinate.X != currentCoordinate.X)
                    {
                        if (previousCoordinate.X < currentCoordinate.X)
                        {
                            var xValues = Enumerable.Range(previousCoordinate.X + 1, currentCoordinate.X - previousCoordinate.X - 1);
                            elements.AddRange(xValues.Select(x => previousCoordinate with { X = x }));
                        }
                        else
                        {
                            var xValues = Enumerable.Range(currentCoordinate.X + 1, previousCoordinate.X - currentCoordinate.X - 1);
                            elements.AddRange(xValues.Select(x => currentCoordinate with { X = x }));
                        }
                    }

                    if (previousCoordinate.Y != currentCoordinate.Y)
                    {
                        if (previousCoordinate.Y < currentCoordinate.Y)
                        {
                            var yValues = Enumerable.Range(previousCoordinate.Y + 1, currentCoordinate.Y - previousCoordinate.Y - 1);
                            elements.AddRange(yValues.Select(y => previousCoordinate with { Y = y }));
                        }
                        else
                        {
                            var yValues = Enumerable.Range(currentCoordinate.Y + 1, previousCoordinate.Y - currentCoordinate.Y - 1); ;
                            elements.AddRange(yValues.Select(y => currentCoordinate with { Y = y }));
                        }
                    }
                }

                elements.Add(currentCoordinate);
            }
        }

        HashSet<Coordinate> elementSet = elements.ToHashSet();
        var lowestPoint = elementSet.Select(e => e.Y).Max();
        Coordinate fallingSand = new Coordinate(500, 0);
        int stoppedSandCount = 0;

        while (SECOND_PART ? true : fallingSand.Y < lowestPoint + 2)
        {
            if (SECOND_PART && fallingSand.Y == lowestPoint + 1)
            {
                elementSet.Add(fallingSand);
                stoppedSandCount++;
                fallingSand = new Coordinate(500, 0);
            }

            if (!elementSet.Contains(fallingSand with { Y = fallingSand.Y + 1 }))
            {
                fallingSand = fallingSand with { Y = fallingSand.Y + 1 };
                continue;
            }

            if (!elementSet.Contains(new Coordinate(X: fallingSand.X - 1, Y: fallingSand.Y + 1)))
            {
                fallingSand = new Coordinate(X: fallingSand.X - 1, Y: fallingSand.Y + 1);
                continue;
            }

            if (!elementSet.Contains(new Coordinate(X: fallingSand.X + 1, Y: fallingSand.Y + 1)))
            {
                fallingSand = new Coordinate(X: fallingSand.X + 1, Y: fallingSand.Y + 1);
                continue;
            }

            elementSet.Add(fallingSand);
            stoppedSandCount++;
            fallingSand = new Coordinate(500, 0);

            if (SECOND_PART && elementSet.Contains(fallingSand))
            {
                break;
            }
        }

        return stoppedSandCount.ToString();
    }

    private record Coordinate(int X, int Y);
}