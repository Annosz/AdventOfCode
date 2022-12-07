namespace _2022;

public class Day7
{
    public static string Solve()
    {
        Node root = new(null, new List<Node>(), "/", false, 0);
        List<Node> allNodes = new List<Node> { root };
        Node currentNode = root;

        // Traverse directory
        foreach (string line in File.ReadLines(@".\Input\Day7.txt"))
        {
            if (line == "$ cd /")
            {
                currentNode = root;
                continue;
            }

            if (line.StartsWith("$ cd "))
            {
                var directoryName = line.Replace("$ cd ", "");

                if (directoryName == "..")
                {
                    if (currentNode.Parent is not null)
                    {
                        currentNode = currentNode.Parent;
                    }
                }
                else
                {
                    currentNode = currentNode.Children.First(n => n.Name == directoryName);
                }
                continue;
            }

            if (line.StartsWith("$ ls"))
            {
                continue;
            }

            if (!line.StartsWith("$"))
            {
                if (line.StartsWith("dir "))
                {
                    var directoryName = line.Replace("dir ", "");
                    var newNode = new Node(currentNode, new List<Node>(), directoryName, false, 0);
                    currentNode.Children.Add(newNode);
                    allNodes.Add(newNode);
                }
                else
                {
                    var split = line.Split(" ");
                    var newNode = new Node(currentNode, null, split[1], true, int.Parse(split[0]));
                    currentNode.Children.Add(newNode);
                    allNodes.Add(newNode);
                }

                continue;
            }
        }

        // Calculate sizes
        for (int i = allNodes.Count - 1; i >= 0; i--)
        {
            if (!allNodes[i].IsFile)
            {
                allNodes[i].Size = allNodes[i].Children.Sum(n => n.Size);
            }
        }

        int freeSpace = 70000000 - root.Size;
        int spaceToFreeUp = 30000000 - freeSpace;

        return allNodes.Where(n => !n.IsFile && n.Size > spaceToFreeUp).Min(n => n.Size).ToString();
    }

    private record Node(Node? Parent, List<Node> Children, string Name, bool IsFile, int Size)
    {
        public int Size { get; set; } = Size;
    };
}