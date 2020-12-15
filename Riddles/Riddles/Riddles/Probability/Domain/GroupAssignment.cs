using System;
using System.Collections.Generic;
using System.Text;

namespace Riddles.Probability.Domain
{
	public class GroupAssignment
	{
		public GroupAssignment(List<GroupAssignmentMember> assignment)
		{
			this.Assignment = assignment;
		}

		public GroupAssignment DeepCopyGroupAssignment()
		{
			var newMembers = new List<GroupAssignmentMember>();
			foreach(var member in this.Assignment)
			{
				newMembers.Add(new GroupAssignmentMember(member.MemberId, member.GroupId));
			}
			return new GroupAssignment(newMembers);
		}

		public List<GroupAssignmentMember> Assignment { get; }
	}

	public class GroupAssignmentMember
	{
		public GroupAssignmentMember(int memberId, int groupId)
		{
			this.MemberId = memberId;
			this.GroupId = groupId;
		}

		public int MemberId { get; }
		public int GroupId { get; set; }
	}
}
