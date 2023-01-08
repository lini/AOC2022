namespace AdventOfCode2022
{
    internal class _2
    {
        public static void Run()
        {
            var input = File.ReadAllLines("input/2_1.txt");
            long score = 0;
            Dictionary<string,int> map = new Dictionary<string,int>();
            map.Add("AX",1+3);
            map.Add("AY",2+6);
            map.Add("AZ",3+0);
            map.Add("BX",1+0);
            map.Add("BY",2+3);
            map.Add("BZ",3+6);
            map.Add("CX",1+6);
            map.Add("CY",2+0);
            map.Add("CZ",3+3);

            Dictionary<string, int> map2 = new Dictionary<string, int>();
            map2.Add("AX", 3 + 0);
            map2.Add("AY", 1 + 3);
            map2.Add("AZ", 2 + 6);
            map2.Add("BX", 1 + 0);
            map2.Add("BY", 2 + 3);
            map2.Add("BZ", 3 + 6);
            map2.Add("CX", 2 + 0);
            map2.Add("CY", 3 + 3);
            map2.Add("CZ", 1 + 6);
            long score2 = 0;

            foreach(var line in input)
            {
                var play = line.Replace(" ", string.Empty);
                score += map[play];
                score2+= map2[play];
            }

            Console.WriteLine(score);
            Console.WriteLine(score2);
        }


    }
}
