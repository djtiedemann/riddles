using System;
using System.Collections.Generic;
using System.Text;

namespace Riddles.Probability.Domain
{
	public class GroupAssignment
	{
		public GroupAssignment(GroupAssignmentMember[] assignment)
		{
			this.Assignment = assignment;
		}

		public GroupAssignment DeepCopyGroupAssignment()
		{
			var newMembers = new GroupAssignmentMember[this.Assignment.Length];
			for (int i = 0; i < this.Assignment.Length; i++)
			{
				newMembers[i] = new GroupAssignmentMember(this.Assignment[i].MemberId, this.Assignment[i].GroupId);
			}
			return new GroupAssignment(newMembers);
		}

		public override int GetHashCode()
		{
			int hash = 17;
			hash = hash * 23 + Assignment.GetHashCode();
			return hash;
		}

		public GroupAssignmentMember[] Assignment { get; }
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
