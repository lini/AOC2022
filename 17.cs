namespace AdventOfCode2022
{
    internal class _17
    {
        const string empty = "       ";
        static readonly List<string[]> shapes = new(){
                                            new string[1] { "  @@@@ " },
                                            new string[3] { "   @   ",
                                                            "  @@@  ",
                                                            "   @   " },
                                            new string[3] { "    @  ",
                                                            "    @  ",
                                                            "  @@@  " },
                                            new string[4] { "  @    ",
                                                            "  @    ",
                                                            "  @    ",
                                                            "  @    " },
                                            new string[2] { "  @@   ",
                                                            "  @@   " }
                                        };
        public static void Run()
        {
            var input = File.ReadAllText("input/17_1.txt").Trim();
            var maxHeight = Calc(2022, input);
            Console.WriteLine(maxHeight);
            maxHeight = Calc(1000000000000, input);
            Console.WriteLine(maxHeight);
        }

        static long Calc(long maxRocks, string input)
        {
            var idx = 0;
            long stoppedRocks = 0;
            var cave = new List<string>() { "#######" };
            int nextShape = 0;
            long loops = 0;
            AddShape(shapes, cave, ref nextShape, out int currentShapeHeight);
            var falls = cave.Count - 1;
            do
            {

                var left = NextWind(input, ref idx);
                var shapeBottom = falls - currentShapeHeight;
                var ableToMove = AbleToMove(cave, falls, shapeBottom, left);
                if (ableToMove)
                {
                    MoveShape(cave, falls, shapeBottom, left);
                }
                var canfall = CanFall(cave, falls, shapeBottom);
                if (canfall)
                {
                    FallShape(cave, falls, shapeBottom);
                    falls--;
                }
                else
                {
                    MarkFall(cave, falls, shapeBottom);
                    stoppedRocks++;
                    AddShape(shapes, cave, ref nextShape, out currentShapeHeight);
                    falls = cave.Count - 1;

                }
                if (stoppedRocks == maxRocks)
                {
                    return CaveMaxHeight(cave) + loops * 2681;
                }
                if (idx == 0)
                {

                    // first wrap, calculate how much more we need
                    maxRocks -= stoppedRocks;
                    loops = maxRocks / 1740;
                    maxRocks = maxRocks % 1740;
                    stoppedRocks = 0;
                }

            }
            while (true);
        }

        private static long CaveMaxHeight(List<string> cave)
        {
            for (int i = cave.Count - 1; i >= 0; i--)
            {
                for (var j = cave[i].Length - 1; j >= 0; j--)
                {
                    if (cave[i][j] == '#')
                    {
                        return i;
                    }
                }
            }

            return 0;
        }

        static void MarkFall(List<string> cave, int idxStart, int idxEnd)
        {
            for (var i = idxStart; i > idxEnd; i--)
            {
                for (var j = 0; j < cave[i].Length; j++)
                {
                    if (cave[i][j] == '@')
                    {
                        if (cave[i][j] != '@')
                        {
                            throw new Exception("Ops, marking wrong square");
                        }
                        cave[i] = cave[i].Remove(j, 1).Insert(j, "#");
                    }
                }
            }
        }

        static void FallShape(List<string> cave, int idxStart, int idxEnd)
        {
            if (cave[idxEnd] == empty)
            {
                cave.RemoveAt(idxEnd);
            }
            else
            {
                for (var i = idxEnd + 1; i <= idxStart; i++)
                {
                    for (var j = 0; j < cave[i].Length; j++)
                    {
                        if (cave[i][j] == '@')
                        {
                            cave[i] = cave[i].Remove(j, 1).Insert(j, " ");
                            cave[i - 1] = cave[i - 1].Remove(j, 1).Insert(j, "@");
                        }
                    }
                }
            }
        }

        static bool CanFall(List<string> cave, int idxStart, int idxEnd)
        {
            var canfall = true;
            for (var i = idxStart; i > idxEnd; i--)
            {
                for (var j = 0; j <= cave[i].Length - 1; j++)
                {
                    if (cave[i][j] == '@' && cave[i - 1][j] == '#')
                    {
                        canfall = false;
                        break;
                    }
                }

                if (!canfall)
                {
                    break;
                }
            }
            return canfall;
        }

        static void MoveShape(List<string> cave, int idxStart, int idxEnd, bool left)
        {
            for (var i = idxStart; i > idxEnd; i--)
            {
                var j = left ? 0 : cave[i].Length - 1;
                var end = left ? cave[i].Length : -1;
                while (j != end)
                {
                    if (cave[i][j] == '@')
                    {
                        if (left)
                        {
                            cave[i] = cave[i].Remove(j - 1, 1).Insert(j, " ");
                        }
                        else
                        {
                            cave[i] = cave[i].Remove(j + 1, 1).Insert(j, " ");
                        }
                    }
                    j = left ? j + 1 : j - 1;
                }
            }
        }

        static bool NextWind(string input, ref int idx)
        {
            var wind = input[idx] == '<';
            idx++;
            if (idx == input.Length)
            {
                idx = 0;
            }
            return wind;
        }

        static long AddShape(List<string[]> shapes, List<string> cave, ref int nextShape, out int currentShapeHeight)
        {
            while (cave[cave.Count - 1] == empty) cave.RemoveAt(cave.Count - 1);
            var deletedRows = CleanCave(cave);
            cave.Add(empty);
            cave.Add(empty);
            cave.Add(empty);
            for (var i = shapes[nextShape].Length - 1; i >= 0; i--)
            {
                cave.Add(shapes[nextShape][i]);
            }
            currentShapeHeight = shapes[nextShape].Length;
            nextShape = (nextShape + 1) % shapes.Count;

            return deletedRows;
        }

        private static long CleanCave(List<string> cave)
        {
            if (false && cave.Count > 10000)
            {
                var deleted = 7000;
                cave.RemoveRange(0, deleted);
                return deleted;
            }
            else
            {
                return 0;
            }
        }

        static void Print(List<string> cave, int maxRows)
        {
            // Console.Clear();
            Console.WriteLine();
            var rows = 0;
            for (var i = cave.Count - 1; i >= 0; i--)
            {
                Console.WriteLine("|" + cave[i] + "|");
                if (rows++ == maxRows)
                {
                    break;
                }
            }
            Console.ReadKey();
        }

        static bool AbleToMove(List<string> cave, int idxStart, int idxEnd, bool left)
        {
            var ableToMove = true;
            for (var i = idxStart; i > idxEnd; i--)
            {
                if (left)
                {
                    var j = 0;
                    while (j < cave[i].Length && cave[i][j] != '@') j++;
                    if (j == cave[i].Length) continue;
                    if (j == 0 || cave[i][j - 1] != ' ')
                    {
                        ableToMove = false;
                        break;
                    }
                }
                else
                {
                    var j = cave[i].Length - 1;
                    while (j >= 0 && cave[i][j] != '@') j--;
                    if (j < 0) continue;
                    if (j == cave[i].Length - 1 || cave[i][j + 1] != ' ')
                    {
                        ableToMove = false;
                        break;
                    }
                }
            }
            return ableToMove;
        }
    }
}
