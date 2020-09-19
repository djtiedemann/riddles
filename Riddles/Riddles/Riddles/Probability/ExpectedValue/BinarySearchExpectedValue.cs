using System;
using System.Collections.Generic;
using System.Text;

namespace Riddles.Probability.ExpectedValue
{
	public class BinarySearchExpectedValue
	{
		public BinarySearchExpectedValue()
		{

		}

		public double CalculateNumExpectedGuesses(int numOptions)
		{
			var worstCaseNumGuesses = (int)Math.Ceiling(Math.Log2(numOptions + 1));
			var expectedValueRunningTotal = 0.0;
			var numOptionsRemaining = numOptions;
			for (int guessNum = 1; guessNum <= worstCaseNumGuesses; guessNum++)
			{
				var numberOfChoicesCoveredByExactlyNGuesses = (int)Math.Min(Math.Pow(2, guessNum - 1), numOptionsRemaining);
				expectedValueRunningTotal += numberOfChoicesCoveredByExactlyNGuesses * guessNum;
				numOptionsRemaining -= numberOfChoicesCoveredByExactlyNGuesses;
			}
			return expectedValueRunningTotal / (double) numOptions;
		}
	}
}
