﻿using System;
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
			{ 4, new int[,] { { 7, 6, 5}, { 9, 8, 2}, { 3, 9, 2}, { 5, 9, 3}, { 1, 4, 1}, { 7, 1, 7 } } },
			{ 5, new int[,] { { 7, 8, 5 }, { 3, 8, 7 }, { 9, 6, 3 }, { 9, 8, 5 }, { 3, 5, 4 }, { 4, 8, 8 }, { 9, 2, 7 } } },
			{ 6, new int[,] { { 3, 6, 9, 9 }, { 8, 8, 2, 1 }, { 8, 8, 7, 6 }, { 5, 1, 5, 5 } } }
		};

		
		[TestCase(2, new int[] { 8_890_560, 156_800, 55_566 }, new int[] { 294, 216, 135, 98, 112, 84, 245, 40})]
		[TestCase(3, new int[] { 3_000, 3_969, 640}, new int[] { 135, 45, 64, 280, 70})]
		[TestCase(4, new int[] { 6_615, 15_552, 420 }, new int[] { 210, 144, 54, 135, 4, 49 })]
		[TestCase(5, new int[] { 183_708, 245_760, 117_600 }, new int[] { 280, 168, 162, 360, 60, 256, 126})]
		[TestCase(6, new int[] { 960, 384, 630, 270 }, new int[] { 1_458, 128, 2_688, 125 })]
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
