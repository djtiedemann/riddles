using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Interviews;

namespace Interviews.Tests
{
	public class FractionToRecurringDecimalSolverTest
	{
		[TestCase(8, 4, "2")]
		[TestCase(800, 4, "200")]
		[TestCase(-800, 4, "-200")]
		[TestCase(-800, -4, "200")]
		[TestCase(800, -4, "-200")]
		[TestCase(7, 1000, "0.007")]
		[TestCase(30, 4, "7.5")]
		[TestCase(3, 4, "0.75")]
		[TestCase(50, 8, "6.25")]
		[TestCase(10, 3, "3.(3)")]
		[TestCase(10, 7, "1.(428571)")]
		[TestCase(5, 6, "0.8(3)")]
		public void TestGetRecurringDecimalRepresentation(int numerator, int denominator, string expectedResult) {
			var fractionSolver = new FractionToRecurringDecimal();
			var actualResult = fractionSolver.GetRecurringDecimalRepresentation(numerator, denominator);
			Assert.AreEqual(expectedResult, actualResult);
		}
	}
}
