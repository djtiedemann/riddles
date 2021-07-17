using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Interviews;

namespace Interviews.Tests
{
	public class LongestSubstringWithoutRepeatingCharactersTest
	{
		[TestCase("abcabcbb", 3)]
		[TestCase("bbbb", 1)]
		[TestCase("pwwkew", 3)]
		public void TestFindLongestSubstringWithoutRepeatingCharacters(string searchString, int expectedAnswer)
		{
			var longestSubstringWithoutRepeatingCharactersSolver = new LongestSubstringWithoutRepeatingCharacters();
			var actualAnswer = longestSubstringWithoutRepeatingCharactersSolver.FindLongestSubstringWithoutRepeatingCharacters(searchString);
			Assert.AreEqual(expectedAnswer, actualAnswer);
		}
	}
}
