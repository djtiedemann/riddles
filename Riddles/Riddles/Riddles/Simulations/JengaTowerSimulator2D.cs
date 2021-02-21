using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Riddles.Simulations
{
	public class JengaTowerSimulator2D
	{
		private Random _randomNumberGenerator;
		public JengaTowerSimulator2D()
		{
			this._randomNumberGenerator = new Random();
		}

		/* 
		 * You have a 2D Jenga tower and are stacking blocks from the bottom to the top.
		 * You place each block on a random position on the block above it so that it's center is on the block below it.
		 * You want to know what's the expected number of blocks.
		 * https://fivethirtyeight.com/features/can-you-win-riddler-jenga/
		 */
		public double Simulate(int numTimes) { 
			if(numTimes <= 0)
			{
				throw new InvalidOperationException("NumTimes must be > 0");
			}
			int runningSum = 0;
			for(int i=0; i<numTimes; i++)
			{
				runningSum += this.Simulate();
			}
			return (double)runningSum / (double)numTimes;
		}

		private int Simulate()
		{
			var blocks = new List<JengaBlock>();
			var numBlocks = 0;
			var blockLength = 1;
			var firstBlock = new JengaBlock(++numBlocks, 0, blockLength);
			blocks.Add(firstBlock);
			
			while(blocks.All(b => b.IsStable()))
			{
				var topBlock = blocks.Last();
				var newBlockCenter = topBlock.MinX + (topBlock.MaxX - topBlock.MinX) * this._randomNumberGenerator.NextDouble();
				var newBlock = new JengaBlock(++numBlocks, newBlockCenter, blockLength);
				foreach(var block in blocks)
				{
					block.AddBlock(newBlockCenter);
				}
				blocks.Add(newBlock);
			}

			return blocks.Count;
		}

		public class JengaBlock
		{
			public JengaBlock(int height, double xCenter, double length)
			{
				this.Height = height;
				this.XCenter = xCenter;
				this.MinX = xCenter - (length / 2.0);
				this.MaxX = xCenter + (length / 2.0);
				this.NumBlocksSupporting = 0;
				this.CenterOfMassOfSupportedBlocks = xCenter;
			}

			public int Height { get;  }
			public double XCenter { get; }			
			public double MinX { get; }
			public double MaxX { get; }
			public double NumBlocksSupporting { get; private set; }
			public double CenterOfMassOfSupportedBlocks { get; private set; }

			public bool IsStable()
			{
				return this.CenterOfMassOfSupportedBlocks >= this.MinX && this.CenterOfMassOfSupportedBlocks <= this.MaxX;
			}

			public void AddBlock(double centerOfNewBlock)
			{
				this.CenterOfMassOfSupportedBlocks = (this.CenterOfMassOfSupportedBlocks * this.NumBlocksSupporting + centerOfNewBlock)
					/ (this.NumBlocksSupporting + 1);
				this.NumBlocksSupporting++;
			}
		}
	}
}
