using System;
using System.Collections.Generic;
using System.Text;
using Riddles.Probability.Domain;
using System.Linq;

namespace Riddles.Probability
{
	public class CitizenRandomHatSolver
	{
		private GroupAssignmentGenerator _groupAssignmentGenerator;
		public CitizenRandomHatSolver()
		{
			this._groupAssignmentGenerator = new GroupAssignmentGenerator();
		}

		public bool VerifySolution(Code code, int numPeople, int numHats)
		{
			var possibleCombinationsOfHats = this._groupAssignmentGenerator.GenerateAllPossibleGroupAssignmentsForDistinctGroupsAndDistinctMembers(
				numPeople, 
				numHats);

			var numPeopleInSmallerLine = code.Code1.OneWaySignals.First().Signal.Assignment.Length;
			var numPeopleInLargerLine = code.Code2.OneWaySignals.First().Signal.Assignment.Length;

			foreach (var combination in possibleCombinationsOfHats)
			{
				var signalFromSmallerLineMembers = new GroupAssignmentMember[numPeopleInSmallerLine];
				var signalFromLargerLineMembers = new GroupAssignmentMember[numPeopleInLargerLine];
				for(int i=0; i< numPeopleInSmallerLine; i++)
				{
					signalFromSmallerLineMembers[i] = new GroupAssignmentMember(combination.Assignment[i].MemberId, combination.Assignment[i].GroupId);
				}
				for(int i=0; i<numPeopleInLargerLine; i++)
				{
					signalFromLargerLineMembers[i] = new GroupAssignmentMember(
						combination.Assignment[i+numPeopleInSmallerLine].MemberId, combination.Assignment[i+numPeopleInSmallerLine].GroupId);
				}
				var signalFromSmallerLine = new GroupAssignment(signalFromSmallerLineMembers);
				var signalFromLargerLine = new GroupAssignment(signalFromLargerLineMembers);

				var responseFromSmallerLine = code.Code2.OneWaySignals.Single(c => c.Signal.Equals(signalFromLargerLine)).Response;
				var responseFromLargerLine = code.Code1.OneWaySignals.Single(c => c.Signal.Equals(signalFromSmallerLine)).Response;

				if(!(this.PairContainsAtLeastOneMatchingHat(signalFromSmallerLine, responseFromSmallerLine)
					|| this.PairContainsAtLeastOneMatchingHat(signalFromLargerLine, responseFromLargerLine)))
				{
					return false;
				}
			}
			return true;
		}

		// If the citizens are put in 2 lines, each which can see the other line, but not themselves, they need to determine which hats to guess
		// based on the set of hats in the other line. So there are 2 signals being sent, 1 by each line. There are 2 responses, 1 by each line.
		//
		// So we need to create a signal for the smaller line to send to the larger line, such that it's possible to create a signal that the larger
		// line can send.
		//
		// So this takes in the code from the smaller line to the larger line and computes the code that the larger line needs to return
		public OneWayCode GetSecondCodeFromFirstCode(OneWayCode code, int numDifferentColors)
		{
			var numPeopleInLineSendingFirstCode = code.OneWaySignals.First().Signal.Assignment.Length;
			var numPeopleInLineSendingSecondCode = code.OneWaySignals.First().Response.Assignment.Length;

			var keySetOfFirstCode = this._groupAssignmentGenerator.GenerateAllPossibleGroupAssignmentsForDistinctGroupsAndDistinctMembers(
				numPeopleInLineSendingFirstCode,
				numDifferentColors);
			var keySetOfSecondCode = this._groupAssignmentGenerator.GenerateAllPossibleGroupAssignmentsForDistinctGroupsAndDistinctMembers(
				numPeopleInLineSendingSecondCode,
				numDifferentColors);		

			var signals = new List<OneWaySignal>();
			foreach(var signal in keySetOfSecondCode)
			{
				var response = this.GetResponseFromSignal(signal, code, keySetOfFirstCode);
				var signalCopy = signal.DeepCopyGroupAssignment();
				var responseCopy = response?.DeepCopyGroupAssignment();
				signals.Add(new OneWaySignal
				{
					Signal = signalCopy,
					Response = responseCopy
				});
			}
			return new OneWayCode { OneWaySignals = signals };
		}

		public GroupAssignment GetResponseFromSignal(GroupAssignment signal, OneWayCode code, List<GroupAssignment> keySetOfFirstCode)
		{
			var responsesToMatch = new List<GroupAssignment>();
			foreach (var oldSignal in code.OneWaySignals)
			{
				if (!this.PairContainsAtLeastOneMatchingHat(signal, oldSignal.Response))
				{
					responsesToMatch.Add(oldSignal.Signal);
				}
			}

			foreach (var possibleResponse in keySetOfFirstCode)
			{
				if (responsesToMatch.All(r => this.PairContainsAtLeastOneMatchingHat(possibleResponse, r)))
				{
					
					return possibleResponse;
				}
			}
			return null;			
		}

		public bool PairContainsAtLeastOneMatchingHat(GroupAssignment assignment1, GroupAssignment assignment2)
		{
			if(assignment1.Assignment.Length != assignment2.Assignment.Length)
			{
				throw new InvalidOperationException("the rows being compared must contain the same number of people");
			}

			for(int i=0; i<assignment1.Assignment.Length; i++)
			{
				if (assignment1.Assignment[i].MemberId != assignment2.Assignment[i].MemberId)
				{
					throw new InvalidOperationException("the memberIds don't line up");
				}

				if (assignment1.Assignment[i].GroupId == assignment2.Assignment[i].GroupId)
				{
					return true;
				}
			}
			return false;
		}

		public class Code
		{
			public OneWayCode Code1 { get; set; }
			public OneWayCode Code2 { get; set; }
		}

		public class OneWayCode
		{
			public List<OneWaySignal> OneWaySignals { get; set; }
		}

		public class OneWaySignal
		{
			public GroupAssignment Signal { get; set; }
			public GroupAssignment Response { get; set; }
		}
	}
}
