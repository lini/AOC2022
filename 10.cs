using System.Linq;
using System.Text;

namespace AdventOfCode2022
{
    internal class _10
    {
        public static void Run()
        {
            var input = File.ReadAllLines("input/10_1.txt").ToList();
            int x = 1;
            var sum = 0;
            int clock = 1;
            foreach (var line in input)
            {
                var vals = line.Split(' ');
                var inst = vals[0];
                var val = vals.Length > 1 ? int.Parse(vals[1]) : 0;
                var toAdd = inst == "noop" ? 1 : 2;
                if ((clock + 20) % 40 == 0 || (toAdd == 2 && (clock + 21) % 40 == 0))
                {
                    var c = clock % 20 == 0 ? clock : clock + 1;
                    sum += x * c;
                }

                clock += toAdd;
                x += val;
            }

            Console.WriteLine(sum);
           
            var output = new StringBuilder("#");
            x = 1;
            clock = 1;
            for (var i = input.Count - 1; i >= 0; i--)
            {
                if (input[i] != "noop")
                {
                    input.Insert(i, "noop");
                }
            }
            foreach (var line in input)
            {
                var vals = line.Split(' ');
                var op = vals[0];
                var change = vals.Length > 1 ? int.Parse(vals[1]) : 0;
                x += change;
                var pos = clock % 40;
                output.Append(Math.Abs(x - pos) > 1 ? '.' : '#');
                clock++;
            }
            for (var i = 0; i < output.Length; i++)
            {
                if (i % 40 == 0)
                {
                    Console.WriteLine();
                }
                Console.Write(output[i]);
            }

        }
    }
}
