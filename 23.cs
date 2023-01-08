namespace AdventOfCode2022
{
    internal class _23
    {
        static readonly (int x1, int y1, int x2, int y2, int x3, int y3)[] dirs = new (int x1, int y1, int x2, int y2, int x3, int y3)[4]
        {
            (-1, -1,
             -1,  0,
             -1,  1), // N
            ( 1, -1,
              1,  0,
              1,  1), // S
            (-1, -1,
              0, -1,
              1, -1), // W
            (-1,  1,
              0,  1,
              1,  1)  // E
        };
        static readonly (int x, int y)[] neighbours = new (int x, int y)[8]
        {
            (-1, -1),
            (-1,  0),
            (-1,  1),
            ( 0, -1),
            ( 0,  1),
            ( 1, -1),
            ( 1,  0),
            ( 1,  1)
        };
        internal class Elf
        {
            public int x;
            public int y;
            public int dirChoice = 0;
            public Elf(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }
        static readonly List<Elf> elves = new List<Elf>();
        public static void Run()
        {
            var input = File.ReadAllLines("input/23_1.txt");
            for (var i = 0; i < input.Length; i++)
            {
                for (var j = 0; j < input[0].Length; j++)
                {
                    if (input[i][j] == '#')
                    {
                        var e = new Elf(i + 500, j + 500);
                        elves.Add(e);
                    }
                }
            }

            var round = 0;
            var dir = 0;
            int minX, maxX, minY, maxY;
            var newPos = new Dictionary<(int x, int y), int>();
            while (newPos.Count != 0 || round < 10)
            {
                newPos.Clear();
                minX = int.MaxValue;
                maxX = 0;
                minY = int.MaxValue;
                maxY = 0;
                for (var i = 0; i < elves.Count; i++)
                {
                    var e = elves[i];
                    if (e.x > maxX) maxX = e.x;
                    if (e.x < minX) minX = e.x;
                    if (e.y > maxY) maxY = e.y;
                    if (e.y < minY) minY = e.y;
                }

                var grid = new List<char[]>();
                for (var i = 0; i < maxX - minX + 1; i++)
                {
                    var c = new char[maxY - minY + 1];
                    Array.Fill(c, ' ');
                    grid.Add(c);
                }

                for (var i = 0; i < elves.Count; i++)
                {
                    var e = elves[i];
                    grid[e.x - minX][e.y - minY] = '#';
                }

                for (var i = 0; i < elves.Count; i++)
                {
                    var e = elves[i];
                    var alone = true;
                    for (var j = 0; j < neighbours.Length; j++)
                    {
                        int nx = neighbours[j].x, ny = neighbours[j].y;
                        if (
                            e.x - minX + nx >= 0 && e.y - minY + ny >= 0 &&
                            e.x - minX + nx < grid.Count && e.y - minY + ny < grid[0].Length &&
                            grid[e.x - minX + nx][e.y - minY + ny] == '#')
                        {
                            alone = false;
                            break;
                        }
                    }
                    if (alone)
                    {
                        continue;
                    }

                    var foundDir = 0;
                    while (foundDir < dirs.Length)
                    {
                        var pos = dirs[(foundDir + dir) % 4];
                        if ((e.x - minX + pos.x1 < 0 || e.y - minY + pos.y1 < 0 || e.x - minX + pos.x1 >= grid.Count || e.y - minY + pos.y1 >= grid[0].Length || grid[e.x - minX + pos.x1][e.y - minY + pos.y1] == ' ') &&
                            (e.x - minX + pos.x2 < 0 || e.y - minY + pos.y2 < 0 || e.x - minX + pos.x2 >= grid.Count || e.y - minY + pos.y2 >= grid[0].Length || grid[e.x - minX + pos.x2][e.y - minY + pos.y2] == ' ') &&
                            (e.x - minX + pos.x3 < 0 || e.y - minY + pos.y3 < 0 || e.x - minX + pos.x3 >= grid.Count || e.y - minY + pos.y3 >= grid[0].Length || grid[e.x - minX + pos.x3][e.y - minY + pos.y3] == ' '))
                        {
                            var ch = (e.x + pos.x2, e.y + pos.y2);
                            if (newPos.ContainsKey(ch))
                            {
                                // conflict
                                newPos[ch] = -1;
                            }
                            else
                            {
                                newPos.Add(ch, i);
                            }

                            break;
                        }
                        foundDir++;
                    }
                }

                foreach (var p in newPos.Keys)
                {
                    var e = newPos[p];
                    if (e >= 0)
                    {
                        elves[e].x = p.x;
                        elves[e].y = p.y;
                    }
                }

                if (round == 10)
                {
                    CalcFirst();
                }
                round++;
                dir = (dir + 1) % 4;
            }

            Console.WriteLine(round);
        }

        private static void CalcFirst()
        {
            int minX = int.MaxValue;
            int maxX = 0;
            int minY = int.MaxValue;
            int maxY = 0;
            for (var i = 0; i < elves.Count; i++)
            {
                var e = elves[i];
                if (e.x > maxX) maxX = e.x;
                if (e.x < minX) minX = e.x;
                if (e.y > maxY) maxY = e.y;
                if (e.y < minY) minY = e.y;
            }

            var result = ((maxY - minY + 1) * (maxX - minX + 1)) - elves.Count;

            var grid2 = new List<char[]>();
            for (var i = 0; i < maxX - minX + 1; i++)
            {
                var c = new char[maxY - minY + 1];
                Array.Fill(c, ' ');
                grid2.Add(c);
            }

            for (var i = 0; i < elves.Count; i++)
            {
                var e = elves[i];
                grid2[e.x - minX][e.y - minY] = '#';
            }
            Console.WriteLine(result);
        }

        private static void Print(List<char[]> grid)
        {
            Console.Clear();
            for (var i = 0; i < grid.Count; i++)
            {
                for (var j = 0; j < grid[i].Length; j++)
                {
                    Console.Write(grid[i][j] == ' ' ? "." : "#");
                }
                Console.WriteLine();
            }
            Console.ReadLine();
        }
    }
}
