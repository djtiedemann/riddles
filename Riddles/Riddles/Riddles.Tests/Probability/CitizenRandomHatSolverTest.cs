using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NUnit.Framework;
using Riddles.Probability;
using Riddles.Probability.Domain;

namespace Riddles.Tests.Probability
{
	public class CitizenRandomHatSolverTest
	{
		private Dictionary<int, List<Tuple<List<int>, List<int>>>> testCases = new Dictionary<int, List<Tuple<List<int>, List<int>>>>
		{
			{1, new List<Tuple<List<int>, List<int>>>{
				Tuple.Create(new List<int>{ 1, 1}, new List<int> { 1, 1, 1}),
				Tuple.Create(new List<int>{ 1, 2}, new List<int> { 2, 2, 2}),
				Tuple.Create(new List<int>{ 3, 1}, new List<int> { 3, 3, 3}),

				Tuple.Create(new List<int>{ 2, 2}, new List<int> { 1, 2, 3}),
				Tuple.Create(new List<int>{ 2, 1}, new List<int> { 2, 3, 1}),
				Tuple.Create(new List<int>{ 3, 2}, new List<int> { 3, 1, 2}),

				Tuple.Create(new List<int>{ 3, 3}, new List<int> { 1, 3, 2}),
				Tuple.Create(new List<int>{ 1, 3}, new List<int> { 3, 2, 1}),
				Tuple.Create(new List<int>{ 2, 3}, new List<int> { 2, 1, 3}),
			}}
		};

		[TestCase(1, 3)]
		public void TestGetSecondCodeFromFirstCode(int testCaseId, int numDifferentColors)
		{
			var code = this.CreateOneWayCodeFromTestCase(testCases[testCaseId]);
			var citizenRandomHatSolver = new CitizenRandomHatSolver();
			var result = citizenRandomHatSolver.GetSecondCodeFromFirstCode(code, numDifferentColors);
		}

		private CitizenRandomHatSolver.OneWayCode CreateOneWayCodeFromTestCase(List<Tuple<List<int>, List<int>>> data)
		{
			var signals = new List<CitizenRandomHatSolver.OneWaySignal> { };
			foreach(var tuple in data)
			{
				var signal = tuple.Item1.Select((group, member) => new GroupAssignmentMember(memberId: member + 1, groupId: group)).ToList();
				var response = tuple.Item2.Select((group, member) => new GroupAssignmentMember(memberId: member + 1, groupId: group)).ToList();


				signals.Add(new CitizenRandomHatSolver.OneWaySignal
				{
					Signal = new GroupAssignment(signal),
					Response = new GroupAssignment(response)
				});
			}
			return new CitizenRandomHatSolver.OneWayCode { OneWaySignals = signals };
		}

	}
}
