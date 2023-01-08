namespace AdventOfCode2022
{
    public static class ListExtensions
    {
        public static void AddToResult(this List<int> res, IEnumerable<char> set)
        {
            var item = set.FirstOrDefault();
            res.Add((item.ToString() == item.ToString().ToUpperInvariant()) ? item - 38 : item - 96);
        }
    }

    internal class _3
    {
        public static void Run()
        {
            var input = File.ReadAllLines("input/3_1.txt");
            var results = new List<int>();
            var results2 = new List<int>();
            HashSet<char> set1, set2;
            var set3 = new HashSet<char>();
            var count = 0;
            foreach (var line in input)
            {
                if (count % 3 == 0)
                {
                    if (set3.Count > 0)
                    {
                        results2.AddToResult(set3);
                    }
                    set3 = new HashSet<char>(line);
                }
                var half = line.Length / 2;
                set1 = new HashSet<char>(line.Substring(0, half));
                set2 = new HashSet<char>(line.Substring(half));

                var intersection = set1.Intersect(set2);
                results.AddToResult(intersection);
                set3.IntersectWith(line);
                count++;
            }

            // last group
            results2.AddToResult(set3);

            Console.WriteLine(results.Sum());
            Console.WriteLine(results2.Sum());

        }
    }
}
