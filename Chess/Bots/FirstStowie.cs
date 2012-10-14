using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chess.Foundation;
using System.Collections.ObjectModel;

namespace Chess.Bots
{
    public class FirstStowie : IChessBot
    {
        public string Name
        {
            get
            {
                return "First Stowie";
            }
        }

        public FirstStowie()
        {
        }

        public Move SelectNextMove()
        {
            ObservableCollection<Move> moves = new ObservableCollection<Move>();
            moves = This.Game.CurrentPlayer.AllMovesNoKing();
            return moves.Random();
        }
    }
}