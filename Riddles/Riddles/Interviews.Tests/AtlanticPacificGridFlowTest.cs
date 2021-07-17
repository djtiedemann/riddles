using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Interviews;

namespace Interviews.Tests
{
	public class AtlanticPacificGridFlowTest
	{
		[Test]
		public void SolveAtlanticPacificFlowTest()
		{
			int[,] graph = new int[,] {
				{ 1, 2, 2, 3, 5 },
				{ 3, 2, 3, 4, 4 },
				{ 2, 4, 5, 3, 1 },
				{ 6, 7, 1, 4, 5 },
				{ 5, 1, 1, 2, 4 },
			};

			var atlanticPacificGridFlowSolver = new AtlanticPacificGridFlow();
			var solution = atlanticPacificGridFlowSolver.SolveAtlanticPacificFlow(graph);
		}
	}
}
