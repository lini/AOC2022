namespace AdventOfCode2022
{
    internal class _24
    {
        public static (int x, int y)[] next = new (int x, int y)[5]
        {
            (1, 0),
            (0, 1),
            (-1, 0),
            (0, -1),
            (0, 0)
        };

        public class State
        {
            public List<int[]> grid;
            public int x, y;
            public int moves;

            public State(int newX, int newY, List<int[]> newGrid, int newMoves)
            {
                this.x = newX;
                this.y = newY;
                this.grid = newGrid;
                this.moves = newMoves;
            }
        }

        public static void Run()
        {
            var input = File.ReadAllLines("input/24_1.txt").Select(s => s.ToCharArray()).ToList();
            int initialY = 0, endY = 0;
            for (var i = 0; i < input[0].Length; i++)
            {
                if (input[0][i] == '.')
                {
                    initialY = i;
                }
                if (input[input.Count - 1][i] == '.')
                {
                    endY = i;
                }
            }

            var grid = new List<int[]>();
            for (var i = 0; i < input.Count; i++)
            {
                grid.Add(new int[input[i].Length]);
                for (var j = 0; j < input[i].Length; j++)
                {
                    grid[i][j] = input[i][j] switch
                    {
                        '#' => 1,
                        '.' => 0,
                        '^' => 1 << 1,
                        '>' => 1 << 2,
                        'v' => 1 << 3,
                        '<' => 1 << 4,
                        _ => throw new ArgumentException("Invalid Input!"),
                    };
                }
            }

            grid = RunState(grid, 0, initialY, grid.Count - 1, endY, out int time);
            Console.WriteLine(time);
            grid = RunState(grid, grid.Count - 1, endY, 0, initialY, out int timeBack);
            grid = RunState(grid, 0, initialY, grid.Count - 1, endY, out int timeAgain);
            Console.WriteLine(time + timeBack + timeAgain);
        }

        private static List<int[]> RunState(List<int[]> inGrid, int inX, int inY, int endX, int endY, out int minTime)
        {
            var q = new Queue<State>();
            var initial = new State(inX, inY, inGrid, 0);
            q.Enqueue(initial);

            var minMoves = new List<int[]>(initial.grid.Count);
            for (var i = 0; i < initial.grid.Count; i++)
            {
                minMoves.Add(new int[initial.grid[i].Length]);
                Array.Fill(minMoves[i], int.MaxValue);
            }

            minMoves[initial.x][initial.y] = 0;
            var buffer = LCM(initial.grid.Count - 2, initial.grid[0].Length - 2);
            List<int[]>? endGrid = null;
            while (q.Count > 0)
            {

                var s = q.Dequeue();
                if ((s.moves - buffer) > minMoves[s.x][s.y])
                {
                    continue;
                }
                else if (s.moves < minMoves[s.x][s.y])
                {
                    minMoves[s.x][s.y] = s.moves;
                    if (s.x == endX && s.y == endY)
                    {
                        endGrid = s.grid;
                    }
                }

                var newGrid = UpdateWinds(s.grid);
                for (var k = 0; k < next.Length; k++)
                {
                    var newX = s.x + next[k].x;
                    var newY = s.y + next[k].y;
                    if (newX < 0 || newX >= newGrid.Count || newY < 0 || newY >= newGrid[newX].Length || newGrid[newX][newY] != 0)
                    {
                        continue;
                    }

                    var newS = new State(newX, newY, newGrid, s.moves + 1);
                    if (CheckExistingStates(q, newS))
                    {
                        q.Enqueue(newS);
                    }
                }
            }

            minTime = minMoves[endX][endY];
            if (endGrid == null)
            {
                throw new InvalidDataException("Could not reach end!");
            }

            return endGrid;
        }

        private static bool CheckExistingStates(Queue<State> q, State newS)
        {
            foreach (var item in q)
            {
                if (item.x == newS.x && item.y == newS.y && item.moves == newS.moves)
                {
                    return false;
                }
            }

            return true;
        }

        private static List<int[]> UpdateWinds(List<int[]> oldGrid)
        {
            var newGrid = new List<int[]>(oldGrid.Count);
            oldGrid.ForEach((item) => { newGrid.Add(new int[item.Length]); });
            for (var i = 0; i < newGrid.Count; i++)
            {
                for (var j = 0; j < newGrid[0].Length; j++)
                {
                    if (oldGrid[i][j] == 0)
                    {
                        continue;
                    }

                    if (oldGrid[i][j] == 1)
                    {
                        newGrid[i][j] = 1;
                        continue;
                    }

                    if ((oldGrid[i][j] & 2) != 0)
                    {
                        var newX = i == 1 ? oldGrid.Count - 2 : i - 1;
                        newGrid[newX][j] = newGrid[newX][j] | (1 << 1);
                    }

                    if ((oldGrid[i][j] & 4) != 0)
                    {
                        var newY = j == oldGrid[i].Length - 2 ? 1 : j + 1;
                        newGrid[i][newY] = newGrid[i][newY] | (1 << 2);
                    }

                    if ((oldGrid[i][j] & 8) != 0)
                    {
                        var newX = i == oldGrid.Count - 2 ? 1 : i + 1;
                        newGrid[newX][j] = newGrid[newX][j] | (1 << 3);
                    }

                    if ((oldGrid[i][j] & 16) != 0)
                    {
                        var newY = j == 1 ? oldGrid[i].Length - 2 : j - 1;
                        newGrid[i][newY] = newGrid[i][newY] | (1 << 4);
                    }
                }
            }

            return newGrid;
        }

        static int GFC(int a, int b)
        {
            while (b != 0)
            {
                int t = b;
                b = a % b;
                a = t;
            }

            return a;
        }

        static int LCM(int a, int b) => (a / GFC(a, b)) * b;
    }
}
