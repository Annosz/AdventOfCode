namespace _2024;

public static class Day9
{
    public static class SpaceType
    {
        public const string File = "file";
        public const string Free = "free";
    }
    public record FileSystemSpace(string Type, int Size, int Id);

    private static readonly List<FileSystemSpace> FileSystemMap = [];

    public static string Solve()
    {
        var isFile = true;
        var id = 0;
        foreach (var c in File.ReadAllText(@".\Input\Day9.txt"))
        {
            FileSystemMap.Add(new FileSystemSpace(isFile ? SpaceType.File : SpaceType.Free, int.Parse(c.ToString()), id));

            id += isFile ? 1 : 0;
            isFile = !isFile;
        }

        var j = FileSystemMap.Count - 1;
        while (j > 0)
        {
            if (FileSystemMap[j].Type == SpaceType.Free)
            {
                j--;
                continue;
            }

            for (var i = 0; i < j; i++)
            {
                if (FileSystemMap[i].Type == SpaceType.Free && FileSystemMap[i].Size >= FileSystemMap[j].Size)
                {
                    var movedFile = FileSystemMap[j];

                    // No need to concat freed up space, because we will never move files to the right
                    FileSystemMap[j] = FileSystemMap[j] with { Type = SpaceType.Free, Id = 0 };

                    // No need to delete 0 size free space, because it won't affect the checksum and no files can be moved to it
                    FileSystemMap[i] = FileSystemMap[i] with { Size = FileSystemMap[i].Size - movedFile.Size };

                    FileSystemMap.Insert(i, movedFile);

                    // Debugging log
                    //Console.WriteLine(string.Join("", ExpandFileSystem().Select(i => i != -1 ? i.ToString() : ".").ToList()));
                    break;
                }
            }

            j--;
        }

        var expandedFileSystem = ExpandFileSystem();

        var checksum = CalculateChecksum(expandedFileSystem);

        return checksum.ToString();
    }

    private static List<int> ExpandFileSystem()
    {
        List<int> expandedFileSystem = [];
        foreach (var space in FileSystemMap)
        {
            if (space.Type == SpaceType.File)
            {
                expandedFileSystem.AddRange(Enumerable.Repeat(space.Id, space.Size));
            }
            else if (space.Type == SpaceType.Free)
            {
                expandedFileSystem.AddRange(Enumerable.Repeat(-1, space.Size));
            }
        }

        return expandedFileSystem;
    }

    private static long CalculateChecksum(List<int> expandedFileSystem)
    {
        var checksum = 0L;
        for (var i = 0; i < expandedFileSystem.Count; i++)
        {
            checksum += expandedFileSystem[i] != -1 ? expandedFileSystem[i] * i : 0;
        }

        return checksum;
    }
}