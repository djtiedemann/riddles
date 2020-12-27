using System;
using System.Collections.Generic;
using System.Text;
using Riddles.Probability.SetGeneration;
using System.Linq;

namespace Riddles.Optimization
{
	public class ContinuousLockSolver
	{
		private PasscodeGenerator _passcodeGenerator;
		// problem: https://fivethirtyeight.com/features/what-are-the-odds-world-cup-teams-play-each-other-twice/
		// answer: https://fivethirtyeight.com/features/how-fast-can-you-deliver-pbjs-how-many-meerkats-can-survive/
		public ContinuousLockSolver()
		{
			this._passcodeGenerator = new PasscodeGenerator();
		}

		public string FindShortestStringContainingAllPossiblePasscodes(int lengthOfPasscode, int numDigitsOnPadlock)
		{
			var allPossiblePasscodes = this._passcodeGenerator.GenerateAllPasscodes(lengthOfPasscode, numDigitsOnPadlock, '0');
			var digitsOnPadlock = Enumerable.Range(0, numDigitsOnPadlock).Select(digit => (char)('0' + digit)).ToList();
			var totalNumberOfPasscodes = (int)Math.Pow(numDigitsOnPadlock, lengthOfPasscode);

			var passcodesHashSet = allPossiblePasscodes.ToHashSet();
			foreach(var startingPasscode in allPossiblePasscodes)
			{
				passcodesHashSet.Remove(startingPasscode);

				var shortestStringContainingAllPossiblePasscodesInternal = this.FindShortestStringContainingAllPossiblePasscodesInternal(
					currentString: startingPasscode,
					passcodesRemaining: passcodesHashSet,
					lengthOfPasscode: lengthOfPasscode,
					digitsOnPadlock: digitsOnPadlock,
					numPasscodesFound: 1,
					totalNumberOfPasscodes: totalNumberOfPasscodes
					);
				if(shortestStringContainingAllPossiblePasscodesInternal != null)
				{
					return shortestStringContainingAllPossiblePasscodesInternal;
				}
				passcodesHashSet.Add(startingPasscode);
			}
			return null;
		}

		private string FindShortestStringContainingAllPossiblePasscodesInternal(
			string currentString, 
			HashSet<string> passcodesRemaining, 
			int lengthOfPasscode,
			List<char> digitsOnPadlock,
			int numPasscodesFound,
			int totalNumberOfPasscodes)
		{
			if(numPasscodesFound == totalNumberOfPasscodes)
			{
				return currentString;
			}

			foreach(var digit in digitsOnPadlock)
			{
				var nextPassword = $"{currentString.Substring(currentString.Length - lengthOfPasscode + 1)}{digit}";
				if (passcodesRemaining.Contains(nextPassword))
				{
					passcodesRemaining.Remove(nextPassword);
					var solution = this.FindShortestStringContainingAllPossiblePasscodesInternal(
						currentString: $"{currentString}{digit}",
						passcodesRemaining: passcodesRemaining,
						lengthOfPasscode: lengthOfPasscode,
						digitsOnPadlock: digitsOnPadlock,
						numPasscodesFound: numPasscodesFound + 1,
						totalNumberOfPasscodes: totalNumberOfPasscodes
					);
					if(solution != null)
					{
						return solution;
					}
					passcodesRemaining.Add(nextPassword);
				}
			}
			return null;
		}
	}
}
