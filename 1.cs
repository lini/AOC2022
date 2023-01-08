namespace AdventOfCode2022
{
    internal class _1
    {
        public static void Run()
        {
            var input = File.ReadAllLines("input/1_1.txt");
            var calories = new SortedSet<long>();
            long sum = 0;

            for (var i = 0; i < input.Length; i++)
            {
                if (string.IsNullOrEmpty(input[i]?.Trim()))
                {
                    calories.Add(sum);
                    sum = 0;
                }
                else if (long.TryParse(input[i], out long val))
                {
                    sum += val;
                }
            }

            if (sum > 0)
            {
                calories.Add(sum);
            }

            Console.WriteLine(calories.Last());
            Console.WriteLine(calories.TakeLast(3).Sum());
        }

    }
}
