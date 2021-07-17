using System;
using System.Collections.Generic;
using System.Text;
using Interviews;
using NUnit.Framework;

namespace Interviews.Tests
{
	public class MinimumWindowSubstringTest
	{
		[Test]
		public void TestFindMinimumWindowSubstring()
		{
			var searchString = "ADOBECODEBANC";
			var targetString = "ABC";
			var minimumWindowSubstringSolver = new MinimumWindowSubstring();
			var minimumWindowSubstring = minimumWindowSubstringSolver.FindMinimumWindowSubstring(searchString, targetString);
		}
	}
}
