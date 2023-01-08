using System.Collections;
using System.Text.RegularExpressions;

namespace AdventOfCode2022
{
    internal class _16
    {
        private class ltKey
        {
            public int c;
            public int e;
            public int[] opened;

            public ltKey()
            {
                c = -1;
                e = -1;
                opened = new int[0];
            }

            public ltKey(int c, int e, int[] opened)
            {
                this.c = c;
                this.e = e;
                this.opened = opened;
            }

            public override bool Equals(object? obj)
            {
                if (obj == null) return false;
                if (!(obj is ltKey)) return false;
                ltKey k = (ltKey)obj;
                if (c != k.c || e != k.e || !opened.SequenceEqual(k.opened)) return false;
                return true;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(c, e, StructuralComparisons.StructuralEqualityComparer.GetHashCode(opened));
            }
        }
        static readonly Dictionary<int, (int flow, List<int> links)> caves = new();
        static readonly Dictionary<string, int> names = new();
        public static void Run()
        {
            var input = File.ReadAllLines("input/16_1.txt");
            foreach (var line in input)
            {
                var groups = Regex.Match(line, "Valve ([^ ]+) has flow rate=([^;]+); tunnels? leads? to valves? (.+)$");
                if (groups.Success)
                {
                    var name = AddName(groups.Groups[1].Value);
                    caves.Add(name, (int.Parse(groups.Groups[2].Value), groups.Groups[3].Value.Split(", ").Select(s => AddName(s)).ToList()));
                }
                else
                {
                    throw new InvalidDataException(line);
                }
            }

            var loop = AddName("LOOP");
            // loopback cave
            caves.Add(loop, (0, new int[1] { loop }.ToList()));
            var maxFlow = OptimizeFlowTwo(30, false);
            Console.WriteLine(maxFlow);
            var maxFlow2 = OptimizeFlowTwo(26, true);
            Console.WriteLine(maxFlow2);
        }

        private static int AddName(string value)
        {
            if (names.ContainsKey(value))
            {
                return names[value];
            }
            else
            {
                var newValue = names.Count;
                names.Add(value, newValue);
                return newValue;
            }
        }

        private static int OptimizeFlowTwo(int minutes, bool useSecond)
        {
            var lt = new List<Dictionary<ltKey, int>> {
                new Dictionary<ltKey, int>()
            };
            var startC = AddName("AA");
            var startE = useSecond ? startC : AddName("LOOP");
            lt[0].Add(new ltKey(startC, startE, new int[0]), 0);
            for (var i = 1; i <= minutes; i++)
            {
                lt.Add(new Dictionary<ltKey, int>());
            }

            var max = 0;
            max = OldSolution(minutes, lt);

            return max;
        }

        private static int OldSolution(int minutes, List<Dictionary<ltKey, int>> lt)
        {
            var maxOpen = 0;
            foreach (var cave in caves)
            {
                if (cave.Value.flow > 0)
                {
                    maxOpen++;
                }
            }
            var maxOpenLength = maxOpen * 2 + (maxOpen - 1);

            for (var i = 1; i <= minutes; i++)
            {
                var newTime = lt[i];
                foreach (var item in lt[i - 1])
                {
                    var nextFlow = CalculateFlow(item.Key.opened);
                    if (item.Key.opened.Length == maxOpenLength)
                    {
                        AddIfBetter(item.Key, newTime, nextFlow + item.Value);
                        continue;
                    }

                    var nextCavesC = caves[item.Key.c].links;
                    var nextCavesE = caves[item.Key.e].links;
                    bool addedC = false, addedE = false;
                    if (!item.Key.opened.Contains(item.Key.c) && caves[item.Key.c].flow > 0)
                    {
                        addedC = true;
                        nextCavesC.Add(item.Key.c);
                    }
                    if (!item.Key.opened.Contains(item.Key.e) && caves[item.Key.e].flow > 0)
                    {
                        addedE = true;
                        nextCavesE.Add(item.Key.e);
                    }

                    foreach (var nextC in nextCavesC)
                    {
                        foreach (var nextE in nextCavesE)
                        {
                            if (nextC == nextE && nextC == item.Key.c && addedC && addedE)
                            {
                                continue;
                            }

                            var nextOpened = item.Key.opened;
                            if (nextC == item.Key.c && addedC)
                            {
                                nextOpened = AppendStringList(nextOpened, item.Key.c);
                            }
                            if (nextE == item.Key.e && addedE)
                            {
                                nextOpened = AppendStringList(nextOpened, item.Key.e);
                            }
                            var nextKey = nextC <= nextE ? new ltKey(nextC, nextE, nextOpened) : new ltKey(nextE, nextC, nextOpened);
                            AddIfBetter(nextKey, newTime, nextFlow + item.Value);
                        }
                    }

                    if (addedC)
                    {
                        nextCavesC.RemoveAt(nextCavesC.Count - 1);
                    }
                    if (addedE)
                    {
                        nextCavesE.RemoveAt(nextCavesE.Count - 1);
                    }
                }

                // Thanos
                var j = 0;
                var keys = newTime.Keys.ToList();
                var maxValue = -1;
                if (i > 10)
                {
                    maxValue = (int)Math.Round(newTime.Values.Max() * 0.7);

                }

                j = 0;
                while (j < keys.Count - 1)
                {

                    var firstKey = keys[j];
                    if (maxValue > newTime[firstKey])
                    {
                        keys.RemoveAt(j);
                        newTime.Remove(firstKey);
                        continue;
                    }
                    var k = j + 1;
                    int result = 0;
                    while (k < keys.Count - 1)
                    {

                        var secondKey = keys[k];
                        if (firstKey.c == secondKey.c && firstKey.e == secondKey.e)
                        {
                            result = StringListContains(firstKey, secondKey);
                            if (result == 1)
                            {
                                // first is better
                                if (newTime[secondKey] <= newTime[firstKey])
                                {
                                    newTime.Remove(secondKey);
                                    keys.RemoveAt(k);
                                    continue;
                                }
                            }
                            else if (result == 2)
                            {
                                if (newTime[secondKey] >= newTime[firstKey])
                                {
                                    break;
                                }
                                else
                                {
                                    result = 0;
                                }
                            }
                        }
                        k++;
                    }

                    if (result == 2)
                    {
                        // second is better
                        newTime.Remove(firstKey);
                        keys.RemoveAt(j);
                    }
                    else
                    {
                        j++;
                    }
                }
            }

            var max = 0;
            foreach (var end in lt[minutes])
            {
                if (end.Value > max) { max = end.Value; }
            }

            return max;
        }

        private static void AddIfBetter(ltKey nextKey, Dictionary<ltKey, int> newTime, int newValue)
        {
            var currentItem = newTime.ContainsKey(nextKey) ? newTime[nextKey] : -1;
            if (currentItem < newValue)
            {
                newTime[nextKey] = newValue;
            }
        }

        private static int StringListContains(ltKey firstKey, ltKey secondKey)
        {
            if (firstKey.c != secondKey.c || firstKey.e != secondKey.e)
            {
                return 0;
            }

            if (firstKey.opened.Length > 0 && secondKey.opened.Length == 0)
            {
                return 1;
            }

            if (firstKey.opened.Length == 0 && secondKey.opened.Length > 0)
            {
                return 2;
            }

            if (firstKey.opened.Length == secondKey.opened.Length)
            {
                return 0;
            }

            var firstIsLarger = firstKey.opened.Length >= secondKey.opened.Length;
            bool result;
            if (firstIsLarger)
            {
                result = ContainList(firstKey.opened, secondKey.opened);
            }
            else
            {
                result = ContainList(secondKey.opened, firstKey.opened);
            }

            return result ? (firstIsLarger ? 1 : 2) : 0;
        }

        private static bool ContainList(int[] larger, int[] smaller)
        {
            //var h1 = new HashSet<int>(larger);
            //var h2 = new HashSet<int>(smaller);
            //return h1.IsSupersetOf(h2);

            for (var i = 0; i < smaller.Length; i++)
            {
                if (!larger.Contains(smaller[i]))
                {
                    return false;
                }
            }
            return true;
        }

        private static int[] AppendStringList(int[] opened, int c)
        {
            var spl = new List<int>(opened);
            spl.Add(c);
            spl.Sort();
            return spl.ToArray();
        }

        private static int CalculateFlow(int[] list)
        {
            var flow = 0;
            foreach (var cave in list)
            {
                flow += caves[cave].flow;
            }

            return flow;
        }
    }
}
