using System;
using System.Collections.Generic;
using System.Linq;

namespace Riddles.MarkovChains
{

    public class TennisGameSolver
    {
        private MarkovChainSolver _markovChainSolver;
        public TennisGameSolver() { 
            this._markovChainSolver = new MarkovChainSolver();
        }

        public Dictionary<int, double> CalculateOddsOfWinningStandardGame()
        {
            return this.CalculateOddsOfWinningByX(
                this.StandardGameIsTerminalStateFunc,
                this.GameTerminalStateLabel
            );
        }

        public Dictionary<int, double> CalculateOddsOfWinningTiebreakGame()
        {
            return this.CalculateOddsOfWinningByX(
                this.TiebreakGameIsTerminalStateFunc,
                this.GameTerminalStateLabel
            );
        }

        public Dictionary<int, double> CalculateOddsOfWinningSet()
        {
            return this.CalculateOddsOfWinningByX(
                this.SetIsTerminalStateFunc,
                this.SetOrMatchTerminalLabelFunc
            );
        }

        public Dictionary<int, double> CalculateOddsOfWinningMatch()
        {
            return this.CalculateOddsOfWinningByX(
                this.MatchIsTerminalStateFunc,
                this.SetOrMatchTerminalLabelFunc
            );
        }

        private Dictionary<int, double> CalculateOddsOfWinningByX(
            Func<int, int, bool> isTerminalStateFunc,
            Func<int, int, string> terminalLabelFunc
        )
        {
            var odds = new Dictionary<int, double>();
            var args = new TennisGameSolverArgs(
                isTerminalStateFunc,
                terminalLabelFunc
            );
            var initialState = new TennisGameState(
                0,
                0,
                isTerminalStateFunc,
                terminalLabelFunc
            );
            var probabilitiesByState = this._markovChainSolver
                .CalculateProbabilityOfArrivingAtAllTerminalStates(
                    initialState,
                    this.GetStateTransitions,
                    args
                ).ToDictionary(k => int.Parse(k.Key.TerminalStateLabel()), 
                    k => k.Value);

            return probabilitiesByState;
        }

        public bool StandardGameIsTerminalStateFunc(
            int numWinsTeamOne, 
            int numWinsTeamTwo)
        {
            return numWinsTeamOne >= 4
                   || numWinsTeamTwo >= 4
                   || (numWinsTeamOne >= 2 && numWinsTeamTwo >= 2);
        }

        public bool TiebreakGameIsTerminalStateFunc(
            int numWinsTeamOne,
            int numWinsTeamTwo)
        {
            return numWinsTeamOne >= 7
                   || numWinsTeamTwo >= 7
                   || (numWinsTeamOne >= 5 && numWinsTeamTwo >= 5);
        }

        public string GameTerminalStateLabel(
            int numWinsTeamOne,
            int numWinsTeamTwo
        )
        {
            var label = Math.Abs(numWinsTeamOne - numWinsTeamTwo);
            if (label < 2)
            {
                label = 2;
            }
            return $"{label}";
        }

        public bool SetIsTerminalStateFunc(
            int numWinsTeamOne, 
            int numWinsTeamTwo
        )
        {
            return (numWinsTeamOne >= 6 && numWinsTeamTwo <= 4)
                || (numWinsTeamTwo >= 6 && numWinsTeamOne <= 4)
                || numWinsTeamOne >= 7
                || numWinsTeamTwo >= 7;
        }

        public bool MatchIsTerminalStateFunc(
            int numWinsTeamOne,
            int numWinsTeamTwo
        )
        {
            return numWinsTeamOne >= 2 || numWinsTeamTwo >= 2;
        }

        public string SetOrMatchTerminalLabelFunc(
            int numWinsTeamOne, 
            int numWinsTeamTwo
        )
        {
            return $"{Math.Abs(numWinsTeamOne - numWinsTeamTwo)}";
        }

        public Dictionary<TennisGameState, double> GetStateTransitions(
           TennisGameState state,
           TennisGameSolverArgs args)
        {
            if (state.IsStateTerminalState())
            {
                return new Dictionary<TennisGameState, double>();
            }
            if(
                args.IsTerminalStateFunc(
                    state.NumWinsTeamOne + 1,
                    state.NumWinsTeamTwo
                ) && args.IsTerminalStateFunc(
                    state.NumWinsTeamOne,
                    state.NumWinsTeamTwo + 1
                )
            )
            {
                // don't add the same key twice if this will result in
                // a win either way
                return new Dictionary<TennisGameState, double>
                {
                    { new TennisGameState(
                    state.NumWinsTeamOne + 1,
                    state.NumWinsTeamTwo,
                    args.IsTerminalStateFunc,
                    args.TerminalLabelFunc
                    ), 1.0 }
                };
            }
            return new Dictionary<TennisGameState, double> {
                { new TennisGameState(
                    state.NumWinsTeamOne + 1,
                    state.NumWinsTeamTwo,
                    args.IsTerminalStateFunc,
                    args.TerminalLabelFunc
                ), 0.5 },
                { new TennisGameState(
                    state.NumWinsTeamOne,
                    state.NumWinsTeamTwo + 1,
                    args.IsTerminalStateFunc,
                    args.TerminalLabelFunc
                ), 0.5 },
            };
        }

        public class TennisGameState : IMarkovChainState
        {
            private Func<int, int, bool> _isTerminalStateFunc;
            private Func<int, int, string> _terminalLabelFunc;
            public TennisGameState(
                int numWinsTeamOne, 
                int numWinsTeamTwo,
                Func<int, int, bool> isTerminalStateFunc,
                Func<int, int, string> terminalLabelFunc
            )
            {
                this.NumWinsTeamOne = numWinsTeamOne;
                this.NumWinsTeamTwo = numWinsTeamTwo;
                this._isTerminalStateFunc = isTerminalStateFunc;
                this._terminalLabelFunc = terminalLabelFunc;
                this.StringRepresentation = this.GetStringRepresentation();
            }

            public int NumWinsTeamOne { get; }
            public int NumWinsTeamTwo { get; }
            public string StringRepresentation { get; } 
            
            private string GetStringRepresentation()
            {
                if (this.IsStateTerminalState())
                {
                    return $"Y{this.TerminalStateLabel()}";
                }
                return $"N{this.NumWinsTeamOne}:{this.NumWinsTeamTwo}";
            }

            public bool IsStateTerminalState()
            {
                return this._isTerminalStateFunc(
                    this.NumWinsTeamOne,
                    this.NumWinsTeamTwo
                );
            }

            public string TerminalStateLabel()
            {
                return this._terminalLabelFunc(
                    this.NumWinsTeamOne,
                    this.NumWinsTeamTwo
                );
            }

            public override bool Equals(object obj)
            {
                if (!(obj is TennisGameState))
                {
                    return false;
                }
                return ((TennisGameState)obj).StringRepresentation 
                    == this.StringRepresentation;
            }

            public override int GetHashCode()
            {
                return this.StringRepresentation.GetHashCode();
            }
        }
    }

    public class TennisGameSolverArgs
    {
        public TennisGameSolverArgs(
            Func<int, int, bool> isTerminalStateFunc,
            Func<int, int, string> terminalLabelFunc
        ) { 
            this.IsTerminalStateFunc = isTerminalStateFunc;
            this.TerminalLabelFunc = terminalLabelFunc;
        }
        public Func<int, int, bool> IsTerminalStateFunc { get; }
        public Func<int, int, string> TerminalLabelFunc { get; }

    }
}