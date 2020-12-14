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
