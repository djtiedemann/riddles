using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Riddles.MarkovChains
{
	public interface MarkovChainState
	{
		Dictionary<MarkovChainState, double> GetStateTransitions();
	}

	public class MarkovChainSolver
	{
		//public double GetExpectedValueOfNumTurnsToReachTerminalState<TMarkovChainState>(TMarkovChainState initialState)
		//	where TMarkovChainState : MarkovChainState
		//{
		//	var statesToProcess = new List<TMarkovChainState>();
		//	var stateTransitionDictionary = new Dictionary<TMarkovChainState, Dictionary<TMarkovChainState, double>>();
		//	statesToProcess.Add(initialState);

		//	// while there are states that we haven't yet processed, calculate the transitions for those states
		//	// add any new states that we've found as states to process
		//	while (statesToProcess.Any())
		//	{
		//		var stateToProcess = statesToProcess.First();
		//		var stateTransitions = this.GetStateTransitions(stateToProcess, numBalls);
		//		stateTransitionDictionary.Add(stateToProcess, stateTransitions);
		//		statesToProcess.Remove(stateToProcess);

		//		foreach (var transitionState in stateTransitions.Keys)
		//		{
		//			if (!statesToProcess.Contains(transitionState) && !stateTransitionDictionary.ContainsKey(transitionState))
		//			{
		//				var newState = this.DeepCloneState(transitionState);
		//				statesToProcess.Add(newState);
		//			}
		//		}
		//	}
		//}
	}
}
