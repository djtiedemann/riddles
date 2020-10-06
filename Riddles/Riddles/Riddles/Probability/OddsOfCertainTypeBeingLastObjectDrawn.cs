using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Riddles.Probability
{
	public interface CalculationState<TObjectType> 
		where TObjectType : Enum
	{
		string GetMemoizationKey();
		bool IsTerminalState();
		Dictionary<TObjectType, double> GetProbabilitiesFromTerminalState();
		List<ProbabilityOfEachObjectBeingDrawnGivenState<TObjectType>> GetProbabilityOfAllNewStatesFromCurrentState();
	}

	public class ProbabilityOfEachObjectBeingDrawnGivenState<TObjectType>
		where TObjectType : Enum
	{
		public CalculationState<TObjectType> CalculationState { get; set; }
		public double Probability { get; set; }
	}

	public class OddsOfCertainTypeBeingLastObjectDrawn
	{
		

		public OddsOfCertainTypeBeingLastObjectDrawn()
		{

		}

		public Dictionary<TObjectType, double> GetOddsCertainTypeIsLastTypeDrawn<TObjectType, TCurrentState>(TCurrentState currentState)
			where TObjectType : Enum
			where TCurrentState : CalculationState<TObjectType>
		{
			var memoizationDictionary = new Dictionary<string, Dictionary<TObjectType, double>>();
			return this.GetOddsCertainTypeIsLastTypeDrawnInternal(
				currentState, memoizationDictionary
			);
		}

		private Dictionary<TObjectType, double> GetOddsCertainTypeIsLastTypeDrawnInternal<TObjectType, TCurrentState>(TCurrentState currentState,
			Dictionary<string, Dictionary<TObjectType, double>> memoizationDictionary)
			where TObjectType : Enum
			where TCurrentState : CalculationState<TObjectType>
		{
			var memoizationKey = currentState.GetMemoizationKey();
			if (memoizationDictionary.ContainsKey(memoizationKey))
			{
				return memoizationDictionary[memoizationKey];
			}

			if (currentState.IsTerminalState())
			{
				memoizationDictionary[memoizationKey] = currentState.GetProbabilitiesFromTerminalState();
				return memoizationDictionary[memoizationKey];
			}

			var stateTransitions = currentState.GetProbabilityOfAllNewStatesFromCurrentState();
			Dictionary<TObjectType, double> oddsCertainTypeIsLastTypeDrawn = new Dictionary<TObjectType, double>();
			foreach(var value in Enum.GetValues(typeof(TObjectType)))
			{
				oddsCertainTypeIsLastTypeDrawn[(TObjectType)value] = 0.0;
			}
			foreach(var nextState in stateTransitions)
			{
				var oddsCertainTypeIsLastDrawnForNextState = this.GetOddsCertainTypeIsLastTypeDrawnInternal(nextState.CalculationState, memoizationDictionary);
				foreach(var objectType in oddsCertainTypeIsLastDrawnForNextState.Keys)
				{
					oddsCertainTypeIsLastTypeDrawn[objectType] += oddsCertainTypeIsLastDrawnForNextState[objectType] * nextState.Probability;
				}
			}
			memoizationDictionary[memoizationKey] = oddsCertainTypeIsLastTypeDrawn;
			return memoizationDictionary[memoizationKey];
		}
	}
}
