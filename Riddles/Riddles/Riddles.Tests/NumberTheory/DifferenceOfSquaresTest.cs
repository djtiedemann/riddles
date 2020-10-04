using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Riddles.NumberTheory;

namespace Riddles.Tests.NumberTheory
{
	public class DifferenceOfSquaresTest
	{
		[TestCase(11, 5, true)]
		[TestCase(9, 3, true)]
		[TestCase(17, 1, true)]
		[TestCase(8, 2, true)]
		[TestCase(26, 6, true)]
		public void TestGetPairOfSquaresWithCriteria(int averageValue, int numSquaresTotal, bool shouldHaveValue)
		{
			var differenceOfSquaresUtility = new DifferenceOfSquares();
			var squares = differenceOfSquaresUtility.GetPairOfSquaresWithCriteria(averageValue, numSquaresTotal);
			if (!shouldHaveValue)
			{
				Assert.IsNull(squares);
				return;
			}
			Assert.AreEqual(averageValue * numSquaresTotal, squares.LargerSquare - squares.SmallerSquare);
		}
	}
}
