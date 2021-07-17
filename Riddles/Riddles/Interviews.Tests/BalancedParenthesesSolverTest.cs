using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Interviews;

namespace Interviews.Tests
{
	public class BalancedParenthesesSolverTest
	{
		[Test]
		public void FindLongestBalancedParenthesisLength()
		{
			var parentheses = ")(()))((()())()(()()))";
			var parenthesesSolver = new BalancedParenthesesSolver();
			var answer = parenthesesSolver.FindLongestBalancedParenthesisLength(parentheses);
		}

		[Test]
		public void TestFindMatchedParenthesesScore()
		{
			var parentheses = "(()(()())(()))";
			var parenthesesSolver = new BalancedParenthesesSolver();
			var answer = parenthesesSolver.FindMatchedParenthesesScore(parentheses);
		}
	}
}
