using System;
using System.Collections.Generic;
using System.Text;

namespace Interviews
{
	public class PrerequisiteSolver
	{
		public bool CanCreateCourseSchedule(List<Prerequisite> prerequisites, int numCourses) {
			HashSet<int> coursesWeHaveNotFoundAWayToTake = new HashSet<int> { };
			for (int i = 0; i < numCourses; i++) {
				coursesWeHaveNotFoundAWayToTake.Add(i);
			}
			while (true)
			{
				HashSet<int> coursesWeCannotTakeYet = new HashSet<int> { };
				foreach(var prerequisite in prerequisites)
				{
					if (coursesWeHaveNotFoundAWayToTake.Contains(prerequisite.PrerequisiteNum) 
						&& !coursesWeCannotTakeYet.Contains(prerequisite.CourseNum)) {
						coursesWeCannotTakeYet.Add(prerequisite.CourseNum);
					}
				}
				var numCoursesWeCouldNotTakeAtStartOfSemester = coursesWeHaveNotFoundAWayToTake.Count;
				coursesWeHaveNotFoundAWayToTake.IntersectWith(coursesWeCannotTakeYet);
				var numCoursesWeCannotTakeNow = coursesWeHaveNotFoundAWayToTake.Count;

				if (numCoursesWeCannotTakeNow == numCoursesWeCouldNotTakeAtStartOfSemester)
				{
					return false;
				}
				if (numCoursesWeCannotTakeNow == 0) {
					return true;
				}
			}
			return false;
		}

		public class Prerequisite
		{
			public int CourseNum { get; set; }
			public int PrerequisiteNum { get; set; }
		}


	}
}
