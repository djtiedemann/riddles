using System;
using System.Collections.Generic;
using System.Text;
using Riddles.Combinatorics.SetGeneration;
using System.Linq;

namespace Riddles.Probability
{
	// https://fivethirtyeight.com/features/can-you-bat-299-in-299-games/
	public class TrianglesFromRandomThreadsSolver
	{
		private PasscodeGenerator _passcodeGenerator;
		public TrianglesFromRandomThreadsSolver()
		{
			this._passcodeGenerator = new PasscodeGenerator();
		}

		public double GetProbabilityOfBeingAbleToFormTriangle(int[] threadLengths) {
			var firstChar = '0';
			var threadLengthMappingDictionary = threadLengths.Select((length, index) => new { key = (char)(firstChar + index), length })
				.ToDictionary(k => k.key, k => k.length);
			var possibleCombinations = this._passcodeGenerator.GenerateAllPasscodes(3, threadLengths.Length, firstChar);
			var possibleCombinationsOfThreads = possibleCombinations.Select(c => c.Select(i => threadLengthMappingDictionary[i]).ToList()).ToList();

			var numCombinations = possibleCombinations.Count();
			var combinationsThatFormTriangle = possibleCombinationsOfThreads.Where(p => {
				var orderedThreads = p.OrderByDescending(c => c).ToList();
				return orderedThreads[0] < orderedThreads[1] + orderedThreads[2];
			}).ToList();

			return (double)combinationsThatFormTriangle.Count() / numCombinations;
		}
	}
}
