using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Riddles.Combinatorics.Core.SetGeneration;
using System.Linq;

namespace Riddles.Tests.Probability.SetGeneration
{
	public class OutcomeGeneratorTest
	{
		[TestCase(1, 1)]
		[TestCase(3, 2)]
		[TestCase(2, 3)]
		[TestCase(5, 3)]
		public void TestGenerateAllPossibleGroupAssignmentsForDistinctGroupsAndDistinctMembers(int numOutcomes, int numTrials)
		{
			var outcomeGenerator = new OutcomeGenerator();
			var outcomes = outcomeGenerator.GenerateAllOutcomes(numTrials, numOutcomes, '1');
			Assert.AreEqual(outcomes.Count, Math.Pow(numOutcomes, numTrials));
			var maxScore = 0;
			// note, this only works for n < 10
			foreach(var outcome in outcomes)
			{
				var score = int.Parse(outcome.Aggregate("", (score, i) => $"{score}{i}"));
				Assert.Greater(score, maxScore);
				maxScore = score;
			}
		}
	}
}
