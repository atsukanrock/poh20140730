using System;
using System.Collections.Generic;
using System.Linq;

namespace PaizaOnlineHackathon201407
{
    internal class Solution
    {
        private static void Main()
        {
            var m = int.Parse(Console.ReadLine());
            var n = int.Parse(Console.ReadLine());
            var allSubcons = Enumerable.Range(0, n)
                                       .Select(i => Console.ReadLine().Trim().Split(' ').Select(int.Parse).ToArray())
                                       .Select(qr => new Subcontractor(qr[0], qr[1]))
                                       .ToArray();

            var result = int.MaxValue;

            var yetIndexes1 = new List<int>(n);
            // 1 subcontractor
            for (int i = 0; i < n; i++)
            {
                var currSubcon = allSubcons[i];
                if (currSubcon.Q >= m)
                {
                    result = Math.Min(currSubcon.R, result);
                    continue;
                }
                yetIndexes1.Add(i);
            }

            // 2 subcontractors
            var yetIndexes = new List<int[]>();
            foreach (var indexes in yetIndexes1.SelectMany(index => Next(index, n)))
            {
                var currSubcons = indexes.Select(index => allSubcons[index]).ToArray();
                if (currSubcons.Sum(subcon => subcon.Q) >= m)
                {
                    result = Math.Min(currSubcons.Sum(subcon => subcon.R), result);
                    continue;
                }
                yetIndexes.Add(indexes);
            }

            // n subcontractors
            for (int count = 2; count < n; count++)
            {
                var currIndexesArray = yetIndexes.SelectMany(indexes => Next(indexes, n)).ToArray();
                yetIndexes.Clear();
                foreach (var indexes in currIndexesArray)
                {
                    var currSubcons = indexes.Select(i => allSubcons[i]).ToArray();
                    if (currSubcons.Sum(subcon => subcon.Q) >= m)
                    {
                        result = Math.Min(currSubcons.Sum(subccon => subccon.R), result);
                        continue;
                    }
                    yetIndexes.Add(indexes);
                }
            }

            Console.WriteLine(result);
        }

        private static IEnumerable<int[]> Next(int index, int length)
        {
            for (var i = index + 1; i < length; i++)
            {
                yield return new[] {index, i};
            }
        }

        private static IEnumerable<int[]> Next(int[] currentIndexes, int length)
        {
            for (var i = currentIndexes.Last() + 1; i < length; i++)
            {
                var result = new int[currentIndexes.Length + 1];
                currentIndexes.CopyTo(result, 0);
                result[result.Length - 1] = i;
                yield return result;
            }
        }

        private struct Subcontractor
        {
            private readonly int _q;
            private readonly int _r;

            public Subcontractor(int q, int r)
            {
                _q = q;
                _r = r;
            }

            public int Q
            {
                get { return _q; }
            }

            public int R
            {
                get { return _r; }
            }
        }
    }
}