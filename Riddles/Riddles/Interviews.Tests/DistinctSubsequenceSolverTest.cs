using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Interviews;

namespace Interviews.Tests
{
	public class DistinctSubsequenceSolverTest
	{
		[TestCase("babgbag", "bag")]
		public void TestFindNumDistinctSubsequences(string searchString, string subsequence) {
			var distinctSubsequencesSolver = new DistinctSubsequencesSolver();
			var numUniqueSubsequences = distinctSubsequencesSolver.FindNumDistinctSubsequences(searchString, subsequence);
		}
	}
}
