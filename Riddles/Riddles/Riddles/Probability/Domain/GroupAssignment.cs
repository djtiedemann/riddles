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

		public override bool Equals(object obj)
		{
			if (!(obj is GroupAssignment))
			{
				return false;
			}
			var otherGroupAssignment = (GroupAssignment)obj;
			if(otherGroupAssignment.Assignment.Length != this.Assignment.Length)
			{
				return false;
			}
			for(int i=0; i<this.Assignment.Length; i++)
			{
				if (!this.Assignment[i].Equals(otherGroupAssignment.Assignment[i]))
				{
					return false;
				}
			}
			return true;
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

		public override bool Equals(object obj)
		{
			if (!(obj is GroupAssignmentMember))
			{
				return false;
			}
			var otherGroupAssignment = (GroupAssignmentMember)obj;
			return this.MemberId == otherGroupAssignment.MemberId
				&& this.GroupId == otherGroupAssignment.GroupId;
		}

		public override int GetHashCode()
		{
			int hash = 17;
			hash = hash * 23 + MemberId.GetHashCode();
			hash = hash * 23 + GroupId.GetHashCode();
			return hash;
		}
	}
}
