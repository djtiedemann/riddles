using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Riddles.Combinatorics.Core.SetGeneration
{
	public class OutcomeGenerator
	{
		public OutcomeGenerator()
		{

		}

		public List<string> GenerateAllOutcomes(int numTrials, int numOutcomes, char firstOutcome)
		{
			if (numTrials <= 0 || numOutcomes <= 0)
			{
				return null;
			}

			List<string> outcomes = new List<string>();
			var initialOutcome = new String(firstOutcome, numTrials);
			var lastOutcome = (char)(firstOutcome + (numOutcomes - 1));

			var currentPasscode = initialOutcome;
			outcomes.Add(currentPasscode);
			while (currentPasscode != null)
			{
				currentPasscode = this.GenerateNextOutcome(currentPasscode, firstOutcome, lastOutcome);
				if (currentPasscode != null)
				{
					outcomes.Add(currentPasscode);
				}
			}
			return outcomes;
		}

		private string GenerateNextOutcome(string currentOutcome, char firstOutcome, char lastOutcome)
		{
			var currentOutcomeAsCharArray = currentOutcome.ToCharArray();

			// this is the last passcode if every character is the last character
			if (currentOutcomeAsCharArray.All(c => c == lastOutcome))
			{
				return null;
			}

			for (int i = currentOutcomeAsCharArray.Length - 1; i >= 0; i--)
			{
				if (currentOutcomeAsCharArray[i] != lastOutcome)
				{
					currentOutcomeAsCharArray[i]++;
					for (int j = i + 1; j < currentOutcomeAsCharArray.Length; j++)
					{
						currentOutcomeAsCharArray[j] = firstOutcome;
					}
					break;
				}
			}
			var nextOutcome = currentOutcomeAsCharArray.Aggregate("", (agg, c) => $@"{agg}{c}");
			return nextOutcome;
		}
	}
}
