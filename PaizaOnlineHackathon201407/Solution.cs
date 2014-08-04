using System;
using System.Collections.Generic;
using System.Linq;

namespace PaizaOnlineHackathon201407
{
    internal class Solution
    {
        private static void Main()
        {
            // parse inputs
            var m = int.Parse(Console.ReadLine().Trim());
            var n = int.Parse(Console.ReadLine().Trim());
            var allSubcons = new Subcontractor[n];
            for (int i = 0; i < n; i++)
            {
                var qr = Console.ReadLine().Trim().Split(' ');
                allSubcons[i] = new Subcontractor(int.Parse(qr[0]), int.Parse(qr[1]));
            }

            var result = int.MaxValue;

            var unfinishedCalcResults = new List<CalcResult>(n);
            // 1 subcontractor
            for (int i = 0; i < n; i++)
            {
                var currSubcon = allSubcons[i];
                if (currSubcon.R > result)
                {
                    continue;
                }
                if (currSubcon.Q >= m)
                {
                    result = Math.Min(currSubcon.R, result);
                    continue;
                }
                unfinishedCalcResults.Add(new CalcResult(i, currSubcon.Q, currSubcon.R));
            }
            if (!unfinishedCalcResults.Any())
            {
                Console.WriteLine(result);
                return;
            }

            // n subcontractors
            for (int count = 1; count < n; count++)
            {
                var calcResults = unfinishedCalcResults.SelectMany(calcResult => CalcNext(calcResult, allSubcons))
                                                       .ToArray();
                unfinishedCalcResults.Clear();
                foreach (var calcResult in calcResults)
                {
                    if (calcResult.SumR > result)
                    {
                        continue;
                    }
                    if (calcResult.SumQ >= m)
                    {
                        result = Math.Min(calcResult.SumR, result);
                        continue;
                    }
                    unfinishedCalcResults.Add(calcResult);
                }
                if (!unfinishedCalcResults.Any())
                {
                    Console.WriteLine(result);
                    return;
                }
            }

            Console.WriteLine(result);
        }

        private static IEnumerable<CalcResult> CalcNext(CalcResult currCalcResult,
                                                        Subcontractor[] allSubcontractors)
        {
            for (var i = currCalcResult.LastIndex + 1; i < allSubcontractors.Length; i++)
            {
                var nextSubcon = allSubcontractors[i];
                yield return new CalcResult(i, currCalcResult.SumQ + nextSubcon.Q, currCalcResult.SumR + nextSubcon.R);
            }
        }

        private struct Subcontractor
        {
            public readonly int Q;
            public readonly int R;

            public Subcontractor(int q, int r)
            {
                Q = q;
                R = r;
            }
        }

        private struct CalcResult
        {
            public readonly int LastIndex;
            public readonly int SumQ;
            public readonly int SumR;

            public CalcResult(int lastIndex, int sumQ, int sumR)
            {
                LastIndex = lastIndex;
                SumQ = sumQ;
                SumR = sumR;
            }
        }
    }
}