using System;
using System.Collections.Generic;

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
                var qr = Console.ReadLine().Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
                allSubcons[i] = new Subcontractor(int.Parse(qr[0]), int.Parse(qr[1]));
            }
            if (n == 1)
            {
                Console.WriteLine(allSubcons[0].R);
                return;
            }

            var result = int.MaxValue;
            Compute(new CalcResult(-1, 0, 0), allSubcons, m, ref result);
            Console.WriteLine(result);
        }

        private static void Compute(CalcResult calcResult, Subcontractor[] allSubcontractors, int m, ref int result)
        {
            if (calcResult.SumR >= result)
            {
                return;
            }
            if (calcResult.SumQ >= m)
            {
                if (calcResult.SumR < result)
                {
                    result = calcResult.SumR;
                }
                return;
            }
            foreach (var nextCalcResult in CalcNext(calcResult, allSubcontractors))
            {
                Compute(nextCalcResult, allSubcontractors, m, ref result);
            }
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

        private class Subcontractor
        {
            public readonly int Q;
            public readonly int R;

            public Subcontractor(int q, int r)
            {
                Q = q;
                R = r;
            }
        }

        private class CalcResult
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