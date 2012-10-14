using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Chess.Foundation;

namespace Chess.Algorithm
{
    public class Attack
    {
        public static ObservableCollection<Move> AllAttackMoves(ObservableCollection<Move> moves)
        {
            ObservableCollection<Move> possible = new ObservableCollection<Move>();
            foreach (Move move in moves)
            {
                if (move.EndPosition.Piece != null)
                    possible.Add(move);
            }
            return possible;
        }
        public static ObservableCollection<Move> AllAbsoluteAttackMoves(ObservableCollection<Move> moves)
        {
            ObservableCollection<Move> possible = new ObservableCollection<Move>();
            ObservableCollection<Move> attackMoves = AllAttackMoves(moves);
            foreach (Move move in attackMoves) 
            {
                using (Simulator simulator = new Simulator(move))
                {
                    bool add = true;
                    foreach(Move returnMove in This.Game.OppositePlayer.AllMovesNoKing())
                    {
                        if (returnMove.EndPosition.PieceEquals(move.EndPosition))
                            add = false;
                    }
                    if (add) possible.Add(move);
                }
            }
            return possible;
        }
        public static ObservableCollection<Move> AllWinningAttackMoves(ObservableCollection<Move> moves)
        {
            ObservableCollection<Move> possible = new ObservableCollection<Move>();
            ObservableCollection<Move> attackMoves = AllAttackMoves(moves);
            foreach (Move move in attackMoves)
            {
                using (Simulator simulator = new Simulator(move))
                {
                    bool add = true;
                    foreach (Move returnMove in This.Game.OppositePlayer.AllMovesNoKing())
                    {
                        if (returnMove.EndPosition.PieceEquals(move.EndPosition) && !simulator.OldPiece.IsBetterThan(returnMove.EndPosition.Piece))
                            add = false;
                    }
                    if (add) possible.Add(move);
                }
            }
            return possible;
        }
        public static ObservableCollection<Move> AllWinningOrEqualAttackMoves(ObservableCollection<Move> moves)
        {
            ObservableCollection<Move> possible = new ObservableCollection<Move>();
            ObservableCollection<Move> attackMoves = AllAttackMoves(moves);
            foreach (Move move in attackMoves)
            {
                using (Simulator simulator = new Simulator(move))
                {
                    bool add = true;
                    foreach (Move returnMove in This.Game.OppositePlayer.AllMovesNoKing())
                    {
                        if (returnMove.EndPosition.PieceEquals(move.EndPosition) && !simulator.OldPiece.IsBetterThanOrEqual(returnMove.EndPosition.Piece))
                            add = false;                        
                    }
                    if (add) possible.Add(move);
                }
            }
            return possible;
        }
    }
}