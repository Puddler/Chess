using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chess.Foundation;
using System.Windows.Media;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows.Threading;

namespace Chess.VM
{
    public class BoardSquareVM : VMBase
    {
        private SolidColorBrush _originalColor;
        public SolidColorBrush OriginalColor
        {
            get
            {
                return _originalColor;
            }
            set
            {
                _originalColor = value;
                RaisePropertyChanged("OriginalColor");
            }
        }

        private SolidColorBrush _color;
        public SolidColorBrush Color
        {
            get
            {
                return _color;
            }
            set
            {
                _color = value;
                RaisePropertyChanged("Color");
            }
        }

        private char _file;
        public char File
        {
            get
            {
                return _file;
            }
            set
            {
                _file = value;
                RaisePropertyChanged("File");
            }
        }

        private int _rank;
        public int Rank
        {
            get
            {
                return _rank;
            }
            set
            {
                _rank = value;
                RaisePropertyChanged("Rank");
            }
        }

        private Piece _piece;
        public Piece Piece
        {
            get
            {
                return _piece;
            }
            set
            {
                _piece = value;
                RaisePropertyChanged("Piece");
            }
        } 
        
        private RelayCommand _clickCommand;
        public RelayCommand ClickCommand
        {
            get
            {
                if (_clickCommand == null)
                {
                    _clickCommand = new RelayCommand(this.ClickExecute);
                }
                return _clickCommand;
            }
        }

        public BoardSquareVM(char file, int rank, SolidColorBrush color)
        {
            this.OriginalColor = color;            
            this.Color = color;
            this.File = file;
            this.Rank = rank;            
        }
     
        public void ResetColor()
        {
            if (This.Game.CurrentPlayer.KingStatus == KingStatus.Mate) this.Color = System.Windows.Media.Brushes.Red;
            else this.Color = this._originalColor;            
        }
        public bool HasWhitePiece()
        {
            if (this.Piece != null) return this.Piece.IsWhite();
            else return false;
        }
        public bool HasBlackPiece()
        {
            if (this.Piece != null) return this.Piece.IsBlack();
            else return false;
        }
        public bool HasAKing()
        {
            if (this.Piece != null) return this.Piece.Material.IsAKing();
            else return false;
        }
        public bool PieceEquals(BoardSquareVM otherSquare)
        {
            if (this.Piece == null || otherSquare.Piece == null) return false;
            return this.Piece.StartFile == otherSquare.Piece.StartFile && this.Piece.StartRank == otherSquare.Piece.StartRank;
        }
        public bool IsBetterThan(BoardSquareVM otherSquare)
        {
            if (this.Piece == null || otherSquare.Piece == null) return false;
            return this.Piece.IsBetterThan(otherSquare.Piece);
        }
        public bool IsBetterThanOrEqual(BoardSquareVM otherSquare)
        {
            if (this.Piece == null || otherSquare.Piece == null) return false;
            return this.Piece.IsBetterThanOrEqual(otherSquare.Piece);
        }
        
        private void ClickExecute()
        {
            if (This.Bot == null) return;
            if (This.Game.CurrentPlayer == null || This.Game.CurrentPlayer.KingStatus == KingStatus.Mate || This.Game.OppositePlayer.KingStatus == KingStatus.Mate) return;
            if (This.Board.StartSquare == null || (this.Piece != null && This.Board.StartSquare.Piece != null && This.Board.StartSquare.Piece.Material.MatchesColor(this.Piece.Material)))
            {
                if (this.Piece != null && this.Piece.Material.CorrectPiece(This.Game.CurrentPlayer))
                {
                    This.Board.ResetBoardColors();
                    foreach (Move move in this.Piece.LegalMoves)
                    {
                        if (move.EndPosition.Piece == null) move.EndPosition.Color = System.Windows.Media.Brushes.Yellow;
                        else move.EndPosition.Color = System.Windows.Media.Brushes.Red;
                    }
                    This.Board.StartSquare = this;
                }
            }
            else if (This.Board.StartSquare != this)
            {
                if (This.Board.StartSquare.Piece.LegalMoves.FirstOrDefault(m => m.EndPosition.Rank == this.Rank && m.EndPosition.File == this.File) != null)
                {
                    This.Game.ClearUnusedMoves();
                    Move move = new Move(This.Board.StartSquare, this);
                    Move clone = move.Clone();
                    This.Game.OppositePlayer.PreviousMove = This.Game.OppositePlayer.CurrentMove;
                    This.Game.OppositePlayer.SetCurrentMove(null);                    
                    This.Game.CurrentPlayer.SetCurrentMove(clone);
                    if (move.EndPosition.Piece != null)
                    {
                        This.Game.OppositePlayer.CapturedMaterials.Add(move.EndPosition.Piece);
                        This.Game.OppositePlayer.Materials.Remove(move.EndPosition.Piece);
                    }
                    This.Game.CurrentPlayer.Moves.Add(clone);                    
                    This.Game.MakeMove(move);
                    This.Game.TempMoves.Clear();
                    This.Game.CurrentPlayer = This.Game.OppositePlayer;
                    This.Board.ResetLegalMoves();
                    This.Board.ResetBoardColors();
                    This.Board.StartSquare = null;
                    
                    if (This.Game.CurrentPlayer.Team == Team.Black && This.Game.CurrentPlayer.KingStatus != KingStatus.Mate)
                    {
                        Dispatcher.CurrentDispatcher.Invoke(new Action(() => Thread.Sleep(1)), DispatcherPriority.Render, null);
                        Move nextMove = This.Bot.SelectNextMove();
                        DateTime start = DateTime.Now;
                        TimeSpan span = DateTime.Now.Subtract(start);
                        while (DateTime.Now.Subtract(start).TotalMilliseconds < 1000) Dispatcher.CurrentDispatcher.Invoke(new Action(() => Thread.Sleep(1)), DispatcherPriority.Render, null);
                        This.Board[nextMove.StartPosition.File][nextMove.StartPosition.Rank].ClickCommand.Execute(null);
                        This.Board[nextMove.EndPosition.File][nextMove.EndPosition.Rank].ClickCommand.Execute(null);
                    }
                }                     
                else
                {
                    This.Board.StartSquare = null;
                    This.Board.ResetBoardColors();
                }
            }
        }        
    }
}