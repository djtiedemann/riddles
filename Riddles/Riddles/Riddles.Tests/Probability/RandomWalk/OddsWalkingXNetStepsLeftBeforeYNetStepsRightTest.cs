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
			{3, new double[] { 3.0/4.0, 1.0/2.0, 1.0/4.0} }
		};

		[TestCase(1)]
		[TestCase(2)]
		[TestCase(3)]
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
