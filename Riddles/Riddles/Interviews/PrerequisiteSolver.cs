using System;
using System.Collections.Generic;
using System.Text;

namespace Interviews
{
	public class PrerequisiteSolver
	{
		public bool CanCreateCourseSchedule(List<Prerequisite> prerequisites, int numCourses) {
			var adjacencyLists = this.BuildAdjacencyLists(prerequisites, numCourses);
			HashSet<int> coursesAlreadyProcessed = new HashSet<int> { };
			for (int i = 0; i < numCourses; i++) {
				HashSet<int> coursesFoundAlongThisPath = new HashSet<int> { };
				if (coursesAlreadyProcessed.Contains(i)) {
					continue;
				}
				var success = this.TopologicalSort(i, adjacencyLists, coursesAlreadyProcessed, coursesFoundAlongThisPath);
				if (!success) {
					return false;
				}
			}
			return true;
		}

		private bool TopologicalSort(int currentCourse, List<int>[] adjacencyList, HashSet<int> coursesAlreadyProcessed, 
			HashSet<int> coursesFoundAlongThisPath)
		{
			coursesFoundAlongThisPath.Add(currentCourse);
			if (adjacencyList[currentCourse] != null) {
				foreach (var subsequentCourse in adjacencyList[currentCourse])
				{
					if (coursesFoundAlongThisPath.Contains(subsequentCourse)) { return false; /* found a cycle */ }
					if (coursesAlreadyProcessed.Contains(subsequentCourse)) { continue; }
					var success = this.TopologicalSort(subsequentCourse, adjacencyList, coursesAlreadyProcessed, coursesFoundAlongThisPath);
					if (!success) { return false; }
				}
			}			
			coursesAlreadyProcessed.Add(currentCourse);
			coursesFoundAlongThisPath.Remove(currentCourse);
			return true;
		}

		private List<int>[] BuildAdjacencyLists(List<Prerequisite> prerequisites, int numCourses)
		{
			var adjacencyLists = new List<int>[numCourses];			
			foreach (var prerequisite in prerequisites) { 
				if(adjacencyLists[prerequisite.PrerequisiteNum] == null)
				{
					adjacencyLists[prerequisite.PrerequisiteNum] = new List<int>();
				}
				adjacencyLists[prerequisite.PrerequisiteNum].Add(prerequisite.CourseNum);
			}
			return adjacencyLists;
		}

		public class Prerequisite
		{
			public int CourseNum { get; set; }
			public int PrerequisiteNum { get; set; }
		}


	}
}
