using Shared;

namespace _2022;

public static class Day16
{
    static string starterNode = "AA";

    public record Edge(string U, string V, int Weight);

    static Dictionary<string, int> nodeValues = new Dictionary<string, int>();
    static List<Edge> edges = new List<Edge>();

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
        HashSet<string> collectedNodes = nodeValues.Where(nv => nv.Value == 0).Select(nv => nv.Key).ToHashSet();

        // Part 1
        //SearchForBestSolutionFromMinute(0, 0, 30, starterNode, starterNode, collectedNodes);

        // Part 2
        SearchForBestSolutionFromMinute(0, 4, 4, starterNode, starterNode, collectedNodes);

        return maxValvePressure.ToString();
    }

    public static void SearchForBestSolutionFromMinute(int collectedValue, int humanMinute, int elephantMinute,
        string humanNode, string elephantNode, HashSet<string> collectedNodes)
    {
        if (collectedNodes.Count == nodeValues.Count)
            return;

        if (collectedNodes.Contains(humanNode))
        {
            foreach (var nextHumanEdge in edges.Where(e => e.U == humanNode && !collectedNodes.Contains(e.V) && humanMinute + e.Weight < 30))
            {
                // Elephant has lower role, it steps after the human and does not step on the same space
                foreach (var nextElephantEdge in edges.Where(e => e.U == elephantNode && !collectedNodes.Contains(e.V) && e.V != nextHumanEdge.V && elephantMinute + e.Weight < 30))
                {
                    SearchForBestSolutionFromMinute(collectedValue, humanMinute + nextHumanEdge.Weight, elephantMinute + nextElephantEdge.Weight,
                        nextHumanEdge.V, nextElephantEdge.V, collectedNodes);
                }

                // Elephant does not even has to step
                SearchForBestSolutionFromMinute(collectedValue, humanMinute + nextHumanEdge.Weight, elephantMinute,
                    nextHumanEdge.V, elephantNode, collectedNodes);
            }
        }
        else
        {
            bool humanCanOpen = !collectedNodes.Contains(humanNode);
            bool elephantCanOpen = !collectedNodes.Contains(elephantNode);

            int flowUntilEnd = (humanCanOpen ? nodeValues[humanNode] * (30 - humanMinute - 1) : 0)
                + (elephantCanOpen ? nodeValues[elephantNode] * (30 - elephantMinute - 1) : 0);

            maxValvePressure = Math.Max(maxValvePressure, collectedValue + flowUntilEnd);

            SearchForBestSolutionFromMinute(collectedValue + flowUntilEnd, humanMinute + (humanCanOpen ? 1 : 0), elephantMinute + (elephantCanOpen ? 1 : 0), 
                humanNode, elephantNode, new HashSet<string>(collectedNodes) { humanNode, elephantNode });
        }
    }
}