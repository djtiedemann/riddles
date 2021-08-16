using System;
using System.Collections.Generic;
using System.Text;
using Riddles.Combinatorics.SetGeneration;
using System.Linq;

namespace Riddles.Optimization
{
	public class ContinuousLockSolver
	{
		private const char FirstCharacterToProcess = '0';

		private OutcomeGenerator _outcomeGenerator;
		// problem: https://fivethirtyeight.com/features/what-are-the-odds-world-cup-teams-play-each-other-twice/
		// answer: https://fivethirtyeight.com/features/how-fast-can-you-deliver-pbjs-how-many-meerkats-can-survive/
		public ContinuousLockSolver()
		{
			this._outcomeGenerator = new OutcomeGenerator();
		}

		public string FindShortestStringContainingAllPossiblePasscodes(int lengthOfPasscode, int numDigitsOnPadlock)
		{
			var allPossiblePasscodes = this._outcomeGenerator.GenerateAllOutcomes(lengthOfPasscode, numDigitsOnPadlock, FirstCharacterToProcess);
			var digitsOnPadlock = Enumerable.Range(0, numDigitsOnPadlock).Select(digit => (char)(FirstCharacterToProcess + digit)).ToArray();
			Dictionary<char, char?> nextDigitToProcess = new Dictionary<char, char?>();
			for(int i=0; i<numDigitsOnPadlock; i++)
			{
				nextDigitToProcess[digitsOnPadlock[i]] = i < numDigitsOnPadlock - 1 ? digitsOnPadlock[i + 1] : (char?)null;
			}

			var totalNumberOfPasscodes = (int)Math.Pow(numDigitsOnPadlock, lengthOfPasscode);
			
			foreach (var startingPasscode in allPossiblePasscodes)
			{
				var passcodesHashSet = allPossiblePasscodes.ToHashSet();
				passcodesHashSet.Remove(startingPasscode);
				var currentPasscodeString = startingPasscode;

				var stateToProcess = new CalculationState
				{
					CurrentString = currentPasscodeString,
					RemainingPasscodes = passcodesHashSet,
					NumPasscodesRemaining = totalNumberOfPasscodes - 1
				};

				while (stateToProcess != null && !stateToProcess.IsTerminalState)
				{
					stateToProcess = this.GetNextStateToProcess(stateToProcess, lengthOfPasscode, digitsOnPadlock, nextDigitToProcess);
				}
				if (stateToProcess.IsTerminalState)
				{
					return stateToProcess.CurrentString;
				}
			}
			return null;
		}

		private CalculationState GetNextStateToProcess(CalculationState currentState, 
			int lengthOfPasscode, 
			char[] digitsOnPadlock,
			Dictionary<char, char?> nextDigitToProcess)
		{
			// first see if it's possible to continue with this state
			foreach(var digit in digitsOnPadlock)
			{
				var nextPasscodeAdded = $"{currentState.CurrentString.Substring(currentState.CurrentString.Length - lengthOfPasscode + 1)}{digit}";
				if (currentState.RemainingPasscodes.Contains(nextPasscodeAdded))
				{
					var newRemainingPasscodes = currentState.RemainingPasscodes;
					newRemainingPasscodes.Remove(nextPasscodeAdded);
					var newPasscodeString = $"{currentState.CurrentString}{digit}";
					return new CalculationState { 
						CurrentString = newPasscodeString,
						RemainingPasscodes = newRemainingPasscodes,
						NumPasscodesRemaining = currentState.NumPasscodesRemaining - 1
					};
				}
			}

			// if it's not possible to continue with the current state, go back until we find a state where can continue
			bool canContinueWithCurrentState = false;
			var updatedPasscodeStringForNextState = currentState.CurrentString;
			var updatedRemainingPasscodes = currentState.RemainingPasscodes;
			var updatedNumPasscodesRemaining = currentState.NumPasscodesRemaining;
			while (true)
			{
				var lastPasscodeAdded = updatedPasscodeStringForNextState.Substring(updatedPasscodeStringForNextState.Length - lengthOfPasscode);
				var lastDigitAdded = lastPasscodeAdded[lastPasscodeAdded.Length - 1];

				// remove the most recent digit added to the string because it was incorrect
				updatedRemainingPasscodes.Add(lastPasscodeAdded);				
				updatedPasscodeStringForNextState = updatedPasscodeStringForNextState.Substring(0, updatedPasscodeStringForNextState.Length - 1);
				updatedNumPasscodesRemaining += 1;

				// try each digit we haven't tried before. If we reach a branch that could be correct, add that digit and continue processing
				var nextDigit = nextDigitToProcess[lastDigitAdded];
				while (nextDigit != null)
				{
					var nextPasscodeAdded = 
						$"{updatedPasscodeStringForNextState.Substring(updatedPasscodeStringForNextState.Length - lengthOfPasscode + 1)}{nextDigit}";
					if (updatedRemainingPasscodes.Contains(nextPasscodeAdded))
					{
						updatedRemainingPasscodes.Remove(nextPasscodeAdded);
						updatedPasscodeStringForNextState = $"{updatedPasscodeStringForNextState}{nextDigit}";
						updatedNumPasscodesRemaining -= 1;
						return new CalculationState
						{
							CurrentString = updatedPasscodeStringForNextState,
							RemainingPasscodes = updatedRemainingPasscodes,
							NumPasscodesRemaining = updatedNumPasscodesRemaining
						};
					}
					nextDigit = nextDigitToProcess[nextDigit.Value];
				}

				// if no digits are valid, we'll need to remove the previous digit as well
				// first check if we've exhausted the current initial password. If so, return null and try another
				if(updatedPasscodeStringForNextState.Length <= lengthOfPasscode)
				{
					return null;
				}
			}
		}

		private class CalculationState
		{
			public string CurrentString	{ get; set; }		
			public HashSet<string> RemainingPasscodes { get; set; }
			public int NumPasscodesRemaining { get; set; }

			public bool IsTerminalState
			{
				get { return this.NumPasscodesRemaining == 0; }
			}
		}
	}
}
