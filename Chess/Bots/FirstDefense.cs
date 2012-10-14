using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chess.Foundation;
using System.Collections.ObjectModel;
using Chess.VM;
using Chess.Algorithm;

namespace Chess.Bots
{
    public class FirstDefense : IChessBot
    {
        public string Name
        {
            get
            {
                return "First Defense";
            }
        }

        public FirstDefense()
        {
        }

        public Move SelectNextMove()
        {
            ObservableCollection<Move> moves = Defense.GetMoveRatings(This.Game.CurrentPlayer.AllMoves());            
            if (moves.Count == 0) moves = This.Game.CurrentPlayer.AllMovesNoKing();
            if (moves.Count == 0) moves = This.Game.CurrentPlayer.AllMoves();
            return this.RateMoves(moves);
        }
        private Move RateMoves(ObservableCollection<Move> moves)
        {
            return moves.Random();
        }
    }
}