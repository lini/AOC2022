namespace AdventOfCode2022
{
    internal class _12
    {
        public static void Run()
        {
            var input = File.ReadAllLines("input/12_1.txt");

            int m = input.Length, n = input[0].Length;
            var weights = new List<int[]>();
            var q = new Queue<(int x, int y)>();
            (int x, int y) final = (0, 0);
            for (var i = 0; i < m; i++)
            {
                weights.Add(new int[n]);
                Array.Fill(weights[i], int.MaxValue);
                for (var j = 0; j < n; j++)
                {
                    if (input[i][j] == 'S')
                    {
                        q.Enqueue((i, j));
                        input[i] = input[i].Remove(j, 1).Insert(j, "a");
                        weights[i][j] = 0;
                    }

                    if (input[i][j] == 'E')
                    {
                        final = (i, j);
                        input[i] = input[i].Remove(j, 1).Insert(j, "z");
                    }
                }
            }

            while (q.TryDequeue(out var p))
            {
                var w = weights[p.x][p.y];
                if (p.x > 0 && weights[p.x - 1][p.y] > w + 1 && input[p.x - 1][p.y] <= input[p.x][p.y] + 1)
                {
                    weights[p.x - 1][p.y] = w + 1;
                    q.Enqueue((p.x - 1, p.y));
                }

                if (p.x < m - 1 && weights[p.x + 1][p.y] > w + 1 && input[p.x + 1][p.y] <= input[p.x][p.y] + 1)
                {
                    weights[p.x + 1][p.y] = w + 1;
                    q.Enqueue((p.x + 1, p.y));
                }

                if (p.y > 0 && weights[p.x][p.y - 1] > w + 1 && input[p.x][p.y - 1] <= input[p.x][p.y] + 1)
                {
                    weights[p.x][p.y - 1] = w + 1;
                    q.Enqueue((p.x, p.y - 1));
                }

                if (p.y < n - 1 && weights[p.x][p.y + 1] > w + 1 && input[p.x][p.y + 1] <= input[p.x][p.y] + 1)
                {
                    weights[p.x][p.y + 1] = w + 1;
                    q.Enqueue((p.x, p.y + 1));
                }
                // Print(input, weights);
            }

            int finalValue = weights[final.x][final.y];
            Console.WriteLine(finalValue);

            q.Enqueue(final);
            for (var i = 0; i < m; i++)
            {
                Array.Fill(weights[i], int.MaxValue);
            }
            weights[final.x][final.y] = 0;

            while (q.TryDequeue(out var p))
            {
                var w = weights[p.x][p.y];
                if (p.x > 0 && weights[p.x - 1][p.y] > w + 1 && input[p.x - 1][p.y] >= input[p.x][p.y] - 1)
                {
                    weights[p.x - 1][p.y] = w + 1;
                    q.Enqueue((p.x - 1, p.y));
                }

                if (p.x < m - 1 && weights[p.x + 1][p.y] > w + 1 && input[p.x + 1][p.y] >= input[p.x][p.y] - 1)
                {
                    weights[p.x + 1][p.y] = w + 1;
                    q.Enqueue((p.x + 1, p.y));
                }

                if (p.y > 0 && weights[p.x][p.y - 1] > w + 1 && input[p.x][p.y - 1] >= input[p.x][p.y] - 1)
                {
                    weights[p.x][p.y - 1] = w + 1;
                    q.Enqueue((p.x, p.y - 1));
                }

                if (p.y < n - 1 && weights[p.x][p.y + 1] > w + 1 && input[p.x][p.y + 1] >= input[p.x][p.y] - 1)
                {
                    weights[p.x][p.y + 1] = w + 1;
                    q.Enqueue((p.x, p.y + 1));
                }
                // Print(input, weights);
            }

            var minValue = int.MaxValue;
            for (var i = 0; i < input.Length; i++)
            {
                for (var j = 0; j < input[0].Length; j++)
                {
                    if (input[i][j] == 'a' && weights[i][j] < minValue)
                    {
                        minValue = weights[i][j];
                    }
                }
            }

            Console.WriteLine(minValue);
        }

        private static void Print(string[] input, List<int[]> weights)
        {
            Console.Clear();
            for (var i = 0; i < input.Length; i++)
            {
                for (var j = 0; j < input[0].Length; j++)
                {
                    Console.Write(weights[i][j] != int.MaxValue ? input[i][j].ToString() : '.');
                }
                Console.WriteLine();
            }

            Thread.Sleep(100);
        }
    }
}
