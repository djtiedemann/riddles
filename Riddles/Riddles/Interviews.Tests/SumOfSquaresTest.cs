using System;
using System.Collections.Generic;
using System.Text;
using Interviews;
using NUnit.Framework;

namespace Interviews.Tests
{
	public class SumOfSquaresTest
	{
		[TestCase(12)]
		public void TestFindMinimumNumberOfSquaresSummingTo(int n)
		{
			var squareSolver = new SumOfSquares();
			var numSquares = squareSolver.FindMinimumNumberOfSquaresSummingTo(n);
		}
	}
}
