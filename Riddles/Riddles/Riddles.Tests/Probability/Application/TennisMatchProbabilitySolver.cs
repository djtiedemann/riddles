using System;
using System.Collections.Generic;
using System.Text;

namespace Riddles.Tests.Probability.Application
{
    // in a 3 set tennis match, what are the odds that you win
    // the game, but lose the majority of the points
    public class TennisMatchProbabilitySolver
    {
        private Dictionary<GameState, double> _probability;

        public TennisMatchProbabilitySolver() { 
            this._probability = new Dictionary<GameState, double>();
        }

        public double CalculateProbabilityOfWinningGameAndLosingMorePoints()
        {
            var initialState = new GameState(0, 0, 0, 0, 0, 0, 0);
            var initialProbability = 1.0;
            List<(GameState, double)> gameStatesToProcess =
                new List<(GameState, double)> 
                    { (initialState, initialProbability)};
            while(gameStatesToProcess.Count != 0)
            {
                var (gameState, probability) = gameStatesToProcess[0];
                gameStatesToProcess.RemoveAt(0);
                // if we've reached a state we've already seen before
                // increase the probability for that state
                if (this._probability.ContainsKey(gameState))
                {
                    this._probability[gameState] += probability;
                }
                // if the match is over, don't add to the queue
                if(this.IsMatchOver(
                    gameState.NumSetsWonPlayerOne,
                    gameState.NumSetsWonPlayerTwo))
                {
                    continue;
                }
                var nextStates = this.GetNextGameStates(gameState, probability);
                gameStatesToProcess.AddRange(nextStates);
            }

            // go through all terminal states and count the ones that
            // were won with a minority of points
            double probabilityOfWinningAndWinningMorePoints = 0.0;
            double probabilityOfWinningAndWinningFewerPoints = 0.0;

            foreach(var gameState in this._probability.Keys)
            {
                if(this.IsMatchOver(
                    gameState.NumSetsWonPlayerOne, 
                    gameState.NumSetsWonPlayerTwo
                ))
                {
                    if(gameState.NumSetsWonPlayerOne > gameState.NumSetsWonPlayerTwo)
                    {
                        if(gameState.NetPointDifferential > 0)
                        {
                            probabilityOfWinningAndWinningMorePoints +=
                                this._probability[gameState];
                        }
                        else
                        {
                            probabilityOfWinningAndWinningFewerPoints +=
                                this._probability[gameState];
                        }
                    }
                    else
                    {
                        if(gameState.NetPointDifferential < 0)
                        {
                            probabilityOfWinningAndWinningMorePoints +=
                                this._probability[gameState];
                        }
                        else
                        {
                            probabilityOfWinningAndWinningFewerPoints +=
                                this._probability[gameState];
                        }
                    }
                }
            }
            return probabilityOfWinningAndWinningFewerPoints;
        }

        private List<(GameState, double)> GetNextGameStates(
            GameState gameState, double currentProbability)
        {
            var newGameStates = new List<(GameState, double)>();
            newGameStates.AddRange(this.GetGameStatesAfterWin(
                gameState: gameState,
                probability: currentProbability * 0.5,
                didPlayerOneWin: true
            ));
            newGameStates.AddRange(this.GetGameStatesAfterWin(
                gameState: gameState,
                probability: currentProbability * 0.5,
                didPlayerOneWin: false
            ));
            return newGameStates;
        }

        private List<(GameState, double)> GetGameStatesAfterWin(
            GameState gameState,
            double probability,
            bool didPlayerOneWin
        )
        {
            bool isTiebreakGame = gameState.NumSetsWonPlayerOne == 6
                && gameState.NumSetsWonPlayerTwo == 6;
            bool isGameOver = isTiebreakGame
                ? this.IsTiebreakGameOverInfo(
                    gameState.WinningPlayerNumPoints(didPlayerOneWin) + 1,
                    gameState.LosingPlayerNumPoints(didPlayerOneWin))
                : this.IsStandardGameOverInfo(
                    gameState.WinningPlayerNumPoints(didPlayerOneWin) + 1,
                    gameState.LosingPlayerNumPoints(didPlayerOneWin));
            bool isTie = isGameOver &&
                gameState.WinningPlayerNumPoints(didPlayerOneWin) < 
                    gameState.LosingPlayerNumPoints(didPlayerOneWin);
            bool isSetOver = isGameOver
                && this.IsSetOver(
                    gameState.WinningPlayerNumSets(didPlayerOneWin) + 1,
                    gameState.NumSetsWonPlayerTwo);
            var newProbability = isTie ? probability * 0.5 : probability;
            var newGameStates = new List<(GameState, double)>
            {
                (this.GetNextGameState(
                    gameState: gameState,
                    didPlayerOneWin: didPlayerOneWin,
                    isGameOver: isGameOver,
                    isSetOver: isSetOver,
                    isTie: isTie
                ), newProbability)
            };
            if (isTie)
            {
                newGameStates.Add((
                    this.GetNextGameState(
                        gameState: gameState,
                        didPlayerOneWin: didPlayerOneWin,
                        isGameOver: isGameOver,
                        isSetOver: isSetOver,
                        isTie: isTie
                    ), newProbability
                ));
            }
            return newGameStates;
        }

        private GameState GetNextGameState(
            GameState gameState,
            bool didPlayerOneWin,
            bool isGameOver,
            bool isSetOver,
            bool isTie)
        {
            var newNumSetsWonPlayerOne = isSetOver && didPlayerOneWin
                ? gameState.NumSetsWonPlayerOne + 1
                : gameState.NumSetsWonPlayerOne;
            var newNumSetsWonPlayerTwo = isSetOver && !didPlayerOneWin
                ? gameState.NumSetsWonPlayerTwo + 1
                : gameState.NumSetsWonPlayerTwo;
            // a tie means the winning player will eventually net 2 points
            var changeInPointDifferential = isTie ? 2 : 1;
            // a negative point differential means player 2 has won more points
            if (!didPlayerOneWin)
            {
                changeInPointDifferential *= -1;
            }
            var newNumPointsWonByPlayerOne = isGameOver 
                ? 0
                : (didPlayerOneWin 
                    ? gameState.NumPointsWonPlayerOne + 1
                    : gameState.NumPointsWonPlayerOne);
            var newNumPointsWonByPlayerTwo = isGameOver
                ? 0
                : (didPlayerOneWin
                    ? gameState.NumPointsWonPlayerTwo + 1
                    : gameState.NumPointsWonPlayerTwo);
            var newNumGamesWonByPlayerOne = isSetOver
                ? 0
                : ((isGameOver && didPlayerOneWin)
                    ? gameState.NumGamesWonPlayerOne + 1
                    : gameState.NumGamesWonPlayerOne
                );
            var newNumGamesWonByPlayerTwo = isSetOver
                ? 0
                : ((isGameOver && !didPlayerOneWin)
                    ? gameState.NumGamesWonPlayerOne + 1
                    : gameState.NumGamesWonPlayerOne
                );
            return new GameState(
                numSetsWonPlayerOne: newNumSetsWonPlayerOne,
                numSetsWonPlayerTwo: newNumSetsWonPlayerTwo,
                numGamesWonPlayerOne: newNumGamesWonByPlayerOne,
                numGamesWonPlayerTwo: newNumGamesWonByPlayerTwo,
                numPointsWonPlayerOne: newNumPointsWonByPlayerOne,
                numPointsWonPlayerTwo: newNumPointsWonByPlayerTwo,
                netPointDifferential: gameState.NetPointDifferential +
                    changeInPointDifferential
            );
        }

        private bool IsStandardGameOverInfo(
            int numPointsPlayerOne,
            int numPointsPlayerTwo
        )
        {
            return (numPointsPlayerOne == 3 && numPointsPlayerTwo == 3) ||
                (numPointsPlayerOne >= 4 || numPointsPlayerTwo >= 4);
        }

        private bool IsTiebreakGameOverInfo(
            int numPointsPlayerOne,
            int numPointsPlayerTwo
        )
        {
           return (numPointsPlayerOne == 6 && numPointsPlayerTwo == 6) || 
                (numPointsPlayerOne >= 7 || numPointsPlayerTwo >= 7);
        }

        private bool IsSetOver(int numGamesPlayerOne, int numGamesPlayerTwo)
        {
            return (numGamesPlayerOne >= 7 || numGamesPlayerTwo >= 7) ||
                ((numGamesPlayerOne >= 6 && numGamesPlayerTwo <= 4)
                || (numGamesPlayerTwo >=6 && numGamesPlayerOne <=4));
        }

        private bool IsMatchOver(int numSetsPlayerOne, int numSetsPlayerTwo)
        {
            return (numSetsPlayerOne >= 2 || numSetsPlayerTwo >= 2);
        }
        
        public class GameState : Tuple<int, int, int, int, int, int, int>
        {
            public GameState(
                int numSetsWonPlayerOne,
                int numSetsWonPlayerTwo,
                int numGamesWonPlayerOne,
                int numGamesWonPlayerTwo,
                int numPointsWonPlayerOne,
                int numPointsWonPlayerTwo,
                int netPointDifferential
            ) : base(
                numSetsWonPlayerOne, 
                numSetsWonPlayerTwo, 
                numGamesWonPlayerOne,
                numGamesWonPlayerTwo,
                numPointsWonPlayerOne,
                numPointsWonPlayerTwo,
                netPointDifferential
            )
            {
            }

            public int NumSetsWonPlayerOne { get { return base.Item1; } }
            public int NumSetsWonPlayerTwo { get { return base.Item2; } }
            public int NumGamesWonPlayerOne { get { return base.Item3; } }
            public int NumGamesWonPlayerTwo { get { return base.Item4; } }
            public int NumPointsWonPlayerOne { get { return base.Item5; } }
            public int NumPointsWonPlayerTwo { get { return base.Item6; } }
            public int NetPointDifferential { get { return base.Item7; } }
            public int WinningPlayerNumPoints(bool didPlayerOneWin)
            {
                return didPlayerOneWin
                    ? this.NumPointsWonPlayerOne
                    : this.NumPointsWonPlayerTwo;
            }
            public int LosingPlayerNumPoints(bool didPlayerOneWin)
            {
                return didPlayerOneWin
                    ? this.NumPointsWonPlayerTwo
                    : this.NumPointsWonPlayerOne;
            }
            public int WinningPlayerNumGames(bool didPlayerOneWin)
            {
                return didPlayerOneWin
                    ? this.NumGamesWonPlayerOne
                    : this.NumGamesWonPlayerTwo;
            }
            public int LosingPlayerNumGames(bool didPlayerOneWin)
            {
                return didPlayerOneWin
                    ? this.NumGamesWonPlayerTwo
                    : this.NumGamesWonPlayerOne;
            }
            public int WinningPlayerNumSets(bool didPlayerOneWin)
            {
                return didPlayerOneWin
                    ? this.NumSetsWonPlayerOne
                    : this.NumSetsWonPlayerTwo;
            }
            public int LosingPlayerNumSets(bool didPlayerOneWin)
            {
                return didPlayerOneWin
                    ? this.NumSetsWonPlayerTwo
                    : this.NumSetsWonPlayerOne;
            }
        }
    }
}