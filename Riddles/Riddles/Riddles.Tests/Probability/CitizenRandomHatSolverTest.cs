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

		[TestCase(2, 3, 2, 3, true)]
		public void TestSolution(int codeId, int numDifferentColors, int numPeopleInFirstLine, int numPeopleInSecondLine, bool expectedResult)
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
			Assert.AreEqual(expectedResult, isCorrectCode);
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
			var signal = this.CreateSignalFromTestCase(signalDictionary[signalId], numPeopleInFirstLine + 1);

			var citizenRandomHatSolver = new CitizenRandomHatSolver();
			
			var keySetOfFirstCode = citizenRandomHatSolver.GenerateAllPossibleHatAssignments(
				numPeopleInFirstLine,
				numDifferentColors,
				1);

			var response = citizenRandomHatSolver.GetResponseFromSignal(signal, code, keySetOfFirstCode);
			var expectedResponse = this.CreateSignalFromTestCase(responseDictionary[responseId], 1);

			if(response == null && expectedResponse == null)
			{
				return;
			}

			Assert.AreEqual(response.Assignment.Length, expectedResponse.Assignment.Length);

			for(int i=0; i< response.Assignment.Length; i++)
			{
				Assert.AreEqual(expectedResponse.Assignment[i].CitizenId, response.Assignment[i].CitizenId);
				Assert.AreEqual(expectedResponse.Assignment[i].HatColor, response.Assignment[i].HatColor);
			}
		}

		private CitizenRandomHatSolver.OneWayCode CreateOneWayCodeFromTestCase(
			List<Tuple<List<int>, 
			List<int>>> data
		)
		{
			var signals = new List<CitizenRandomHatSolver.OneWaySignal> { };
			foreach(var tuple in data)
			{
				var signal = tuple.Item1.Select((hatColor, citizenId) => 
					new CitizenRandomHatSolver.IndividualAssignment { 
						CitizenId = citizenId + 1, 
						HatColor = (CitizenRandomHatSolver.HatColor)hatColor
					}).ToArray();
				var response = 
					tuple.Item2.Select((hatColor, citizenId) => new CitizenRandomHatSolver.IndividualAssignment {
						CitizenId = citizenId + tuple.Item1.Count + 1,
						HatColor = (CitizenRandomHatSolver.HatColor)hatColor
					}).ToArray();


				signals.Add(new CitizenRandomHatSolver.OneWaySignal
				{
					Signal = new CitizenRandomHatSolver.HatAssignment { Assignment = signal },
					Response = new CitizenRandomHatSolver.HatAssignment { Assignment = response }
				});
			}
			return new CitizenRandomHatSolver.OneWayCode { OneWaySignals = signals };
		}

		private CitizenRandomHatSolver.HatAssignment CreateSignalFromTestCase(List<int> data, int minCitizenId)
		{
			var signal = data.Select((hatColor, citizenId) => new CitizenRandomHatSolver.IndividualAssignment {
				CitizenId = citizenId + minCitizenId,
				HatColor = (CitizenRandomHatSolver.HatColor)hatColor
			}).ToArray();
			return new CitizenRandomHatSolver.HatAssignment { Assignment = signal };
		}
	}
}
