using System;
using System.Collections.Generic;
using System.Text;

namespace Interviews
{
	public class DistinctSubsequencesSolver
	{
		public int FindNumDistinctSubsequences(string search, string subsequence) {
			return this.FindNumDistinctSubsequencesInternal(search, subsequence, new Dictionary<Tuple<string, string>, int>());
		}

		private int FindNumDistinctSubsequencesInternal(string search, string subsequence, Dictionary<Tuple<string, string>, int> memo) {
			var searchSubsequenceTuple = Tuple.Create(search, subsequence);
			if (memo.ContainsKey(searchSubsequenceTuple)) {
				return memo[searchSubsequenceTuple];
			}
			if (search == subsequence) {
				return 1;
			}
			if (subsequence.Length == 0) {
				return 1;
			}

			if (search.Length < subsequence.Length ) {
				return 0;
			}
			// if the first letter doesn't match, chop it off from the search string and continue
			if (search[0] != subsequence[0]) {
				memo[searchSubsequenceTuple] = this.FindNumDistinctSubsequencesInternal(search.Substring(1), subsequence, memo);
				return memo[searchSubsequenceTuple];
			}
			// if the first letter does match, then the answer is:
			// the number of ways to find distinct subsequences by chopping off the first letter of the search
			// plus the number of ways to find distinct subsequences by chopping off the first letter of the search and the subsequence
			memo[searchSubsequenceTuple] =
				this.FindNumDistinctSubsequencesInternal(search.Substring(1), subsequence.Substring(1), memo)
				+ this.FindNumDistinctSubsequencesInternal(search.Substring(1), subsequence, memo);
			return memo[searchSubsequenceTuple];
		}
	}
}
