using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Riddles.LinearAlgebra;

namespace Riddles.Tests.LinearAlgebra
{
	public class MatrixUtilitiesTest
	{
		private Dictionary<int, TestCase> TestCaseIdToMatrixDictionary = new Dictionary<int, TestCase>
		{
			{1, new TestCase
			{
				Equations = new double[][]
				{
					new double[]{ 2, 1 },
					new double[]{ 4, 1 }
				},
				Constants = new double[] {3, 5},
				Solution = new double[] {1, 1}
			} },
			{2, new TestCase
			{
				Equations = new double[][]
				{
					new double[]{ 1, 2, 1 },
					new double[]{ 3, 8, 1 },
					new double[]{ 0, 4, 1 },
				},
				Constants = new double[] {2, 12, 2},
				Solution = new double[] {2, 1, -2}
			} },
			{3, new TestCase
			{
				Equations = new double[][]
				{
					new double[]{ 0, 1, 1, 1 },
					new double[]{ 1, 0, 1, 1 },
					new double[]{ 1, 1, 0, 1 },
					new double[]{ 1, 1, 1, 0 },
				},
				Constants = new double[] {70, 75, 80, 75},
				Solution = new double[] {30, 25, 20, 25}
			} },
			{4, new TestCase
			{
				Equations = new double[][]
				{
					new double[]{ 0, 1, 1, 1 },
					new double[]{ 1, 1, 0, 1 },
					new double[]{ 1, 0, 1, 1 },					
					new double[]{ 1, 1, 1, 0 },
				},
				Constants = new double[] {70, 80, 75, 75},
				Solution = new double[] {30, 25, 20, 25}
			} },
			{5, new TestCase
			{
				Equations = new double[][]
				{
					new double[]{ 0, 0, 1 },
					new double[]{ 1, 0, 1 },
					new double[]{ 0, 1, 0 },					
				},
				Constants = new double[] {5, 7, 3},
				Solution = new double[] {2, 3, 5}
			} },
			{6, new TestCase
			{
				Equations = new double[][]
				{
					new double[]{ 2, 0, 0 },
					new double[]{ 4, 1, 0 },
					new double[]{ 6, 0, 1 },
				},
				Constants = new double[] {2, 5, 7},
				Solution = new double[] {1, 1, 1}
			} },
			{7, new TestCase
			{
				Equations = new double[][]
				{
					new double[]{ 1, 1 },
					new double[]{ 1, -0.5 },
					
				},
				Constants = new double[] {1, 0.5},
				Solution = new double[] {2.0/3.0, 1.0/3.0}
			} },
			{8, new TestCase
			{
				Equations = new double[][]
				{
					new double[]{ 1, 1 },
					new double[]{ -0.5, 1 },

				},
				Constants = new double[] {1, 0},
				Solution = new double[] {2.0/3.0, 1.0/3.0}
			} },
			{9, new TestCase
			{
				Equations = new double[][]
				{
					new double[]{ 1, 1, 1 },
					new double[]{ 1, -0.5, 0 },
					new double[]{ -0.5, 1, -0.5 },

				},
				Constants = new double[] {1.5, 0.5, 0},
				Solution = new double[] {0.75, 0.5, 0.25}
			} },
			{10, new TestCase
			{
				Equations = new double[][]
				{
					new double[]{ 1, 1, 1, 1 },
					new double[]{ 1, -0.5, 0, 0 },
					new double[]{ -0.5, 1, -0.5, 0 },
					new double[]{ 0, -0.5, 1, -0.5  },

				},
				Constants = new double[] {2, 0.5, 0, 0},
				Solution = new double[] {0.8, 0.6, 0.4, 0.2}
			} }
		};

		[TestCase(1, Description = "2x2 matrix, simple solution")]
		[TestCase(2, Description = "3x3 matrix, simple solution")]
		[TestCase(3, Description = "4x4 matrix, requires switching rows")]
		[TestCase(4, Description = "same as 3, but rows in different orders")]
		[TestCase(5, Description = "3x3 requires switching rows")]
		[TestCase(6, Description = "3x3 requires eliminating multiple rows at once")]
		[TestCase(7, Description = "Test case from randomly passing left or right")]
		[TestCase(8, Description = "Test case from randomly passing left or right")]
		[TestCase(9, Description = "Test case from randomly passing left or right - 3 steps")]
		[TestCase(10, Description = "Test case from randomly passing left or right - 4 steps")]
		public void TestSolveNEquationsNUnknowns(int testCaseId)
		{
			double epsilon = 0.0001;
			var matrixUtilities = new MatrixUtilities();
			var testCase = this.TestCaseIdToMatrixDictionary[testCaseId];
			var solution = matrixUtilities.SolveNEquationsNUnknowns(testCase.Equations, testCase.Constants);
			var resultFromMultiplication = matrixUtilities.MultiplyMatrixByVector(testCase.Equations, solution);
			Assert.AreEqual(testCase.Solution.Length, solution.Length);
			Assert.AreEqual(testCase.Constants.Length, resultFromMultiplication.Length);
			for (int i=0; i<solution.Length; i++)
			{
				Assert.LessOrEqual(Math.Abs(solution[i] - testCase.Solution[i]), epsilon);
				Assert.LessOrEqual(Math.Abs(resultFromMultiplication[i] - testCase.Constants[i]), epsilon);
			}
		}
	}	

	public class TestCase
	{
		public double[][] Equations { get; set; }
		public double[] Constants { get; set; }
		public double[] Solution { get; set; }
	}
}
