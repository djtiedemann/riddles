using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Interviews;

namespace Interviews.Tests
{
	public class CombinationSumInArrayTest
	{
		[TestCase(new int[] { 1, 2, 3}, 4, 7)]
		public void TestFindNumberOfCombinationsSummingToValue(int[] searchArray, int target, int expected)
		{
			var numberOfCombinationsSummingToValueSolver = new CombinationSumInArray();
			var actual = numberOfCombinationsSummingToValueSolver.FindNumberOfCombinationsSummingToValue(searchArray, target);
			Assert.AreEqual(expected, actual);
		}
	}
}
