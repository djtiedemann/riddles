using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Interviews;

namespace Interviews.Tests
{
	public class MaxAreaRectangleSolverTest
	{
		[Test]
		public void TestFindMaxiumumSizeRectangle()
		{
			var matrix = new int[,] {
				{ 1, 0, 1, 0, 0 },
				{ 1, 0, 1, 1, 1 },
				{ 1, 1, 1, 1, 1 },
				{ 1, 0, 0, 1, 0 },
			};
			var maximumSizeRectangleSolver = new MaximumAreaRectangleSolver();
			var area = maximumSizeRectangleSolver.FindMaxiumumSizeRectangle(matrix);
		}
	}
}
