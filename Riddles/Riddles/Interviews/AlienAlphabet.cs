using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Interviews
{
	public class AlienAlphabet
	{
		private char _delimeter;
		public AlienAlphabet() {
			this._delimeter = '|';
		}
		public AlienAlphabet(char delimeter) {
			this._delimeter = delimeter;
		}

		public string GetLetterOrder(List<string> wordsInAlphabeticalOrder) {
			var letters = new HashSet<char>();
			foreach (var word in wordsInAlphabeticalOrder) {
				foreach (var letter in word) {
					if (!letters.Contains(letter)) {
						letters.Add(letter);
					}
				}
			}
			var letterDependencies = letters.ToDictionary(l => l, l => new List<char>());
			var letterOrder = "";
			this.SetLetterDependencies(wordsInAlphabeticalOrder, letterDependencies, 0);
			letterOrder = this.PerformTopologicalSort(letterDependencies, letterOrder, this._delimeter);
			return letterOrder;
		}

		private string PerformTopologicalSort(Dictionary<char, List<char>> letterDependencies, string letterOrder, char delimeter) {
			var letters = letterDependencies.Keys;
			if (letters.Count() == 0) {
				return letterOrder;
			}

			var lettersWithDependencies = new HashSet<char>();
			foreach (var key in letterDependencies.Keys) {
				foreach (var letter in letterDependencies[key]) {
					if (!lettersWithDependencies.Contains(letter)) {
						lettersWithDependencies.Add(letter);
					}
				}
			}
			var lettersWithNoDependencies = letters.Except(lettersWithDependencies).ToList();
			// if topological sort fails to find a letter with no dependencies, we have a circular dictionary
			if (lettersWithNoDependencies.Count() == 0) {
				letterOrder = null;
				return letterOrder;
			}

			if (letterOrder == "")
			{
				letterOrder = $"{letterOrder}{new string(lettersWithNoDependencies.OrderBy(l => l).ToArray())}";
			}
			else {
				letterOrder = $"{letterOrder}{delimeter}{new string(lettersWithNoDependencies.OrderBy(l => l).ToArray())}";
			}

			foreach (var key in lettersWithNoDependencies) {
				letterDependencies.Remove(key);
			}
			return this.PerformTopologicalSort(letterDependencies, letterOrder, delimeter);
		}

		private void SetLetterDependencies(List<string> words, Dictionary<char, List<char>> letterDependencies, int letterBeingCompared) {
			List<char> lettersObserved = new List<char>();
			words = words.Where(w => w.Length > letterBeingCompared).ToList();
			if (words.Count() <= 1) {
				return;
			}
			foreach(var word in words) {
				if (!lettersObserved.Contains(word[letterBeingCompared]))
				{
					lettersObserved.Add(word[letterBeingCompared]);
				}
			}
			
			// update dictionary for the letter we're currently comparing
			for(int currentLetter=0; currentLetter < lettersObserved.Count(); currentLetter++)
			{
				var unionedList = letterDependencies[lettersObserved[currentLetter]]
					.Union(lettersObserved.Where((letter, index) => index > currentLetter).ToList()).ToList();
				letterDependencies[lettersObserved[currentLetter]] = unionedList;
				
			}

			// need to recursively update the dictionary for the next letter in the word
			var wordsSplitByCurrentLetter = words.GroupBy(w => w[letterBeingCompared]).Select(g => g.ToList()).ToList();
			letterBeingCompared++;
			foreach (var wordSet in wordsSplitByCurrentLetter) {
				this.SetLetterDependencies(wordSet, letterDependencies, letterBeingCompared);
			}
		}
	}
}
