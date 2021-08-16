using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Riddles.Optimization;
using NUnit.Framework;

namespace Riddles.Tests.Optimization
{
	public class DiceRerollOptimizerTest
	{
		double epsilon = 0.000001;

		[TestCase(1, 8, 4.5)]
		[TestCase(1, 6, 3.5)]
		[TestCase(2, 6, 4.11805556)]
		[TestCase(3, 6, 4.4749657064471906)]
		[TestCase(4, 6, 4.700422001219243)]
		[TestCase(5, 6, 4.8496710045584317)]
		[TestCase(6, 6, 4.9538701332341306)]
		public void TestGetExpectedValueOfRerollingDice(int numDice, int numSides, double expectedExpectedValue) {
			var diceRerollOptimizer = new DiceRerollOptimizer();
			var actualExpectedValue = diceRerollOptimizer.GetExpectedValueOfRerollingDice(numDice, numSides) / numDice;
			Assert.LessOrEqual(Math.Abs(actualExpectedValue - expectedExpectedValue), epsilon);
		}

		[TestCase(new int[] { 4, 5, 6}, 15)]
		[TestCase(new int[] { 6, 4, 5 }, 15)]
		[TestCase(new int[] { 3, 6, 5}, 14.5)]
		[TestCase(new int[] { 3, 5, 6 }, 14.5)]
		[TestCase(new int[] { 5, 3, 6 }, 14.5)]
		[TestCase(new int[] { 5, 6, 3 }, 14.5)]
		[TestCase(new int[] { 6, 3, 5 }, 14.5)]
		[TestCase(new int[] { 6, 5, 3 }, 14.5)]
		[TestCase(new int[] { 1, 6, 5 }, 14.5)]
		[TestCase(new int[] { 1, 5, 6 }, 14.5)]
		[TestCase(new int[] { 5, 1, 6 }, 14.5)]
		[TestCase(new int[] { 5, 6, 1 }, 14.5)]
		[TestCase(new int[] { 6, 1, 5 }, 14.5)]
		[TestCase(new int[] { 6, 5, 1 }, 14.5)]
		[TestCase(new int[] { 1, 6, 4 }, 14.23611112)]
		[TestCase(new int[] { 1, 4, 6 }, 14.23611112)]
		[TestCase(new int[] { 4, 1, 6 }, 14.23611112)]
		[TestCase(new int[] { 4, 6, 1 }, 14.23611112)]
		[TestCase(new int[] { 6, 1, 4 }, 14.23611112)]
		[TestCase(new int[] { 6, 4, 1 }, 14.23611112)]
		public void TestCalculateExpectedValueOfCurrentDiceRoll(int[] diceRoll, double expectedExpectedValue)
		{
			var diceRerollOptimizer = new DiceRerollOptimizer();
			var memo = new Dictionary<int, double> { { 1, 3.5 }, { 2, 4.11805556 } };
			var actualExpectedValue = diceRerollOptimizer.CalculateExpectedValueOfCurrentDiceRoll(diceRoll.ToList(), diceRoll.Length, memo);
			Assert.LessOrEqual(Math.Abs(actualExpectedValue - expectedExpectedValue), epsilon);
		}
	}
}
