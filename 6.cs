namespace AdventOfCode2022
{
    internal class _6
    {
        public static void Run()
        {
            var input = File.ReadAllText("input/6_1.txt");
            var startCode = string.Empty;
            var startMessage = string.Empty;
            var foundStartCode = false;
            for (int i = 0; i < input.Length; i++)
            {
                if (!foundStartCode)
                {
                    startCode += input[i];
                    if (startCode.Length > 4)
                    {
                        startCode = startCode.Remove(0, 1);
                        var sorted = new List<char>(startCode);
                        sorted.Sort();
                        if (sorted[0] != sorted[1] && sorted[1] != sorted[2] && sorted[2] != sorted[3])
                        {
                            Console.WriteLine(i + 1);
                            foundStartCode = true;
                        }
                    }
                }

                startMessage += input[i];
                if (startMessage.Length > 14)
                {
                    startMessage = startMessage.Remove(0, 1);
                    var sortedM = new List<char>(startMessage);
                    sortedM.Sort();
                    if (sortedM[0] != sortedM[1] && sortedM[1] != sortedM[2] && sortedM[2] != sortedM[3] &&
                        sortedM[3] != sortedM[4] && sortedM[4] != sortedM[5] && sortedM[5] != sortedM[6] &&
                        sortedM[6] != sortedM[7] && sortedM[7] != sortedM[8] && sortedM[8] != sortedM[9] &&
                        sortedM[9] != sortedM[10] && sortedM[10] != sortedM[11] && sortedM[11] != sortedM[12] &&
                        sortedM[12] != sortedM[13])
                    {
                        Console.WriteLine(i + 1);
                        break;
                    }
                }
            }
        }
    }
}
