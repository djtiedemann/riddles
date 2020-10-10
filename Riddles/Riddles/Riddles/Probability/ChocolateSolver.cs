using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Riddles.Probability
{
	public enum ChocolateType { 
		Dark = 1,
		Milk = 2
	}

	public class ChocolateState : CalculationState<ChocolateType>
	{
		private Dictionary<ChocolateType, int> numChocolatesOfEachTypeRemaining;
		private ChocolateType? lastChocolateTypeDrawn;
		public ChocolateState(Dictionary<ChocolateType, int> numChocolatesOfEachTypeRemaining, ChocolateType? lastChocolateTypeDrawn)
		{
			this.numChocolatesOfEachTypeRemaining = numChocolatesOfEachTypeRemaining;
			this.lastChocolateTypeDrawn = lastChocolateTypeDrawn;
		}

		public string GetMemoizationKey()
		{
			var memoizationKey = this.IsTerminalState() 
				? "" 
				: $"{(lastChocolateTypeDrawn.HasValue ? $"{lastChocolateTypeDrawn}" : "None")}:";
			foreach (var chocolateType in numChocolatesOfEachTypeRemaining.Keys.OrderBy(k => k))
			{
				memoizationKey += $"{chocolateType}:{numChocolatesOfEachTypeRemaining[chocolateType]},";
			}
			return memoizationKey;
		}

		public bool IsTerminalState()
		{
			return numChocolatesOfEachTypeRemaining.Values.Where(v => v > 0).Count() <= 1;
		}

		public Dictionary<ChocolateType, double> GetProbabilitiesFromTerminalState()
		{
			if (!this.IsTerminalState())
			{
				throw new Exception($"{nameof(ChocolateState.GetProbabilitiesFromTerminalState)} shouldn't be called outside of a terminal state");
			}

			return numChocolatesOfEachTypeRemaining.Keys.Select(k => new
			{
				ChocolateType = k,
				Probability = numChocolatesOfEachTypeRemaining[k] > 0 ? 1.0 : 0.0
			}).ToDictionary(c => c.ChocolateType, c => c.Probability);
		}

		public List<ProbabilityOfEachObjectBeingDrawnGivenState<ChocolateType>> GetProbabilityOfAllNewStatesFromCurrentState()
		{
			var numChocolatesRemaining = (double)numChocolatesOfEachTypeRemaining.Values.Sum();
			var numChocolatesRemainingOfTypeLastDrawn = lastChocolateTypeDrawn.HasValue 
				? (double)numChocolatesOfEachTypeRemaining[lastChocolateTypeDrawn.Value]
				: 0;
			var oddsOfSameChocolateTypeBeingDrawnAgain = numChocolatesRemainingOfTypeLastDrawn / numChocolatesRemaining;
			var oddsOfNewChocolateTypeBeingDrawn = 1 - oddsOfSameChocolateTypeBeingDrawnAgain;

			List<ProbabilityOfEachObjectBeingDrawnGivenState<ChocolateType>> newStates
					= new List<ProbabilityOfEachObjectBeingDrawnGivenState<ChocolateType>>();
		
			foreach(var chocolateType in numChocolatesOfEachTypeRemaining.Keys)
			{
				var probabilityOfThisChocolateBeingDrawn = chocolateType == lastChocolateTypeDrawn
					? oddsOfSameChocolateTypeBeingDrawnAgain + oddsOfNewChocolateTypeBeingDrawn * oddsOfSameChocolateTypeBeingDrawnAgain
					: oddsOfNewChocolateTypeBeingDrawn * (double)numChocolatesOfEachTypeRemaining[chocolateType] / numChocolatesRemaining;
				var nextState = numChocolatesOfEachTypeRemaining.Keys.Select(k => new
				{
					ChocolateType = k,
					NumChocolatesRemaining = k == chocolateType ? numChocolatesOfEachTypeRemaining[k] - 1 : numChocolatesOfEachTypeRemaining[k]
				}).ToDictionary(k => k.ChocolateType, k => k.NumChocolatesRemaining);
				newStates.Add(new ProbabilityOfEachObjectBeingDrawnGivenState<ChocolateType>
				{
					Probability = probabilityOfThisChocolateBeingDrawn,
					CalculationState = new ChocolateState(nextState, chocolateType)
				});
			}
			return newStates;
		}
	}

	public class ChocolateSolver
	{
		public OddsOfCertainTypeBeingLastObjectDrawn oddsOfCertainTypeBeingLastObjectDrawn;
		public ChocolateSolver()
		{
			this.oddsOfCertainTypeBeingLastObjectDrawn = new OddsOfCertainTypeBeingLastObjectDrawn();
		}

		public Dictionary<ChocolateType, double> GetOddsOfEachChocolateTypeBeingLastChocolateDrawn(
			Dictionary<ChocolateType, int> initialChocolatesForEachType, ChocolateType? lastChocolateType)
		{
			var initialState = new ChocolateState(initialChocolatesForEachType, lastChocolateType);
			return this.oddsOfCertainTypeBeingLastObjectDrawn.GetOddsCertainTypeIsLastTypeDrawn<ChocolateType, ChocolateState>(initialState);
		}
	}
}
