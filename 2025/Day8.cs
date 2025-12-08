using JetBrains.Annotations;
using Shared.AoC;

namespace _2025;

[UsedImplicitly]
public class Day8 : IAoCTask
{
    public class JunctionBox(int x, int y, int z)
    {
        public readonly int X = x;
        public readonly int Y = y;
        public readonly int Z = z;

        public int Circuit { get; set; }

        public double DistanceFrom(JunctionBox other)
        {
            return Math.Sqrt(Math.Pow(X - other.X, 2) + Math.Pow(Y - other.Y, 2) + Math.Pow(Z - other.Z, 2));
        }
    }

    public string Solve()
    {
        List<JunctionBox> junctionBoxes = [];
        PriorityQueue<(JunctionBox, JunctionBox), double> priorityQueue = new();
        Dictionary<int, HashSet<JunctionBox>> circuitMapping = [];
        var circuitCount = 0;

        foreach (var line in File.ReadLines(@$".\Input\{GetType().Name}.txt"))
        {
            var coordinates = line.Split(',').Select(int.Parse).ToList();
            var newJunctionBox = new JunctionBox(coordinates[0], coordinates[1], coordinates[2]);

            foreach (var junctionBox in junctionBoxes)
            {
                priorityQueue.Enqueue((newJunctionBox, junctionBox), newJunctionBox.DistanceFrom(junctionBox));
            }

            junctionBoxes.Add(newJunctionBox);
        }

        var circuitProduct = 0;
        long wallDistance = 0;
        var round = 0;
        var allOneCircuit = false;
        while (!allOneCircuit)
        {
            if (!priorityQueue.TryDequeue(out var pair, out _))
                break;

            round++;

            var (junctionBox1, junctionBox2) = pair;

            if (junctionBox1.Circuit == 0 && junctionBox2.Circuit == 0)
            {
                circuitCount++;
                junctionBox1.Circuit = circuitCount;
                junctionBox2.Circuit = circuitCount;
                circuitMapping[circuitCount] = [junctionBox1, junctionBox2];
            }
            else if (junctionBox1.Circuit != 0 && junctionBox2.Circuit == 0)
            {
                junctionBox2.Circuit = junctionBox1.Circuit;
                circuitMapping[junctionBox1.Circuit].Add(junctionBox2);
            }
            else if (junctionBox1.Circuit == 0 && junctionBox2.Circuit != 0)
            {
                junctionBox1.Circuit = junctionBox2.Circuit;
                circuitMapping[junctionBox2.Circuit].Add(junctionBox1);
            }
            else if (junctionBox1.Circuit != junctionBox2.Circuit)
            {
                var oldCircuit = junctionBox2.Circuit;
                for (var x = 0; x < circuitMapping[oldCircuit].Count; x++)
                {
                    var jb = circuitMapping[oldCircuit].ElementAt(x);
                    jb.Circuit = junctionBox1.Circuit;
                    circuitMapping[junctionBox1.Circuit].Add(jb);
                }
                circuitMapping.Remove(oldCircuit);
            }

            if (round == 1000)
            {
                var topCircuits = circuitMapping.Select(c => c.Value.Count).OrderDescending().ToList();
                circuitProduct = topCircuits[0] * topCircuits[1] * topCircuits[2];
            }

            if (circuitMapping.Count == 1 && junctionBoxes.All(jb => jb.Circuit != 0))
            {
                allOneCircuit = true;
                wallDistance = (long)junctionBox1.X * junctionBox2.X;
            }
        }

        return $"Circuit Product: {circuitProduct}; Wall Distance: {wallDistance}";
    }
}