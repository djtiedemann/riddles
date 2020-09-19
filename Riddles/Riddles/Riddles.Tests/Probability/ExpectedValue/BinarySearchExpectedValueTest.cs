using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Riddles.Probability.ExpectedValue;

namespace Riddles.Tests.Probability.ExpectedValue
{
	public class BinarySearchExpectedValueTest
	{
		public double Epsilon = 0.000001;

		[TestCase(1, 1)]
		[TestCase(2, 1.5)]
		[TestCase(3, 1.666667)]
		[TestCase(4, 2)]
		[TestCase(5, 2.2)]
		[TestCase(6, 2.33333333)]
		[TestCase(7, 2.42857143)]
		[TestCase(8, 2.625)]
		[TestCase(267_751, 17.041956892784714)]
		public void TestCalculateNumExpectedGuesses(int numOptions, double expectedExpectedValue)
		{
			var binarySearchExpectedValueSolver = new BinarySearchExpectedValue();
			var expectedValue = binarySearchExpectedValueSolver.CalculateNumExpectedGuesses(numOptions);
			Assert.LessOrEqual(Math.Abs(expectedExpectedValue - expectedValue), this.Epsilon);
		}
	}
}
