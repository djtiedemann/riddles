using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Interviews;

namespace Interviews.Tests
{
	public class UniquePathSolverTest
	{
		[Test]
		public void TestUniquePathsWithObstacles()
		{
			var grid = new int[,]
			{
				{ 1, 1, 1, },
				{ 1, 0, 1, },
				{ 1, 1, 1, }
			};
			var uniquePathSolver = new UniquePathSolver();
			var numPaths = uniquePathSolver.UniquePathsWithObstacles(grid);
		}
	}
}
