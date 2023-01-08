namespace AdventOfCode2022
{
    internal class _9
    {
        static bool showPath = false;
        static int maxMove = 400;
        static int[] Tx = new int[10], Ty = new int[10];
        static List<int[]> grid = new List<int[]>();
        static int xmax = 0, ymax = 0, xmin = 0, ymin = 0;
        static int[] visited = new int[10];
        public static void Run()
        {
            var input = File.ReadAllLines("input/9_1.txt");
            for (var i = 0; i < maxMove * 2; i++)
            {
                grid.Add(new int[maxMove * 2]);
                Array.Fill(grid[i], 0);
            }

            Array.Fill(Tx, maxMove);
            Array.Fill(Ty, maxMove);
            Array.Fill(visited, 0);
            grid[maxMove][maxMove] = 3;

            int xsize = 0, ysize = 0;
            foreach (var line in input)
            {
                var vals = line.Split(' ');
                var dir = vals[0];
                var amount = int.Parse(vals[1]);
                if (dir == "U")
                {
                    ysize -= amount; if (ysize < ymin) ymin = ysize;
                    Simulate(Tx[0], Ty[0] - amount);
                }
                if (dir == "D")
                {
                    ysize += amount; if (ysize > ymax) ymax = ysize;
                    Simulate(Tx[0], Ty[0] + amount);
                }
                if (dir == "R")
                {
                    xsize += amount; if (xsize > xmax) xmax = xsize;
                    Simulate(Tx[0] + amount, Ty[0]);
                }
                if (dir == "L")
                {
                    xsize -= amount; if (xsize < xmin) xmin = xsize;
                    Simulate(Tx[0] - amount, Ty[0]);
                }
            }

            for (var y = maxMove + ymin; y <= maxMove + ymax; y++)
            {
                for (var x = maxMove + xmin; x <= maxMove + xmax; x++)
                {
                    if (grid[y][x] == 3 || grid[y][x] == 1)
                    {
                        visited[1]++;
                    }
                    if (grid[y][x] == 3 || grid[y][x] == 2)
                    {
                        visited[9]++;
                    }
                }
            }

            Console.WriteLine(visited[1]);
            Console.WriteLine(visited[9]);
        }

        private static void Simulate(int x, int y)
        {
            if (x == Tx[0])
            {
                // vertical
                var dir = y - Ty[0];
                var step = dir > 0 ? 1 : -1;
                for (var i = Ty[0]; i != y; i += step)
                {
                    Ty[0] += step;
                    for (var j = 1; j <= 9; j++)
                    {
                        UpdatePos(Tx[j - 1], Ty[j - 1], j);
                    }

                    print();
                }
            }
            else
            {
                // horizontal
                var dir = x - Tx[0];
                var step = dir > 0 ? 1 : -1;
                for (var i = Tx[0]; i != x; i += step)
                {
                    Tx[0] += step;
                    for (var j = 1; j <= 9; j++)
                    {
                        UpdatePos(Tx[j - 1], Ty[j - 1], j);
                    }

                    print();
                }
            }
        }

        private static void UpdatePos(int x, int y, int idx)
        {
            if (Math.Abs(y - Ty[idx]) > 1)
            {
                if (x != Tx[idx])
                {
                    Tx[idx] += (x > Tx[idx] ? 1 : -1);
                }

                Ty[idx] += y > Ty[idx] ? 1 : -1;
            }
            else if (Math.Abs(x - Tx[idx]) > 1)
            {
                if (y != Ty[idx])
                {
                    Ty[idx] += (y > Ty[idx] ? 1 : -1);
                }

                Tx[idx] += x > Tx[idx] ? 1 : -1;
            }

            if (idx == 1 && (grid[Ty[idx]][Tx[idx]] == 0 || grid[Ty[idx]][Tx[idx]] == 2))
            {
                grid[Ty[idx]][Tx[idx]] += 1;
            }
            else if (idx == 9 && (grid[Ty[idx]][Tx[idx]] == 0 || grid[Ty[idx]][Tx[idx]] == 1))
            {
                grid[Ty[idx]][Tx[idx]] += 2;
            }
        }

        private static void print()
        {
            if (!showPath) return;
            Console.Clear();
            for (var y = maxMove + ymin; y <= maxMove + ymax; y++)
            {
                for (var x = maxMove + xmin; x <= maxMove + xmax; x++)
                {
                    var c = grid[y][x] > 0 ? "#" : ".";
                    for (var i = 9; i >= 0; i--)
                    {
                        if (x == Tx[i] && y == Ty[i])
                        {
                            c = i.ToString();
                        }
                    }
                    Console.Write(c + " ");
                }
                Console.WriteLine();
            }
            Thread.Sleep(100);
        }
    }
}
