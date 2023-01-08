using System.Text;

namespace AdventOfCode2022
{
    internal class _22
    {
        const int cubeSize = 50;
        public static void Run()
        {
            var inp = File.ReadAllLines("input/22_1.txt").ToList();
            var commands = inp[^1].ToCharArray();
            inp.RemoveRange(inp.Count - 2, 2);
            var input = inp.Select(s => s.PadRight(inp[0].Length).ToCharArray()).ToList();
            var startY = 0;
            for (var i = 0; i < input[0].Length; i++)
            {
                if (input[0][i] == '.')
                {
                    startY = i;
                    break;
                }
            }
            var endPos = Move(input, commands, 0, startY);
            var res = (1000 * (endPos.x + 1)) + (4 * (endPos.y + 1)) + endPos.dir;
            Console.WriteLine(res);
            endPos = Move(input, commands, 0, startY, true);
            res = (1000 * (endPos.x + 1)) + (4 * (endPos.y + 1)) + endPos.dir;
            Console.WriteLine(res);
        }

        private static (int x, int y, int dir) Move(List<char[]> input, char[] commands, int startX, int startY, bool cube = false)
        {
            var x = startX; var y = startY; var dir = 0;
            var commandIndex = 0;
            do
            {
                // Print(input, x, y, dir);
                (int move, bool left) = ParseCommand(commands, ref commandIndex);
                (x, y, dir) = MoveXY(input, x, y, move, dir, cube);
                if (commandIndex < commands.Length)
                {
                    dir = !left ? (dir + 1) % 4 : dir - 1;
                    if (dir < 0)
                    {
                        dir = 3;
                    }
                }

                // Print(input, x, y, dir);
            } while (commandIndex < commands.Length);

            // Print(input, x, y, dir);
            return (x, y, dir);
        }

        private static void Print(List<char[]> input, int x, int y, int dir, int x2 = -1, int y2 = -1, int dir2 = -1)
        {
            using (var file = File.Create("output.txt"))
            {
                var str = string.Empty;

                // Console.Clear();
                for (var i = 0; i < input.Count; i++)
                {
                    for (var j = 0; j < input[i].Length; j++)
                    {
                        if (i == x && j == y)
                        {
                            var d = dir switch
                            {
                                0 => '>',
                                1 => 'v',
                                2 => '<',
                                3 => '^',
                                _ => '?'
                            };
                            // Console.Write(d);
                            str += d;
                        }
                        else if (i == x2 && j == y2)
                        {
                            var d = dir2 switch
                            {
                                0 => '>',
                                1 => 'v',
                                2 => '<',
                                3 => '^',
                                _ => '?'
                            };
                            // Console.Write(d);
                            str += d;
                        }
                        else
                        {
                            str += input[i][j];
                            // Console.Write(input[i][j]);
                        }
                        str += ' ';
                    }
                    str += '\n';
                    //Console.WriteLine();
                }
                file.Write(Encoding.ASCII.GetBytes(str));
            }
            Console.ReadKey();
        }

        private static (int x, int y, int dir) MoveXY(List<char[]> input, int x, int y, int move, int dir, bool cube)
        {
            var end = move;
            var vertical = dir % 2 != 0;
            var step = dir == 0 || dir == 1 ? 1 : -1;
            var idxEnd = step == 1 ? (vertical ? input.Count - 1 : input[x].Length - 1) : 0;
            var idxStart = step == 1 ? 0 : (vertical ? input.Count - 1 : input[x].Length - 1);
            while (end > 0)
            {
                if (
                    (vertical && (x == idxEnd || input[x + step][y] == ' ')) ||
                    (!vertical && (y == idxEnd || input[x][y + step] == ' '))
                   )
                {
                    if (cube)
                    {
                        return WrapCube(input, x, y, dir, end);
                    }
                    else
                    {
                        // wrap around
                        for (var i = idxStart; (step == 1 ? i <= idxEnd : i >= 0); i += step)
                        {
                            if (!vertical)
                            {
                                if (input[x][i] != ' ')
                                {
                                    if (input[x][i] == '.')
                                    {
                                        y = i;
                                    }

                                    break;
                                }
                            }
                            else
                            {
                                if (input[i][y] != ' ')
                                {
                                    if (input[i][y] == '.')
                                    {
                                        x = i;
                                    }

                                    break;
                                }
                            }
                        }
                    }
                }
                else if (!vertical && input[x][y + step] == '.')
                {
                    y += step;
                }
                else if (vertical && input[x + step][y] == '.')
                {
                    x += step;
                }

                end--;
            }

            return (x, y, dir);
        }

        private static (int x, int y, int dir) WrapCube(List<char[]> input, int x, int y, int dir, int move)
        {
            /*
              1122
              1122
              33
              33
            5544
            5544
            66
            66
            */
            var c = GetCube(x, y);
            (int ox, int oy, int odir) = (x, y, dir);
            switch (c)
            {
                case 1:
                    switch (dir)
                    {
                        case 0:
                            y += 1;
                            break;
                        case 1:
                            x += 1;
                            break;
                        case 2:
                            x = (cubeSize * 3) - x - 1;
                            y = 0;
                            dir = 0;
                            break;
                        case 3:
                            x = (cubeSize * 2) + y;
                            y = 0;
                            dir = 0;
                            break;
                        default:
                            break;
                    }
                    break;
                case 2:
                    switch (dir)
                    {
                        case 0:
                            x = (cubeSize * 3) - x - 1;
                            y = (2 * cubeSize) - 1;
                            dir = 2;
                            break;
                        case 1:
                            x = y - cubeSize;
                            y = (cubeSize * 2) - 1;
                            dir = 2;
                            break;
                        case 2:
                            y -= 1;
                            break;
                        case 3:
                            x = input.Count - 1;
                            y = y - (cubeSize * 2);
                            break;
                        default:
                            break;
                    }
                    break;
                case 3:
                    switch (dir)
                    {
                        case 0:
                            y = x + cubeSize;
                            x = cubeSize - 1;
                            dir = 3;
                            break;
                        case 1:
                            x += 1;
                            break;
                        case 2:
                            y = x - cubeSize;
                            x = cubeSize * 2;
                            dir = 1;
                            break;
                        case 3:
                            x -= 1;
                            break;
                        default:
                            break;
                    }
                    break;
                case 4:
                    switch (dir)
                    {
                        case 0:
                            x = (cubeSize * 3) - x - 1;
                            y = (cubeSize * 3) - 1;
                            dir = 2;
                            break;
                        case 1:
                            x = y + (cubeSize * 2);
                            y = cubeSize - 1;
                            dir = 2;
                            break;
                        case 2:
                            y -= 1;
                            break;
                        case 3:
                            x -= 1;
                            break;
                        default:
                            break;
                    }
                    break;
                case 5:
                    switch (dir)
                    {
                        case 0:
                            y += 1;
                            break;
                        case 1:
                            x += 1;
                            break;
                        case 2:
                            x = (cubeSize * 3) - x - 1;
                            y = cubeSize;
                            dir = 0;
                            break;
                        case 3:
                            x = y + cubeSize;
                            y = cubeSize;
                            dir = 0;
                            break;
                        default:
                            break;
                    }
                    break;
                case 6:
                    switch (dir)
                    {
                        case 0:
                            y = x - (cubeSize * 2);
                            x = (cubeSize * 3) - 1;
                            dir = 3;
                            break;
                        case 1:
                            y = y + (cubeSize * 2);
                            x = 0;
                            break;
                        case 2:
                            y = x - (cubeSize * 2);
                            x = 0;
                            dir = 1;
                            break;
                        case 3:
                            x -= 1;
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }

            if (input[x][y] == '.')
            {
                return MoveXY(input, x, y, move - 1, dir, true);
            }
            else
            {
                return (ox, oy, odir);
            }
        }

        private static int GetCube(int x, int y)
        {
            if (y < cubeSize)
            {
                if (x < (cubeSize * 3))
                {
                    return 5;
                }
                else
                {
                    return 6;
                }
            }
            else if (y < (cubeSize * 2))
            {
                if (x < cubeSize)
                {
                    return 1;
                }
                else if (x < (cubeSize * 2))
                {
                    return 3;
                }
                else
                {
                    return 4;
                }
            }
            else
            {
                return 2;
            }
        }
        private static (int move, bool left) ParseCommand(char[] commands, ref int commandIndex)
        {
            var amount = string.Empty;
            while (commandIndex < commands.Length && commands[commandIndex] != 'L' && commands[commandIndex] != 'R')
            {
                amount += commands[commandIndex++];
            }

            var left = commandIndex < commands.Length && commands[commandIndex] == 'L';
            commandIndex++;
            return (int.Parse(amount), left);
        }
    }
}
