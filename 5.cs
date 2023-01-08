namespace AdventOfCode2022
{
    internal class _5
    {
        public static void Run()
        {
            var input = File.ReadAllLines("input/5_1.txt");
            var final = new List<string>(input).FindIndex(l => l.StartsWith(" 1"));

            var numSt = 0;
            var stacks = new Dictionary<int, Stack<char>>();
            var stacks2 = new Dictionary<int, Stack<char>>();

            for (var i = final - 1; i >= 0; i--)
            {
                var s = 0;
                for (var j = 1; j < input[i].Length; j += 4)
                {
                    s++;
                    var ch = input[i][j];
                    if (char.IsLetter(ch))
                    {
                        if (!stacks.ContainsKey(s))
                        {
                            stacks[s] = new Stack<char>();
                            stacks2[s] = new Stack<char>();
                            if (numSt < s)
                            {
                                numSt = s;
                            }
                        }
                        stacks[s].Push(ch);
                        stacks2[s].Push(ch);
                    }
                }
            }

            for (var k = final + 2; k < input.Length; k++)
            {
                var l = input[k].Split(" ", StringSplitOptions.RemoveEmptyEntries);
                int num = int.Parse(l[1]), from = int.Parse(l[3]), to = int.Parse(l[5]);
                var st = string.Empty;
                for (var m = 0; m < num; m++)
                {
                    var ch = stacks2[from].Pop();
                    st += ch;
                    stacks[to].Push(stacks[from].Pop());
                }

                foreach (var rch in st.Reverse())
                {
                    stacks2[to].Push(rch);
                }
            }

            for (var n = 1; n <= numSt; n++)
            {
                Console.Write(stacks[n].Peek());
            }
            Console.WriteLine();

            for (var n = 1; n <= numSt; n++)
            {
                Console.Write(stacks2[n].Peek());
            }
            Console.WriteLine();
        }
    }
}
