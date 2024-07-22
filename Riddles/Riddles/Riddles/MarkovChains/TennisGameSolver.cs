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

        public Dictionary<(int, int), double> CalculateOddsOfWinningStandardGame()
        {
            return this.CalculateOddsOfWinningByX(
                this.StandardGameTerminalLabelFunc
            );
        }

        public Dictionary<(int, int), double> CalculateOddsOfWinningTiebreakGame()
        {
            return this.CalculateOddsOfWinningByX(
                this.TiebreakGameTerminalLabelFunc
            );
        }

        public Dictionary<(int, int), double> CalculateOddsOfWinningSet()
        {
            return this.CalculateOddsOfWinningByX(
                this.SetTerminalLabelFunc
            );
        }

        public Dictionary<(int, int), double> CalculateOddsOfWinningMatch()
        {
            return this.CalculateOddsOfWinningByX(
                this.MatchTerminalLabelFunc
            );
        }

        private Dictionary<(int, int), double> CalculateOddsOfWinningByX(
            Func<int, int, (int?, int?)> TerminalLabelFunc
        )
        {
            var odds = new Dictionary<int, double>();
            var args = new TennisGameSolverArgs(
                TerminalLabelFunc
            );
            var initialState = new TennisGameState(
                0,
                0,
                TerminalLabelFunc
            );
            var probabilitiesByState = this._markovChainSolver
                .CalculateProbabilityOfArrivingAtAllTerminalStates(
                    initialState,
                    this.GetStateTransitions,
                    args
                );
            var deduplicatedProbabilitesByState =
                 probabilitiesByState
                    .GroupBy(k => k.Key.GetTerminalStateNumWinsInfo())
                    .ToDictionary(
                        g => g.Key,
                        g => g.ToList().Sum(g => g.Value)
                    );
              
            return deduplicatedProbabilitesByState;
        }

        public (int?, int?) StandardGameTerminalLabelFunc(
            int numWinsTeamOne, 
            int numWinsTeamTwo)
        {
            if (numWinsTeamOne >= 4 || numWinsTeamTwo >=4)
            {
                return (Math.Max(numWinsTeamOne, numWinsTeamTwo),
                    Math.Min(numWinsTeamOne, numWinsTeamTwo));
            }
            else if (numWinsTeamOne >= 2 && numWinsTeamTwo >= 2)
            {
                return (4, 2);
            }
            return (null, null);
        }

        public (int?, int?) TiebreakGameTerminalLabelFunc(
            int numWinsTeamOne,
            int numWinsTeamTwo)
        {
            if (numWinsTeamOne >= 7 || numWinsTeamTwo >= 7)
            {
                return (Math.Max(numWinsTeamOne, numWinsTeamTwo),
                    Math.Min(numWinsTeamOne, numWinsTeamTwo));
            }
            else if (numWinsTeamOne >= 5 && numWinsTeamTwo >= 5)
            {
                return (7, 5);
            }
            return (null, null);
        }

        public (int?, int?) SetTerminalLabelFunc(
            int numWinsTeamOne, 
            int numWinsTeamTwo
        )
        {
            if((numWinsTeamOne >= 6 && numWinsTeamTwo <= 4)
                || (numWinsTeamTwo >= 6 && numWinsTeamOne <= 4))
            {
                return (Math.Max(numWinsTeamOne, numWinsTeamTwo),
                    Math.Min(numWinsTeamOne, numWinsTeamTwo));
            }
            else if(numWinsTeamOne >= 7 || numWinsTeamTwo >= 7)
            {
                return (7, 6);
            }
            return (null, null);
        }

        public (int?, int?) MatchTerminalLabelFunc(
            int numWinsTeamOne,
            int numWinsTeamTwo
        )
        {
            if(numWinsTeamOne >= 2 || numWinsTeamTwo >= 2)
            {
                return (Math.Max(numWinsTeamOne, numWinsTeamTwo),
                    Math.Min(numWinsTeamOne, numWinsTeamTwo));
            }
            return (null, null);
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
                args.TerminalLabelFunc(
                    state.NumWinsTeamOne + 1,
                    state.NumWinsTeamTwo
                ).Item1 != null && args.TerminalLabelFunc(
                    state.NumWinsTeamOne,
                    state.NumWinsTeamTwo + 1
                ).Item1 != null
            )
            {
                // don't add the same key twice if this will result in
                // a win either way
                return new Dictionary<TennisGameState, double>
                {
                    { new TennisGameState(
                        state.NumWinsTeamOne + 1,
                        state.NumWinsTeamTwo,
                        args.TerminalLabelFunc
                    ), 1.0 }
                };
            }
            return new Dictionary<TennisGameState, double> {
                { new TennisGameState(
                    state.NumWinsTeamOne + 1,
                    state.NumWinsTeamTwo,
                    args.TerminalLabelFunc
                ), 0.5 },
                { new TennisGameState(
                    state.NumWinsTeamOne,
                    state.NumWinsTeamTwo + 1,
                    args.TerminalLabelFunc
                ), 0.5 },
            };
        }

        public class TennisGameState : IMarkovChainState
        {
            private Func<int, int, (int?, int?)> _terminalLabelFunc;
            public TennisGameState(
                int numWinsTeamOne, 
                int numWinsTeamTwo,
                Func<int, int, (int?, int?)> terminalLabelFunc
            ) 
            {
                this.NumWinsTeamOne = numWinsTeamOne;
                this.NumWinsTeamTwo = numWinsTeamTwo;
                this._terminalLabelFunc = terminalLabelFunc;
                this.StringRepresentation = this.GetStringRepresentation();
            }

            public int NumWinsTeamOne { get; }
            public int NumWinsTeamTwo { get; }
            public string StringRepresentation { get; } 
            
            private string GetStringRepresentation()
            {
                return $"{this.NumWinsTeamOne}:{this.NumWinsTeamTwo}";
            }

            public bool IsStateTerminalState()
            {
                return this._terminalLabelFunc(
                    this.NumWinsTeamOne,
                    this.NumWinsTeamTwo
                ).Item1 != null;
            }

            public (int, int) GetTerminalStateNumWinsInfo()
            {
                var numGamesInfo = this._terminalLabelFunc(
                    this.NumWinsTeamOne,
                    this.NumWinsTeamTwo
                );
                return (numGamesInfo.Item1.Value, numGamesInfo.Item2.Value);
            }

            public string TerminalStateLabel()
            {
                return this.GetStringRepresentation();
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
            Func<int, int, (int?, int?)> terminalLabelFunc
        ) { 
            this.TerminalLabelFunc = terminalLabelFunc;
        }
        public Func<int, int, (int?, int?)> TerminalLabelFunc { get; }

    }
}