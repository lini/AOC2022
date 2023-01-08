namespace AdventOfCode2022
{
    internal class _7
    {
        public static long sizeSum = 0;
        public class DirEntry
        {
            public string? Name;
            public long size = 0;
            public DirEntry? Parent;
            public Dictionary<string, long> Files = new Dictionary<string, long>();
            public List<DirEntry> Dirs = new List<DirEntry>();
            public DirEntry() { }
            public DirEntry(string name, DirEntry? parent) { this.Name = name; this.Parent = parent; }
        }

        public static long DirSize(DirEntry dir)
        {
            dir.size = dir.Files.Sum(f => f.Value);
            foreach (var childDir in dir.Dirs)
            {
                dir.size += DirSize(childDir);
            }
            if (dir.size <= 100000)
            {
                sizeSum += dir.size;
            }
            return dir.size;
        }
        public static void Run()
        {
            var input = File.ReadAllLines("input/7_1.txt");
            var disk = new DirEntry("/", null);
            var currentDir = disk;
            foreach (var line in input)
            {

                if (line.StartsWith("$ cd "))
                {
                    var newDir = line.Substring(5);
                    if (newDir == "..")
                    {
                        if (currentDir.Parent != null)
                        {
                            currentDir = currentDir.Parent;
                        }
                    }
                    else if (newDir == "/")
                    {
                        currentDir = disk;
                    }
                    else
                    {
                        var childDir = currentDir.Dirs.FirstOrDefault(d => d.Name == newDir);
                        if (childDir == null)
                        {
                            childDir = new DirEntry(newDir, currentDir);
                        }
                        currentDir = childDir;
                    }
                }
                else if (line == "$ ls")
                {
                    // ignore
                }
                else
                {
                    if (line.StartsWith("dir "))
                    {
                        currentDir.Dirs.Add(new DirEntry(line.Substring(4), currentDir));
                    }
                    else
                    {
                        var fileStr = line.Split(" ");
                        (long size, string name) = (int.Parse(fileStr[0]), fileStr[1]);
                        currentDir.Files.Add(name, size);
                        currentDir.size += size;
                    }
                }
            }

            sizeSum = 0;

            DirSize(disk);
            long freeSpace = 70000000 - disk.size;
            long neededSpace = 30000000 - freeSpace;
            long sizeToDelete = 30000000;
            Queue<DirEntry> q = new Queue<DirEntry>();
            q.Enqueue(disk);
            while (q.Count > 0)
            {
                var dir = q.Dequeue();
                if (dir.size < sizeToDelete && dir.size > neededSpace)
                {
                    sizeToDelete = dir.size;
                }
                foreach (var child in dir.Dirs)
                {
                    q.Enqueue(child);
                }
            }
            Console.WriteLine(sizeSum);
            Console.WriteLine(sizeToDelete);
        }
    }
}
