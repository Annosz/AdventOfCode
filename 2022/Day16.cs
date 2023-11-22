using Shared;

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

        // Reduce graph to value nodes
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

        // Create complete graph
        List<Edge> newlyAddedEdges = new List<Edge>();
        do
        {
            newlyAddedEdges = new List<Edge>();

            foreach (var firstEdge in edges)
            {
                foreach (var secondEdge in edges)
                {
                    if (firstEdge.U != secondEdge.V && firstEdge.V == secondEdge.U && !edges.Any(e => e.U == firstEdge.U && e.V == secondEdge.V))
                    {
                        newlyAddedEdges.Add(new Edge(firstEdge.U, secondEdge.V, firstEdge.Weight + secondEdge.Weight));
                    }
                }
            }

            edges.AddRange(newlyAddedEdges);

        } while (newlyAddedEdges.Count > 0);

        //Initialize 0 values as already collected
        HashSet<string> collectedValues = nodeValues.Where(nv => nv.Value == 0).Select(nv => nv.Key).ToHashSet();
        SearchForBestSolutionFromMinute(0, 0, starterNode, collectedValues, new List<string>());

        foreach (string line in maxTrail)
            Console.WriteLine(line);

        return maxValvePressure.ToString();
    }
    public static void SearchForBestSolutionFromMinute(
        int currentValue, 
        int minute,
        string currentNode,
        HashSet<string> collectedValues,
        List<string> trail)
    {
        if (minute > 30 || collectedValues.Count == nodeValues.Count)
            return;

        if (collectedValues.Contains(currentNode))
        {
            GoToNextValve(currentValue, minute, currentNode, collectedValues, trail);
        }
        else
        {
            TurnOnValve(currentValue, minute, currentNode, collectedValues, trail);
        }
    }

    private static void TurnOnValve(int currentValue, int minute, string currentNode, HashSet<string> collectedValues, List<string> trail)
    {
        trail.Add($"Minute {minute}; Open valve {currentNode}, adding a {nodeValues[currentNode]} flow for {30 - minute} minutes (total {nodeValues[currentNode] * (30 - minute)})");
        collectedValues.Add(currentNode);

        int flowUntilEnd = nodeValues[currentNode] * (30 - minute - 1);
        if (currentValue + flowUntilEnd > maxValvePressure)
        {
            maxTrail = new List<string>(trail);
            maxValvePressure = currentValue + flowUntilEnd;
        }

        SearchForBestSolutionFromMinute(currentValue + flowUntilEnd, minute + 1, currentNode, collectedValues, trail);

        collectedValues.Remove(currentNode);
        trail.RemoveAt(trail.Count - 1);
    }

    private static void GoToNextValve(int currentValue, int minute, string currentNode, HashSet<string> collectedValues, List<string> trail)
    {
        foreach (var nextEdge in edges.Where(e => e.U == currentNode && !collectedValues.Contains(e.V)))
        {
            trail.Add($"Minute {minute}; Go to valve {nextEdge.V}");
            SearchForBestSolutionFromMinute(currentValue, minute + nextEdge.Weight, nextEdge.V, collectedValues, trail);
            trail.RemoveAt(trail.Count - 1);
        }
    }
}