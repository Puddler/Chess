using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chess.Foundation;
using System.Windows.Media;
using System.Collections.ObjectModel;

namespace Chess.VM
{
    public class BoardVM : VMBase
    {
        public Dictionary<int, BoardSquareVM> this[char file]
        {
            get
            {
                return _sortedSquares[file];
            }
            set
            {
                _sortedSquares[file] = value;
                this.RaisePropertyChanged("Squares");
            }
        }

        private Dictionary<char, Dictionary<int, BoardSquareVM>> _sortedSquares;

        private ObservableCollection<BoardSquareVM> _squares;
        public ObservableCollection<BoardSquareVM> Squares
        {
            get
            {
                if (_squares == null)
                {
                    _squares = new ObservableCollection<BoardSquareVM>();
                }
                return _squares;
            }
            set
            {
                _squares = value;
                RaisePropertyChanged("Squares");
            }
        }

        private BoardSquareVM _startSquare;
        public BoardSquareVM StartSquare
        {
            get
            {
                return _startSquare;
            }
            set
            {
                _startSquare = value;
                RaisePropertyChanged("StartSquare");
            }
        }
        
        public BoardVM()
        {
            this._sortedSquares = new Dictionary<char, Dictionary<int, BoardSquareVM>>();
        }
        
        public void SetupStartPosition()
        {
            foreach (Piece piece in This.Game.White.Materials)
            {
                this[piece.StartFile][piece.StartRank].Piece = piece;
            }

            foreach (Piece piece in This.Game.Black.Materials)
            {
                this[piece.StartFile][piece.StartRank].Piece = piece;
            }                                         
        }
        public void ClearBoard()
        {
            foreach (BoardSquareVM square in this.Squares)
                square.Piece = null;
        }        
        public void AddSquare(BoardSquareVM square)
        {
            if (!_sortedSquares.ContainsKey(square.File)) _sortedSquares.Add(square.File, new Dictionary<int, BoardSquareVM>());
            _sortedSquares[square.File].Add(square.Rank, square);
            this.Squares.Add(square);
        }
        
        public void ResetBoardColors()
        {
            foreach (BoardSquareVM square in This.Board.Squares)
                square.ResetColor();                            
        }
        public void ResetLegalMoves()
        {
            int legalMoves = 0;
            List<Move> toRemove = new List<Move>();               
            foreach (Piece piece in This.Game.CurrentPlayer.Materials)
            {
                toRemove.Clear();
                Piece checkPiece = null;
                piece.LegalMoves = this.GetAllLegalMoves(piece);
                legalMoves += piece.LegalMoves.Count;
                foreach (Move move in piece.LegalMoves)
                {
                    Piece oldPiece = move.EndPosition.Piece;
                    This.Game.MakeMove(move);
                    bool legalMove = true;
                    if (piece != null && piece.Material.IsAKing() && This.Game.CurrentPlayer.IsInCheck()) legalMove = false;
                    foreach (Piece oppositePiece in This.Game.OppositePlayer.Materials)
                    {
                        if (oldPiece != null && oppositePiece.StartFile == oldPiece.StartFile && oppositePiece.StartRank == oldPiece.StartRank) continue;
                        ObservableCollection<Move> possibleMoves = this.GetAllLegalMoves(oppositePiece);
                        foreach (Move possibleMove in possibleMoves)
                        {
                            if (!piece.Material.IsAKing() && possibleMove.EndPosition.HasAKing())
                            {
                                legalMove = false;
                                checkPiece = possibleMove.StartPosition.Piece;
                            }
                        }
                    }
                    if (!legalMove) toRemove.Add(move);
                    This.Game.MakeMove(move.ReverseMove());
                    move.EndPosition.Piece = oldPiece;
                }
                if (checkPiece != null)
                {                    
                    foreach (Move possibleMove in toRemove)
                    {
                        if (possibleMove.EndPosition.Piece != null && possibleMove.EndPosition.Piece.StartFile == checkPiece.StartFile && possibleMove.EndPosition.Piece.StartRank == checkPiece.StartRank)
                        {
                            Move capture = toRemove.FirstOrDefault(m => m.AsString == possibleMove.AsString);
                            if (capture != null) toRemove.Remove(capture);
                        }
                    }
                }
                legalMoves -= toRemove.Count;
                foreach (Move remove in toRemove) piece.LegalMoves.Remove(remove);                
            }            
            if (legalMoves == 0) This.Game.CurrentPlayer.KingStatus = KingStatus.Mate;
            else if (This.Game.CurrentPlayer.IsInCheck()) This.Game.CurrentPlayer.KingStatus = KingStatus.Check; 
            else This.Game.CurrentPlayer.KingStatus = KingStatus.Move;            
        }
        public ObservableCollection<Move> GetAllLegalMoves(Piece piece)
        {
            if (piece.Material == Materials.WhitePawn) return this.GetAllLegalWhitePawnMoves(piece);
            else if (piece.Material == Materials.BlackPawn) return this.GetAllLegalBlackPawnMoves(piece);
            else if (piece.Material == Materials.WhiteRook) return this.GetAllLegalWhiteRookMoves(piece);
            else if (piece.Material == Materials.BlackRook) return this.GetAllLegalBlackRookMoves(piece);
            else if (piece.Material == Materials.WhiteKnight) return this.GetAllLegalWhiteKnightMoves(piece);
            else if (piece.Material == Materials.BlackKnight) return this.GetAllLegalBlackKnightMoves(piece);
            else if (piece.Material == Materials.WhiteBishop) return this.GetAllLegalWhiteBishopMoves(piece);
            else if (piece.Material == Materials.BlackBishop) return this.GetAllLegalBlackBishopMoves(piece);
            else if (piece.Material == Materials.WhiteQueen) return this.GetAllLegalWhiteQueenMoves(piece);
            else if (piece.Material == Materials.BlackQueen) return this.GetAllLegalBlackQueenMoves(piece);
            else if (piece.Material == Materials.WhiteKing) return this.GetAllLegalWhiteKingMoves(piece);
            else if (piece.Material == Materials.BlackKing) return this.GetAllLegalBlackKingMoves(piece);
            else return new ObservableCollection<Move>();
        }

        private ObservableCollection<Move> GetAllLegalWhitePawnMoves(Piece piece)
        {
            ObservableCollection<Move> moves = new ObservableCollection<Move>();
            char file = piece.Square.File;
            int rank = piece.Square.Rank;

            if (piece.Square.Rank == 2)
            {
                if (rank + 2 <= 8 && This.Board[file][rank + 1].Piece == null && This.Board[file][rank + 2].Piece == null) 
                    moves.Add(new Move(file, rank, file, rank + 2));
            }
            if (rank + 1 <= 8 && This.Board[file][rank + 1].Piece == null) moves.Add(new Move(file, rank, file, rank + 1));

            if (rank <= 7)
            {
                if (file != 'H' && This.Board[file.Add(1)][rank + 1].HasBlackPiece()) moves.Add(new Move(file, rank, file.Add(1), rank + 1));                
                if (file != 'A' && This.Board[file.Add(-1)][rank + 1].HasBlackPiece()) moves.Add(new Move(file, rank, file.Add(-1), rank + 1));                
            }

            return moves;
        }
        private ObservableCollection<Move> GetAllLegalBlackPawnMoves(Piece piece)
        {
            ObservableCollection<Move> moves = new ObservableCollection<Move>();
            char file = piece.Square.File;
            int rank = piece.Square.Rank;

            if (piece.Square.Rank == 7)
            {
                if (rank - 2 >= 1 && This.Board[file][rank - 1].Piece == null && This.Board[file][rank - 2].Piece == null) 
                    moves.Add(new Move(file, rank, file, rank - 2));
            }
            if (rank - 1 >= 1 && This.Board[file][rank - 1].Piece == null) moves.Add(new Move(file, rank, file, rank - 1));

            if (rank >= 2)
            {
                if (file != 'H' && This.Board[file.Add(1)][rank - 1].HasWhitePiece()) moves.Add(new Move(file, rank, file.Add(1), rank - 1));                
                if (file != 'A' && This.Board[file.Add(-1)][rank - 1].HasWhitePiece()) moves.Add(new Move(file, rank, file.Add(-1), rank - 1));                
            }

            return moves;
        }
        private ObservableCollection<Move> GetAllLegalWhiteRookMoves(Piece piece)
        {
            ObservableCollection<Move> moves = new ObservableCollection<Move>();

            for (int file = piece.Square.File.ToInt() - 1; file >= 65; file--)
            {
                if (This.Board[file.ToChar()][piece.Square.Rank].HasWhitePiece()) break;
                moves.Add(new Move(piece.Square.File, piece.Square.Rank, file.ToChar(), piece.Square.Rank));
                if (This.Board[file.ToChar()][piece.Square.Rank].HasBlackPiece()) break;
            }

            for (int file = piece.Square.File.ToInt() + 1; file <= 72; file++)
            {
                if (This.Board[file.ToChar()][piece.Square.Rank].HasWhitePiece()) break;
                moves.Add(new Move(piece.Square.File, piece.Square.Rank, file.ToChar(), piece.Square.Rank));
                if (This.Board[file.ToChar()][piece.Square.Rank].HasBlackPiece()) break;
            }

            for (int rank = piece.Square.Rank + 1; rank <= 8; rank++)
            {
                if (This.Board[piece.Square.File][rank].HasWhitePiece()) break;
                moves.Add(new Move(piece.Square.File, piece.Square.Rank, piece.Square.File, rank));
                if (This.Board[piece.Square.File][rank].HasBlackPiece()) break;
            }

            for (int rank = piece.Square.Rank - 1; rank >= 1; rank--)
            {
                if (This.Board[piece.Square.File][rank].HasWhitePiece()) break;
                moves.Add(new Move(piece.Square.File, piece.Square.Rank, piece.Square.File, rank));
                if (This.Board[piece.Square.File][rank].HasBlackPiece()) break;
            }

            return moves;
        }
        private ObservableCollection<Move> GetAllLegalBlackRookMoves(Piece piece)
        {
            ObservableCollection<Move> moves = new ObservableCollection<Move>();

            for (int file = piece.Square.File.ToInt() - 1; file >= 65; file--)
            {
                if (This.Board[file.ToChar()][piece.Square.Rank].HasBlackPiece()) break;
                moves.Add(new Move(piece.Square.File, piece.Square.Rank, file.ToChar(), piece.Square.Rank));
                if (This.Board[file.ToChar()][piece.Square.Rank].HasWhitePiece()) break;
            }

            for (int file = piece.Square.File.ToInt() + 1; file <= 72; file++)
            {
                if (This.Board[file.ToChar()][piece.Square.Rank].HasBlackPiece()) break;
                moves.Add(new Move(piece.Square.File, piece.Square.Rank, file.ToChar(), piece.Square.Rank));
                if (This.Board[file.ToChar()][piece.Square.Rank].HasWhitePiece()) break;
            }

            for (int rank = piece.Square.Rank + 1; rank <= 8; rank++)
            {
                if (This.Board[piece.Square.File][rank].HasBlackPiece()) break;
                moves.Add(new Move(piece.Square.File, piece.Square.Rank, piece.Square.File, rank));
                if (This.Board[piece.Square.File][rank].HasWhitePiece()) break;
            }

            for (int rank = piece.Square.Rank - 1; rank >= 1; rank--)
            {
                if (This.Board[piece.Square.File][rank].HasBlackPiece()) break;
                moves.Add(new Move(piece.Square.File, piece.Square.Rank, piece.Square.File, rank));
                if (This.Board[piece.Square.File][rank].HasWhitePiece()) break;
            }

            return moves;
        }
        private ObservableCollection<Move> GetAllLegalWhiteKnightMoves(Piece piece)
        {
            ObservableCollection<Move> moves = new ObservableCollection<Move>();

            int file = piece.Square.File.ToInt();
            int rank = piece.Square.Rank;

            if (file - 2 >= 65 && rank + 1 <= 8 && !This.Board[Convert.ToChar(file - 2)][rank + 1].HasWhitePiece())
                moves.Add(new Move(file.ToChar(), rank, (file - 2).ToChar(), rank + 1));
            
            if (file - 2 >= 65 && rank - 1 >= 1 && !This.Board[Convert.ToChar(file - 2)][rank - 1].HasWhitePiece())
                moves.Add(new Move(file.ToChar(), rank, (file - 2).ToChar(), rank - 1));            

            if (file - 1 >= 65 && rank + 2 <= 8 && !This.Board[Convert.ToChar(file - 1)][rank + 2].HasWhitePiece())
                moves.Add(new Move(file.ToChar(), rank, (file - 1).ToChar(), rank + 2));

            if (file - 1 >= 65 && rank - 2 >= 1 && !This.Board[Convert.ToChar(file - 1)][rank - 2].HasWhitePiece())
                moves.Add(new Move(file.ToChar(), rank, (file - 1).ToChar(), rank - 2));

            if (file + 1 <= 72 && rank + 2 <= 8 && !This.Board[Convert.ToChar(file + 1)][rank + 2].HasWhitePiece())
                moves.Add(new Move(file.ToChar(), rank, (file + 1).ToChar(), rank + 2));

            if (file + 1 <= 72 && rank - 2 >= 1 && !This.Board[Convert.ToChar(file + 1)][rank - 2].HasWhitePiece())
                moves.Add(new Move(file.ToChar(), rank, (file + 1).ToChar(), rank - 2));
            
            if (file + 2 <= 72 && rank + 1 <= 8 && !This.Board[Convert.ToChar(file + 2)][rank + 1].HasWhitePiece())
                moves.Add(new Move(file.ToChar(), rank, (file + 2).ToChar(), rank + 1));

            if (file + 2 <= 72 && rank - 1 >= 1 && !This.Board[Convert.ToChar(file + 2)][rank - 1].HasWhitePiece())
                moves.Add(new Move(file.ToChar(), rank, (file + 2).ToChar(), rank - 1));

            return moves;
        }
        private ObservableCollection<Move> GetAllLegalBlackKnightMoves(Piece piece)
        {
            ObservableCollection<Move> moves = new ObservableCollection<Move>();

            int file = piece.Square.File.ToInt();
            int rank = piece.Square.Rank;

            if (file - 2 >= 65 && rank + 1 <= 8 && !This.Board[Convert.ToChar(file - 2)][rank + 1].HasBlackPiece())
                moves.Add(new Move(file.ToChar(), rank, (file - 2).ToChar(), rank + 1));

            if (file - 2 >= 65 && rank - 1 >= 1 && !This.Board[Convert.ToChar(file - 2)][rank - 1].HasBlackPiece())
                moves.Add(new Move(file.ToChar(), rank, (file - 2).ToChar(), rank - 1));

            if (file - 1 >= 65 && rank + 2 <= 8 && !This.Board[Convert.ToChar(file - 1)][rank + 2].HasBlackPiece())
                moves.Add(new Move(file.ToChar(), rank, (file - 1).ToChar(), rank + 2));

            if (file - 1 >= 65 && rank - 2 >= 1 && !This.Board[Convert.ToChar(file - 1)][rank - 2].HasBlackPiece())
                moves.Add(new Move(file.ToChar(), rank, (file - 1).ToChar(), rank - 2));

            if (file + 1 <= 72 && rank + 2 <= 8 && !This.Board[Convert.ToChar(file + 1)][rank + 2].HasBlackPiece())
                moves.Add(new Move(file.ToChar(), rank, (file + 1).ToChar(), rank + 2));

            if (file + 1 <= 72 && rank - 2 >= 1 && !This.Board[Convert.ToChar(file + 1)][rank - 2].HasBlackPiece())
                moves.Add(new Move(file.ToChar(), rank, (file + 1).ToChar(), rank - 2));

            if (file + 2 <= 72 && rank + 1 <= 8 && !This.Board[Convert.ToChar(file + 2)][rank + 1].HasBlackPiece())
                moves.Add(new Move(file.ToChar(), rank, (file + 2).ToChar(), rank + 1));

            if (file + 2 <= 72 && rank - 1 >= 1 && !This.Board[Convert.ToChar(file + 2)][rank - 1].HasBlackPiece())
                moves.Add(new Move(file.ToChar(), rank, (file + 2).ToChar(), rank - 1));

            return moves;
        }
        private ObservableCollection<Move> GetAllLegalWhiteBishopMoves(Piece piece)
        {
            ObservableCollection<Move> moves = new ObservableCollection<Move>();

            int file = piece.Square.File.ToInt();            
            int count = 1;
            for (int rank = piece.Square.Rank + 1; rank <= 8; rank++)
            {
                if (file + count > 72) break;
                if (This.Board[(file + count).ToChar()][rank].HasWhitePiece()) break;
                moves.Add(new Move(file.ToChar(), piece.Square.Rank, (file + count).ToChar(), rank));
                if (This.Board[(file + count).ToChar()][rank].HasBlackPiece()) break;
                count++;
            }

            count = 1;
            for (int rank = piece.Square.Rank - 1; rank >= 1; rank--)
            {
                if (file + count > 72) break;
                if (This.Board[(file + count).ToChar()][rank].HasWhitePiece()) break;
                moves.Add(new Move(file.ToChar(), piece.Square.Rank, (file + count).ToChar(), rank));
                if (This.Board[(file + count).ToChar()][rank].HasBlackPiece()) break;
                count++;
            }

            count = -1;
            for (int rank = piece.Square.Rank + 1; rank <= 8; rank++)
            {
                if (file + count < 65) break;
                if (This.Board[(file + count).ToChar()][rank].HasWhitePiece()) break;
                moves.Add(new Move(file.ToChar(), piece.Square.Rank, (file + count).ToChar(), rank));
                if (This.Board[(file + count).ToChar()][rank].HasBlackPiece()) break;
                count--;
            }

            count = -1;
            for (int rank = piece.Square.Rank - 1; rank >= 1; rank--)
            {
                if (file + count < 65) break;
                if (This.Board[(file + count).ToChar()][rank].HasWhitePiece()) break;
                moves.Add(new Move(file.ToChar(), piece.Square.Rank, (file + count).ToChar(), rank));
                if (This.Board[(file + count).ToChar()][rank].HasBlackPiece()) break;
                count--;
            }

            return moves;
        }
        private ObservableCollection<Move> GetAllLegalBlackBishopMoves(Piece piece)
        {
            ObservableCollection<Move> moves = new ObservableCollection<Move>();

            int file = piece.Square.File.ToInt();
            int count = 1;
            for (int rank = piece.Square.Rank + 1; rank <= 8; rank++)
            {
                if (file + count > 72) break;
                if (This.Board[(file + count).ToChar()][rank].HasBlackPiece()) break;
                moves.Add(new Move(file.ToChar(), piece.Square.Rank, (file + count).ToChar(), rank));
                if (This.Board[(file + count).ToChar()][rank].HasWhitePiece()) break;
                count++;
            }

            count = 1;
            for (int rank = piece.Square.Rank - 1; rank >= 1; rank--)
            {
                if (file + count > 72) break;
                if (This.Board[(file + count).ToChar()][rank].HasBlackPiece()) break;
                moves.Add(new Move(file.ToChar(), piece.Square.Rank, (file + count).ToChar(), rank));
                if (This.Board[(file + count).ToChar()][rank].HasWhitePiece()) break;
                count++;
            }

            count = -1;
            for (int rank = piece.Square.Rank + 1; rank <= 8; rank++)
            {
                if (file + count < 65) break;
                if (This.Board[(file + count).ToChar()][rank].HasBlackPiece()) break;
                moves.Add(new Move(file.ToChar(), piece.Square.Rank, (file + count).ToChar(), rank));
                if (This.Board[(file + count).ToChar()][rank].HasWhitePiece()) break;
                count--;
            }

            count = -1;
            for (int rank = piece.Square.Rank - 1; rank >= 1; rank--)
            {
                if (file + count < 65) break;
                if (This.Board[(file + count).ToChar()][rank].HasBlackPiece()) break;
                moves.Add(new Move(file.ToChar(), piece.Square.Rank, (file + count).ToChar(), rank));
                if (This.Board[(file + count).ToChar()][rank].HasWhitePiece()) break;
                count--;
            }

            return moves;
        }
        private ObservableCollection<Move> GetAllLegalWhiteQueenMoves(Piece piece)
        {
            ObservableCollection<Move> moves = new ObservableCollection<Move>();

            moves = this.GetAllLegalWhiteRookMoves(piece);
            ObservableCollection<Move> diagonals = this.GetAllLegalWhiteBishopMoves(piece);
            foreach (var diagonal in diagonals) moves.Add(diagonal);

            return moves;
        }
        private ObservableCollection<Move> GetAllLegalBlackQueenMoves(Piece piece)
        {
            ObservableCollection<Move> moves = new ObservableCollection<Move>();

            moves = this.GetAllLegalBlackRookMoves(piece);
            ObservableCollection<Move> diagonals = this.GetAllLegalBlackBishopMoves(piece);
            foreach (var diagonal in diagonals) moves.Add(diagonal);

            return moves;
        }
        private ObservableCollection<Move> GetAllLegalWhiteKingMoves(Piece piece)
        {
            ObservableCollection<Move> moves = new ObservableCollection<Move>();

            int file = piece.Square.File.ToInt();
            int rank = piece.Square.Rank;

            if (file - 1 >= 65 && rank - 1 >= 1 && !This.Board[(file - 1).ToChar()][rank - 1].HasWhitePiece()) moves.Add(new Move(file.ToChar(), rank, (file - 1).ToChar(), rank - 1));
            if (file - 1 >= 65 && !This.Board[(file - 1).ToChar()][rank].HasWhitePiece()) moves.Add(new Move(file.ToChar(), rank, (file - 1).ToChar(), rank));
            if (file - 1 >= 65 && rank + 1 <= 8 && !This.Board[(file - 1).ToChar()][rank + 1].HasWhitePiece()) moves.Add(new Move(file.ToChar(), rank, (file - 1).ToChar(), rank + 1));

            if (rank + 1 <= 8 && !This.Board[file.ToChar()][rank + 1].HasWhitePiece()) moves.Add(new Move(file.ToChar(), rank, file.ToChar(), rank + 1));
            if (rank - 1 >= 1 && !This.Board[file.ToChar()][rank - 1].HasWhitePiece()) moves.Add(new Move(file.ToChar(), rank, file.ToChar(), rank - 1));

            if (file + 1 <= 72 && rank - 1 >= 1 && !This.Board[(file + 1).ToChar()][rank - 1].HasWhitePiece()) moves.Add(new Move(file.ToChar(), rank, (file + 1).ToChar(), rank - 1));
            if (file + 1 <= 72 && !This.Board[(file + 1).ToChar()][rank].HasWhitePiece()) moves.Add(new Move(file.ToChar(), rank, (file + 1).ToChar(), rank));
            if (file + 1 <= 72 && rank + 1 <= 8 && !This.Board[(file + 1).ToChar()][rank + 1].HasWhitePiece()) moves.Add(new Move(file.ToChar(), rank, (file + 1).ToChar(), rank + 1));

            return moves;
        }
        private ObservableCollection<Move> GetAllLegalBlackKingMoves(Piece piece)
        {
            ObservableCollection<Move> moves = new ObservableCollection<Move>();

            int file = piece.Square.File.ToInt();
            int rank = piece.Square.Rank;

            if (file - 1 >= 65 && rank - 1 >= 1 && !This.Board[(file - 1).ToChar()][rank - 1].HasBlackPiece()) moves.Add(new Move(file.ToChar(), rank, (file - 1).ToChar(), rank - 1));
            if (file - 1 >= 65 && !This.Board[(file - 1).ToChar()][rank].HasBlackPiece()) moves.Add(new Move(file.ToChar(), rank, (file - 1).ToChar(), rank));
            if (file - 1 >= 65 && rank + 1 <= 8 && !This.Board[(file - 1).ToChar()][rank + 1].HasBlackPiece()) moves.Add(new Move(file.ToChar(), rank, (file - 1).ToChar(), rank + 1));

            if (rank + 1 <= 8 && !This.Board[file.ToChar()][rank + 1].HasBlackPiece()) moves.Add(new Move(file.ToChar(), rank, file.ToChar(), rank + 1));
            if (rank - 1 >= 1 && !This.Board[file.ToChar()][rank - 1].HasBlackPiece()) moves.Add(new Move(file.ToChar(), rank, file.ToChar(), rank - 1));

            if (file + 1 <= 72 && rank - 1 >= 1 && !This.Board[(file + 1).ToChar()][rank - 1].HasBlackPiece()) moves.Add(new Move(file.ToChar(), rank, (file + 1).ToChar(), rank - 1));
            if (file + 1 <= 72 && !This.Board[(file + 1).ToChar()][rank].HasBlackPiece()) moves.Add(new Move(file.ToChar(), rank, (file + 1).ToChar(), rank));
            if (file + 1 <= 72 && rank + 1 <= 8 && !This.Board[(file + 1).ToChar()][rank + 1].HasBlackPiece()) moves.Add(new Move(file.ToChar(), rank, (file + 1).ToChar(), rank + 1));

            return moves;
        }
    }
}