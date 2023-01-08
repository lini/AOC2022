using System.Text.RegularExpressions;

namespace AdventOfCode2022
{
    internal class _15
    {
        private class Sensor
        {
            public long x;
            public long y;
            public long bx;
            public long by;
        }

        private class Interval
        {
            public long start;
            public long end;

            public override string ToString()
            {
                return $"{start}, {end}";
            }
        }

        static readonly List<Sensor> sensors = new();
        public static void Run()
        {
            var input = File.ReadAllLines("input/15_1.txt");
            SortedSet<long> sln = new();
            var ln = 2000000;
            foreach (var line in input)
            {
                var vals = Regex.Replace(line, "[^-0-9,:]+", string.Empty).Split(',', ':');
                if (vals.Length != 4)
                {
                    throw new Exception("Invalid input");
                }
                var s = new Sensor { x = long.Parse(vals[0]), y = long.Parse(vals[1]), bx = long.Parse(vals[2]), by = long.Parse(vals[3]) };
                sensors.Add(s);
                if (s.by == ln)
                {
                    sln.Add(s.bx);
                }
            }

            var exc = Solve1(ln);
            long count = 0;
            for (var i = 0; i < exc.Count; i++)
            {
                count += exc[i].end - exc[i].start + 1;
                for (var j = 0; j < sln.Count; j++)
                {
                    var bx = sln.ElementAt(j);
                    if (bx >= exc[i].start && bx <= exc[i].end)
                    {
                        count--;
                    }
                }
            }
            Console.WriteLine(count);
            var beacon = Solve2(4000000);
            Console.WriteLine(beacon.start * 4000000 + beacon.end);
        }

        private static List<Interval> Solve1(long ln)
        {
            List<Interval> exc = new();
            foreach (var s in sensors)
            {
               

                var d = Dist(s.x, s.y, s.bx, s.by);
                var rem = s.y > ln ? ln - s.y + d : s.y + d - ln;
                if (rem >= 0)
                {
                    var pair = new Interval { start = s.x - rem, end = s.x + rem };
                    exc.Add(pair);
                }
            }

            exc.Sort(PairCompare);
            long end = exc[0].end;
            for (var i = 0; i < exc.Count - 1; i++)
            {
                var j = i + 1;
                while (j < exc.Count)
                {
                    if (exc[i].end >= exc[j].start)
                    {
                        exc[i].end = Math.Max(exc[i].end, exc[j].end);
                        exc.RemoveAt(j);
                    }
                    else
                    {
                        j++;
                    }
                }
            }

            return exc;
        }

        private static Interval Solve2(long maxX)
        {
            for (var i=0;i<maxX;i++)
            {
                var pairs = Solve1(i);
                if (pairs.Count>1)
                {
                    // Console.WriteLine(string.Join("; ", pairs.Select(p => p.ToString())));
                    var y = pairs[1].start - 1;
                    return new Interval { start = y, end = i };
                }
            }

            throw new Exception("not found!");
        }

        private static int PairCompare(Interval x, Interval y)
        {
            if (x.start != y.start)
            {
                return x.start.CompareTo(y.start);
            }
            else
            {
                return x.end.CompareTo(y.end);
            }
        }

        static long Dist(long x, long y, long x2, long y2)
        {
            return Math.Abs(x - x2) + Math.Abs(y - y2);
        }
    }
}
