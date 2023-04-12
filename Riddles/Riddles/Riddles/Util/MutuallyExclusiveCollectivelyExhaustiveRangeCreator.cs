using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riddles.Util
{
    // note, ranges don't have the concept of inclusive and exclusive boundaries
    // should create an actual concept of a range when handling inclusive and 
    // exclusive boundaries
    public class MutuallyExclusiveCollectivelyExhaustiveRangeCreator
    {
        // create a new list of MECE ranges which cover all
        // ranges within the input list
        // for instance ((0, 1), (2, 4), (8, 12)) and ((0, 5), (6, 11))
        // becomes ((0, 1, true), (1, 2, false), (2, 4, true), (4, 5, false),
        // (5, 6, false), (6, 8, false), (8, 11, true), (11, 12, true))
        // and
        // ((0, 1, true), (1, 2, true), (2, 4, true), (4, 5, true),
        // (5, 6, false), (6, 8, true), (8, 11, true), (11, 12, false))
        // the lists share the same ranges, and have indicator booleans to determine
        // which part of the ranges are within the lists
        public List<List<(double, double, bool)>> CreateMeceRanges(
            List<List<(double, double)>> inputRanges
        )
        {
            var boundaries = inputRanges
                .Select(l => l.SelectMany(l => new List<double> { l.Item1, l.Item2 }))
                .SelectMany(i => i)
                .Distinct()
                .OrderBy(x => x)
                .ToList();
            var meceRanges = new List<(double, double)>();
            for (int i = 0; i < boundaries.Count() - 1; i++)
            {
                meceRanges.Add((boundaries[i], boundaries[i + 1]));
            }

            List<List<(double, double, bool)>> distributions
                = new List<List<(double, double, bool)>>();
            foreach (var inputRange in inputRanges)
            {
                List<(double, double, bool)> distribution
                    = new List<(double, double, bool)>();
                var inputRangePtr = 0;
                foreach(var meceRange in meceRanges)
                {
                    distribution.Add((
                        meceRange.Item1,
                        meceRange.Item2,
                        inputRangePtr < inputRange.Count &&
                            meceRange.Item1 >= inputRange[inputRangePtr].Item1
                    ));
                    if(inputRangePtr < inputRange.Count && 
                        meceRange.Item2 == inputRange[inputRangePtr].Item2)
                    {
                        inputRangePtr++;
                    }
                }
                distributions.Add(distribution);
            }
            return distributions;
        }

        // creates a complementary range such that when
        // combined with the input list, will cover the full range
        // in a MECE way
        public List<(double, double)> CreateComplementaryRanges(
            (double, double) fullRange,
            List<(double, double)> range
        )
        {
            if(range.Count == 0)
            {
                return new List<(double, double)>
                {
                    (fullRange.Item1, fullRange.Item2)
                };
            }
            var startingIndex = range[0].Item1 == fullRange.Item1
                ? 1 : 0;
            var complementaryRange = new List<(double, double)>();
            for(int i=startingIndex; i<range.Count; i++)
            {
                complementaryRange.Add(
                    ((i > 0 ? range[i - 1].Item2 : fullRange.Item1),
                    range[i].Item1)
                );
            }
            if(range.Last().Item2 != fullRange.Item2)
            {
                complementaryRange.Add((range.Last().Item2, fullRange.Item2));
            }
            return complementaryRange;
        }
    }
}
