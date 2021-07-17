using System;
using System.Collections.Generic;
using System.Text;
using Interviews;
using NUnit.Framework;

namespace Interviews.Tests
{
	public class NQueenProblemSolver
	{
		[TestCase(4)]
		public void TestFindQueenPlacements(int boardSize)
		{
			var nQueenProblem = new NQueenProblem();
			var solutions = nQueenProblem.FindQueenPlacements(boardSize);
		}
	}
}
