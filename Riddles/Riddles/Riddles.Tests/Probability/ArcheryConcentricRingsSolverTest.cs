using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Riddles.Probability;

namespace Riddles.Tests.Probability
{
	public class ArcheryConcentricRingsSolverTest
	{
		double epsilon = 0.0001;

		[TestCase(1, 2.0)]
		[TestCase(2, 2.1875)]
		[TestCase(3, 2.3045267)]
		[TestCase(4, 2.3805084228515625)]
		[TestCase(5, 2.433220608)]
		[TestCase(10, 2.558528499476374)]
		//[TestCase(100, 2.7003985518559346)]
		//[TestCase(1000, 2.7164720537641456)]
		//[TestCase(10000, 2.7181006338299238)]
		public void TestCalculateExpectedValueOfPointsForNRings(int numRings, double expectedExpectedValue)
		{
			var archeryConcentricRingSolver = new ArcheryConcentricRingsSolver();
			var actualExpectedValue = archeryConcentricRingSolver.CalculateExpectedValueOfPointsForNRings(numRings);
			Assert.LessOrEqual(Math.Abs(expectedExpectedValue - actualExpectedValue), epsilon);
		}
	}
}
