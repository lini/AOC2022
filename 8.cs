namespace AdventOfCode2022
{
    internal class _8
    {
        public static void Run()
        {
            var input = File.ReadAllLines("input/8_1.txt");
            if (input == null || input.Length <= 0 || input[0]?.Length <= 0)
            {
                return;
            }
            var visible = new List<bool[]>();
            for (var k = 0; k < input.Length; k++)
            {
                visible.Add(new bool[input[k].Length]);
            }
            var allVisible = 0;
            var maxScore = 0;
            for (var i = 0; i < input.Length; i++)
            {
                int j;
                int min = -1;
                for (j = 0; j < input[i].Length; j++)
                {
                    if (min < input[i][j])
                    {
                        min = input[i][j];
                        visible[i][j] = true;
                    }
                    else
                    {
                        visible[i][j] = false;
                    }
                }

                min = -1;
                for (j = input[i].Length - 1; j >= 0; j--)
                {
                    if (min < input[i][j])
                    {
                        min = input[i][j];
                        visible[i][j] = true;
                    }
                }
            }
            for (var i = 0; i < input[0].Length; i++)
            {
                int j;
                int min = -1;
                for (j = 0; j < input.Length; j++)
                {
                    if (min < input[j][i])
                    {
                        min = input[j][i];
                        visible[j][i] = true;
                    }
                }

                min = -1;
                for (j = input.Length - 1; j >= 0; j--)
                {
                    if (min < input[j][i])
                    {
                        min = input[j][i];
                        visible[j][i] = true;
                    }
                }
            }

            for (var i = 0; i < visible.Count; i++)
            {
                for (var j = 0; j < visible[i].Length; j++)
                {
                    if (visible[i][j]) allVisible++;
                    int left = 0, right = 0, up = 0, down = 0;
                    var tree = input[i][j];
                    var limit = i - 1;
                    while (limit >= 0 && input[limit][j] < tree)
                    {
                        limit--;
                        up++;
                    }
                    if (limit>=0) { up++; }

                    limit = j - 1;
                    while (limit >= 0 && input[i][limit] < tree)
                    {
                        limit--;
                        left++;
                    }
                    if (limit >= 0) { left++; }

                    limit = i + 1;
                    while (limit < input.Length && input[limit][j] < tree)
                    {
                        limit++;
                        down++;
                    }
                    if (limit < input.Length) { down++; }

                    limit = j + 1;
                    while (limit < input[i].Length && input[i][limit] < tree)
                    {
                        limit++;
                        right++;
                    }
                    if (limit < input[i].Length) { right++; }

                    var score = up * down * left * right;
                    if (score > maxScore)
                    {
                        maxScore = score;
                    }
                }
            }

            Console.WriteLine(allVisible);
            Console.WriteLine(maxScore);
        }

        private static void print(List<bool[]> visible)
        {
            for (var i = 0; i < visible.Count; i++)
            {
                for (var j = 0; j < visible[i].Length; j++)
                {
                    Console.Write(visible[i][j] ? '1' : '0');
                }
                Console.WriteLine();
            }
        }
    }
}
