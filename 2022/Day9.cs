namespace _2022;

public static class Day9
{
    private const int RopeLength = 9;

    public static string Solve()
    {
        Coordinate headPosition = new Coordinate(0, 0);
        HashSet<Coordinate> visitedPositions = new() { new Coordinate(0, 0) };

        List<Coordinate> ropePiecePositions = new();
        for (int i = 1; i <= RopeLength; i++)
        {
            ropePiecePositions.Add(new Coordinate(0, 0));
        }

        foreach (string line in File.ReadLines(@".\Input\Day9.txt"))
        {
            string[] split = line.Split(" ");
            string direction = split[0];
            int times = int.Parse(split[1]);

            for (int i = 0; i < times; i++)
            {
                switch (direction)
                {
                    case "R":
                        headPosition = headPosition with { x = headPosition.x + 1 };
                        break;
                    case "L":
                        headPosition = headPosition with { x = headPosition.x - 1 };
                        break;
                    case "U":
                        headPosition = headPosition with { y = headPosition.y + 1 };
                        break;
                    case "D":
                        headPosition = headPosition with { y = headPosition.y - 1 };
                        break;
                }

                for (int piece = 0; piece < RopeLength; piece++)
                {
                    var previousPiecePosition = piece == 0 ? headPosition : ropePiecePositions[piece - 1];

                    if (Math.Abs(previousPiecePosition.x - ropePiecePositions[piece].x) > 1)
                    {
                        ropePiecePositions[piece] = previousPiecePosition with { x = (previousPiecePosition.x + ropePiecePositions[piece].x) / 2 };
                    }
                    if (Math.Abs(previousPiecePosition.y - ropePiecePositions[piece].y) > 1)
                    {
                        ropePiecePositions[piece] = previousPiecePosition with { y = (previousPiecePosition.y + ropePiecePositions[piece].y) / 2 };
                    }
                }

                visitedPositions.Add(ropePiecePositions[RopeLength - 1]);
            }
        }

        // Something's not right... good result for the example data, but slightly off for the final solution
        return visitedPositions.Count.ToString();
    }

    public record Coordinate(int x, int y);
}