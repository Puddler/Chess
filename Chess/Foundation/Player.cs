using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace Chess.Foundation
{
    public enum Team { White, Black };
    public class Player : VMBase
    {
        private bool _manualUpdate = false;
        public Move PreviousMove;

        private Team _team;
        public Team Team
        {
            get
            {
                return _team;
            }
            set
            {
                _team = value;
                RaisePropertyChanged("Team");
            }
        }

        private KingStatus _kingStatus;
        public KingStatus KingStatus
        {
            get
            {
                return _kingStatus;
            }
            set
            {
                _kingStatus = value;
                RaisePropertyChanged("KingStatus");
            }
        }

        private Move _currentMove;
        public Move CurrentMove
        {
            get
            {
                return _currentMove;
            }
            set
            {
                if (_manualUpdate) this.GoToMove(value);
                _currentMove = value;
                RaisePropertyChanged("CurrentMove");
            }
        }

        private ObservableCollection<Piece> _materials;
        public ObservableCollection<Piece> Materials
        {
            get
            {
                if (_materials == null)
                {
                    _materials = new ObservableCollection<Piece>();
                }
                return _materials;
            }
            set
            {
                _materials = value;
                RaisePropertyChanged("Materials");
            }
        }

        private ObservableCollection<Piece> _capturedMaterials;
        public ObservableCollection<Piece> CapturedMaterials
        {
            get
            {
                if (_capturedMaterials == null)
                {
                    _capturedMaterials = new ObservableCollection<Piece>();
                }
                return _capturedMaterials;
            }
            set
            {
                _capturedMaterials = value;
                RaisePropertyChanged("CapturedMaterials");
            }
        }

        private ObservableCollection<Move> _moves;
        public ObservableCollection<Move> Moves
        {
            get
            {
                if (_moves == null)
                {
                    _moves = new ObservableCollection<Move>();
                }
                return _moves;
            }
            set
            {
                _moves = value;
                RaisePropertyChanged("Moves");
            }
        }

        public ObservableCollection<Piece> PiecesUnderAttack
        {
            get
            {
                ObservableCollection<Piece> pieces = new ObservableCollection<Piece>();
                foreach (Piece opponentPiece in This.Game.OppositePlayer.Materials)
                {
                    foreach (Piece attackedPiece in opponentPiece.AttackablePieces)
                    {
                        if (!pieces.Contains(attackedPiece))
                            pieces.Add(attackedPiece);
                    }
                }
                return pieces;
            }
        }

        public Player(Team team)
        {
            this.Team = team;
            this.KingStatus = KingStatus.Move;
        }

        public bool IsInCheck()
        {
            if(this.Team == Team.Black)
            {
                foreach (Piece piece in This.Game.White.Materials)
                {
                    piece.LegalMoves = This.Board.GetAllLegalMoves(piece);
                    foreach (Move move in piece.LegalMoves)
                    {
                        if(move.EndPosition.Piece != null && move.EndPosition.Piece.Material == Foundation.Materials.BlackKing)
                            return true;
                    }
                }
            }
            else 
            {
                foreach (Piece piece in This.Game.Black.Materials)
                {
                    piece.LegalMoves = This.Board.GetAllLegalMoves(piece);
                    foreach (Move move in piece.LegalMoves)
                    {
                        if (move.EndPosition.Piece != null && move.EndPosition.Piece.Material == Foundation.Materials.WhiteKing)
                            return true;
                    }
                }
            }
            return false;
        }
        public void SetCurrentMove(Move move)
        {
            this._manualUpdate = false;
            this.CurrentMove = move;
            this._manualUpdate = true;
        }
        public void RemoveMove(Move move)
        {
            this._manualUpdate = false;
            this.Moves.Remove(move);
            this._manualUpdate = true;
        }
        public void GoToMove(Move gotoMove)
        {
            this._manualUpdate = false;
            Move currentMove = this.CurrentMove != null ? this.CurrentMove : this.PreviousMove;
            if (this.Moves.IndexOf(currentMove) >= this.Moves.IndexOf(gotoMove))
            {
                while (true)
                {
                    Move move = This.Game.OppositePlayer.CurrentMove;
                    if (move == gotoMove) break;

                    This.Game.TempMoves.Add(move);
                    Move oppositeMove = move.OppositeMove();
                    This.Game.MakeMove(oppositeMove);
                    This.Game.OppositePlayer.SetCurrentMove(null);
                    This.Board[oppositeMove.StartPosition.File][oppositeMove.StartPosition.Rank].Piece = oppositeMove.StartPosition.Piece;
                    if (oppositeMove.StartPosition.Piece != null) This.Game.CurrentPlayer.Materials.Add(This.Board[oppositeMove.StartPosition.File][oppositeMove.StartPosition.Rank].Piece);
                    if (This.Game.CurrentPlayer.PreviousMove != null)
                    {
                        This.Game.CurrentPlayer.SetCurrentMove(This.Game.CurrentPlayer.PreviousMove);
                        int index = This.Game.CurrentPlayer.Moves.IndexOf(This.Game.CurrentPlayer.PreviousMove);
                        if (index > 0) This.Game.CurrentPlayer.PreviousMove = This.Game.CurrentPlayer.Moves[index - 1];
                        else This.Game.CurrentPlayer.PreviousMove = null;
                    }
                    This.Game.CurrentPlayer = This.Game.OppositePlayer;
                    This.Board.ResetLegalMoves();
                    This.Board.ResetBoardColors();
                    This.Board.StartSquare = null;
                }
            }
            else
            {
                while(true)
                {
                    Move move = This.Game.TempMoves[This.Game.TempMoves.Count - 1];
                    This.Game.TempMoves.Remove(move);
                    This.Game.MakeMove(move);
                    if (move.EndPosition.Piece != null)
                    {
                        This.Game.OppositePlayer.CapturedMaterials.Add(move.EndPosition.Piece);
                        This.Game.OppositePlayer.Materials.Remove(move.EndPosition.Piece);
                    }
                    This.Game.CurrentPlayer.SetCurrentMove(move);
                    This.Game.CurrentPlayer = This.Game.OppositePlayer;
                    This.Game.CurrentPlayer.PreviousMove = This.Game.CurrentPlayer.CurrentMove;                    
                    This.Game.CurrentPlayer.SetCurrentMove(null);
                    This.Board.ResetLegalMoves();
                    This.Board.ResetBoardColors();
                    This.Board.StartSquare = null;

                    if (move == gotoMove) break;
                }
            }
            this._manualUpdate = true;
        }
    }
}
