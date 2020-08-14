using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Riddles.Words
{
	public class ConsecutiveLetterSolver
	{
		public List<string> GetWordsWithNConsecutiveLetters(string dictionaryFileName, int numLetters)
		{
			List<string> wordsWithNConsecutiveLetters = new List<string>();
			using (StreamReader sr = new StreamReader(dictionaryFileName))
			{
				while (sr.Peek() >= 0)
				{
					var nextWord = sr.ReadLine();
					if(this.DoesWordHaveNConsecutiveLetters(nextWord, numLetters))
					{
						wordsWithNConsecutiveLetters.Add(nextWord);
					}
				}
			}
			return wordsWithNConsecutiveLetters;
		}

		public bool DoesWordHaveNConsecutiveLetters(string word, int numLetters)
		{
			var currentStreak = 0;
			var maxStreak = 0;
			for (int i=0; i<word.Length; i++)
			{
				if (i == 0 || word[i] - word[i - 1] != 1)
				{
					currentStreak = 1;
				}
				else
				{
					currentStreak = currentStreak + 1;
				}
				if(currentStreak > maxStreak)
				{
					maxStreak = currentStreak;
				}
			}
			return maxStreak >= numLetters;
		}
	}
}
