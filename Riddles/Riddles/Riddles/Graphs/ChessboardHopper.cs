using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Riddles.Graphs
{
	public enum Piece
	{
		White_Pawn = 1,
		White_Bishop = 2,
		White_Knight = 3,
		White_Rook = 4,
		White_Queen = 5,
		White_King = 6,
		Black_Pawn = 7,
		Black_Bishop = 8,
		Black_Knight = 9,
		Black_Rook = 10,
		Black_Queen = 11,
		Black_King = 12,
	}
	public class Chessboard {
		private Piece[,] _chessboard;
		public Chessboard(Piece[,] chessboard)
		{
			this._chessboard = chessboard;
		}

		public Piece GetPieceAtLocation(Tuple<int, int> location) {
			return this._chessboard[location.Item1, location.Item2];
		}

		public int GetSize()
		{
			return this._chessboard.GetLength(0);
		}
	}

	public class ChessboardHopper
	{
		private Chessboard _chessboard;
		public ChessboardHopper(Chessboard chessboard) {
			this._chessboard = chessboard;
		}

		public List<Tuple<int, int>> FindPathToOpposingKings(Tuple<int, int> startingLocation)
		{
			var previousLocations = new Dictionary<Tuple<int, int>, Tuple<int, int>>();
			previousLocations[startingLocation] = null;
			var locationsToProcess = new List<Tuple<int, int>> { startingLocation };
			Tuple<int, int> currentLocation = null;
			Tuple<int, int> finalLocation = null;
			while(locationsToProcess.Count > 0)
			{
				currentLocation = locationsToProcess[0];
				if (this.IsFinalLocation(currentLocation))
				{
					finalLocation = currentLocation;
					break;
				}
				locationsToProcess.RemoveAt(0);
				var nextLocations = this.GetPossibleMoves(currentLocation);
				nextLocations = nextLocations.Where(l => !previousLocations.ContainsKey(l)).ToList();

				foreach (var nextLocation in nextLocations) {
					previousLocations[nextLocation] = currentLocation;
					locationsToProcess.Add(nextLocation);
				}
			}
			currentLocation = finalLocation;
			var path = new List<Tuple<int, int>>();
			while(currentLocation != null)
			{
				path.Add(currentLocation);
				currentLocation = previousLocations[currentLocation];
			}
			path.Reverse();
			return path;
		}

		private bool IsFinalLocation(Tuple<int, int> location)
		{
			return this._chessboard.GetPieceAtLocation(location) == Piece.Black_King;
		}

		private List<Tuple<int, int>> GetPossibleMoves(Tuple<int, int> currentLocation) {
			var currentPiece = this._chessboard.GetPieceAtLocation(currentLocation);
			List<Tuple<int, int>> possibleMoves;
			switch (currentPiece) {
				case Piece.White_Pawn:
					possibleMoves = new List<Tuple<int, int>> { 
						Tuple.Create(currentLocation.Item1-1, currentLocation.Item2-1),
						Tuple.Create(currentLocation.Item1-1, currentLocation.Item2+1)
					};
					return this.FilterLocationsToThoseWithinBoard(possibleMoves);

				case Piece.White_Bishop:
					possibleMoves = new List<Tuple<int, int>> { 
						Tuple.Create(currentLocation.Item1-1, currentLocation.Item2+1),
						Tuple.Create(currentLocation.Item1+1, currentLocation.Item2+1),
						Tuple.Create(currentLocation.Item1-1, currentLocation.Item2-1),
						Tuple.Create(currentLocation.Item1+1, currentLocation.Item2-1)
					};
					return this.FilterLocationsToThoseWithinBoard(possibleMoves);

				case Piece.White_Rook:
					possibleMoves = new List<Tuple<int, int>> {
						Tuple.Create(currentLocation.Item1-1, currentLocation.Item2),
						Tuple.Create(currentLocation.Item1+1, currentLocation.Item2),
						Tuple.Create(currentLocation.Item1, currentLocation.Item2-1),
						Tuple.Create(currentLocation.Item1, currentLocation.Item2+1)
					};
					return this.FilterLocationsToThoseWithinBoard(possibleMoves);

				case Piece.White_Queen:
					possibleMoves = new List<Tuple<int, int>> {
						Tuple.Create(currentLocation.Item1-1, currentLocation.Item2),
						Tuple.Create(currentLocation.Item1+1, currentLocation.Item2),
						Tuple.Create(currentLocation.Item1, currentLocation.Item2-1),
						Tuple.Create(currentLocation.Item1, currentLocation.Item2+1),
						Tuple.Create(currentLocation.Item1-1, currentLocation.Item2+1),
						Tuple.Create(currentLocation.Item1+1, currentLocation.Item2+1),
						Tuple.Create(currentLocation.Item1-1, currentLocation.Item2-1),
						Tuple.Create(currentLocation.Item1+1, currentLocation.Item2-1)
					};
					return this.FilterLocationsToThoseWithinBoard(possibleMoves);

				case Piece.White_Knight:
					possibleMoves = new List<Tuple<int, int>> {
						Tuple.Create(currentLocation.Item1-2, currentLocation.Item2+1),
						Tuple.Create(currentLocation.Item1-2, currentLocation.Item2-1),
						Tuple.Create(currentLocation.Item1-1, currentLocation.Item2+2),
						Tuple.Create(currentLocation.Item1-1, currentLocation.Item2-2),
						Tuple.Create(currentLocation.Item1+1, currentLocation.Item2+2),
						Tuple.Create(currentLocation.Item1+1, currentLocation.Item2-2),
						Tuple.Create(currentLocation.Item1+2, currentLocation.Item2+1),
						Tuple.Create(currentLocation.Item1+2, currentLocation.Item2-1)
					};
					return this.FilterLocationsToThoseWithinBoard(possibleMoves);

				// based on the rules, we can't continue from any of the black pieces or the white king
				case Piece.Black_Pawn:
				case Piece.Black_Bishop:
				case Piece.Black_Knight:
				case Piece.Black_Rook:
				case Piece.Black_Queen:
				case Piece.Black_King:
				case Piece.White_King:
					return new List<Tuple<int, int>>();

				default:
					throw new InvalidOperationException("Unexpected piece type");

			}
		}

		private List<Tuple<int, int>> FilterLocationsToThoseWithinBoard(List<Tuple<int, int>> possibleMoves)
		{
			return possibleMoves.Where(
				l => l.Item1 >= 0 && 
				l.Item1 < this._chessboard.GetSize() &&
				l.Item2 >= 0 &&
				l.Item2 < this._chessboard.GetSize()
			).ToList();
		}
	}
}
