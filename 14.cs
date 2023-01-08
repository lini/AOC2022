namespace AdventOfCode2022
{
    internal class _14
    {
        static int minX = 0, minY = int.MaxValue, maxX = 1000, maxY = 0;
        static List<int[]> cave = new List<int[]>();
        static bool firstPart = true;
        public static void Run()
        {
            var input = File.ReadAllLines("input/14_1.txt");
            foreach (var line in input)
            {
                var points = line.Split(" -> ");
                foreach (var point in points)
                {
                    var coords = point.Split(',');
                    (int x, int y) = (int.Parse(coords[0]), int.Parse(coords[1]));
                    if (x < minX) minX = x;
                    if (y < minY) minY = y;
                    if (x > maxX) maxX = x;
                    if (y > maxY) maxY = y;
                }
            }

            for (var x = minX; x <= maxX; x++)
            {
                cave.Add(new int[maxY + 3]);
                Array.Fill(cave[cave.Count - 1], 0);
                cave[cave.Count - 1][maxY + 2] = 1;
            }


            foreach (var line in input)
            {

                var points = line.Split(" -> ");
                int px = -1, py = -1;
                foreach (var point in points)
                {
                    var coords = point.Split(',');
                    (int x, int y) = (int.Parse(coords[0]), int.Parse(coords[1]));
                    x = x - minX;
                    if (px != -1)
                    {
                        if (px == x)
                        {
                            var step = py > y ? -1 : 1;
                            for (var j = py; j != y; j += step)
                            {
                                cave[x][j] = 1;
                            }
                            cave[x][y] = 1;

                        }
                        else
                        {
                            var step = px > x ? -1 : 1;
                            for (var j = px; j != x; j += step)
                            {
                                cave[j][y] = 1;
                            }
                            cave[x][y] = 1;
                        }
                    }
                    px = x;
                    py = y;
                }
            }
            var sands = 1;
            while (DropSand())
            {
                // Print();
                sands++;
            }

            Console.WriteLine(sands - 1);

            while (DropSand())
            {
                sands++;
            }

            Console.WriteLine(sands);
        }

        private static bool DropSand()
        {
            int x = 500 - minX, y = 0;
            while (++y < cave[0].Length)
            {
                if (cave[x][y] == 0)
                {
                    continue;
                }
                if (x == 0)
                {
                    y = cave[0].Length;
                    break;
                }
                else if (cave[x - 1][y] == 0)
                {
                    x--;
                    continue;
                }
                if (x == cave.Count - 1)
                {
                    y = cave[0].Length;
                    break;
                }
                else if (cave[x + 1][y] == 0)
                {
                    x++;
                    continue;
                }
                else
                {
                    break;
                }
            }
            if (y == cave[0].Length - 1 && firstPart)
            {
                firstPart = false;
                return false;
            }
            if (y == 1 || y == cave[0].Length)
            {
                return false;
            }
            else
            {
                cave[x][y - 1] = 2;
                return true;
            }
        }

        public static void Print()
        {
            Console.Clear();
            for (var y = 0; y < cave[0].Length; y++)
            {
                Console.WriteLine();
                for (var x = 0; x < cave.Count; x++)
                {
                    char p;
                    switch (cave[x][y])
                    {
                        case 0:
                            p = '.'; break;

                        case 1:
                            p = '#'; break;
                        case 2:
                            p = 'o'; break;
                        default:
                            p = ' '; break;
                    }
                    if (y == 0 && x + minX == 500)
                    {
                        p = 'X';
                    }
                    Console.Write(p);
                }
            }
            Console.ReadKey();
        }
    }
}
