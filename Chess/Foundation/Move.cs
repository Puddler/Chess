using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chess.VM;

namespace Chess.Foundation
{
    public class Move : VMBase
    {
        private BoardSquareVM _startPosition;
        public BoardSquareVM StartPosition
        {
            get
            {
                return _startPosition;
            }
            set
            {
                _startPosition = value;
                RaisePropertyChanged("StartPosition");
                RaisePropertyChanged("AsString");
            }
        }

        private BoardSquareVM _endPosition;
        public BoardSquareVM EndPosition
        {
            get
            {
                return _endPosition;
            }
            set
            {
                _endPosition = value;
                RaisePropertyChanged("EndPosition");
                RaisePropertyChanged("AsString");
            }
        }

        private int _rating;
        public int Rating
        {
            get
            {
                return _rating;
            }
            set
            {
                _rating = value;
                RaisePropertyChanged("Rating");
            }
        }

        public string AsString
        {
            get
            {
                return this.StartPosition.File.ToString() + this.StartPosition.Rank.ToString() + " - " + this.EndPosition.File.ToString() + this.EndPosition.Rank.ToString();
            }
        }
        
        public Move()
        {            
        }
        public Move(BoardSquareVM startPosition, BoardSquareVM endPosition)
        {
            this.StartPosition = startPosition;
            this.EndPosition = endPosition;
        }
        public Move(char startPositionFile, int startPositionRank, char endPositionFile, int endPositionRank)
        {
            this.StartPosition = This.Board[startPositionFile][startPositionRank];
            this.EndPosition = This.Board[endPositionFile][endPositionRank];
        }

        public Move Clone()
        {
            BoardSquareVM startPosition = new BoardSquareVM(this.StartPosition.File, this.StartPosition.Rank, this.StartPosition.OriginalColor);
            if (this.StartPosition.Piece != null)
            {
                startPosition.Piece = this.StartPosition.Piece.Clone();
                startPosition.Piece.Square = startPosition;
            }

            BoardSquareVM endPosition = new BoardSquareVM(this.EndPosition.File, this.EndPosition.Rank, this.EndPosition.OriginalColor);
            if (this.EndPosition.Piece != null)
            {
                endPosition.Piece = this.EndPosition.Piece.Clone();
                endPosition.Piece.Square = endPosition;
            }

            return new Move(startPosition, endPosition);
        }
        public Move OppositeMove()
        {
            return new Move(this.EndPosition, this.StartPosition);
        }
    }
}
