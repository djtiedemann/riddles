using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Interviews
{
	public class NQueenProblem
	{
		public List<bool[,]> FindQueenPlacements(int boardSize) {
			var board = new bool[boardSize, boardSize];
			var solutions = new List<bool[,]> { };
			var columnsSeen = new HashSet<int> { };
			this.FindQueenPlacementsInternal(board, 0, columnsSeen, solutions);
			return solutions;
		}

		private void FindQueenPlacementsInternal(bool[,] currentBoard, int currentRow, HashSet<int> columnsSeen, List<bool[,]> validBoards) {
			// in this case, we've placed n queens, we need to copy and return
			if (currentRow == currentBoard.GetLength(0)) {
				var newBoard = this.CopyBoard(currentBoard);
				validBoards.Add(newBoard);
				return;
			}
			
			// we're going to place a queen in this row. We need to find which column to place the queen in
			for(int col = 0; col<currentBoard.GetLength(1); col++)
			{
				if (!columnsSeen.Contains(col) && this.IsQueenPlacementValid(currentBoard, currentRow, col))
				{
					// update columnsSeen and currentBoard to add a queen to this position
					currentBoard[currentRow, col] = true;
					columnsSeen.Add(col);

					// make recursive call to check validity of this queen position
					this.FindQueenPlacementsInternal(currentBoard, currentRow + 1, columnsSeen, validBoards);

					// update columnsSeen and currentBoard to remove the queen from this position and continue to next column
					currentBoard[currentRow, col] = false;
					columnsSeen.Remove(col);
				}
			}
		}

		private bool[,] CopyBoard(bool[,] currentBoard) {
			var newBoard = new bool[currentBoard.GetLength(0), currentBoard.GetLength(1)];
			for (int row = 0; row < currentBoard.GetLength(0); row++) { 
				for(int col = 0; col<currentBoard.GetLength(1); col++)
				{
					newBoard[row, col] = currentBoard[row, col];
				}
			}
			return newBoard;
		}

		private bool IsQueenPlacementValid(bool[,] currentBoard, int row, int col)
		{
			// need to check diagonals
			// assume that rows and columns have already been checked
			bool wasLastIterationValid = true;
			int distance = 1;
			while (wasLastIterationValid) { 
				if((row - distance >= 0 || row + distance < currentBoard.GetLength(0)) 
					&& (col - distance >= 0 || col + distance < currentBoard.GetLength(0)))
				{
					var locations = new List<Tuple<int, int>> {
						Tuple.Create(row - distance, col - distance),
						Tuple.Create(row - distance, col + distance),
						Tuple.Create(row + distance, col - distance),
						Tuple.Create(row + distance, col + distance),
					};
					if(locations.Any(l => this.LocationHasQueen(currentBoard, l.Item1, l.Item2)))
					{
						return false;
					}
				} else
				{
					break;
				}
				distance++;
			}
			return true;
		}

		private bool LocationHasQueen(bool[,] currentBoard, int row, int col)
		{
			if (row < 0 || col < 0 || row >= currentBoard.GetLength(0) || col >= currentBoard.GetLength(0)) {
				return false;
			}
			return currentBoard[row, col];
		}
	}
}
