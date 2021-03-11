using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Interviews;

namespace Interviews.Tests
{
	public class PrerequisiteSolverTest
	{
		[Test]
		public void TestPrerequisiteSolver()
		{
			var prerequisites = new List<PrerequisiteSolver.Prerequisite> {
				new PrerequisiteSolver.Prerequisite { CourseNum = 5, PrerequisiteNum = 4},
				new PrerequisiteSolver.Prerequisite { CourseNum = 3, PrerequisiteNum = 1},
				new PrerequisiteSolver.Prerequisite { CourseNum = 3, PrerequisiteNum = 2},
			};
			var prerequisiteSovler = new PrerequisiteSolver();
			//var canTakeCourses = prerequisiteSovler.CanCreateCourseSchedule(prerequisites, 6);

			prerequisites = new List<PrerequisiteSolver.Prerequisite> {
				new PrerequisiteSolver.Prerequisite { CourseNum = 5, PrerequisiteNum = 4},
				new PrerequisiteSolver.Prerequisite { CourseNum = 2, PrerequisiteNum = 1},
				new PrerequisiteSolver.Prerequisite { CourseNum = 3, PrerequisiteNum = 2},
				new PrerequisiteSolver.Prerequisite { CourseNum = 1, PrerequisiteNum = 3},
			};
			var canTakeCourses = prerequisiteSovler.CanCreateCourseSchedule(prerequisites, 6);
		}
	}
}
