namespace AdventOfCode2022
{
    internal class _13
    {
        public class msg
        {
            public List<(int?, msg?)> items = new List<(int?, msg?)>();
        }
        public static List<msg> all = new List<msg>();

        public static void Run()
        {
            var input = File.ReadAllLines("input/13_1.txt"); var first = string.Empty;
            var second = string.Empty;
            var idx = 0;
            var sum = 0;
            foreach (var line in input)
            {
                if (string.IsNullOrEmpty(line)) continue;
                if (string.IsNullOrEmpty(first))
                {
                    first = line;
                }
                else if (string.IsNullOrEmpty(second))
                {
                    second = line;
                    idx++;
                    var p1 = Parse(first);
                    var p2 = Parse(second);
                    all.Add(p1);
                    all.Add(p2);
                    if (Compare(p1, p2) > 0)
                    {
                        sum += idx;
                    }
                    first = second = string.Empty;
                }
            }
            var item2 = Parse("[[2]]");
            var item6 = Parse("[[6]]");
            all.Add(item2);
            all.Add(item6);
            Sort();
            var idx2 = all.IndexOf(item2) + 1;
            var idx6 = all.IndexOf(item6) + 1;
            Console.WriteLine(sum);
            Console.WriteLine(idx2 * idx6);
        }
        private static void Sort()
        {
            for (var i = 0; i < all.Count - 1; i++)
            {
                for (var j = i + 1; j < all.Count; j++)
                {
                    if (Compare(all[i], all[j]) < 0)
                    {
                        var temp = all[j];
                        all[j] = all[i];
                        all[i] = temp;
                    }
                }
            }
        }
        private static int Compare(msg? p1, msg? p2)
        {
            if (p1 == null || p2 == null)
            {
                return 0;
            }
            var minSize = Math.Min(p1.items.Count, p2.items.Count);
            for (var i = 0; i < minSize; i++)
            {
                var isNum1 = p1.items[i].Item1 != null;
                var isNum2 = p2.items[i].Item1 != null;
                if (isNum1 && isNum2)
                {
                    if (p1.items[i].Item1 == p2.items[i].Item1) continue;
                    return p1.items[i].Item1 > p2.items[i].Item1 ? -1 : 1;
                }
                else if (isNum1 && !isNum2)
                {
                    var tempMsg = new msg();
                    tempMsg.items.Add((p1.items[i].Item1, null));
                    var result = Compare(tempMsg, p2.items[i].Item2);
                    if (result != 0)
                    {
                        return result;
                    }
                }
                else if (!isNum1 && isNum2)
                {
                    var tempMsg = new msg();
                    tempMsg.items.Add((p2.items[i].Item1, null));
                    var result = Compare(p1.items[i].Item2, tempMsg);
                    if (result != 0)
                    {
                        return result;
                    }
                }
                else
                {
                    var result = Compare(p1.items[i].Item2, p2.items[i].Item2);
                    if (result != 0)
                    {
                        return result;
                    }
                }
            }
            if (p1.items.Count == p2.items.Count)
            {
                return 0;
            }
            else
            {
                return p1.items.Count > p2.items.Count ? -1 : 1;
            }
        }
        private static msg Parse(string item)
        {
            var msg = new msg();
            var idx = 1;
            while (idx < item.Length - 1)
            {
                if (item[idx] == ' ' || item[idx] == ',')
                {
                    idx++;
                }
                else if (item[idx] == '[')
                {
                    var closeBraceIdx = idx + 1;
                    var braceCount = item[closeBraceIdx] == ']' ? 0 : 1;
                    while (braceCount != 0)
                    {
                        if (item[closeBraceIdx] == '[')
                        {
                            braceCount++;
                        }
                        else if (item[closeBraceIdx] == ']')
                        {
                            braceCount--;
                        }
                        closeBraceIdx++;
                    }
                    var childMsg = Parse(item.Substring(idx, closeBraceIdx - idx));
                    msg.items.Add((null, childMsg));
                    idx = closeBraceIdx + 1;
                }
                else
                {
                    var num = string.Empty;
                    while (item[idx] >= '0' && item[idx] <= '9')
                    {
                        num += item[idx++];
                    }
                    msg.items.Add((int.Parse(num), null));
                }
            }
            return msg;
        }
    }
}