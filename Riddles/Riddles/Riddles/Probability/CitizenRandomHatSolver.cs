﻿using System;
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

		// If the citizens are put in 2 lines, each which can see the other line, but not themselves, they need to determine which hats to guess
		// based on the set of hats in the other line. So there are 2 signals being sent, 1 by each line. There are 2 responses, 1 by each line.
		//
		// So we need to create a signal for the smaller line to send to the larger line, such that it's possible to create a signal that the larger
		// line can send.
		//
		// So this takes in the code from the smaller line to the larger line and computes the code that the larger line needs to return
		public OneWayCode GetSecondCodeFromFirstCode(OneWayCode code, int numDifferentColors)
		{
			var numPeopleInLineSendingFirstCode = code.OneWaySignals.First().Signal.Assignment.Count;
			var numPeopleInLineSendingSecondCode = code.OneWaySignals.First().Response.Assignment.Count;

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
				if(response == null)
				{
					// if we couldn't find a valid response for a signal, then the original code was faulty
					return null;
				}
				var signalCopy = signal.DeepCopyGroupAssignment();
				var responseCopy = response.DeepCopyGroupAssignment();
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
			if(assignment1.Assignment.Count != assignment2.Assignment.Count)
			{
				throw new InvalidOperationException("the rows being compared must contain the same number of people");
			}

			var assignment1Internal = assignment1.Assignment.ToArray();
			var assignment2Internal = assignment2.Assignment.ToArray();
			for(int i=0; i<assignment1.Assignment.Count; i++)
			{
				if(assignment1Internal[i] == assignment2Internal[i])
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
