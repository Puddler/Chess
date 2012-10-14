using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chess.VM;
using Chess.Foundation;
using System.Collections.ObjectModel;
using System.Windows.Media;
using Chess.Bots;
using System.Windows;
using Chess.UI;

namespace Chess
{
    public class This : VMBase
    {
        public static Images Images;
        public static This Game;
        public static BoardVM Board;
        public static IChessBot Bot;
        
        public static This CreateNewGame()
        {
            This game = new This();
            This.Images = new Images();
            This.Board = game.CreateNewBoard(Brushes.BlanchedAlmond, Brushes.Chocolate);
            return game;
        }

        public ObservableCollection<Move> TempMoves = new ObservableCollection<Move>();

        private Player _currentPlayer;
        public Player CurrentPlayer
        {
            get
            {
                return _currentPlayer;
            }
            set
            {
                _currentPlayer = value;
                RaisePropertyChanged("CurrentPlayer");
                RaisePropertyChanged("OppositePlayer");
            }
        }

        public Player OppositePlayer
        {
            get
            {
                if (this.CurrentPlayer.Team == Team.White) return This.Game.Black;
                else return This.Game.White;
            }
        }

        private Player _white;
        public Player White
        {
            get
            {
                if (_white == null)
                {
                    _white = new Player(Team.White);
                }
                return _white;
            }
            set
            {
                _white = value;
                RaisePropertyChanged("White");
            }
        }

        private Player _black;
        public Player Black
        {
            get
            {
                if (_black == null)
                {
                    _black = new Player(Team.Black);
                }
                return _black;
            }
            set
            {
                _black = value;
                RaisePropertyChanged("Black");
            }
        }

        private ImageSource _icon;
        public ImageSource Icon
        {
            get
            {
                if (_icon == null)
                {
                    _icon = This.Images.Icon;
                }
                return _icon;
            }
            set
            {
                _icon = value;
                RaisePropertyChanged("Icon");
            }
        }
        
        private RelayCommand _resetBoardCommand;
        public RelayCommand ResetBoardCommand
        {
            get
            {
                if (_resetBoardCommand == null)
                {
                    _resetBoardCommand = new RelayCommand(SetupStartPosition);
                }
                return _resetBoardCommand;
            }
        }

        private RelayCommand _previousCommand;
        public RelayCommand PreviousCommand
        {
            get
            {
                if (_previousCommand == null)
                {
                    _previousCommand = new RelayCommand(PreviousExecute);
                }
                return _previousCommand;
            }
        }

        private RelayCommand _nextCommand;
        public RelayCommand NextCommand
        {
            get
            {
                if (_nextCommand == null)
                {
                    _nextCommand = new RelayCommand(NextExecute);
                }
                return _nextCommand;
            }
        }
        
        public This()
        {            
        }

        public BoardVM CreateNewBoard(SolidColorBrush whiteColor, SolidColorBrush blackColor)
        {
            BoardVM board = new BoardVM();
            List<char> files = new List<char>() { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H' };
            List<int> ranks = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8 };

            foreach (char file in files)
            {
                foreach (int rank in ranks)
                {
                    SolidColorBrush color = Brushes.Transparent;
                    if (file == 'A' || file == 'C' || file == 'E' || file == 'G')
                    {
                        if (rank % 2 == 0) color = whiteColor;
                        else color = blackColor;
                    }
                    else
                    {
                        if (rank % 2 == 0) color = blackColor;
                        else color = whiteColor;
                    }
                    board.AddSquare(new BoardSquareVM(file, rank, color));
                }
            }
            return board;
        }
        public void CreatePieces()
        {
            this.Black.Materials.Clear();
            this.Black.Materials.Add(new Piece(Materials.BlackPawn, 'A', 7));
            this.Black.Materials.Add(new Piece(Materials.BlackPawn, 'B', 7));
            this.Black.Materials.Add(new Piece(Materials.BlackPawn, 'C', 7));
            this.Black.Materials.Add(new Piece(Materials.BlackPawn, 'D', 7));
            this.Black.Materials.Add(new Piece(Materials.BlackPawn, 'E', 7));
            this.Black.Materials.Add(new Piece(Materials.BlackPawn, 'F', 7));
            this.Black.Materials.Add(new Piece(Materials.BlackPawn, 'G', 7));
            this.Black.Materials.Add(new Piece(Materials.BlackPawn, 'H', 7));

            this.Black.Materials.Add(new Piece(Materials.BlackRook, 'A', 8));
            this.Black.Materials.Add(new Piece(Materials.BlackKnight, 'B', 8));
            this.Black.Materials.Add(new Piece(Materials.BlackBishop, 'C', 8));
            this.Black.Materials.Add(new Piece(Materials.BlackQueen, 'D', 8));
            this.Black.Materials.Add(new Piece(Materials.BlackKing, 'E', 8));
            this.Black.Materials.Add(new Piece(Materials.BlackBishop, 'F', 8));
            this.Black.Materials.Add(new Piece(Materials.BlackKnight, 'G', 8));
            this.Black.Materials.Add(new Piece(Materials.BlackRook, 'H', 8));

            this.White.Materials.Clear();
            this.White.Materials.Add(new Piece(Materials.WhiteRook, 'A', 1));
            this.White.Materials.Add(new Piece(Materials.WhiteKnight, 'B', 1));
            this.White.Materials.Add(new Piece(Materials.WhiteBishop, 'C', 1));
            this.White.Materials.Add(new Piece(Materials.WhiteQueen, 'D', 1));
            this.White.Materials.Add(new Piece(Materials.WhiteKing, 'E', 1));
            this.White.Materials.Add(new Piece(Materials.WhiteBishop, 'F', 1));
            this.White.Materials.Add(new Piece(Materials.WhiteKnight, 'G', 1));
            this.White.Materials.Add(new Piece(Materials.WhiteRook, 'H', 1));

            this.White.Materials.Add(new Piece(Materials.WhitePawn, 'A', 2));
            this.White.Materials.Add(new Piece(Materials.WhitePawn, 'B', 2));
            this.White.Materials.Add(new Piece(Materials.WhitePawn, 'C', 2));
            this.White.Materials.Add(new Piece(Materials.WhitePawn, 'D', 2));
            this.White.Materials.Add(new Piece(Materials.WhitePawn, 'E', 2));
            this.White.Materials.Add(new Piece(Materials.WhitePawn, 'F', 2));
            this.White.Materials.Add(new Piece(Materials.WhitePawn, 'G', 2));
            this.White.Materials.Add(new Piece(Materials.WhitePawn, 'H', 2));
        }
        public void SetupStartPosition()
        {
            Window newGameWindow = new NewGameUI();
            if(NewGameVM.VM == null) NewGameVM.VM = new NewGameVM();
            newGameWindow.DataContext = NewGameVM.VM;
            if (This.Bot != null) NewGameVM.VM.SelectedBot = This.Bot;
            newGameWindow.ShowDialog();

            if (NewGameUI.Reset)
            {
                NewGameUI.Reset = false;
                This.Board.ClearBoard();

                this.CreatePieces();

                this.White.SetCurrentMove(null);
                this.White.PreviousMove = null;
                this.White.Moves.Clear();
                this.White.CapturedMaterials.Clear();
                this.White.KingStatus = KingStatus.Move;

                this.Black.SetCurrentMove(null);
                this.Black.PreviousMove = null;
                this.Black.Moves.Clear();
                this.Black.CapturedMaterials.Clear();
                this.Black.KingStatus = KingStatus.Move;

                this.CurrentPlayer = this.White;

                This.Board.SetupStartPosition();
                This.Board.ResetLegalMoves();
                This.Board.ResetBoardColors();
            }
        }
        public void MakeMove(Move move)
        {
            This.Board[move.EndPosition.File][move.EndPosition.Rank].Piece = This.Board[move.StartPosition.File][move.StartPosition.Rank].Piece;
            This.Board[move.EndPosition.File][move.EndPosition.Rank].Piece.Square = This.Board[move.EndPosition.File][move.EndPosition.Rank];

            This.Board[move.StartPosition.File][move.StartPosition.Rank].Piece = null;            
        }
        public void ClearUnusedMoves()
        {
            List<Move> toRemove = new List<Move>();
            int index = this.OppositePlayer.Moves.IndexOf(this.OppositePlayer.CurrentMove);
            if (index >= 0)
            {
                for (int i = index + 1; i <= this.OppositePlayer.Moves.Count - 1; i++)
                {
                    toRemove.Add(this.OppositePlayer.Moves[i]);
                }
                foreach (Move move in toRemove) this.OppositePlayer.Moves.Remove(move);
            }

            index = this.CurrentPlayer.PreviousMove != null ? this.CurrentPlayer.Moves.IndexOf(this.CurrentPlayer.PreviousMove) : -1;            
            for (int i = index + 1; i <= this.CurrentPlayer.Moves.Count - 1; i++)
            {
                toRemove.Add(this.CurrentPlayer.Moves[i]);
            }
            foreach (Move move in toRemove) this.CurrentPlayer.Moves.Remove(move);            
        }

        private void PreviousExecute()
        {
            if (This.Bot == null) return;
            if (this.OppositePlayer.CurrentMove != null)
            {
                Move move = this.OppositePlayer.CurrentMove;            
                this.TempMoves.Add(move);
                Move oppositeMove = move.OppositeMove();                
                this.MakeMove(oppositeMove);
                this.OppositePlayer.SetCurrentMove(null);
                This.Board[oppositeMove.StartPosition.File][oppositeMove.StartPosition.Rank].Piece = oppositeMove.StartPosition.Piece;
                if (oppositeMove.StartPosition.Piece != null) This.Game.CurrentPlayer.Materials.Add(This.Board[oppositeMove.StartPosition.File][oppositeMove.StartPosition.Rank].Piece);
                if (this.CurrentPlayer.PreviousMove != null)
                {
                    this.CurrentPlayer.SetCurrentMove(this.CurrentPlayer.PreviousMove);
                    int index = this.CurrentPlayer.Moves.IndexOf(this.CurrentPlayer.PreviousMove);
                    if (index > 0) this.CurrentPlayer.PreviousMove = this.CurrentPlayer.Moves[index - 1];
                    else this.CurrentPlayer.PreviousMove = null;                    
                }
                this.CurrentPlayer = this.OppositePlayer;                
                This.Board.ResetLegalMoves();
                This.Board.ResetBoardColors();
                This.Board.StartSquare = null;
            }           
        }
        private void NextExecute()
        {
            if (This.Bot == null) return;
            if (this.TempMoves.Count > 0)
            {
                Move move = this.TempMoves[this.TempMoves.Count - 1];                
                this.TempMoves.Remove(move);
                this.MakeMove(move);
                if (move.EndPosition.Piece != null)
                {
                    this.OppositePlayer.CapturedMaterials.Add(move.EndPosition.Piece);
                    this.OppositePlayer.Materials.Remove(move.EndPosition.Piece);
                }
                this.CurrentPlayer.SetCurrentMove(move);
                this.CurrentPlayer = this.OppositePlayer;
                this.CurrentPlayer.PreviousMove = this.CurrentPlayer.CurrentMove;
                this.CurrentPlayer.SetCurrentMove(null);
                This.Board.ResetLegalMoves();
                This.Board.ResetBoardColors();
                This.Board.StartSquare = null;
            }
        }
    }
} 