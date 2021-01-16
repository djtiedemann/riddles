using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NUnit.Framework;
using Riddles.Optimization;

namespace Riddles.Tests.Optimization
{
	public class MysteriousHuntNumberTest
	{
		private Dictionary<int, int[,]> testCaseDictionary = new Dictionary<int, int[,]> {
			{ 1, new int[,] { { 3, 9, 2}, {4, 5, 3} }}
		};

		//[TestCase(1, new int[] { 12, 45, 6 }, new int[] { 54, 60 }, true)]
		//[TestCase(1, new int[] { 12, 45, 6 }, new int[] { 54, 120 }, false)]
		//[TestCase(1, new int[] { 12, 45, 6 }, new int[] { 36, 60 }, false)]
		//[TestCase(1, new int[] { 8, 45, 6 }, new int[] { 54, 60 }, false)]
		//[TestCase(1, new int[] { 12, 45, 18 }, new int[] { 54, 60 }, false)]
		public void TestIsSolutionValid(int solutionId, int[] bottomRowSolutions, int[] rightSideSolutions, bool isValid)
		{
			var mysteriousHuntNumberSolver = new MysteriousNumberHunt();
			var solution = testCaseDictionary[solutionId];
			var isSolutionFinal = mysteriousHuntNumberSolver.isSolutionFinal(solution, bottomRowSolutions, rightSideSolutions);
			Assert.AreEqual(isValid, isSolutionFinal);
		}

		//[TestCase(1, new int[] { 12, 45, 6 }, new int[] { 54, 60 })]
		[TestCase(1, new int[] { 8_890_560, 156_800, 55_566 }, new int[] { 294, 216, 135, 98, 112, 84, 245, 40})]
		public void TestFindSolution(int solutionId, int[] bottomRowSolutions, int[] rightSideSolutions)
		{
			var mysteriousHuntNumberSolver = new MysteriousNumberHunt();
			var solution = mysteriousHuntNumberSolver.SolveMysteryNumbers(bottomRowSolutions, rightSideSolutions);
		}
	}
}
