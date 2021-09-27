using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Riddles.Combinatorics.Application
{
	// https://fivethirtyeight.com/features/how-many-ways-can-you-build-a-staircase/

	public class TiltedStairStacker
	{
		public int GetNumberOfValidWaysToBuildStaircase(int numRows)
		{
			if(numRows < 0)
			{
				throw new InvalidOperationException("numRows cannot be less than 0");
			}
			// for 0 rows, there are 0 ways to do it. for 1 row, there is 1 way to do it
			if(numRows <= 1)
			{
				return numRows;
			}

			var blockInRightmostColumn = new Block(0, numRows - 1);
			// we will start from the second to right row, and determine all of the ways to order those blocks
			// and then use those ways to determine the ways to order the next column to the right, up until we have ordered all columns
			//
			// there's only 1 way to order the column with 1 block.
			var blocksInColumnsToTheRightOfCurrentColumn = new List<List<Block>> { new List<Block> { blockInRightmostColumn } };
			for(int column=numRows - 2; column>=0; column--)
			{
				List<Block> blocksInCurrentColumn = new List<Block>();
				for(int row = 0; row < numRows - column; row++){
					blocksInCurrentColumn.Add(new Block(row, column));
				}
				var blocksInCurrentColumnArray = blocksInCurrentColumn.ToArray();
				List<List<Block>> validWaysToBuildStaircaseForThisColumnToTheRight = new List<List<Block>>();
				foreach(var enumeration in blocksInColumnsToTheRightOfCurrentColumn)
				{
					this.EnumerateValidWaysToBuildStaircase(
						blocksInCurrentColumnArray,
						enumeration.ToArray(),
						validWaysToBuildStaircaseForThisColumnToTheRight,
						0,
						0,
						new List<Block>());
				}
				blocksInColumnsToTheRightOfCurrentColumn = validWaysToBuildStaircaseForThisColumnToTheRight;
			}
			return blocksInColumnsToTheRightOfCurrentColumn.Count();
		}

		private void EnumerateValidWaysToBuildStaircase(
			Block[] blocksInLeftmostColumn, 
			Block[] blocksInOtherColumns,
			List<List<Block>> accumulatedListOfBlocks,
			int indexForBlocksInLeftmostColumn,
			int indexForBlocksInOtherColumns,
			List<Block> blocksInCurrentList)
		{
			// if we've placed all of the blocks, then add it to the accumulated list of blocks and return
			if(blocksInCurrentList.Count == blocksInLeftmostColumn.Length + blocksInOtherColumns.Length)
			{
				accumulatedListOfBlocks.Add(blocksInCurrentList);
				return;
			}

			// if the next block in the leftmost column list could go before the next bock in the remaining column list
			// then place that block next and continue with the recursion
			if(indexForBlocksInLeftmostColumn < blocksInLeftmostColumn.Length)
			{
				// if there are no remaining blocks in the blocks for other columns, then the next block in the leftmost column list must go next
				// if there is a remaining block in the blocks for other columns, then the next block in the leftmost column can go next if it's
				// allowed to be placed before the next block in the other columns
				if(indexForBlocksInOtherColumns >= blocksInOtherColumns.Length ||
					blocksInLeftmostColumn[indexForBlocksInLeftmostColumn]
					.CanBlockBePlacedBeforeOtherBlock(blocksInOtherColumns[indexForBlocksInOtherColumns]))
				{
					var updatedListOfBlocks = new List<Block>(blocksInCurrentList);
					updatedListOfBlocks.Add(blocksInLeftmostColumn[indexForBlocksInLeftmostColumn]);
					this.EnumerateValidWaysToBuildStaircase(
						blocksInLeftmostColumn, blocksInOtherColumns, accumulatedListOfBlocks,
						indexForBlocksInLeftmostColumn + 1, indexForBlocksInOtherColumns, updatedListOfBlocks);
				}
			}

			// now, do the same thing, except check if the next block in the reamining column list can go before the next block in the leftmost column list
			if(indexForBlocksInOtherColumns < blocksInOtherColumns.Length)
			{
				if (indexForBlocksInLeftmostColumn >= blocksInLeftmostColumn.Length ||
					blocksInOtherColumns[indexForBlocksInOtherColumns]
					.CanBlockBePlacedBeforeOtherBlock(blocksInLeftmostColumn[indexForBlocksInLeftmostColumn]))
				{
					var updatedListOfBlocks = new List<Block>(blocksInCurrentList);
					updatedListOfBlocks.Add(blocksInOtherColumns[indexForBlocksInOtherColumns]);
					this.EnumerateValidWaysToBuildStaircase(
						blocksInLeftmostColumn, blocksInOtherColumns, accumulatedListOfBlocks,
						indexForBlocksInLeftmostColumn, indexForBlocksInOtherColumns + 1, updatedListOfBlocks);
				}
			}
		}

		public class Block
		{
			public Block(int row, int column)
			{
				this.Row = row;
				this.Column = column;
			}
			public int Row { get; }
			public int Column { get; }

			public bool CanBlockBePlacedBeforeOtherBlock(Block other)
			{
				if (this.Row == other.Row && this.Column == other.Column) { return true; }
				if (this.Row < other.Row && this.Column < other.Column) { return true; }
				if (this.Row > other.Row && this.Column > other.Column) { return false; }
				if (this.Row == other.Row) { return this.Column < other.Column ? true : false; }
				if (this.Column == other.Column) { return this.Row < other.Row ? true : false; }
				return true;
			}
		}
	}
}
