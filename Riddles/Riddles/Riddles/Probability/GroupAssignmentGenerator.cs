using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Riddles.Probability.Domain;
using Riddles.Probability.SetGeneration;

namespace Riddles.Probability
{
	public class GroupAssignmentGenerator
	{
		private PasscodeGenerator _passcodeGenerator;
		public GroupAssignmentGenerator()
		{
			this._passcodeGenerator = new PasscodeGenerator();
		}

		/// <summary>
		/// Assigns members to groups in all possible ways. Groups do not have to have equal number of members.
		/// Both members and groups are distinctive
		/// </summary>
		/// <param name="numMembers"></param>
		/// <param name="numGroups"></param>
		/// <returns></returns>
		public List<GroupAssignment> GenerateAllPossibleGroupAssignmentsForDistinctGroupsAndDistinctMembers(int numMembers, int numGroups, int firstMemberId)
		{
			var passcodes = this._passcodeGenerator.GenerateAllPasscodes(numMembers, numGroups, '1');

			return passcodes.Select(p =>
			{
				var groupAssignmentMembers = p.Select((character, index) => new GroupAssignmentMember(index + firstMemberId, (character - '1') + 1)).ToArray();
				return new GroupAssignment(groupAssignmentMembers);
			}).ToList();
		}
	}	
}
