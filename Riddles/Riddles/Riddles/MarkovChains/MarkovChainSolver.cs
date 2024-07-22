using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Riddles.LinearAlgebra;

namespace Riddles.MarkovChains
{
	public interface IMarkovChainState
	{
		bool IsStateTerminalState();
		string TerminalStateLabel();
	}

	public class MarkovChainSolver
	{
		private MatrixUtilities _matrixUtilities;
        private bool _useCache;
        private bool _cachedStateTransitionDict;
		public MarkovChainSolver(bool useCache=false)
		{
			this._matrixUtilities = new MatrixUtilities();
            this._useCache = useCache;
		}

        public double GetExpectedValueOfNumTurnsToReachTerminalState<TMarkovChainState, TAdditionalProblemState>(TMarkovChainState initialState,
            Func<TMarkovChainState, TAdditionalProblemState, Dictionary<TMarkovChainState, double>> getStateTransitions,
            TAdditionalProblemState additionalProblemState = default(TAdditionalProblemState))
            where TMarkovChainState : IMarkovChainState
        {
            return this.CalculateMarkovChainValue(
                initialState,
                getStateTransitions,
                this.CalculateExpectedValueOfNumTurnsToReachTerminalState<TMarkovChainState, TAdditionalProblemState>,
                additionalProblemState,
                additionalAggregationArgs: default(object)
            );
        }

        public double GetProbabilityOfArrivingAtSpecificTerminalStateLabel<TMarkovChainState, TAdditionalProblemState>(TMarkovChainState initialState,
            Func<TMarkovChainState, TAdditionalProblemState, Dictionary<TMarkovChainState, double>> getStateTransitions,
            string terminalStateLabel,
            TAdditionalProblemState additionalProblemState = default(TAdditionalProblemState))
            where TMarkovChainState : IMarkovChainState
        {
            var additionalAggregationArgs =
                new ProbabilityOfEndingAtParticularStateAdditionalArgs(terminalStateLabel);
            return this.CalculateMarkovChainValue(
                initialState,
                getStateTransitions,
                this.CalculateProbabilityOfArrivingAtSpecificTerminalStateLabel<TMarkovChainState, TAdditionalProblemState>,
                additionalProblemState,
                additionalAggregationArgs
            );
        }

        public Dictionary<TMarkovChainState, double> 
            CalculateProbabilityOfArrivingAtAllTerminalStates<TMarkovChainState, TAdditionalProblemState>(
            TMarkovChainState initialState,
            Func<TMarkovChainState, TAdditionalProblemState, Dictionary<TMarkovChainState, double>> getStateTransitions,
            TAdditionalProblemState additionalProblemState = default(TAdditionalProblemState))
            where TMarkovChainState : IMarkovChainState
        {
            var (stateTransitionDictionary, termDictionary) =
                this.GetMarkovStateInfo(
                    initialState,
                    getStateTransitions,
                    additionalProblemState
                );
            var terminalStates = stateTransitionDictionary.Keys
                .Where(k => k.IsStateTerminalState()).ToList();
            var probabilities = new Dictionary<TMarkovChainState, double>();
            foreach(var terminalState in terminalStates)
            {
                var additionalAggregationArgs =
                    new ProbabilityOfEndingAtParticularStateAdditionalArgs(
                        terminalState.TerminalStateLabel()
                    );
                var probability = this.CalculateMarkovChainValue(
                    initialState,
                    getStateTransitions,
                    this.CalculateProbabilityOfArrivingAtSpecificTerminalStateLabel<TMarkovChainState, TAdditionalProblemState>,
                    additionalProblemState,
                    additionalAggregationArgs
                );
                probabilities.Add(terminalState, probability);
            }
            return probabilities;
        }

        private (Dictionary<TMarkovChainState, Dictionary<TMarkovChainState, double>>,
            Dictionary<TMarkovChainState, int>)
            GetMarkovStateInfo<TMarkovChainState, TAdditionalProblemState>(
                TMarkovChainState initialState,
                Func<TMarkovChainState, TAdditionalProblemState, Dictionary<TMarkovChainState, double>> getStateTransitions,
                TAdditionalProblemState additionalProblemState = default
            )
        {
            var statesToProcess = new List<TMarkovChainState>();
            var stateTransitionDictionary = new Dictionary<TMarkovChainState, Dictionary<TMarkovChainState, double>>();
            statesToProcess.Add(initialState);

            // while there are states that we haven't yet processed, calculate the transitions for those states
            // add any new states that we've found as states to process
            while (statesToProcess.Any())
            {
                var stateToProcess = statesToProcess.First();
                var stateTransitions = getStateTransitions(stateToProcess, additionalProblemState);
                stateTransitionDictionary.Add(stateToProcess, stateTransitions);
                statesToProcess.Remove(stateToProcess);

                foreach (var transitionState in stateTransitions.Keys)
                {
                    if (!statesToProcess.Contains(transitionState) && !stateTransitionDictionary.ContainsKey(transitionState))
                    {
                        statesToProcess.Add(transitionState);
                    }
                }
            }

            // once we've calculated all of the state transitions and found all possible states, turn those state transitions into linear equations

            // first assign each state an id which represents the column it will be in the matrix representation
            var termDictionary = new Dictionary<TMarkovChainState, int>();
            var nextStateId = 0;
            foreach (var state in stateTransitionDictionary.Keys)
            {
                termDictionary[state] = nextStateId;
                nextStateId++;
            }
            return (stateTransitionDictionary, termDictionary);
        }

        private double CalculateMarkovChainValue<TMarkovChainState, TAdditionalProblemState, TAdditionalAggregationArgs>(
            TMarkovChainState initialState, 
			Func<TMarkovChainState, TAdditionalProblemState, Dictionary<TMarkovChainState, double>> getStateTransitions,
            Func<TMarkovChainState, Dictionary<TMarkovChainState, Dictionary<TMarkovChainState, double>>, Dictionary<TMarkovChainState, int>, TAdditionalAggregationArgs, double> calculateMarkovChainValueFunc,
			TAdditionalProblemState additionalProblemState = default,
            TAdditionalAggregationArgs additionalAggregationArgs = default)
			where TMarkovChainState : IMarkovChainState
		{
            var (stateTransitionDictionary, termDictionary) =
                this.GetMarkovStateInfo(
                    initialState,
                    getStateTransitions,
                    additionalProblemState
                );
            return calculateMarkovChainValueFunc(
                initialState, stateTransitionDictionary, termDictionary, additionalAggregationArgs
            );
		}

        private double CalculateExpectedValueOfNumTurnsToReachTerminalState<TMarkovChainState, TAdditionalProblemState>(
            TMarkovChainState initialState,
            Dictionary<TMarkovChainState, Dictionary<TMarkovChainState, double>> stateTransitionDictionary,
            Dictionary<TMarkovChainState, int> termDictionary,
            object additionalExpectedValueInformation = default)
            where TMarkovChainState : IMarkovChainState
        {
            // use those variableIds in order to construct the linear equations
            var equationsToSolve = new List<LinearEquation>();
            foreach (var state in stateTransitionDictionary.Keys)
            {
                // basically a state transition means that 1 turn after the current state we will be in a set of states probabilistically
                // this means that the expected value to finish is 1 turn more than the equation formed by each of the states
                // so we can construct the linear equation:
                // v_i = 1 + v_a*p_a + v_b*p_b + ... +
                // where v_i is the state we're looking at and v_a, v_b, ..., are the variables in the state transition
                // if the state is a terminal state, then we know the number of turns left to complete the game is zero,
                // so we can hard-code that equation
                // 
                // if we bring all the variables to the left hand side and the constant to the right hand side we get
                // 1) the coefficient for v_i = 1 - p_i (the probability we transition to the same state)
                // 2) the coefficient for any variable v_a that's in the state transition dictionary is -p_a
                // 3) the constant coefficient is always 1, expect for the terminal state where it is 0
                if (state.IsStateTerminalState())
                {
                    equationsToSolve.Add(new LinearEquation(
                        terms: new List<LinearTerm> { new LinearTerm(coefficient: 1, variableId: termDictionary[state]) },
                        constant: 0
                    ));
                    continue;
                }

                var terms = new List<LinearTerm>();
                var termForThisState = new LinearTerm(
                    coefficient: 1 - (stateTransitionDictionary[state].ContainsKey(state) ? stateTransitionDictionary[state][state] : 0),
                    variableId: termDictionary[state]
                );
                terms.Add(termForThisState);

                foreach (var transitionState in stateTransitionDictionary[state].Keys)
                {
                    if (!transitionState.Equals(state))
                    {
                        terms.Add(new LinearTerm(
                            coefficient: stateTransitionDictionary[state][transitionState] * -1,
                            variableId: termDictionary[transitionState]
                        ));
                    }
                }
                equationsToSolve.Add(new LinearEquation(terms: terms, constant: 1));
            }

            var expectedValuesForEachState = this._matrixUtilities.SolveLinearSystemOfEquations(equationsToSolve);
            return expectedValuesForEachState[termDictionary[initialState]];
        }

        private double CalculateProbabilityOfArrivingAtSpecificTerminalStateLabel<TMarkovChainState, TAdditionalProblemState>(
            TMarkovChainState initialState,
            Dictionary<TMarkovChainState, Dictionary<TMarkovChainState, double>> stateTransitionDictionary,
            Dictionary<TMarkovChainState, int> termDictionary,
            ProbabilityOfEndingAtParticularStateAdditionalArgs additionalInformation = default)
            where TMarkovChainState : IMarkovChainState
        {
            // use those variableIds in order to construct the linear equations
            var equationsToSolve = new List<LinearEquation>();
            foreach (var state in stateTransitionDictionary.Keys)
            {
                // if the state is terminal, then check the label. If the label matches
                // the probability is 1, otherwise it is 0
                if (state.IsStateTerminalState())
                {
                    equationsToSolve.Add(new LinearEquation(
                        terms: new List<LinearTerm> { new LinearTerm(coefficient: 1, variableId: termDictionary[state]) },
                        constant: state.TerminalStateLabel() == additionalInformation.Label ? 1 : 0
                    ));
                    continue;
                }

                var terms = new List<LinearTerm>();
                // need to take into account the chance that the state could transition to itself
                // if it can't, the coefficient will be 1
                var termForThisState = new LinearTerm(
                    coefficient: 1 - (stateTransitionDictionary[state].ContainsKey(state) ? stateTransitionDictionary[state][state] : 0),
                    variableId: termDictionary[state]
                );
                terms.Add(termForThisState);

                // construct an equation such that
                // p[state] = p[state_1]*transition_probability_1 + ... + p[state_n]*transition_probability_n
                // bring all variables to the left hand side of the equation and set the constant to 0
                foreach (var transitionState in stateTransitionDictionary[state].Keys)
                {
                    if (!transitionState.Equals(state))
                    {
                        terms.Add(new LinearTerm(
                            coefficient: stateTransitionDictionary[state][transitionState] * -1,
                            variableId: termDictionary[transitionState]
                        ));
                    }
                }
                equationsToSolve.Add(new LinearEquation(terms: terms, constant: 0));
            }

            var expectedValuesForEachState = this._matrixUtilities.SolveLinearSystemOfEquations(equationsToSolve);
            return expectedValuesForEachState[termDictionary[initialState]];
        }

        private class ProbabilityOfEndingAtParticularStateAdditionalArgs
        {
            public ProbabilityOfEndingAtParticularStateAdditionalArgs(string label)
            {
                this.Label = label;
            }
            public string Label { get; }
        }
    }
}