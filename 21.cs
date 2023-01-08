namespace AdventOfCode2022
{
    internal class _21
    {
        class Monkey
        {
            public string m1;
            public string m2;
            public long value;
            public string f;
            public Func<long, long, long> op;
            private long cachedValue = long.MinValue;

            public Monkey(string _m1, string _m2, long _value)
            {
                m1 = _m1;
                m2 = _m2;
                value = _value;
                f = string.Empty;
                op = (long a, long b) => { return a + b; };
            }

            public Monkey(string _m1, string _m2, string _op)
            {
                m1 = _m1;
                m2 = _m2;
                value = long.MinValue;
                f = _op;
                op = GetOp(_op);
            }

            public long Yell()
            {
                if (cachedValue == long.MinValue)
                {
                    if (!string.IsNullOrEmpty(m1) && !string.IsNullOrEmpty(m2))
                    {
                        var a = monkeys.First(m => m.Key == m1).Value;
                        var b = monkeys.First(m => m.Key == m2).Value;
                        cachedValue = op(a.Yell(), b.Yell());
                    }
                    else
                    {
                        if (value == long.MinValue)
                        {
                            throw new InvalidDataException("missing value");
                        }

                        cachedValue = value;
                    }
                }
                return cachedValue;
            }

            public void ClearCache()
            {
                cachedValue = long.MinValue;
            }

            private Func<long, long, long> GetOp(string op)
            {
                Func<long, long, long> f = op switch
                {
                    "+" => (long a, long b) => { return a + b; }
                    ,
                    "-" => (long a, long b) => { return a - b; }
                    ,
                    "*" => (long a, long b) => { return a * b; }
                    ,
                    "/" => (long a, long b) => { return a / b; }
                    ,
                    _ => throw new InvalidDataException("invalid operation")
                };

                return f;
            }
        }

        static readonly Dictionary<string, Monkey> monkeys = new Dictionary<string, Monkey>();
        public static void Run()
        {
            var input = File.ReadAllLines("input/21_1.txt").ToArray();
            foreach (var line in input)
            {
                var splitLine = line.Split(':');
                var name = splitLine[0];
                var op = splitLine[1].Trim();
                Monkey newMonkey;
                if (op.IndexOf(' ') != -1)
                {
                    var splitOp = op.Split(' ');
                    var m1name = splitOp[0];
                    var m2name = splitOp[2];
                    var f = splitOp[1];
                    newMonkey = new Monkey(m1name, m2name, f);
                }
                else
                {
                    var value = long.Parse(op);
                    newMonkey = new Monkey(string.Empty, string.Empty, value);
                }

                monkeys.Add(name, newMonkey);
            }

            var root = monkeys.First(m => m.Key == "root").Value;
            Console.WriteLine(root.Yell());

            var humn = monkeys.First(m => m.Key == "humn").Value;
            humn.value = long.MinValue;

            bool found = false;
            long equal = long.MinValue;
            while (!found)
            {
                foreach (var m in monkeys)
                {
                    m.Value.ClearCache();
                }

                var e1 = root.m1;
                var e2 = root.m2;
                var m1 = monkeys.First(m => m.Key == e1).Value;
                var m2 = monkeys.First(m => m.Key == e2).Value;

                long newValue1 = long.MinValue, newValue2 = long.MinValue;
                try
                {
                    newValue1 = m1.Yell();
                }
                catch (Exception) { }
                try
                {
                    newValue2 = m2.Yell();
                }
                catch (Exception) { }

                if (equal == long.MinValue)
                {
                    if (newValue1 == long.MinValue)
                    {
                        equal = newValue2;
                        root = m1;
                    }
                    else
                    {
                        equal = newValue1;
                        root = m2;
                    }
                }
                else
                {
                    switch (root.f)
                    {
                        case "+":
                            equal = equal - (newValue1 == long.MinValue ? newValue2 : newValue1);
                            break;
                        case "*":
                            equal = equal / (newValue1 == long.MinValue ? newValue2 : newValue1);
                            break;
                        case "-":
                            if (newValue1 == long.MinValue)
                            {
                                equal = equal + newValue2;
                            }
                            else
                            {
                                equal = newValue1 - equal;
                            }
                            break;
                        case "/":
                            if (newValue1 == long.MinValue)
                            {
                                equal = equal * newValue2;
                            }
                            else
                            {
                                equal = newValue1 / equal;
                            }
                            break;
                        default:
                            throw new InvalidDataException("invalid operation!");
                    }

                    root = newValue1 == long.MinValue ? m1 : m2;
                }

                if (root == humn)
                {
                    found = true;
                }
            }

            Console.WriteLine(equal);
        }

    }
}
