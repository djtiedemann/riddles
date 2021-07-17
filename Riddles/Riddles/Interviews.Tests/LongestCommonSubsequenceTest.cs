using System;
using System.Collections.Generic;
using System.Text;
using Interviews;
using NUnit.Framework;

namespace Interviews.Tests
{
	public class LongestCommonSubsequenceTest
	{
		[TestCase("hieroglyphology", "michelangelo")]
		public void TestGetLongestCommonSubsequence(string x, string y)
		{
			var longestCommonSubsequenceSolver = new LongestCommonSubsequence();
			var result = longestCommonSubsequenceSolver.GetLongestCommonSubsequence(x, y);
		}
	}
}
