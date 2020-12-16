using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Riddles.Probability.Domain;
using Riddles.Probability;
using System.Linq;

namespace Riddles.Tests.Probability
{
	public class GroupAssignmentGeneratorTest
	{
		[TestCase(1, 1)]
		[TestCase(3, 2)]
		[TestCase(2, 3)]
		[TestCase(5, 3)]
		public void TestGenerateAllPossibleGroupAssignmentsForDistinctGroupsAndDistinctMembers(int numMembers, int numGroups)
		{
			var groupAssignmentGenerator = new GroupAssignmentGenerator();
			var groupAssignments = groupAssignmentGenerator
				.GenerateAllPossibleGroupAssignmentsForDistinctGroupsAndDistinctMembers(numMembers, numGroups, 1);
			Assert.AreEqual(groupAssignments.Count, Math.Pow(numGroups, numMembers));
			var maxScore = 0;
			// note, this only works for n < 10
			foreach(var assignment in groupAssignments)
			{
				var score = int.Parse(assignment.Assignment.Aggregate("", (score, i) => $"{score}{i.GroupId}"));
				Assert.Greater(score, maxScore);
				maxScore = score;
			}
		}
	}
}
