using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riddles.Util
{
    public class RangeConsolidator
    {
        // create a new list of range which consolidates all of the
        // ranges within the input list
        // for instance ((0, 1), (2, 4), (8, 12)) and ((0, 5), (6, 11))
        // becomes ((0, 1, true), (1, 2, false), (2, 4, true), (4, 5, false),
        // (5, 6, false), (6, 8, false), (8, 11, true), (11, 12, true))
        // and
        // ((0, 1, true), (1, 2, true), (2, 4, true), (4, 5, true),
        // (5, 6, false), (6, 8, true), (8, 11, true), (11, 12, false))
        // the lists share the same ranges, and have indicator booleans to determine
        // which part of the ranges are within the lists
        public List<List<(double, double, bool)>> ConsolidateRanges(
            List<List<(double, double)>> lists
        )
        {
            var consolidatedList = lists
                .Select(l => l.SelectMany(l => new List<double> { l.Item1, l.Item2 }))
                .SelectMany(i => i)
                .Distinct()
                .OrderBy(x => x)
                .ToList();
            var ranges = new List<(double, double)>();
            for (int i = 0; i < consolidatedList.Count() - 1; i++)
            {
                ranges.Add((consolidatedList[i], consolidatedList[i + 1]));
            }

            List<List<(double, double, bool)>> distributions
                = new List<List<(double, double, bool)>>();
            foreach (var list in lists)
            {
                List<(double, double, bool)> distribution
                    = new List<(double, double, bool)>();
                var coveredRangePtr = 0;
                foreach(var consolidatedRange in ranges)
                {
                    distribution.Add((
                        consolidatedRange.Item1,
                        consolidatedRange.Item2,
                        coveredRangePtr < list.Count &&
                            consolidatedRange.Item1 >= list[coveredRangePtr].Item1
                    ));
                    if(coveredRangePtr < list.Count && 
                        consolidatedRange.Item2 == list[coveredRangePtr].Item2)
                    {
                        coveredRangePtr++;
                    }
                }
                distributions.Add(distribution);
            }
            return distributions;
        }
    }
}
