using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Riddles.Probability.ExpectedValue;

namespace Riddles.Tests.Probability.ExpectedValue
{
	public class VarianceCalculatorTest
	{
		const double Epsilon = 0.000001;

		[TestCase(new double[] { 0.1, 0.2, 0.3, 0.4 }, 0.01666666667)]
		[TestCase(new double[] { 0.25, 0.25, 0.25, 0.25 }, 0)]
		[TestCase(new double[] { 1.0, 0.0, 0.0, 0.0 }, 0.25)]
		[TestCase(new double[] { 0.0, 0.0, 0.0, 1.0 }, 0.25)]
		[TestCase(new double[] { 1.0 }, null)]
		[TestCase(new double[] { }, null)]
		public void TestCalculateVarianceOfProbabilityDistribution(double[] probabilityDistribution, double? expectedVariance)
		{
			var varianceCalculator = new VarianceCalculator();
			var actualVariance = varianceCalculator.CalculateVarianceOfProbabilityDistribution(probabilityDistribution);
			if (expectedVariance != null || actualVariance != null) {
				Assert.LessOrEqual(Math.Abs(actualVariance.Value - expectedVariance.Value), Epsilon);
			}
		}
	}
}
