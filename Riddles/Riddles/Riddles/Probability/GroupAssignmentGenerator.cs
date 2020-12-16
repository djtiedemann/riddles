using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Riddles.Probability.Domain;

namespace Riddles.Probability
{
	public class GroupAssignmentGenerator
	{
		public GroupAssignmentGenerator()
		{

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
			if(numMembers <= 0 || numGroups <= 0)
			{
				return null;
			}
			List<GroupAssignment> assignments = new List<GroupAssignment>();
			var initialGroupAssignmentInternal = new GroupAssignmentMember[numMembers];
			for(int i=0; i< initialGroupAssignmentInternal.Length; i++)
			{
				initialGroupAssignmentInternal[i] = new GroupAssignmentMember(i+firstMemberId, 1);
			}
			var initialGroupAssignment = new GroupAssignment(initialGroupAssignmentInternal);
			assignments.Add(initialGroupAssignment);
			var currentAssignment = initialGroupAssignment;
			while (currentAssignment != null)
			{
				currentAssignment = this.GenerateNextGroupAssignment(currentAssignment, numMembers, numGroups);
				if(currentAssignment != null)
				{
					assignments.Add(currentAssignment);
				}
			}
			return assignments;
		}

		private GroupAssignment GenerateNextGroupAssignment(GroupAssignment currentAssignment, int numMembers, int numGroups)
		{
			var currentAssignmentInternal = currentAssignment.Assignment.ToArray();
			// this is the last assignment if all members are assigned to the last group
			if(currentAssignmentInternal.All(a => a.GroupId == numGroups))
			{
				return null;
			}
			var nextAssignment = new GroupAssignmentMember[numMembers];
			// first start by copying over the group assignment
			for (int i=0; i<currentAssignmentInternal.Length; i++)
			{
				nextAssignment[i] = new GroupAssignmentMember(currentAssignmentInternal[i].MemberId, currentAssignmentInternal[i].GroupId);
			}

			for(int i=nextAssignment.Length - 1; i>=0; i--)
			{
				if(nextAssignment[i].GroupId != numGroups)
				{
					nextAssignment[i].GroupId++;
					for(int j=i+1; j<nextAssignment.Length; j++)
					{
						nextAssignment[j].GroupId = 1;
					}
					break;
				}
			}
			return new GroupAssignment(nextAssignment);
		}
	}	
}
