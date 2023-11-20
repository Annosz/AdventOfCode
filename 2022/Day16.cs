using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;

namespace _2022;

public static class Day16
{
    static string starterNode = "AA";

    public record Edge(string U, string V, int Weight);

    static Dictionary<string, int> nodeValues = new Dictionary<string, int>();
    static List<Edge> edges = new List<Edge>();

    static List<string> maxTrail;
    static int maxValvePressure = 0;

    public static string Solve()
    {
        // Read input
        foreach (var line in File.ReadLines(@".\Input\Day16.txt"))
        {
            string lineWorkingCopy = line.Remove(0, "Valve ".Length);
            string node = lineWorkingCopy.Substring(0, 2);

            lineWorkingCopy = lineWorkingCopy.Remove(0, " has flow rate=".Length + 2);
            var lineSplit = lineWorkingCopy.Split(";");
            int valveFlowValue = int.Parse(lineWorkingCopy.Substring(0, lineSplit[0].Length));

            nodeValues.Add(node, valveFlowValue);

            lineWorkingCopy = lineSplit[1];
            lineWorkingCopy = lineWorkingCopy.Contains(" tunnels lead to valves ")
                ? lineWorkingCopy.Remove(0, " tunnels lead to valves ".Length)
                : lineWorkingCopy.Remove(0, " tunnel leads to valve ".Length);
            lineSplit = lineWorkingCopy.Split(", ", StringSplitOptions.RemoveEmptyEntries);

            edges.AddRange(lineSplit.Select(v => new Edge(node, v, 1)));
        }

        // Reduce graph
        var zeroValueNodes = nodeValues.Where(nv => nv.Value == 0).Select(nv => nv.Key).ToList();
        zeroValueNodes.Remove(starterNode);
        foreach (var zeroValueNode in zeroValueNodes)
        {
            List<string> connectedNodes = new List<string>();
            connectedNodes.AddRange(edges.Where(e => e.U == zeroValueNode).Select(e => e.V));
            connectedNodes.AddRange(edges.Where(e => e.V == zeroValueNode).Select(e => e.U));
            foreach (var u in connectedNodes)
            {
                foreach (var v in connectedNodes)
                {
                    if (u != v && !edges.Any(e => e.U == u && e.V == v))
                    {
                        edges.Add(new Edge(u, v, edges.First(e => e.U == u && e.V == zeroValueNode).Weight + edges.First(e => e.U == zeroValueNode && e.V == v).Weight));
                    }
                }
            }

            edges.RemoveAll(e => e.U == zeroValueNode || e.V == zeroValueNode);
            nodeValues.Remove(zeroValueNode);
        }

        // Initialize 0 values as already collected
        HashSet<string> collectedValues = nodeValues.Where(nv => nv.Value == 0).Select(nv => nv.Key).ToHashSet();
        SearchForBestSolutionFromMinute(0, 1, "", starterNode, collectedValues, new List<string>());

        foreach(string line in maxTrail)
            Console.WriteLine(line);

        return maxValvePressure.ToString();
    }

    public static void SearchForBestSolutionFromMinute(
        int currentValue, 
        int minute,
        string previousNode,
        string currentNode,
        HashSet<string> collectedValues,
        List<string> trail)
    {
        if (minute > 30 || collectedValues.Count == nodeValues.Count)
            return;

        if (!collectedValues.Contains(currentNode))
        {
            trail.Add($"Minute {minute}; Open valve {currentNode}, adding a {nodeValues[currentNode]} flow for {30 - minute} minutes (total {nodeValues[currentNode] * (30 - minute)})");
            collectedValues.Add(currentNode);

            int flowUntilEnd = nodeValues[currentNode] * (30 - minute);
            if (currentValue + flowUntilEnd > maxValvePressure)
            {
                maxTrail = new List<string>(trail);
                maxValvePressure = currentValue + flowUntilEnd;
            }

            SearchForBestSolutionFromMinute(currentValue + flowUntilEnd, minute + 1, "", currentNode, collectedValues, trail);

            trail.RemoveAt(trail.Count - 1);
            collectedValues.Remove(currentNode);
        }

        foreach (var nextEdge in edges.Where(e => e.U == currentNode && e.V != previousNode))
        {
            if (collectedValues.Contains(nextEdge.V) && edges.Count(e => e.U == nextEdge.V) == 1)
                continue;

            trail.Add($"Minute {minute}; Go to valve {nextEdge.V}");
            SearchForBestSolutionFromMinute(currentValue, minute + nextEdge.Weight, currentNode, nextEdge.V, collectedValues, trail);
            trail.RemoveAt(trail.Count - 1);
        }
    }
}