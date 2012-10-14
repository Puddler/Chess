using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Chess.Foundation;

namespace Chess.Algorithm
{
    public class Defense
    {
        public static ObservableCollection<Move> AllAvoidAttackMoves()
        {
            ObservableCollection<Piece> attackablePieces = new ObservableCollection<Piece>();
            foreach (Move move in This.Game.OppositePlayer.AllMoves())
            {
                if (move.EndPosition.Piece != null && !attackablePieces.Contains(move.EndPosition.Piece))
                    attackablePieces.Add(move.EndPosition.Piece);
            }
            ObservableCollection<Move> possible = new ObservableCollection<Move>();
            foreach (Piece piece in attackablePieces)
            {
                possible.AddRange<Move>(piece.LegalMoves);
            }
            return possible;
        }
        public static ObservableCollection<Move> AllAvoidAttackDefenseMoves(ObservableCollection<Move> moves)
        {            
            ObservableCollection<Move> avoidAttackMoves = AllAvoidAttackMoves();
            ObservableCollection<Move> possible = AllDefenseMoves(avoidAttackMoves);
            return possible;
        }
        public static ObservableCollection<Move> AllDefenseMoves(ObservableCollection<Move> moves)
        {
            ObservableCollection<Move> possible = new ObservableCollection<Move>();
            foreach (Move move in moves)
            {
                using (Simulator simulator = new Simulator(move))
                {
                    bool add = true;
                    foreach (Move returnMove in This.Game.OppositePlayer.AllMoves())
                    {
                        if (returnMove.EndPosition.PieceEquals(move.EndPosition))
                        {
                            add = false;
                        }
                    }
                    if (add) possible.Add(move);
                }
            }           
            return possible;
        }
        public static ObservableCollection<Move> AllAbsoluteDefenseMoves(ObservableCollection<Move> moves)
        {
            ObservableCollection<Move> defenseMoves = moves;// AllDefenseMoves(moves);
            ObservableCollection<Move> possible = new ObservableCollection<Move>();
            ObservableCollection<Move> possible2 = new ObservableCollection<Move>();  
            int rating = This.Game.CurrentPlayer.PiecesUnderAttack.TotalRating();
            int lowestRating = rating;
            int lowestRating2 = rating;
            foreach (Move move in defenseMoves)
            {                
                using (Simulator simulator = new Simulator(move))
                {
                    int newRating = This.Game.CurrentPlayer.PiecesUnderAttack.TotalRating();
                    if (newRating < lowestRating)
                    {
                        lowestRating = newRating;
                        possible.Add(move);
                    }
                    if (newRating <= lowestRating2)
                    {
                        lowestRating2 = newRating;
                        possible2.Add(move);
                    }
                }
            }
            if (possible.Count > 0) return possible;
            else return possible2;
        }

        public static ObservableCollection<Move> GetMoveRatings(ObservableCollection<Move> moves)
        {
            ObservableCollection<Move> possible = new ObservableCollection<Move>();
            ObservableCollection<Move> possible2 = new ObservableCollection<Move>();
            int rating = This.Game.CurrentPlayer.PiecesUnderAttack.TotalRating();
            int rating2 = rating;
            foreach(Move move in This.Game.CurrentPlayer.AllMoves())
            {
                using(Simulator simulator = new Simulator(move))
                {
                    int newRating = This.Game.CurrentPlayer.PiecesUnderAttack.TotalRating();
                    if (newRating < rating)
                    {
                        //rating = newRating;
                        possible.Add(move);
                    }
                    if (newRating <= rating2)
                    {
                        //rating2 = newRating;
                        possible2.Add(move);
                    }
                }
            }
            if (possible.Count > 0) return possible;
            else return possible2;
        }
    }
}