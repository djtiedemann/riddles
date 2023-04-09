using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riddles.LinearAlgebra
{
    public class TransformationUtil
    {
        public double TransformFractionToUniformDistribution(
            double fraction,
            List<(double, double)> ranges)
        {
            var rangeSize = ranges.Sum(x => x.Item2 - x.Item1);
            var scaledFraction = fraction * rangeSize;

            foreach(var range in ranges)
            {
                var currentRangeSize = range.Item2 - range.Item1;
                if(scaledFraction > currentRangeSize)
                {
                    scaledFraction -= currentRangeSize;
                }
                else
                {
                    return range.Item1 + scaledFraction;
                }
            }
            return ranges.Last().Item2;
        }
    }
}
