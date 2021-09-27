using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Riddles.NumberTheory.Core;

namespace Riddles.Tests.NumberTheory
{
	public class SquaresUtilityTest
	{
		[TestCase(-1, null)]
		[TestCase(4, null)]
		[TestCase(0, 0)]
		[TestCase(1, 1)]
		[TestCase(3, 4)]
		[TestCase(5, 9)]
		[TestCase(7, 16)]
		[TestCase(9, 25)]
		[TestCase(11, 36)]
		public void TestFindSquareBySummingOddNumbersUpTo(int n, long? expectedSquare)
		{
			var squaresUtility = new SquaresUtility();
			var square = squaresUtility.FindSquareBySummingOddNumbersUpTo(n);
			Assert.AreEqual(expectedSquare, square);
		}
	}
}
