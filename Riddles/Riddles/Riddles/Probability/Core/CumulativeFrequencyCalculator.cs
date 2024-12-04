using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riddles.Probability.Core
{
    public class CumulativeFrequencyCalculator
    {
        public Dictionary<int, double> CalculateCumulativeFrequency(
            Dictionary<int, double> frequencies,
            // used to allow cumulative distributions that are ascending
            // or descending
            Func<int, int, bool> comparator
        )
        {
            return frequencies.ToDictionary(
                x => x.Key,
                x => frequencies.Keys.Sum(
                    k => comparator(k, x.Key) ? frequencies[k] : 0
                )
            );
        }
    }
}
