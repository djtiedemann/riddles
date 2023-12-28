using System;
using System.Collections.Generic;
using System.Text;
using Riddles.Combinatorics.Core.Domain;
using Riddles.Combinatorics.Core.Permutations;
using System.Linq;

namespace Riddles.Probability
{
	public class CitizenRandomHatSolver
	{
		private PermutationWithRepetitionGenerator _permutationGenerator;
		public CitizenRandomHatSolver()
		{
			this._permutationGenerator = new PermutationWithRepetitionGenerator();
		}

		public List<HatAssignment> GenerateAllPossibleHatAssignments(int numCitizens, int numHatColors, int firstCitizenId)
		{
			var assignments = this._permutationGenerator.GenerateAllOutcomes(numCitizens, numHatColors, '1');

			return assignments.Select(p =>
			{
				return new HatAssignment { 
					Assignment = p.Select((character, index) => 
					new IndividualAssignment { CitizenId = index + firstCitizenId, HatColor = (HatColor)((character - '1') + 1) }).ToArray()
				};				
			}).ToList();
		}

		public bool VerifySolution(Code code, int numPeople, int numHats)
		{
			var possibleCombinationsOfHats = this.GenerateAllPossibleHatAssignments(
				numPeople, 
				numHats,
				1);

			var numPeopleInSmallerLine = code.Code1.OneWaySignals.First().Signal.Assignment.Length;
			var numPeopleInLargerLine = code.Code2.OneWaySignals.First().Signal.Assignment.Length;

			foreach (var combination in possibleCombinationsOfHats)
			{
				var signalFromSmallerLineMembers = new IndividualAssignment[numPeopleInSmallerLine];
				var signalFromLargerLineMembers = new IndividualAssignment[numPeopleInLargerLine];
				for(int i=0; i< numPeopleInSmallerLine; i++)
				{
					signalFromSmallerLineMembers[i] = new IndividualAssignment { 
						CitizenId = combination.Assignment[i].CitizenId, 
						HatColor = combination.Assignment[i].HatColor 
					};
				}
				for(int i=0; i<numPeopleInLargerLine; i++)
				{
					signalFromLargerLineMembers[i] = new IndividualAssignment
					{
						CitizenId = combination.Assignment[i + numPeopleInSmallerLine].CitizenId,
						HatColor = combination.Assignment[i + numPeopleInSmallerLine].HatColor
					}; 
				}
				var signalFromSmallerLine = new HatAssignment { Assignment = signalFromSmallerLineMembers };
				var signalFromLargerLine = new HatAssignment { Assignment = signalFromLargerLineMembers };

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

			var keySetOfFirstCode = this.GenerateAllPossibleHatAssignments(
				numPeopleInLineSendingFirstCode,
				numDifferentColors,
				1);
			var keySetOfSecondCode = this.GenerateAllPossibleHatAssignments(
				numPeopleInLineSendingSecondCode,
				numDifferentColors,
				1 + numPeopleInLineSendingFirstCode);		

			var signals = new List<OneWaySignal>();
			foreach(var signal in keySetOfSecondCode)
			{
				var response = this.GetResponseFromSignal(signal, code, keySetOfFirstCode);
				var signalCopy = signal.DeepCopyHatAssignment();
				var responseCopy = response?.DeepCopyHatAssignment();
				signals.Add(new OneWaySignal
				{
					Signal = signalCopy,
					Response = responseCopy
				});
			}
			return new OneWayCode { OneWaySignals = signals };
		}

		public HatAssignment GetResponseFromSignal(HatAssignment signal, OneWayCode code, List<HatAssignment> keySetOfFirstCode)
		{
			var responsesToMatch = new List<HatAssignment>();
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

		public bool PairContainsAtLeastOneMatchingHat(HatAssignment assignment1, HatAssignment assignment2)
		{
			if(assignment1.Assignment.Length != assignment2.Assignment.Length)
			{
				throw new InvalidOperationException("the rows being compared must contain the same number of people");
			}

			for(int i=0; i<assignment1.Assignment.Length; i++)
			{
				if (assignment1.Assignment[i].CitizenId != assignment2.Assignment[i].CitizenId)
				{
					throw new InvalidOperationException("the memberIds don't line up");
				}

				if (assignment1.Assignment[i].HatColor == assignment2.Assignment[i].HatColor)
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
			public HatAssignment Signal { get; set; }
			public HatAssignment Response { get; set; }
		}

		public class HatAssignment
		{
			public IndividualAssignment[] Assignment { get; set; }

			public HatAssignment DeepCopyHatAssignment()
			{
				var newMembers = new IndividualAssignment[this.Assignment.Length];
				for (int i = 0; i < this.Assignment.Length; i++)
				{
					newMembers[i] = new IndividualAssignment { CitizenId = this.Assignment[i].CitizenId , HatColor = this.Assignment[i].HatColor };
				}
				return new HatAssignment { Assignment = newMembers };
			}

			public override bool Equals(object obj)
			{
				if (!(obj is HatAssignment))
				{
					return false;
				}
				var otherHatAssignment = (HatAssignment)obj;
				if (otherHatAssignment.Assignment.Length != this.Assignment.Length)
				{
					return false;
				}
				for (int i = 0; i < this.Assignment.Length; i++)
				{
					if (!this.Assignment[i].Equals(otherHatAssignment.Assignment[i]))
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
		}

		public class IndividualAssignment
		{
			public int CitizenId { get; set; }
			public HatColor HatColor { get; set; }

			public override bool Equals(object obj)
			{
				if (!(obj is IndividualAssignment))
				{
					return false;
				}
				var otherIndividualAssignment = (IndividualAssignment)obj;
				return this.CitizenId == otherIndividualAssignment.CitizenId
					&& this.HatColor == otherIndividualAssignment.HatColor;
			}

			public override int GetHashCode()
			{
				int hash = 17;
				hash = hash * 23 + CitizenId.GetHashCode();
				hash = hash * 23 + HatColor.GetHashCode();
				return hash;
			}
		}		

		public enum HatColor { 
			Red = 1,
			Blue = 2,
			Green = 3
		}
	}
}
