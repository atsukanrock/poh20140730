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
                                       .Select((qr, index) => new Subcontractor(qr[0], qr[1], index))
                                       .ToArray();

            var result = int.MaxValue;

            var unfinishedSubcons = new List<Subcontractor>(n);
            // 1 subcontractor
            for (int i = 0; i < n; i++)
            {
                var currSubcon = allSubcons[i];
                if (currSubcon.Q >= m)
                {
                    result = Math.Min(currSubcon.R, result);
                    continue;
                }
                unfinishedSubcons.Add(currSubcon);
            }
            if (!unfinishedSubcons.Any())
            {
                Console.WriteLine(result);
                return;
            }

            // 2 subcontractors
            var unfinishedCalcResults = new List<CalcResult>(n);
            foreach (var calcResult in unfinishedSubcons.SelectMany(subcon => CalcNext(subcon, allSubcons)))
            {
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

            // n subcontractors
            for (int count = 2; count < n; count++)
            {
                var calcResults = unfinishedCalcResults.SelectMany(calcResult => CalcNext(calcResult, allSubcons))
                                                       .ToArray();
                unfinishedCalcResults.Clear();
                foreach (var calcResult in calcResults)
                {
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

        private static IEnumerable<CalcResult> CalcNext(Subcontractor currSubcontractor,
                                                        IList<Subcontractor> allSubcontractors)
        {
            for (var i = currSubcontractor.Index + 1; i < allSubcontractors.Count; i++)
            {
                var nextSubcon = allSubcontractors[i];
                yield return new CalcResult(new[] {currSubcontractor, nextSubcon},
                                            currSubcontractor.Q + nextSubcon.Q,
                                            currSubcontractor.R + nextSubcon.R);
            }
        }

        private static IEnumerable<CalcResult> CalcNext(CalcResult currCalcResult,
                                                        IList<Subcontractor> allSubcontractors)
        {
            for (var i = currCalcResult.Subcontractors.Last().Index + 1; i < allSubcontractors.Count; i++)
            {
                var resultSubcons = new Subcontractor[currCalcResult.Subcontractors.Length + 1];
                currCalcResult.Subcontractors.CopyTo(resultSubcons, 0);
                var nextSubcon = allSubcontractors[i];
                resultSubcons[resultSubcons.Length - 1] = nextSubcon;
                yield return new CalcResult(resultSubcons,
                                            currCalcResult.SumQ + nextSubcon.Q,
                                            currCalcResult.SumR + nextSubcon.R);
            }
        }

        private class Subcontractor
        {
            public readonly int Q;
            public readonly int R;
            public readonly int Index;

            public Subcontractor(int q, int r, int index)
            {
                Q = q;
                R = r;
                Index = index;
            }
        }

        private class CalcResult
        {
            public readonly Subcontractor[] Subcontractors;
            public readonly int SumQ;
            public readonly int SumR;

            public CalcResult(Subcontractor[] subcontractors, int sumQ, int sumR)
            {
                Subcontractors = subcontractors;
                SumQ = sumQ;
                SumR = sumR;
            }
        }
    }
}