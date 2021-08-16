using System;
using System.Collections.Generic;
using System.Text;
using Riddles.Combinatorics.SetGeneration;
using System.Linq;

namespace Riddles.Optimization
{
	// https://fivethirtyeight.com/features/are-you-clever-enough/
	public class DiceRerollOptimizer
	{
		private OutcomeGenerator _outcomeGenerator;
		public DiceRerollOptimizer() {
			_outcomeGenerator = new OutcomeGenerator();
		}

		/// <summary>
		/// You want to optimize your score when rolling N dice. You roll all of the dice, and after the roll, you must freeze at least 1 dice.
		/// You may reroll any number of the other dice that you choose. What is the maximum expected value you can acheive in this situation.
		/// </summary>
		public double GetExpectedValueOfRerollingDice(int numDice, int numSides) { 
			if(numDice <= 0 || numSides <= 0)
			{
				return 0;
			}

			Dictionary<int, double> numDiceToExpectedValuePerDieDictionary = new Dictionary<int, double>();
			for (int i = 1; i <= numDice; i++) {
				var outcomes = this._outcomeGenerator.GenerateAllOutcomes(i, numSides, '1');
				var diceRolls = this.TransformOutcomesToDiceRolls(outcomes);
				numDiceToExpectedValuePerDieDictionary[i] = this.GetExpectedValueInternal(diceRolls, i, numDiceToExpectedValuePerDieDictionary);
			}
			return numDiceToExpectedValuePerDieDictionary[numDice] * numDice;
		}

		private double GetExpectedValueInternal(List<List<int>> diceRolls, int numDice, Dictionary<int, double> memo) {
			double rollingSumOfExpectedValues = 0;
			int numDiceRolls = diceRolls.Count;
			foreach (var diceRoll in diceRolls) {
				var expectedValue = this.CalculateExpectedValueOfCurrentDiceRoll(diceRoll, numDice, memo);
				rollingSumOfExpectedValues += expectedValue;
			}

			return (rollingSumOfExpectedValues / numDiceRolls) / numDice;
		}

		public double CalculateExpectedValueOfCurrentDiceRoll(List<int> diceRoll, int numDice, Dictionary<int, double> memo)
		{
			var orderedDiceRoll = diceRoll.OrderByDescending(d => d).ToList();
			double expectedValue = orderedDiceRoll[0];
			for (int i = 1; i < orderedDiceRoll.Count; i++)
			{
				int numDiceRemaining = orderedDiceRoll.Count - i;
				if (orderedDiceRoll[i] > memo[numDiceRemaining])
				{
					expectedValue += orderedDiceRoll[i];
				}
				else
				{
					expectedValue += memo[numDiceRemaining] * numDiceRemaining;
					break;
				}
			}
			return expectedValue;
		}

		private List<List<int>> TransformOutcomesToDiceRolls(List<string> outcomes) {
			return outcomes.Select(outcome => outcome.ToCharArray().Select(roll => (roll - '1') + 1).ToList()).ToList();
		}
	}
}
