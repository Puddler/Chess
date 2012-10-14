using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chess.Foundation;

namespace Chess.Algorithm
{
    public class Simulator : IDisposable
    {
        public Move Move;
        public Piece OldPiece;
        public Simulator(Move move)
        {
            this.Move = move;
            this.OldPiece = move.EndPosition.Piece;
            This.Game.MakeMove(move);
            This.Board.ResetLegalMoves();    
        }

        public void Dispose()
        {
            This.Game.MakeMove(this.Move.ReverseMove());            
            this.Move.EndPosition.Piece = this.OldPiece;
            This.Board.ResetLegalMoves();
        }
    }
}