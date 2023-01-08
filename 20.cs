
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AdventOfCode2022
{
    internal class _20
    {

        public static void Run()
        {
            var input = File.ReadAllLines("input/20_1.txt").Select(x => long.Parse(x)).ToArray();
            Solve(input);

            Solve(input, 811589153);
        }


        private static void Solve(long[] input, long decryptionKey = 1)
        {
            LinkedList<long> q = new();
            var i = 0;
            while (i < input.Length)
            {
                q.AddLast(input[i++] * decryptionKey);
            }
            var l = new LinkedListNode<long>[q.Count];
            i = 0;
            var n = q.First;
            while (i < q.Count)
            {
                if (n != null)
                {
                    l[i] = n;
                    n = n.Next;
                }
                i++;
            }
            int mixes = 1;
            if (decryptionKey != 1)
            {
                mixes = 10;
            }
            LinkedListNode<long>? z = null;
            while (mixes > 0)
            {
                i = 0;
                while (i < l.Length)
                {
                    n = l[i];
                    if (n.Value.CompareTo(0) == 0)
                    {
                        z = n;
                    }
                    else
                    {
                        var step = n.Value.CompareTo(0);
                        var steps = Math.Abs(n.Value) % (q.Count - 1);
                        var list = n?.List;
                        while (steps != 0)
                        {
                            if (n != null && list != null)
                            {
                                if (step > 0)
                                {
                                    n = n.Next ?? list.First;
                                }
                                else
                                {
                                    n = n.Previous ?? list.Last;
                                }
                            }
                            if (l[i]?.List != null)
                            {
                                list?.Remove(l[i]);
                            }
                            steps--;
                        }
                        if (n != null && list != null)
                        {
                            if (step > 0)
                            {
                                list.AddAfter(n, l[i]);
                            }
                            else
                            {
                                list.AddBefore(n, l[i]);
                            }
                        }
                        else
                        {
                            throw new Exception("Adasd");
                        }
                    }
                    i++;
                }
                mixes--;
            }
            n = z?.List?.First;
            if (z != null)
            {
                var i1000 = ValueAtIndex(z, 1000);
                var i2000 = ValueAtIndex(z, 2000);
                var i3000 = ValueAtIndex(z, 3000);
                Console.WriteLine($"{i1000} + {i2000} + {i3000} = {i1000 + i2000 + i3000}");
            }
        }

        static T ValueAtIndex<T>(LinkedListNode<T> z, int index)
        {
            var n = z;
            while (index > 0)
            {
                if (n != null && n.List != null)
                {
                    n = n.Next ?? n.List.First;
                }
                index--;
            }
            if (n == null)
            {
                throw new Exception("ASdasd");
            }
            return n.Value;
        }
    }
}
