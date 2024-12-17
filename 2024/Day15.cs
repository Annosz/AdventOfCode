namespace _2024;

public static class Day15
{
    private static bool IsReadingMap = true;

    private static Robot Player;
    private static List<Element> Elements = [];
    private static string Movements;

    private static Dictionary<char, Point> Directions = new()
    {
        { '^', new Point(-1, 0) },
        { '<', new Point(0, -1) },
        { '>', new Point(0, 1) },
        { 'v', new Point(1, 0) }
    };

    public static string Solve()
    {
        int i = 0;
        foreach (var line in File.ReadLines(@$".\Input\Day15.txt"))
        {
            if (line == "")
            {
                IsReadingMap = false;
            }

            if (IsReadingMap)
            {
                int j = 0;
                foreach (var c in line)
                {
                    switch (c)
                    {
                        case '#':
                            Elements.Add(new Wall(new Point(i, j)));
                            break;
                        case 'O':
                            Elements.Add(new Box(new Point(i, j)));
                            break;
                        case '@':
                            Player = new Robot(new Point(i, j));
                            break;
                    }

                    j += 2;
                }
            }
            else
            {
                Movements += line;
            }

            i++;
        }

        foreach (var move in Movements)
        {
            Player.Move(Directions[move]);
        }

        return $"Part 2 result: {Elements.Sum(e => e.Score())}";
    }

    public record Point(int X, int Y);

    public class Element(Point point)
    {
        public Point Point { get; set; } = point;

        public virtual bool CanMove(Point direction)
        {
            var elementsToPush = GetElementsInDirection(direction);
            return !elementsToPush.Any() || elementsToPush.All(e => e.CanMove(direction));
        }

        public virtual void Move(Point direction)
        {
            if (!CanMove(direction))
            {
                return;
            }

            var elementsToPush = GetElementsInDirection(direction);
            foreach (var elementToPush in elementsToPush)
            {
                elementToPush.Move(direction);
            }

            Point = new Point(Point.X + direction.X, Point.Y + direction.Y);
        }

        protected virtual List<Element> GetElementsInDirection(Point direction) =>
            Elements.Where(e => e.Point != Point && (e.Point == new Point(Point.X + direction.X, Point.Y + direction.Y)
                || e.Point == new Point(Point.X + direction.X, Point.Y + direction.Y - 1) || e.Point == new Point(Point.X + direction.X, Point.Y + direction.Y + 1))).ToList();

        public virtual int Score() => Point.X * 100 + Point.Y;
    }

    public class Wall(Point point) : Element(point)
    {
        public override bool CanMove(Point direction) => false;

        public override int Score() => 0;
    }

    public class Box(Point point) : Element(point) { }

    public class Robot(Point point) : Element(point)
    {
        // The robot is not wide, so it does not push that many elements
        protected override List<Element> GetElementsInDirection(Point direction) =>
            Elements.Where(e => e.Point != Point
                && (e.Point == new Point(Point.X + direction.X, Point.Y + direction.Y) || e.Point == new Point(Point.X + direction.X, Point.Y + direction.Y - 1))).ToList();

        public override int Score() => 0;
    }
}