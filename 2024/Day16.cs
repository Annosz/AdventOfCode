namespace _2024;

public static class Day16
{
    private static List<string> Map = [];
    private static readonly List<Position> Directions = [new(0, 1), new(1, 0), new(0, -1), new(-1, 0)];

    private static readonly Dictionary<(Position, Position), int> ScoresByDirection = [];
    private static readonly Dictionary<(Position, int), HashSet<Position>> TrailByScore = [];
    private static readonly PriorityQueue<Traversal, int> TraversalQueue = new();

    public static string Solve()
    {
        Map = File.ReadLines(@".\Input\Day16.txt").ToList();

        Position? endPosition = null;
        for (var i = 0; i < Map.Count; i++)
        {
            var j = Map[i].IndexOf('S');
            if (j != -1)
            {
                var nextTrail = new HashSet<Position> { new(i, j) };
                TraversalQueue.Enqueue(new Traversal(new Position(i, j), new Position(0, 1), nextTrail, 0), 0);
            }

            j = Map[i].IndexOf('E');
            if (j != -1)
            {
                endPosition = new Position(i, j);
            }
        }

        while (TraversalQueue.TryDequeue(out var currentPosition, out _))
        {
            // Keep track of the trail even if we reached this position with the same score already
            if (TrailByScore.ContainsKey((currentPosition.Position, currentPosition.Score)))
            {
                TrailByScore[(currentPosition.Position, currentPosition.Score)].UnionWith(currentPosition.Trail);
            }
            else
            {
                TrailByScore[(currentPosition.Position, currentPosition.Score)] = currentPosition.Trail;
            }

            // We do the path finding on every node, even if we reached it with a smaller score beforehand
            // This helps us find all possible routes AND helps with the tricky cases where turning would disturb the order
            if (!ScoresByDirection.ContainsKey((currentPosition.Position, currentPosition.Direction)) || ScoresByDirection[(currentPosition.Position, currentPosition.Direction)] > currentPosition.Score)
            {
                ScoresByDirection[(currentPosition.Position, currentPosition.Direction)] = currentPosition.Score;
            }

            foreach (var direction in Directions)
            {
                var nextPosition = new Position(currentPosition.Position.X + direction.X, currentPosition.Position.Y + direction.Y);
                if (Map[nextPosition.X][nextPosition.Y] == '#')
                {
                    continue;
                }

                var turnScore = currentPosition.Direction == direction ? 0 : 1000;
                if (!ScoresByDirection.ContainsKey((nextPosition, direction)))
                {
                    var nextTrail = new HashSet<Position> { nextPosition };
                    nextTrail.UnionWith(currentPosition.Trail);
                    TraversalQueue.Enqueue(new Traversal(nextPosition, direction, nextTrail, currentPosition.Score + turnScore + 1), currentPosition.Score + turnScore + 1);
                }
            }
        }

        var minimumScore = TrailByScore.Keys.Where(k => k.Item1 == endPosition!).Select(k => k.Item2).Min();
        var trailLength = TrailByScore[(endPosition!, minimumScore)].Count;
        return $"Part 1: {minimumScore}; Part 2: {trailLength}";
    }

    public record Position(int X, int Y);
    public record Traversal(Position Position, Position Direction, HashSet<Position> Trail, int Score);

}