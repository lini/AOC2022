namespace AdventOfCode2022
{
    internal class _11
    {
        internal class Monkey
        {
            public Queue<long> originalContents = new();
            public Queue<List<(int, long)>> items = new();
            public Queue<List<(int, long)>> items10000 = new();
            public int trueToss;
            public int falseToss;
            public int testDivisible;
            public Func<long, long> op = i => i;
            public Monkey() { }
        }

        static Func<long, long>[] ops = new Func<long, long>[8]
        {
            i => checked(i*2),
            i => checked(i*i),
            i => checked(i+6),
            i => checked(i+2),
            i => checked(i*11),
            i => checked(i+7),
            i => checked(i+1),
            i => checked(i+5)
        };

        public static void Run()
        {
            var input = File.ReadAllLines("input/11_1.txt");
            var monkeys = new List<Monkey>();
            Monkey? current = null;
            foreach (var line in input)
            {
                var item = line.Trim();
                if (item.StartsWith("Monkey "))
                {
                    if (current != null)
                    {
                        monkeys.Add(current);
                    }

                    current = new Monkey();
                }
                else if (item.StartsWith("Starting items: "))
                {
                    if (current == null) { throw new InvalidDataException(); }
                    var items = item.Substring("Starting items: ".Length).Split(", ").Select(i => int.Parse(i));
                    foreach (var i in items)
                    {
                        current.originalContents.Enqueue(i);
                    }
                }
                else if (item.StartsWith("Operation:"))
                {
                    if (current == null) { throw new InvalidDataException(); }
                    current.op = ops[monkeys.Count];
                }
                else if (item.StartsWith("Test: divisible by "))
                {
                    if (current == null) { throw new InvalidDataException(); }
                    var test = int.Parse(item.Substring("Test: divisible by ".Length));
                    current.testDivisible = test;
                }
                else if (item.StartsWith("If true: throw to monkey "))
                {
                    if (current == null) { throw new InvalidDataException(); }
                    var toss = int.Parse(item.Substring("If true: throw to monkey ".Length));
                    current.trueToss = toss;
                }
                else if (item.StartsWith("If false: throw to monkey "))
                {
                    if (current == null) { throw new InvalidDataException(); }
                    var toss = int.Parse(item.Substring("If false: throw to monkey ".Length));
                    current.falseToss = toss;
                }
            }
            if (current == null) { throw new InvalidDataException(); }
            monkeys.Add(current);

            for (var i = 0; i < monkeys.Count; i++)
            {
                var orig = monkeys[i].originalContents.ToArray();
                for (var k = 0; k < orig.Length; k++)
                {
                    var num = orig[k];
                    var remainders = new List<(int, long)>();
                    for (var j = 0; j < monkeys.Count; j++)
                    {
                        var div = monkeys[j].testDivisible;
                        remainders.Add((div, num % div));
                    }

                    var copy1 = remainders.Select(v => (v.Item1, v.Item2)).ToList();
                    monkeys[i].items.Enqueue(remainders);
                    monkeys[i].items10000.Enqueue(copy1);
                }
            }

            var countInspected = new long[monkeys.Count];
            Array.Fill(countInspected, 0);
            for (var round = 1; round <= 20; round++)
            {
                MonkeyInspectRound(monkeys, countInspected, false, round);
                // Print(monkeys, round, false);
            }
            Array.Sort(countInspected);

            var countInspected10000 = new long[monkeys.Count];
            Array.Fill(countInspected10000, 0);
            var checks = new int[12] { 1, 20, 1000, 2000, 3000, 4000, 5000, 6000, 7000, 8000, 9000, 10000 };
            for (var round = 1; round <= 10000; round++)
            {
                MonkeyInspectRound(monkeys, countInspected10000, true, round);
                // Print(monkeys, round, true);
                if (checks.Contains(round))
                {
                    // PrintInspected(countInspected10000, round);
                }

            }
            Array.Sort(countInspected10000);


            var result1 = countInspected[countInspected.Length - 1] * countInspected[countInspected.Length - 2];
            var result2 = countInspected10000[countInspected10000.Length - 1] * countInspected10000[countInspected10000.Length - 2];
            Console.WriteLine(result1);
            Console.WriteLine(result2);
        }

        private static void MonkeyInspectRound(List<Monkey> monkeys, long[] countInspected, bool worried, int round)
        {
            for (var i = 0; i < monkeys.Count; i++)
            {
                var m = monkeys[i];
                countInspected[i] += (worried ? m.items10000 : m.items).Count;
                var q = worried ? m.items10000 : m.items;
                while (q.TryDequeue(out var item))
                {
                    var newItems = item.Select(n =>
                    {
                        var newItem = m.op(n.Item2);
                        if (!worried)
                        {
                            newItem = Convert.ToInt32(Math.Floor((double)newItem / 3));
                        }

                        return (n.Item1, newItem % n.Item1);
                    }).ToList();

                    var divided = newItems.First(n => n.Item1 == m.testDivisible).Item2;
                    var newm = monkeys[divided == 0 ? m.trueToss : m.falseToss];
                    if (worried)
                    {
                        newm.items10000.Enqueue(newItems);
                    }
                    else
                    {
                        newm.items.Enqueue(newItems);
                        long full = m.originalContents.Dequeue();
                        full = Convert.ToInt64(Math.Floor((double)m.op(full) / 3));
                        newm.originalContents.Enqueue(full);
                    }
                }
            }
        }

        private static void Print(List<Monkey> monkeys, int round, bool worried)
        {
            Console.WriteLine("Round " + round + ":");
            for (var i = 0; i < monkeys.Count; i++)
            {
                Console.Write(i + ": ");
                foreach (var item in monkeys[i].originalContents)
                {
                    Console.Write(item + ",");
                }
                Console.WriteLine();
            }
        }

        private static void PrintInspected(long[] countInspected, int round)
        {
            Console.WriteLine("Round " + round + ":");
            for (var i = 0; i < countInspected.Length; i++)
            {
                Console.WriteLine(countInspected[i]);
            }
        }
    }
}
