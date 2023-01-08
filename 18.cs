
using System.Drawing;

namespace AdventOfCode2022
{
    internal class _18
    {
        public static void Run()
        {
            var input = File.ReadAllLines("input/18_1.txt").Select<string, (int x, int y, int z)>(x =>
            {
                var split = x.Split(',');
                return (int.Parse(split[0]), int.Parse(split[1]), int.Parse(split[2]));
            }).ToList();
            int max = 0;
            for (var i = 0; i < input.Count; i++)
            {
                if (input[i].x > max) max = input[i].x;
                if (input[i].y > max) max = input[i].y;
                if (input[i].z > max) max = input[i].z;
            }
            max += 1;
            var space = new int[max, max, max];
            for (var i = 0; i < input.Count; i++)
            {
                space[input[i].x, input[i].y, input[i].z] = 1;
            }

            var sides = Touching(input, max);
            Console.WriteLine(sides);
            var innerSides = Inner(space, max);
            Console.WriteLine(sides - innerSides);
        }

        private static int Inner(int[,,] space, int max)
        {
            var added = new HashSet<(int x, int y, int z)>();
            var q = new Queue<(int x, int y, int z)>();
            int total = 0;
            for (var i = 0; i < max; i++)
            {
                for (var j = 0; j < max; j++)
                {
                    if (space[i, j, 0] == 0)
                    {
                        space[i, j, 0] = 2;
                        q.Enqueue((i, j, 0));
                    }
                    if (space[i, j, max - 1] == 0)
                    {
                        space[i, j, max - 1] = 2;
                        q.Enqueue((i, j, max - 1));
                    }

                    if (space[i, 0, j] == 0)
                    {
                        space[i, 0, j] = 2;
                        q.Enqueue((i, 0, j));
                    }
                    if (space[i, max - 1, j] == 0)
                    {
                        space[i, max - 1, j] = 2;
                        q.Enqueue((i, max - 1, j));
                    }

                    if (space[0, i, j] == 0)
                    {
                        space[0, i, j] = 2;
                        q.Enqueue((0, i, j));
                    }
                    if (space[max - 1, i, j] == 0)
                    {
                        space[max - 1, i, j] = 2;
                        q.Enqueue((max - 1, i, j));
                    }
                }
            }

            while (q.TryDequeue(out (int x, int y, int z) qp))
            {
                var qpSides = GetSides(qp.x, qp.y, qp.z, max);
                foreach (var side in qpSides)
                {
                    if (space[side.x, side.y, side.z] == 0)
                    {
                        space[side.x, side.y, side.z] = 2;
                        q.Enqueue((side.x, side.y, side.z));
                    }
                }
            }

            for (int i = 0; i < max; i++)
            {
                for (var j = 0; j < max; j++)
                {
                    for (var k = 0; k < max; k++)
                    {
                        if (space[i, j, k] == 0)
                        {
                            var sides = GetSides(i, j, k, max);

                            foreach (var side in sides)
                            {
                                if (space[side.x, side.y, side.z] == 1)
                                {
                                    total++;
                                }
                            }
                        }

                    }
                }
            }

            return total;
        }

        private static int Touching(List<(int x, int y, int z)> input, int max)
        {
            int total = 0;
            foreach (var cube in input)
            {
                total += 6;
                var sides = GetSides(cube.x, cube.y, cube.z, max);
                foreach (var side in sides)
                {
                    if (input.Contains(side))
                    {
                        total--;
                    }
                }
            }
            return total;
        }

        private static List<(int x, int y, int z)> GetSides(int x, int y, int z, int max)
        {
            var sides = new List<(int x, int y, int z)>();
            if (x < max - 1)
                sides.Add((x + 1, y, z));
            if (x > 0)
                sides.Add((x - 1, y, z));
            if (y < max - 1)
                sides.Add((x, y + 1, z));
            if (y > 0)
                sides.Add((x, y - 1, z));
            if (z < max - 1)
                sides.Add((x, y, z + 1));
            if (z > 0)
                sides.Add((x, y, z - 1));

            return sides;
        }
    }
}
