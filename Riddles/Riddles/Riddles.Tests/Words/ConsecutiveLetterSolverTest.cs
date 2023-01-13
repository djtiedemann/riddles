using NUnit.Framework;
using Riddles.Words;
using System;
using System.Collections.Generic;
using System.Text;

namespace Riddles.Tests.Words
{
	public class ConsecutiveLetterSolverTest
	{
		//[Test]
		// Note: need to add this file to the repository
		public void FindAnswer()
		{
			var consecutiveLetterSolver = new ConsecutiveLetterSolver();
			var actualAnswer = consecutiveLetterSolver.GetWordsWithNConsecutiveLetters(@"C:\Users\djt74_000\Projects\riddles\Riddles\Riddles\Riddles.Tests\Words\enable1dictionary.txt", 4);
		}

		[TestCase("hijab", 2, true)]
		[TestCase("hijab", 3, true)]
		[TestCase("hijab", 4, false)]
		[TestCase("nope", 3, true)]
		[TestCase("nope", 4, false)]
		public void TestWordHasNConsecutiveLetters(string word, int numConsecutiveLetters, bool expectedAnswer)
		{
			var consecutiveLetterSolver = new ConsecutiveLetterSolver();
			var actualAnswer = consecutiveLetterSolver.DoesWordHaveNConsecutiveLetters(word, numConsecutiveLetters);
			Assert.AreEqual(expectedAnswer, actualAnswer);
		}
	}
}
