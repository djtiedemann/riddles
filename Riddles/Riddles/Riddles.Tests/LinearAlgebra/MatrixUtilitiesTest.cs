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
			} }
		};

		[TestCase(1)]
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
