using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Collections.ObjectModel;
using Chess.VM;

namespace Chess.Foundation
{
    public enum Materials
    {
        Empty,

        WhitePawn,
        WhiteRook,
        WhiteBishop,
        WhiteKnight,
        WhiteQueen,
        WhiteKing,

        BlackPawn,
        BlackRook,
        BlackBishop,
        BlackKnight,
        BlackQueen,
        BlackKing
    }
    public class Piece : VMBase
    {
        private Materials _material;
        public Materials Material
        {
            get
            {
                return _material;
            }
            set
            {
                _material = value;
                RaisePropertyChanged("Piece");
            }
        }

        private ImageSource _image;
        public ImageSource Image
        {
            get
            {
                return _image;
            }
            set
            {
                _image = value;
                RaisePropertyChanged("Image");
            }
        }

        private char _startFile;
        public char StartFile
        {
            get
            {
                return _startFile;
            }
            set
            {
                _startFile = value;
                RaisePropertyChanged("StartFile");
            }
        }

        private int _startRank;
        public int StartRank
        {
            get
            {
                return _startRank;
            }
            set
            {
                _startRank = value;
                RaisePropertyChanged("StartRank");
            }
        }

        private BoardSquareVM _square;
        public BoardSquareVM Square
        {
            get
            {
                return _square;
            }
            set
            {
                _square = value;
                RaisePropertyChanged("Square");
            }
        }
        
        private ObservableCollection<Move> _legalMoves;
        public ObservableCollection<Move> LegalMoves
        {
            get
            {
                if (_legalMoves == null)
                {
                    _legalMoves = new ObservableCollection<Move>();
                }
                return _legalMoves;
            }
            set
            {
                _legalMoves = value;
                RaisePropertyChanged("LegalMoves");
            }
        }

        public ObservableCollection<Piece> AttackablePieces
        {
            get
            {
                ObservableCollection<Piece> pieces = new ObservableCollection<Piece>();
                foreach (Move move in this.LegalMoves)
                {
                    if (move.EndPosition.Piece != null && !pieces.Contains(move.EndPosition.Piece))
                        pieces.Add(move.EndPosition.Piece);
                }
                return pieces;
            }
        }
        public int Rating
        {
            get
            {
                if (this.Material.IsAPawn()) return 1;
                else if (this.Material.IsAKnight()) return 4;
                else if (this.Material.IsABishop()) return 5;
                else if (this.Material.IsARook()) return 7;
                else if (this.Material.IsAQueen()) return 9;
                else return 0;
            }
        }

        public Piece(Materials piece, char startFile, int startRank)
        {            
            this.StartFile = startFile;
            this.StartRank = startRank;
            this.SetMaterial(piece);
            this.Square = This.Board[StartFile][startRank];
        }

        public Piece Clone()
        {
            Piece piece = new Piece(this.Material, this.StartFile, this.StartRank);
            piece.SetMaterial(this.Material);
            return piece;
        }
        public void SetMaterial(Materials material)
        {
            if (material == Materials.Empty)
            {
                this.Image = null;
                this.Material = Materials.Empty;
            }
            else if (material == Materials.BlackBishop)
            {
                this.Image = This.Images.BlackBishop;
                this.Material = Materials.BlackBishop;
            }
            else if (material == Materials.BlackKing)
            {
                this.Image = This.Images.BlackKing;
                this.Material = Materials.BlackKing;
            }
            else if (material == Materials.BlackKnight)
            {
                this.Image = This.Images.BlackKnight;
                this.Material = Materials.BlackKnight;
            }
            else if (material == Materials.BlackPawn)
            {
                this.Image = This.Images.BlackPawn;
                this.Material = Materials.BlackPawn;
            }
            else if (material == Materials.BlackQueen)
            {
                this.Image = This.Images.BlackQueen;
                this.Material = Materials.BlackQueen;
            }
            else if (material == Materials.BlackRook)
            {
                this.Image = This.Images.BlackRook;
                this.Material = Materials.BlackRook;
            }
            else if (material == Materials.WhiteBishop)
            {
                this.Image = This.Images.WhiteBishop;
                this.Material = Materials.WhiteBishop;
            }
            else if (material == Materials.WhiteKing)
            {
                this.Image = This.Images.WhiteKing;
                this.Material = Materials.WhiteKing;
            }
            else if (material == Materials.WhiteKnight)
            {
                this.Image = This.Images.WhiteKnight;
                this.Material = Materials.WhiteKnight;
            }
            else if (material == Materials.WhitePawn)
            {
                this.Image = This.Images.WhitePawn;
                this.Material = Materials.WhitePawn;
            }
            else if (material == Materials.WhiteQueen)
            {
                this.Image = This.Images.WhiteQueen;
                this.Material = Materials.WhiteQueen;
            }
            else if (material == Materials.WhiteRook)
            {
                this.Image = This.Images.WhiteRook;
                this.Material = Materials.WhiteRook;
            }                     
        }
        public bool IsWhite()
        {
            return this.Material.IsWhite();
        }
        public bool IsBlack()
        {            
            return this.Material.IsBlack();
        }
    }
}