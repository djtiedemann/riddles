﻿using System;
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

		//[TestCase(1, 3)]
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

			Assert.AreEqual(response.Assignment.Count, expectedResponse.Assignment.Count);
			var expectedResponseInternal = expectedResponse.Assignment.ToArray();
			var responseInternal = response.Assignment.ToArray();

			for(int i=0; i<responseInternal.Length; i++)
			{
				Assert.AreEqual(expectedResponseInternal[i].MemberId, responseInternal[i].MemberId);
				Assert.AreEqual(expectedResponseInternal[i].GroupId, responseInternal[i].GroupId);
			}
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

		private GroupAssignment CreateSignalFromTestCase(List<int> data)
		{
			var signal = data.Select((group, member) => new GroupAssignmentMember(memberId: member + 1, groupId: group)).ToList();
			return new GroupAssignment(signal);
		}
	}
}
