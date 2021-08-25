using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Riddles.Probability.DistributionGenerators;

namespace Riddles.Tests.Probability.DistributionGenerators
{
	public class CumulativeSumOfIndependentTrialsDistributionGeneratorTest
	{
		double epsilon = 0.000001;

		private Dictionary<int, Dictionary<int, double>> initialDistribution = new Dictionary<int, Dictionary<int, double>>
		{
			{1,  new Dictionary<int, double>{
				{ 1, 1.0/6.0 },
				{ 2, 1.0/6.0 },
				{ 3, 1.0/6.0 },
				{ 4, 1.0/6.0 },
				{ 5, 1.0/6.0 },
				{ 6, 1.0/6.0 },
			} },
			{2, new Dictionary<int, double>{
				{ 0, 0.75 },
				{ 1, 0.25 }
			} },
			{3, new Dictionary<int, double>{
				{ 0, 0.25 },
				{ 1, 0.75 }
			} }
		};

		private Dictionary<int, Dictionary<int, double>> outcomeDistribution = new Dictionary<int, Dictionary<int, double>>
		{
			{1,  new Dictionary<int, double>{
				{ 2, 1.0/36.0 },
				{ 3, 2.0/36.0 },
				{ 4, 3.0/36.0 },
				{ 5, 4.0/36.0 },
				{ 6, 5.0/36.0 },
				{ 7, 6.0/36.0 },
				{ 8, 5.0/36.0 },
				{ 9, 4.0/36.0 },
				{ 10, 3.0/36.0 },
				{ 11, 2.0/36.0 },
				{ 12, 1.0/36.0 },
			} },
			{2,  new Dictionary<int, double>{
				{ 1, 1.0/6.0 },
				{ 2, 1.0/6.0 },
				{ 3, 1.0/6.0 },
				{ 4, 1.0/6.0 },
				{ 5, 1.0/6.0 },
				{ 6, 1.0/6.0 },
			} },
			{3, new Dictionary<int, double> { } },
			{4, new Dictionary<int, double> {
				{ 0, 0.31640625 },
				{ 1, 0.421875 },
				{ 2, 0.2109375 },
				{ 3, 0.046875 },
				{ 4, 0.00390625 }
			} },
			{5, new Dictionary<int, double> {
				{ 0, 0.00390625 },
				{ 1, 0.046875 },
				{ 2, 0.2109375 },
				{ 3, 0.421875 },
				{ 4, 0.31640625 }
			} },
		};

		[TestCase(1, 1, 2, Description = "fair 6 sided dice rolled twice")]
		[TestCase(1, 2, 1, Description = "fair 6 sided dice rolled once")]
		[TestCase(1, 3, 0, Description = "0 trials")]
		[TestCase(2, 4, 4, Description = "Coin biased towards 0, flipped 4 times")]
		[TestCase(3, 5, 4, Description = "Coin biased towards 1, flipped 4 times")]
		public void TestGetOutcomeProbabilityDistribution(int inputId, int outputId, int numTrials)
		{
			var inputDistribution = initialDistribution[inputId];
			var expectedDistribution = outcomeDistribution[outputId];

			var distributionGenerator = new CumulativeSumOfIndependentTrialsDistributionGenerator();
			var actualDistribution = distributionGenerator.GetOutcomeProbabilityDistribution(inputDistribution, numTrials);
			Assert.AreEqual(expectedDistribution.Keys.Count, actualDistribution.Keys.Count);
			foreach(var key in actualDistribution.Keys)
			{
				Assert.IsTrue(expectedDistribution.ContainsKey(key));
				Assert.LessOrEqual(Math.Abs(actualDistribution[key] - expectedDistribution[key]), epsilon);
			}
		}
	}
}
