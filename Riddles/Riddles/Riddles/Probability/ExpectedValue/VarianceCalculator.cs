using System;
using System.Collections.Generic;
using System.Text;

namespace Riddles.Probability.ExpectedValue
{
	public class VarianceCalculator
	{
		// Calculates the variance of a probability distribution. We assume that the values of the probability distribution sum to 1
		public double? CalculateVarianceOfProbabilityDistribution(double[] probabilityDistribution) {
			if (probabilityDistribution.Length <= 1) { return null; }
			var averageProbability = 1.0 / probabilityDistribution.Length;
			var cumulativeVariance = 0.0;
			foreach(var probability in probabilityDistribution)
			{
				cumulativeVariance += Math.Pow(probability - averageProbability, 2);
			}
			return cumulativeVariance / (probabilityDistribution.Length - 1);
		}
	}
}
