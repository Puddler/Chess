using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace Chess.Foundation
{
    public static class MethodExtensions
    {
        public static void AddRange<T>(this ObservableCollection<T> collection1, ObservableCollection<T> collection2)
        {
            foreach (T t in collection2)
                collection1.Add(t);
        }
        public static char Add(this char file, int moves)
        {
            return Convert.ToChar((int)file + moves);
        }
        public static char ToChar(this int file)
        {
            return Convert.ToChar(file);
        }
        public static bool CorrectPiece(this Materials material, Player turn)
        {
            if (turn.Team == Team.White && material.IsWhite()) return true;
            if (turn.Team == Team.Black && material.IsBlack()) return true;
            return false;
        }
        public static int ToInt(this char file)
        {
            return Convert.ToInt32(file);
        }
        public static bool IsWhite(this Materials material)
        {
            return material == Materials.WhiteBishop || material == Materials.WhiteKing || material == Materials.WhiteKnight || material == Materials.WhitePawn || material == Materials.WhiteQueen || material == Materials.WhiteRook;
        }
        public static bool IsBlack(this Materials material)
        {
            return material == Materials.BlackBishop || material == Materials.BlackKing || material == Materials.BlackKnight || material == Materials.BlackPawn || material == Materials.BlackQueen || material == Materials.BlackRook;
        }
        public static bool MatchesColor(this Materials firstMaterial, Materials secondMaterial)
        {
            if (firstMaterial.IsWhite() && secondMaterial.IsWhite()) return true;
            else if (firstMaterial.IsBlack() && secondMaterial.IsBlack()) return true;
            else return false;
        }
        public static bool IsAKing(this Materials material)
        {
            return material == Materials.WhiteKing || material == Materials.BlackKing;
        }
        public static bool IsAPawn(this Materials material)
        {
            return material == Materials.WhitePawn || material == Materials.BlackPawn;
        }
        public static bool IsARook(this Materials material)
        {
            return material == Materials.WhiteRook || material == Materials.BlackRook;
        }
        public static bool IsAKnight(this Materials material)
        {
            return material == Materials.WhiteKnight || material == Materials.BlackKnight;
        }
        public static bool IsABishop(this Materials material)
        {
            return material == Materials.WhiteBishop || material == Materials.BlackBishop;
        }
        public static bool IsAQueen(this Materials material)
        {
            return material == Materials.WhiteQueen || material == Materials.BlackQueen;
        }
        public static Move ReverseMove(this Move move)
        {
            return new Move(move.EndPosition.File, move.EndPosition.Rank, move.StartPosition.File, move.StartPosition.Rank);
        }
        public static Move Random(this ObservableCollection<Move> moves)
        {
            ObservableCollection<Move> nonKing = new ObservableCollection<Move>(moves.Where(m => !m.StartPosition.Piece.IsAKing()));
            if (nonKing.Count == 0) return moves[new Random(DateTime.Now.Millisecond).Next(0, moves.Count() - 1)];
            else
            {
                Random random = new Random(DateTime.Now.Millisecond);
                int index = random.Next(0, nonKing.Count() - 1);
                return nonKing[index];
            }
        }
        public static bool IsAKing(this Piece piece)
        {
            return piece != null && (piece.Material == Materials.BlackKing || piece.Material == Materials.WhiteKing);
        }
        public static ObservableCollection<Piece> MaterialsNoKing(this Player player)
        {
            return new ObservableCollection<Piece>(player.Materials.Where(m => !m.Material.IsAKing()));            
        }
        public static ObservableCollection<Move> AllMoves(this Player player)
        {
            ObservableCollection<Move> moves = new ObservableCollection<Move>();
            foreach (Piece piece in player.Materials)
            {
                foreach (Move move in piece.LegalMoves)
                {
                    moves.Add(move);
                }
            }
            return moves;             
        }
        public static ObservableCollection<Move> AllMovesNoKing(this Player player)
        {
            ObservableCollection<Move> moves = new ObservableCollection<Move>();
            foreach (Piece piece in player.Materials)
            {
                if (piece.IsAKing()) continue;
                foreach (Move move in piece.LegalMoves)
                {
                    moves.Add(move);
                }
            }
            return moves;            
        }
        public static bool IsBetterThan(this Piece piece1, Piece piece2)
        {
            return piece1.Rating > piece2.Rating;
        }
        public static bool IsBetterThanOrEqual(this Piece piece1, Piece piece2)
        {
           return piece1.Rating >= piece2.Rating;
        }
        public static int TotalRating(this ObservableCollection<Piece> pieces)
        {
            return pieces.Sum(s => s.Rating);
        }
    }
}