namespace _2023;

public static class Day10
{
    private static readonly Dictionary<Point, List<Point>> Map = new();
    private static readonly List<Point> Trail = new();
    private static readonly List<Point> InsideFields = new();
    private static readonly Dictionary<Point, char> Image = new();
    private static Point Start = new(0, 0);

    public static string Solve()
    {
        int lineNumber = 0;
        int charNumber = 0;
        foreach (var line in File.ReadLines(@".\Input\Day10.txt"))
        {
            charNumber = 0;
            foreach (char c in line)
            {
                List<Point> connectedPoints = new List<Point>();
                switch (c)
                {
                    case '|':
                        {
                            connectedPoints.Add(new Point(lineNumber - 1, charNumber));
                            connectedPoints.Add(new Point(lineNumber + 1, charNumber));
                            break;
                        }
                    case '-':
                        {
                            connectedPoints.Add(new Point(lineNumber, charNumber - 1));
                            connectedPoints.Add(new Point(lineNumber, charNumber + 1));
                            break;
                        }
                    case 'L':
                        {
                            connectedPoints.Add(new Point(lineNumber - 1, charNumber));
                            connectedPoints.Add(new Point(lineNumber, charNumber + 1));
                            break;
                        }
                    case 'J':
                        {
                            connectedPoints.Add(new Point(lineNumber - 1, charNumber));
                            connectedPoints.Add(new Point(lineNumber, charNumber - 1));
                            break;
                        }
                    case '7':
                        {
                            connectedPoints.Add(new Point(lineNumber + 1, charNumber));
                            connectedPoints.Add(new Point(lineNumber, charNumber - 1));
                            break;
                        }
                    case 'F':
                        {
                            connectedPoints.Add(new Point(lineNumber + 1, charNumber));
                            connectedPoints.Add(new Point(lineNumber, charNumber + 1));
                            break;
                        }
                    case 'S':
                        {
                            Start = new Point(lineNumber, charNumber);
                            charNumber++;
                            continue;
                        }
                    case '.':
                        {
                            charNumber++;
                            continue;
                        }
                }

                Map.Add(new Point(lineNumber, charNumber), connectedPoints);
                Image.Add(new Point(lineNumber, charNumber), c);
                charNumber++;
            }
            lineNumber++;
        }

        // The start point can either be the element at 0 or at 1... Did not find a way to determine it
        // Should check the drawn image of the tasks to determine which direction is the good for traversal
        List<Point> potentialLoopStarts = Map.Where(p => p.Value.Any(e => e == Start)).Select(p => p.Key).ToList();
        Point currentPoint = potentialLoopStarts[1];

        // Traversal and length counting
        Point previousPoint = Start;
        Trail.Add(currentPoint);
        int length = 1;
        while (currentPoint != Start)
        {
            Point nextNode = Map[currentPoint].First(p => p != previousPoint);
            previousPoint = currentPoint;
            currentPoint = nextNode;
            Trail.Add(currentPoint);
            length++;
        }

        // Magic think to see which field align to the left side of the pipe ~ enclosed areas
        for (int i = 1; i < Trail.Count; i++)
        {
            if (Trail[i - 1].X < Trail[i].X)
            {
                InsideFields.Add(Trail[i - 1] with { Y = Trail[i - 1].Y + 1 });
                InsideFields.Add(Trail[i] with { Y = Trail[i - 1].Y + 1 });
            }
            if (Trail[i - 1].X > Trail[i].X)
            {
                InsideFields.Add(Trail[i - 1] with { Y = Trail[i - 1].Y - 1 });
                InsideFields.Add(Trail[i] with { Y = Trail[i - 1].Y - 1 });
            }
            if (Trail[i - 1].Y < Trail[i].Y)
            {
                InsideFields.Add(Trail[i - 1] with { X = Trail[i - 1].X - 1 });
                InsideFields.Add(Trail[i] with { X = Trail[i - 1].X - 1 });
            }
            if (Trail[i - 1].Y > Trail[i].Y)
            {
                InsideFields.Add(Trail[i - 1] with { X = Trail[i - 1].X + 1 });
                InsideFields.Add(Trail[i] with { X = Trail[i - 1].X + 1 });
            }
        }

        // The pipe itself is not enclosed area
        InsideFields.RemoveAll(f => Trail.Contains(f));

        // Fill algorithm starting from the points that we determined previously
        List<Point> newInsideField = new();
        do
        {
            newInsideField = new List<Point>();
            foreach (Point insideField in InsideFields)
            {
                newInsideField.Add(insideField with { X = insideField.X + 1 });
                newInsideField.Add(insideField with { X = insideField.X - 1 });
                newInsideField.Add(insideField with { Y = insideField.Y + 1 });
                newInsideField.Add(insideField with { Y = insideField.Y - 1 });
            }
            newInsideField.RemoveAll(f => Trail.Contains(f));
            newInsideField.RemoveAll(f => InsideFields.Contains(f));
            InsideFields.AddRange(newInsideField);
        } while (newInsideField.Any());

        // If needed for debugging
        //DrawPipe(lineNumber, charNumber);

        return $"Part 1: {length / 2}; Part 2: {InsideFields.Distinct().Count()}";
    }

    private static void DrawPipe(int lineNumber, int charNumber)
    {
        for (int i = 0; i < lineNumber; i++)
        {
            for (int j = 0; j < charNumber; j++)
            {
                Console.Write(Trail.Contains(new Point(i, j)) ? "X" : InsideFields.Contains(new Point(i, j)) ? "." : " ");
            }
            Console.WriteLine();
        }
    }

    public record Point(int X, int Y);
}