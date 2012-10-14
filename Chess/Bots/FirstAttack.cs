using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chess.Foundation;
using Chess.VM;
using System.Collections.ObjectModel;
using Chess.Algorithm;

namespace Chess.Bots
{
    public class FirstAttack : IChessBot
    {
        public string Name
        {
            get
            {
                return "First Attack";
            }
        }

        public FirstAttack()
        {
        }

        public Move SelectNextMove()
        {
            ObservableCollection<Move> moves = Attack.AllAbsoluteAttackMoves(This.Game.CurrentPlayer.AllMovesNoKing());            
            if (moves.Count == 0) moves = Attack.AllAbsoluteAttackMoves(This.Game.CurrentPlayer.AllMoves());
            if (moves.Count == 0) moves = Attack.AllWinningAttackMoves(This.Game.CurrentPlayer.AllMoves());
            if (moves.Count == 0) moves = Attack.AllWinningOrEqualAttackMoves(This.Game.CurrentPlayer.AllMoves());
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