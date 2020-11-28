using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Riddles.Probability;

namespace Riddles.Tests.Probability
{
	public class RandomCranberrySaucePasserTest
	{
		private Dictionary<int, double[]> NumSeatsToProbabilityDistributionDictionary = new Dictionary<int, double[]>
		{
			{2, new double[] {0.0, 1.0} },
			{3, new double[] {0.0, 0.5, 0.5} },
			{4, new double[] {0.0, 1.0/3.0, 1.0/3.0, 1.0/3.0 } },
			{5, new double[] {0.0, 1.0/4.0, 1.0/4.0, 1.0/4.0, 1.0/4.0 } },
			{6, new double[] {0.0, 1.0/5.0, 1.0/5.0, 1.0/5.0, 1.0/5.0, 1.0/5.0 } },
			{20, new double[] {0.0, 1.0/19.0, 1.0/19.0, 1.0 / 19.0, 1.0 / 19.0, 1.0 / 19.0, 1.0 / 19.0, 1.0 / 19.0, 1.0 / 19.0, 1.0 / 19.0,
				1.0/19.0, 1.0/19.0, 1.0/19.0, 1.0 / 19.0, 1.0 / 19.0, 1.0 / 19.0, 1.0 / 19.0, 1.0 / 19.0, 1.0 / 19.0, 1.0 / 19.0} }
		};

		[TestCase(2)]
		[TestCase(3)]
		[TestCase(4)]
		[TestCase(5)]
		[TestCase(6)]
		[TestCase(20)]
		public void TestFindProbabilitiesOfEachPersonBeingLastToReceiveCranberrySauce(int numSeatsAtTable)
		{
			double epsilon = 0.0001;
			var cranberrySaucePasser = new RandomCranberrySaucePasser();
			var actualDistribution = cranberrySaucePasser.FindProbabilitiesOfEachPersonBeingLastToReceiveCranberrySauce(numSeatsAtTable);
			var expectedDistribution = this.NumSeatsToProbabilityDistributionDictionary[numSeatsAtTable];
			Assert.AreEqual(expectedDistribution.Length, actualDistribution.Length);
			for (int i = 0; i < actualDistribution.Length; i++)
			{
				Assert.LessOrEqual(Math.Abs(expectedDistribution[i] - actualDistribution[i]), epsilon);
			}
		}
	}
}
