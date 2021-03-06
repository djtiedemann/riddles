using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Riddles.Combinatorics.SetGeneration;
using System.Linq;

namespace Riddles.Tests.Probability.SetGeneration
{
	public class PasscodeGeneratorTest
	{
		[TestCase(1, 1)]
		[TestCase(3, 2)]
		[TestCase(2, 3)]
		[TestCase(5, 3)]
		public void TestGenerateAllPossibleGroupAssignmentsForDistinctGroupsAndDistinctMembers(int sizeOfCharacterSet, int passwordLength)
		{
			var passcodeGenerator = new PasscodeGenerator();
			var passcodes = passcodeGenerator.GenerateAllPasscodes(passwordLength, sizeOfCharacterSet, '1');
			Assert.AreEqual(passcodes.Count, Math.Pow(sizeOfCharacterSet, passwordLength));
			var maxScore = 0;
			// note, this only works for n < 10
			foreach(var passcode in passcodes)
			{
				var score = int.Parse(passcode.Aggregate("", (score, i) => $"{score}{i}"));
				Assert.Greater(score, maxScore);
				maxScore = score;
			}
		}
	}
}
