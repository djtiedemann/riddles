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
		private Dictionary<int, List<Tuple<List<int>, List<int>>>> codeDictionary = new Dictionary<int, List<Tuple<List<int>, List<int>>>>
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
			}},
			{2, new List<Tuple<List<int>, List<int>>>{
				Tuple.Create(new List<int>{ 2, 1}, new List<int> { 1, 1, 1}),
				Tuple.Create(new List<int>{ 2, 2}, new List<int> { 2, 2, 2}),
				Tuple.Create(new List<int>{ 1, 1}, new List<int> { 3, 3, 3}),

				Tuple.Create(new List<int>{ 1, 3}, new List<int> { 1, 2, 3}),
				Tuple.Create(new List<int>{ 3, 3}, new List<int> { 2, 3, 1}),
				Tuple.Create(new List<int>{ 1, 2}, new List<int> { 3, 1, 2}),

				Tuple.Create(new List<int>{ 3, 2}, new List<int> { 1, 3, 2}),
				Tuple.Create(new List<int>{ 3, 1}, new List<int> { 3, 2, 1}),
				Tuple.Create(new List<int>{ 2, 3}, new List<int> { 2, 1, 3}),
			}}
		};

		private Dictionary<int, List<int>> signalDictionary = new Dictionary<int, List<int>>
		{
			{ 1, new List<int>{ 1, 2, 3} },
			{ 2, new List<int>{ 3, 3, 3} },
			{ 3, new List<int>{ 3, 2, 1} },
			{ 4, new List<int>{ 2, 2, 1} },
			{ 5, new List<int>{ 3, 2, 3} },
		};

		private Dictionary<int, List<int>> responseDictionary = new Dictionary<int, List<int>>
		{
			{ 1, new List<int>{ 2, 2 } },
			{ 2, new List<int>{ 1, 1 } },
			{ 3, new List<int>{ 1, 3 } },
			{ 4, new List<int>{ 3, 1 } },
			{ 5, new List<int>{ 3, 1 } },
		};

		[TestCase(2, 3, 2, 3)]
		public void TestSolution(int codeId, int numDifferentColors, int numPeopleInFirstLine, int numPeopleInSecondLine)
		{
			var code1 = this.CreateOneWayCodeFromTestCase(codeDictionary[codeId]);
			var citizenRandomHatSolver = new CitizenRandomHatSolver();
			var code2 = citizenRandomHatSolver.GetSecondCodeFromFirstCode(code1, numDifferentColors);
			var code = new CitizenRandomHatSolver.Code
			{
				Code1 = code1,
				Code2 = code2
			};
			var isCorrectCode = citizenRandomHatSolver.VerifySolution(code, (numPeopleInFirstLine + numPeopleInSecondLine), numDifferentColors);
		}

		[TestCase(2, 3)]
		public void TestGetSecondCodeFromFirstCode(int codeId, int numDifferentColors)
		{
			var code = this.CreateOneWayCodeFromTestCase(codeDictionary[codeId]);
			var citizenRandomHatSolver = new CitizenRandomHatSolver();
			var result = citizenRandomHatSolver.GetSecondCodeFromFirstCode(code, numDifferentColors);
		}


		[TestCase(1, 1, 1, 2, 3, 3)]
		[TestCase(1, 2, 2, 2, 3, 3)]
		[TestCase(1, 3, 3, 2, 3, 3)]
		[TestCase(1, 4, 4, 2, 3, 3)]
		[TestCase(1, 5, 5, 2, 3, 3)]
		public void TestGetResponseFromSignal(
			int codeId, int signalId, int responseId, int numPeopleInFirstLine, int numPeopleInSecondLine, int numDifferentColors)
		{
			var code = this.CreateOneWayCodeFromTestCase(codeDictionary[codeId]);
			var signal = this.CreateSignalFromTestCase(signalDictionary[signalId]);

			var citizenRandomHatSolver = new CitizenRandomHatSolver();
			var groupAssignmentGenerator = new GroupAssignmentGenerator();

			var keySetOfFirstCode = groupAssignmentGenerator.GenerateAllPossibleGroupAssignmentsForDistinctGroupsAndDistinctMembers(
				numPeopleInFirstLine,
				numDifferentColors);

			var response = citizenRandomHatSolver.GetResponseFromSignal(signal, code, keySetOfFirstCode);
			var expectedResponse = this.CreateSignalFromTestCase(responseDictionary[responseId]);

			if(response == null && expectedResponse == null)
			{
				return;
			}

			Assert.AreEqual(response.Assignment.Length, expectedResponse.Assignment.Length);

			for(int i=0; i< response.Assignment.Length; i++)
			{
				Assert.AreEqual(expectedResponse.Assignment[i].MemberId, response.Assignment[i].MemberId);
				Assert.AreEqual(expectedResponse.Assignment[i].GroupId, response.Assignment[i].GroupId);
			}
		}

		private CitizenRandomHatSolver.OneWayCode CreateOneWayCodeFromTestCase(List<Tuple<List<int>, List<int>>> data)
		{
			var signals = new List<CitizenRandomHatSolver.OneWaySignal> { };
			foreach(var tuple in data)
			{
				var signal = tuple.Item1.Select((group, member) => new GroupAssignmentMember(memberId: member + 1, groupId: group)).ToArray();
				var response = tuple.Item2.Select((group, member) => new GroupAssignmentMember(memberId: member + 1, groupId: group)).ToArray();


				signals.Add(new CitizenRandomHatSolver.OneWaySignal
				{
					Signal = new GroupAssignment(signal),
					Response = new GroupAssignment(response)
				});
			}
			return new CitizenRandomHatSolver.OneWayCode { OneWaySignals = signals };
		}

		private GroupAssignment CreateSignalFromTestCase(List<int> data)
		{
			var signal = data.Select((group, member) => new GroupAssignmentMember(memberId: member + 1, groupId: group)).ToArray();
			return new GroupAssignment(signal);
		}
	}
}
