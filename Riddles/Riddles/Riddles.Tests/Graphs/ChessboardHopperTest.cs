using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Riddles.Graphs.Core;
using Riddles.Graphs.Application;
using System.Linq;

namespace Riddles.Tests.Graphs
{
	public class ChessboardHopperTest
	{

		Dictionary<int, Chessboard> inputDictionary = new Dictionary<int, Chessboard> {
			{ 1, new Chessboard(new Piece[,]{
				{ Piece.Black_King, Piece.Black_Bishop, Piece.White_Bishop, Piece.Black_Bishop, Piece.Black_King, Piece.White_Rook, Piece.White_Bishop, Piece.White_Rook },
				{ Piece.White_Knight, Piece.White_Rook, Piece.White_Knight, Piece.White_Knight, Piece.White_Pawn, Piece.White_Knight, Piece.Black_King, Piece.White_Bishop },
				{ Piece.White_Rook, Piece.Black_Pawn, Piece.White_Knight, Piece.Black_Knight, Piece.White_Pawn, Piece.White_Rook, Piece.White_Pawn, Piece.White_Rook },
				{ Piece.Black_Pawn, Piece.Black_Bishop, Piece.White_Pawn, Piece.White_Rook, Piece.Black_Knight, Piece.White_Knight, Piece.Black_Rook, Piece.Black_Knight },
				{ Piece.White_Knight, Piece.White_Knight, Piece.White_Knight, Piece.White_Bishop, Piece.Black_Rook, Piece.White_Bishop, Piece.White_Rook, Piece.White_Bishop },
				{ Piece.White_Queen, Piece.White_Rook, Piece.White_Knight, Piece.White_Pawn, Piece.Black_Rook, Piece.White_Knight, Piece.White_Rook, Piece.White_Queen },
				{ Piece.Black_Bishop, Piece.Black_Rook, Piece.White_Rook, Piece.White_Pawn, Piece.White_Bishop, Piece.White_Pawn, Piece.White_Bishop, Piece.White_Queen },
				{ Piece.Black_King, Piece.White_Bishop, Piece.White_Queen, Piece.White_Knight, Piece.White_Pawn, Piece.White_Rook, Piece.White_Knight, Piece.White_Knight }
			})}
		};

		Dictionary<int, List<Tuple<int, int>>> correctPath = new Dictionary<int, List<Tuple<int, int>>> {
			{ 1, new List<Tuple<int, int>>{
				new Tuple<int, int>(6, 4),
				new Tuple<int, int>(5, 3),
				new Tuple<int, int>(4, 2),
				new Tuple<int, int>(5, 0),
				new Tuple<int, int>(4, 1),
				new Tuple<int, int>(2, 2),
				new Tuple<int, int>(1, 4),
				new Tuple<int, int>(0, 5),
				new Tuple<int, int>(0, 4)
			} }
		};

		[TestCase(1, 6, 4)]
		public void TestFindPathToOpposingKings(int testCaseId, int startX, int startY)
		{
			var chessboard = inputDictionary[testCaseId];
			var chessboardHopper = new ChessboardHopper(chessboard);
			var path = chessboardHopper.FindPathToOpposingKings(Tuple.Create(startX, startY));
			var expectedPath = this.correctPath[testCaseId];
			Assert.AreEqual(path.Count(), expectedPath.Count());
			for (int i = 0; i < path.Count(); i++) {
				Assert.AreEqual(path[i].Item1, expectedPath[i].Item1);
				Assert.AreEqual(path[i].Item2, expectedPath[i].Item2);
			}
		}
	}
}
