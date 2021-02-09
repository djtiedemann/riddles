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
			{ 1, new int[,] { { 3, 9, 2 }, {4, 5, 3 } }},
			{ 2, new int[,]{ {7, 7, 6 }, { 9, 8, 3}, { 9, 5, 3 }, { 7, 2, 7 }, { 8, 2, 7 }, { 7, 4, 3 }, { 5, 7, 7 }, { 8, 5, 1 } } },
			{ 3, new int[,] { { 3, 9, 5 }, { 5, 9, 1 }, { 8, 1, 8 }, { 5, 7, 8 }, { 5, 7, 2 }, } },
		};

		[TestCase(1, new int[] { 12, 45, 6 }, new int[] { 54, 60 }, true)]
		[TestCase(1, new int[] { 12, 45, 6 }, new int[] { 54, 120 }, false)]
		[TestCase(1, new int[] { 12, 45, 6 }, new int[] { 36, 60 }, false)]
		[TestCase(1, new int[] { 8, 45, 6 }, new int[] { 54, 60 }, false)]
		[TestCase(1, new int[] { 12, 45, 18 }, new int[] { 54, 60 }, false)]
		public void TestIsSolutionValid(int solutionId, int[] bottomRowSolutions, int[] rightSideSolutions, bool isValid)
		{
			var mysteriousHuntNumberSolver = new MysteriousNumberHunt();
			var solution = testCaseDictionary[solutionId];
			var isSolutionFinal = mysteriousHuntNumberSolver.isSolutionFinal(solution, bottomRowSolutions, rightSideSolutions);
			Assert.AreEqual(isValid, isSolutionFinal);
		}

		
		[TestCase(2, new int[] { 8_890_560, 156_800, 55_566 }, new int[] { 294, 216, 135, 98, 112, 84, 245, 40})]
		[TestCase(3, new int[] { 3_000, 3_969, 640}, new int[] { 135, 45, 64, 280, 70})]
		public void TestFindSolution(int solutionId, int[] bottomRowSolutions, int[] rightSideSolutions)
		{
			var mysteriousHuntNumberSolver = new MysteriousNumberHunt();
			var solution = mysteriousHuntNumberSolver.SolveMysteryNumbers(bottomRowSolutions, rightSideSolutions);
			var expectedSolution = testCaseDictionary[solutionId];
			Assert.AreEqual(expectedSolution.GetLength(0), solution.GetLength(0));
			Assert.AreEqual(expectedSolution.GetLength(1), solution.GetLength(1));
			for(int i=0; i<solution.GetLength(0); i++)
			{
				for(int j=0; j<expectedSolution.GetLength(1); j++)
				{
					Assert.AreEqual(expectedSolution[i, j], solution[i, j]);
				}
			}
		}
	}
}
