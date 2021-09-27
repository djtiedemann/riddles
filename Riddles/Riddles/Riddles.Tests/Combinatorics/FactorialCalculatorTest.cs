using System;
using System.Collections.Generic;
using System.Text;
using Riddles.Combinatorics.Core;
using NUnit.Framework;

namespace Riddles.Tests.Combinatorics
{
	public class FactorialCalculatorTest
	{
		[TestCase(1, 1)]
		[TestCase(2, 2)]
		[TestCase(3, 6)]
		[TestCase(4, 24)]
		[TestCase(5, 120)]
		[TestCase(6, 720)]
		public void TestCalculateFactorial(int n, int expectedFactorial)
		{
			var factorialCalculator = new FactorialCalculator();
			var actualFactorial = factorialCalculator.Factorial(n);
			Assert.AreEqual(expectedFactorial, actualFactorial);
		}
	}
}
