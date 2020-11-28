using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Riddles.Probability;

namespace Riddles.Tests.Probability
{
	public class ChocolateSolverTest
	{
		private Dictionary<int, Dictionary<ChocolateType, int>> initialChocolateCounts = new Dictionary<int, Dictionary<ChocolateType, int>>
		{
			{ 1, new Dictionary<ChocolateType, int> { { ChocolateType.Milk, 2}, { ChocolateType.Dark, 1} } },
			{ 2, new Dictionary<ChocolateType, int> { { ChocolateType.Milk, 2}, { ChocolateType.Dark, 2} } },
			{ 3, new Dictionary<ChocolateType, int> { { ChocolateType.Milk, 3}, { ChocolateType.Dark, 1} } },
			{ 4, new Dictionary<ChocolateType, int> { { ChocolateType.Milk, 4}, { ChocolateType.Dark, 1} } },
			{ 5, new Dictionary<ChocolateType, int> { { ChocolateType.Milk, 3}, { ChocolateType.Dark, 2} } },
			{ 6, new Dictionary<ChocolateType, int> { { ChocolateType.Milk, 3}, { ChocolateType.Dark, 3} } },
			{ 7, new Dictionary<ChocolateType, int> { { ChocolateType.Milk, 2}, { ChocolateType.Dark, 8} } },
			{ 8, new Dictionary<ChocolateType, int> { { ChocolateType.Milk, 1}, { ChocolateType.Dark, 1} } },
			{ 9, new Dictionary<ChocolateType, int> { { ChocolateType.Milk, 2}, { ChocolateType.Dark, 1} } },
			{ 10, new Dictionary<ChocolateType, int> { { ChocolateType.Milk, 2}, { ChocolateType.Dark, 1} } },
			{ 11, new Dictionary<ChocolateType, int> { { ChocolateType.Milk, 2}, { ChocolateType.Dark, 2} } },
		};

		private Dictionary<int, Dictionary<ChocolateType, double>> expectedProbabilitiesForEachChocolateType
			= new Dictionary<int, Dictionary<ChocolateType, double>>
		{
			{ 1, new Dictionary<ChocolateType, double>{ { ChocolateType.Milk, 0.5}, { ChocolateType.Dark, 0.5} } },
			{ 2, new Dictionary<ChocolateType, double>{ { ChocolateType.Milk, 0.5}, { ChocolateType.Dark, 0.5} } },
			{ 3, new Dictionary<ChocolateType, double>{ { ChocolateType.Milk, 0.5}, { ChocolateType.Dark, 0.5} } },
			{ 4, new Dictionary<ChocolateType, double>{ { ChocolateType.Milk, 0.5}, { ChocolateType.Dark, 0.5} } },
			{ 5, new Dictionary<ChocolateType, double>{ { ChocolateType.Milk, 0.5}, { ChocolateType.Dark, 0.5} } },
			{ 6, new Dictionary<ChocolateType, double>{ { ChocolateType.Milk, 0.5}, { ChocolateType.Dark, 0.5} } },
			{ 7, new Dictionary<ChocolateType, double>{ { ChocolateType.Milk, 0.5}, { ChocolateType.Dark, 0.5} } },
			{ 8, new Dictionary<ChocolateType, double>{ { ChocolateType.Milk, 1.0/4.0}, { ChocolateType.Dark, 3.0/4.0} } },
			{ 9, new Dictionary<ChocolateType, double>{ { ChocolateType.Milk, 1.0/3.0}, { ChocolateType.Dark, 2.0/3.0} } },
			{ 10, new Dictionary<ChocolateType, double>{ { ChocolateType.Milk, 2.0/3.0}, { ChocolateType.Dark, 1.0/3.0} } },
			{ 11, new Dictionary<ChocolateType, double>{ { ChocolateType.Milk, 5.0/12.0}, { ChocolateType.Dark, 7.0/12.0} } },
		};

		[TestCase(1, null)]
		[TestCase(2, null)]
		[TestCase(3, null)]
		[TestCase(4, null)]
		[TestCase(5, null)]
		[TestCase(6, null)]
		[TestCase(7, null)]
		[TestCase(8, ChocolateType.Milk)]
		[TestCase(9, ChocolateType.Milk)]
		[TestCase(10, ChocolateType.Dark)]
		[TestCase(11, ChocolateType.Milk)]

		public void TestGetOddsOfEachChocolateTypeBeingLastChocolateDrawn(int testCaseId, ChocolateType? lastChocolateDrawn)
		{
			double epsilon = 0.0001;
			var initialChocolateCounts = this.initialChocolateCounts[testCaseId];
			var chocolateSolver = new ChocolateSolver();
			var actualProbabilitiesForEachChocolateType = chocolateSolver.GetOddsOfEachChocolateTypeBeingLastChocolateDrawn(
				initialChocolateCounts, lastChocolateDrawn);
			var expectedProbabilitiesForEachChocolateType = this.expectedProbabilitiesForEachChocolateType[testCaseId];
			Assert.AreEqual(actualProbabilitiesForEachChocolateType.Keys.Count, expectedProbabilitiesForEachChocolateType.Keys.Count);
			foreach(var chocolateType in actualProbabilitiesForEachChocolateType.Keys)
			{
				Assert.IsTrue(expectedProbabilitiesForEachChocolateType.ContainsKey(chocolateType));
				Assert.IsTrue(actualProbabilitiesForEachChocolateType.ContainsKey(chocolateType));
				Assert.LessOrEqual(Math.Abs(expectedProbabilitiesForEachChocolateType[chocolateType] 
					- actualProbabilitiesForEachChocolateType[chocolateType]), epsilon);
			}
		}
	}
}
