using System;
using System.Collections.Generic;
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
                this.GameTerminalStateLabel,
                2,
                4
            );
        }

        public Dictionary<int, double> CalculateOddsOfWinningTiebreakGame()
        {
            return this.CalculateOddsOfWinningByX(
                this.TiebreakGameIsTerminalStateFunc,
                this.GameTerminalStateLabel,
                2,
                7
            );
        }

        private Dictionary<int, double> CalculateOddsOfWinningByX(
            Func<int, int, bool> isTerminalStateFunc,
            Func<int, int, string> terminalLabelFunc,
            int minPointsToWinBy,
            int maxPointsToWinBy
        )
        {
            var odds = new Dictionary<int, double>();
            var args = new TennisGameSolverArgs(
                isTerminalStateFunc,
                terminalLabelFunc
            );
            for(int i=minPointsToWinBy; i<=maxPointsToWinBy; i++)
            {
                var oddsOfSpecificState =
                    this._markovChainSolver
                        .GetProbabilityOfArrivingAtSpecificTerminalStateLabel(
                            new TennisGameState(
                                0, 
                                0, 
                                isTerminalStateFunc, 
                                terminalLabelFunc
                            ),
                            this.GetStateTransitions,
                            $"{i}",
                            args
                        );
                odds[i] = oddsOfSpecificState;
            }
            return odds;
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

        public Dictionary<TennisGameState, double> GetStateTransitions(
           TennisGameState state,
           TennisGameSolverArgs args)
        {
            if (state.IsStateTerminalState())
            {
                return new Dictionary<TennisGameState, double>();
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