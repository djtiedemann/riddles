using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Interviews;

namespace Interviews.Tests
{
	public class MaximumAreaSquareTest
	{
		[Test]
		public void TestMaximumAreaSquare() {
			var matrix = new int[,]
			{
				{ 1, 1, 0, 0, 1, 1, 1},
				{ 1, 0, 0, 0, 0, 1, 1},
				{ 1, 1, 1, 1, 1, 1, 1},
				{ 0, 0, 1, 1, 1, 0, 1},
				{ 1, 1, 1, 1, 1, 1, 1},
			};

			var maximumAreaSquareSolver = new MaximumAreaSquare();
			var result = maximumAreaSquareSolver.FindMaximumAreaSquare(matrix);
		}
	}
}
