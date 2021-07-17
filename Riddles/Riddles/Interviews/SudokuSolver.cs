using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Generic;

namespace Interviews
{
	public class SudokuSolver
	{
		char minChar = '1';
		char maxChar = '9';
		public char[,] SolveSudoku(char[,] sudoku)
		{
			this.SolveSudokuInternal(sudoku, 0, 0);
			return sudoku;
		}

		private bool SolveSudokuInternal(char[,] sudoku, int row, int col)
		{
			if(row == sudoku.GetLength(0))
			{
				return true;
			}

			var nextCol = (col + 1) % sudoku.GetLength(1);
			var nextRow = nextCol == 0 ? row + 1 : row;
			if(sudoku[row, col] != '.')
			{
				return this.SolveSudokuInternal(sudoku, nextRow, nextCol);
			}

			var charToAdd = minChar;
			while(charToAdd <= maxChar)
			{
				sudoku[row, col] = charToAdd;
				charToAdd++;
				if (!IsSolutionStillValid(sudoku, row, col)) { continue; }
				var isValidSolution = this.SolveSudokuInternal(sudoku, nextRow, nextCol);
				if (isValidSolution) { return true; }				
			}
			sudoku[row, col] = '.';
			return false;
		}

		private bool IsSolutionStillValid(char[,] sudoku, int i, int j)
		{
			// check row
			HashSet<char> valuesSeen = new HashSet<char>();
			for(int col=0; col<sudoku.GetLength(1); col++)
			{
				if(valuesSeen.Contains(sudoku[i, col])) { return false; }
				if (sudoku[i, col] != '.') { valuesSeen.Add(sudoku[i, col]); }
			}

			// check col
			valuesSeen = new HashSet<char>();
			for(int row=0; row<sudoku.GetLength(0); row++)
			{
				if(valuesSeen.Contains(sudoku[row, j])) { return false; }
				if (sudoku[row, j] != '.') { valuesSeen.Add(sudoku[row, j]); }
			}

			// check box
			int rowStart = i - (i % 3);
			int colStart = j - (j % 3);
			valuesSeen = new HashSet<char>();
			for(int row=rowStart; row<rowStart+3; row++)
			{
				for(int col=colStart; col<colStart+3; col++)
				{
					if (valuesSeen.Contains(sudoku[row, col])) { return false; }
					if (sudoku[row, col] != '.') { valuesSeen.Add(sudoku[row, col]); }
				}
			}
			return true;
		}
	}
}
