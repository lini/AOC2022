namespace AdventOfCode2022
{
    internal class _4
    {
        public static void Run()
        {
            var input = File.ReadAllLines("input/4_1.txt");
            var overlaps = 0;
            var partialOverlaps = 0;
            foreach (var line in input)
            {
                var elves = line.Split(",").Select(e => e.Split("-")).ToArray();
                (int e10, int e11, int e20, int e21) = (int.Parse(elves[0][0]), int.Parse(elves[0][1]), int.Parse(elves[1][0]), int.Parse(elves[1][1]));
                if ((e10 <= e20 && e11 >= e21) || (e10 >= e20 && e11 <= e21))
                {
                    overlaps++;
                }

                if ((e11 >= e20 && e11 <= e21) || (e21 >= e10 && e21 <= e11))
                {
                    partialOverlaps++;
                }
            }

            Console.WriteLine(overlaps);
            Console.WriteLine(partialOverlaps);
        }
    }
}
