using NUnit.Framework;
using System.Collections.Generic;
using Interviews;

namespace Interviews.Tests
{
	public class AlienAlphabetTest
	{
		private Dictionary<int, List<string>> testCaseDictionary = new Dictionary<int, List<string>> {
				{ 1, new List<string> { "abacus", "admiral", "adrenaline", "batch", "random", "rank", "straw" } }
			};

		[TestCase(1, "acehilmnotuw|b|dr|ks")]
		public void TestGetLetterOrder(int testCaseId, string expectedResult)
		{
			var alienAlphabetSolver = new AlienAlphabet();
			var result = alienAlphabetSolver.GetLetterOrder(testCaseDictionary[testCaseId]);
			Assert.AreEqual(result, expectedResult);
		}
	}
}