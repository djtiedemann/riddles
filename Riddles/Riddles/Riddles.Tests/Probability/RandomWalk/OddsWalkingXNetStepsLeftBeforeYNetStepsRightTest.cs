using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Riddles.Probability.RandomWalk;

namespace Riddles.Tests.Probability.RandomWalk
{
	public class OddsWalkingXNetStepsLeftBeforeYNetStepsRightTest
	{
		private Dictionary<int, double[]> _testCaseToProbabilityDistribution = new Dictionary<int, double[]>
		{
			{1, new double[] { 0.5 } },
			{2, new double[] { 2.0/3.0, 1.0/3.0 } },
			{3, new double[] { 3.0/4.0, 1.0/2.0, 1.0/4.0} },
			{4, new double[] { 4.0/5.0, 3.0 / 5.0, 2.0/5.0, 1.0/5.0 } },
			{9, new double[] { 0.9, 0.8, 0.7, 0.6, 0.5, 0.4, 0.3, 0.2, 0.1 } },
			{18, new double[] { 18.0/19.0, 17.0/19.0, 16.0/19.0, 15.0/19.0, 14.0/19.0, 13.0/19.0, 12.0/19.0,
				11.0/19.0, 10.0/19.0, 9.0/19.0, 8.0/19.0, 7.0/19.0, 6.0/19.0, 5.0/19.0, 4.0/19.0, 3.0/19.0, 2.0/19.0, 1.0/19.0} }
		};

		[TestCase(1)]
		[TestCase(2)]
		[TestCase(3)]
		[TestCase(9)]
		[TestCase(18)]
		public void TestOddsWalkingXNetStepsLeftBeforeYNetStepsRight(int numStepsWithinRange)
		{
			double epsilon = 0.0001;
			var oddsWalkingXNetStepsLeftBeforeYNetStepsRight = new OddsWalkingXNetStepsLeftBeforeYNetStepsRight();
			var actualSolution = oddsWalkingXNetStepsLeftBeforeYNetStepsRight.GetProbabilityDistributionForARangeOfNSteps(numStepsWithinRange);
			var expectedSolution = this._testCaseToProbabilityDistribution[numStepsWithinRange];
			Assert.AreEqual(expectedSolution.Length, actualSolution.Length);
			for(int i=0; i<expectedSolution.Length; i++)
			{
				Assert.LessOrEqual(Math.Abs(expectedSolution[i] - actualSolution[i]), epsilon);
			}
		}
	}
}
