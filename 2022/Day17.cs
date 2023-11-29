using Shared;

namespace _2022;

public static class Day17
{
    private static List<int> Chamber = new();
    private static List<int> FallingRock = new();

    private static List<char> Wind = File.ReadLines(@".\Input\Day17.txt").First().ToList();
    private static List<List<int>> Rocks = new()
    {
        new List<int>{ 0b0011110 },
        new List<int>{ 0b0001000, 0b0011100, 0b0001000 },
        new List<int>{ 0b0011100, 0b0000100, 0b0000100 },
        new List<int>{ 0b0010000, 0b0010000, 0b0010000, 0b0010000 },
        new List<int>{ 0b0011000, 0b0011000 },
    };

    public static string Solve()
    {
        for (int round = 0; round < 2022; round++)
        {
            InitializeChamber();
            InitializeFallingRock();

            while(RockFalls())
            {

            }

            AddRockToChamber();
        }

        return Chamber.Count.ToString();
    }

    private static void InitializeChamber() => Chamber.AddRange(Enumerable.Repeat(0, 3 + Rocks.First().Count));

    private static void InitializeFallingRock() => FallingRock = Enumerable.Repeat(0, Chamber.FindIndex(e => e == 0) + 3).Concat(ListHelpers.ShiftToEnd(ref Rocks)).ToList();

    private static bool RockFalls()
    {
        // Wind pushes the rock
        char wind = ListHelpers.ShiftToEnd(ref Wind);
        if (wind == '>' && !FallingRock.Any(r => (r & 0b0000001) != 0))
        {
            var shiftedRock = FallingRock.Select(r => r >> 1).ToList();
            if (!WouldCollideWithExistingPile(shiftedRock))
            {
                FallingRock = shiftedRock;
            }
        }
        else if (wind == '<' && !FallingRock.Any(r => (r & 0b1000000) != 0))
        {
            var shiftedRock = FallingRock.Select(r => r << 1).ToList();
            if (!WouldCollideWithExistingPile(shiftedRock))
            {
                FallingRock = shiftedRock;
            }
        }

        // Check if rock hits the buttom of the chamber
        if (FallingRock[0] != 0)
            return false;

        // Rock falls
        var fallingRock = FallingRock.Skip(1).ToList();
        if (WouldCollideWithExistingPile(fallingRock))
        {
            return false;
        }

        // Rock is free to fall
        FallingRock = fallingRock;
        return true;
    }

    private static bool WouldCollideWithExistingPile(IEnumerable<int> rock) => Chamber.Zip(rock, (c, r) => c & r).Any(s => s != 0);

    private static void AddRockToChamber() => Chamber = Chamber.Zip(FallingRock, (c, r) => c | r)
        .Concat(Chamber.Skip(FallingRock.Count))
        .Where(c => c != 0).ToList();
}

