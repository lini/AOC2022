
using System.Text.RegularExpressions;

namespace AdventOfCode2022
{
    internal class _19
    {
        internal class DataItem
        {
            public int orebots = 0;
            public int claybots = 0;
            public int obsidianbots = 0;
            public int geodebots = 0;
            public int ore = 0;
            public int clay = 0;
            public int obsidian = 0;
            public int geode = 0;

            public DataItem()
            {

            }

            public override string ToString()
            {
                return $"{orebots}, {claybots}, {obsidianbots}, {geodebots}, {ore}, {clay}, {obsidian}, {geode}";
            }
        }
        static Dictionary<string, (int ore, int clay, int obsidian)> costs = new();
        public static void Run()
        {
            var input = File.ReadAllLines("input/19_1.txt");
            var res = 0;
            var res2 = 1;
            for (var i = 0; i < 3; i++)
            {
                var line = input[i];
                costs = ParseCosts(line, out int id);
                int max = FindMax(costs, 32);
                res2 *= max;
                Console.WriteLine($"{id}: {max}");
                Console.WriteLine();
            }

            Console.WriteLine("Res 2: " + res2);

            foreach (var line in input)
            {
                costs = ParseCosts(line, out int id);
                int max = FindMax(costs, 24);
                res += id * max;
                Console.WriteLine($"#{id}: {max}");
                Console.WriteLine();

            }

            Console.WriteLine("Res 1: " + res);
        }

        private static int FindMax(Dictionary<string, (int ore, int clay, int obsidian)> costs, int minutes)
        {
            var t = new List<DataItem>[minutes + 1];
            t[0] = new List<DataItem>() { new DataItem { orebots = 1 } };
            var newBots = new List<int[]>() { new int[4] { 0, 0, 0, 0 }, new int[4] { 0, 0, 0, 1 }, new int[4] { 1, 0, 0, 0 }, new int[4] { 0, 1, 0, 0 }, new int[4] { 0, 0, 1, 0 } };
            for (var i = 1; i <= minutes; i++)
            {
                t[i] = new List<DataItem>();
                foreach (var prev in t[i - 1])
                {
                    (int ore, int clay, int obsidian, int geode) = (prev.ore, prev.clay, prev.obsidian, prev.geode);
                    int start = costs["geode"].obsidian <= obsidian && costs["geode"].ore <= ore ? 1 : 0;
                    for (var b = start; b < newBots.Count; b++)
                    {



                        int neworebots = newBots[b][0], newclaybots = newBots[b][1], newobsidianbots = newBots[b][2], newgeodebots = newBots[b][3];
                        if ((neworebots * costs["ore"].ore) + (newclaybots * costs["clay"].ore) + (newobsidianbots * costs["obsidian"].ore) + (newgeodebots * costs["geode"].ore) <= ore
                            && (newobsidianbots * costs["obsidian"].clay <= clay)
                            && (newgeodebots * costs["geode"].obsidian) <= obsidian && newgeodebots < 2)
                        {
                            var newGeode = new DataItem()
                            {
                                orebots = prev.orebots + neworebots,
                                claybots = prev.claybots + newclaybots,
                                obsidianbots = prev.obsidianbots + newobsidianbots,
                                geodebots = prev.geodebots + newgeodebots,
                                ore = prev.orebots + ore - ((neworebots * costs["ore"].ore) + (newclaybots * costs["clay"].ore) + (newobsidianbots * costs["obsidian"].ore) + (newgeodebots * costs["geode"].ore)),
                                clay = prev.claybots + clay - (newobsidianbots * costs["obsidian"].clay),
                                obsidian = prev.obsidianbots + obsidian - (newgeodebots * costs["geode"].obsidian),
                                geode = prev.geodebots + geode
                            };

                            var skipAdd = false;

                            for (var k = 1; k <= i; k++)
                                for (var j = t[k].Count - 1; j >= 0; j--)
                                {
                                    var otherGeode = t[k][j];
                                    if (otherGeode.orebots == newGeode.orebots &&
                                        otherGeode.claybots == newGeode.claybots &&
                                        otherGeode.geodebots == newGeode.geodebots &&
                                        otherGeode.obsidianbots == newGeode.obsidianbots)
                                    {
                                        // same number of bots? keep item with strictly more resources
                                        if (otherGeode.ore >= newGeode.ore &&
                                            otherGeode.clay >= newGeode.clay &&
                                            otherGeode.geode >= newGeode.geode &&
                                            otherGeode.obsidian >= newGeode.obsidian)
                                        {
                                            skipAdd = true;
                                            break;
                                        }
                                        else if (i == k)
                                        {
                                            if (otherGeode.ore <= newGeode.ore &&
                                                otherGeode.clay <= newGeode.clay &&
                                                otherGeode.geode <= newGeode.geode &&
                                                otherGeode.obsidian <= newGeode.obsidian)
                                            {
                                                // strictly worse
                                                t[k].RemoveAt(j);
                                            }

                                        }
                                    }
                                    else if (i == k && newGeode.geode > otherGeode.geode)
                                    {
                                        // unfit
                                        t[k].RemoveAt(j);
                                    }
                                }

                            if (!skipAdd)
                            {
                                t[i].Add(newGeode);
                            }
                        }
                    }
                }
            }

            var g = 0;
            for (var x = 0; x < t[minutes].Count; x++)
            {
                if (t[minutes][x].geode > g)
                {
                    g = t[minutes][x].geode;
                }
            }

            return g;
        }

        private static Dictionary<string, (int ore, int clay, int obsidian)> ParseCosts(string line, out int id)
        {
            var res = new Dictionary<string, (int ore, int clay, int obsidian)>();
            var m = Regex.Match(line, "Blueprint (\\d+):");
            id = int.Parse(m.Groups[1].Value);
            var robots = new string[4] { "ore", "clay", "obsidian", "geode" };

            m = Regex.Match(line, "Each ore robot costs (\\d+) ore.");
            int ore = int.Parse(m.Groups[1].Value);
            res.Add("ore", (ore, 0, 0));

            m = Regex.Match(line, "Each clay robot costs (\\d+) ore.");
            ore = int.Parse(m.Groups[1].Value);
            res.Add("clay", (ore, 0, 0));

            m = Regex.Match(line, "Each obsidian robot costs (\\d+) ore and (\\d+) clay.");
            ore = int.Parse(m.Groups[1].Value);
            var clay = int.Parse(m.Groups[2].Value);
            res.Add("obsidian", (ore, clay, 0));

            m = Regex.Match(line, "Each geode robot costs (\\d+) ore and (\\d+) obsidian.");
            ore = int.Parse(m.Groups[1].Value);
            var obsidian = int.Parse(m.Groups[2].Value);
            res.Add("geode", (ore, 0, obsidian));

            return res;
        }
    }
}
