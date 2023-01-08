using System.Linq;

namespace AdventOfCode2022
{
    internal class _25
    {
        public static void Run()
        {
            var input = File.ReadAllLines("input/25_1.txt");
            ulong sum = 0;
            foreach (var line in input)
            {
                var num = StoI(line);
                if (num < 0)
                {
                    throw new InvalidDataException("invalid number");
                }
                sum += num;
            }

            Console.WriteLine(ItoS(sum));
        }

        private static string ItoS(ulong num)
        {
            var rem = num;
            var str = new List<char>();
            while (rem > 0)
            {
                var n = rem % 5;
                rem = rem / 5;
                str.Insert(0, n.ToString()[0]);
            }

            rem = 0;
            for (var i = str.Count - 1; i >= 0; i--)
            {
                var n = ulong.Parse(str[i].ToString()) + rem;
                if (n == 3)
                {
                    str[i] = '=';
                    rem = 1;
                }
                else if (n == 4)
                {
                    str[i] = '-';
                    rem = 1;
                }
                else if (n == 5)
                {
                    str[i] = '0';
                    rem = 1;
                }
                else
                {
                    str[i] = n.ToString()[0];
                    rem = 0;
                }
            }
            if (rem > 0)
            {
                str.Insert(0, (rem + 1).ToString()[0]);
            }

            return string.Join(null, str);
        }

        private static ulong StoI(string line)
        {
            long res = 0;
            for (var i = line.Length - 1; i >= 0; i--)
            {
                var pos = (long)Math.Pow(5, line.Length - i - 1);
                switch (line[i])
                {
                    case '0':
                        pos *= 0;
                        break;
                    case '1':
                        pos *= 1;
                        break;
                    case '2':
                        pos *= 2;
                        break;
                    case '-':
                        pos *= -1;
                        break;
                    case '=':
                        pos *= -2;
                        break;
                    default:
                        break;
                }

                res += pos;
            }

            return (ulong)res;
        }
    }
}
