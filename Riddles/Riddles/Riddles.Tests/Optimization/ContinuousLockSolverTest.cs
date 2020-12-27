using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Riddles.Optimization;
using System.Linq;

namespace Riddles.Tests.Optimization
{
	public class ContinuousLockSolverTest
	{
		[TestCase(1, 1)]
		[TestCase(1, 10)]
		[TestCase(3, 1)]
		[TestCase(2, 2)]
		[TestCase(3, 2)]
		[TestCase(3, 3)]
		public void TestFindShortestStringContainingAllPossiblePasscodes(int lengthOfPasscode, int numDigits)
		{
			var continuousLockSolver = new ContinuousLockSolver();
			var solution = continuousLockSolver.FindShortestStringContainingAllPossiblePasscodes(lengthOfPasscode, numDigits);
			var totalNumberOfPasscodes = (int)Math.Pow(numDigits, lengthOfPasscode);
			var expectedLengthOfSolution = totalNumberOfPasscodes + (lengthOfPasscode - 1);
			Assert.AreEqual(expectedLengthOfSolution, solution.Length);
			var areAllPasswordsUnique = this.AreAllPasscodesUnique(solution, lengthOfPasscode, totalNumberOfPasscodes);
			Assert.IsTrue(areAllPasswordsUnique);
		}

		//[TestCase("123123", 3, 4, false)]
		//[TestCase("112233", 2, 5, true)]
		public void TestAreAllPasscodesUnique(string passcode, int lengthOfPasscode, int totalNumberOfPasscodes, bool expectedValue)
		{
			var areAllPasswordsUnique = this.AreAllPasscodesUnique(passcode, lengthOfPasscode, totalNumberOfPasscodes);
			Assert.IsTrue(areAllPasswordsUnique);
		}

		private bool AreAllPasscodesUnique(string solution, int lengthOfPasscode, int totalNumberOfPasscodes)
		{
			HashSet<string> passcodesInString = new HashSet<string>();
			for(int i=0; i<totalNumberOfPasscodes; i++)
			{
				var passcode = solution.Substring(i, lengthOfPasscode);
				if (passcodesInString.Contains(passcode))
				{
					return false;
				}
			}
			return true;
		}
	}
}
