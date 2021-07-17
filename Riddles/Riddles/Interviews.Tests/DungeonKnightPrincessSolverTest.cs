using System;
using System.Collections.Generic;
using System.Text;
using Interviews;
using NUnit.Framework;

namespace Interviews.Tests
{
	public class DungeonKnightPrincessSolverTest
	{
		[Test]
		public void TestCalculateMinimumHP()
		{
			var dungeon = new int[][] { 
				new int[] { -2, -3, 3 },
				new int[] { -5, -10, 1 },
				new int[] { 10, 30, -5} 
			};
			dungeon = new int[][] {
				new int[] { -2, -1, -5 },
				new int[] { -10, -1, -7 },
				new int[] { 100, -1, -30 }
			};
			var dungeonKnightPrincessSolver = new DungeonKnightPrincessSolver();
			var result = dungeonKnightPrincessSolver.CalculateMinimumHP(dungeon);
		}
	}
}
