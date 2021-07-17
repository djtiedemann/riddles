using System;
using System.Collections.Generic;
using System.Text;
using Interviews;
using NUnit.Framework;

namespace Interviews.Tests
{
	public class CoinChangeSolverTest
	{
		[TestCase(11, new int[] { 1, 2, 5}, 3)]
		[TestCase(3, new int[] { 2 }, -1)]
		[TestCase(0, new int[] { 1 }, 0)]
		[TestCase(1, new int[] { 1 }, 1)]
		[TestCase(2, new int[] { 1 }, 2)]
		public void TestFindMinimumNumberOfCoins(int amount, int[] numCoins, int expected)
		{
			var coinChangeSolver = new CoinChangeSolver();
			var actual = coinChangeSolver.FindMinimumNumberOfCoins(amount, numCoins);
			Assert.AreEqual(expected, actual);
		}
	}
}
