using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chess.Foundation;
using Chess.VM;
using System.Collections.ObjectModel;

namespace Chess.Bots
{
    public class RandomMoves : IChessBot
    {
        public string Name
        {
            get
            {
                return "Random Moves";
            }
        }

        public RandomMoves()
        {
        }

        public Move SelectNextMove()
        {
            ObservableCollection<Move> possible = This.Game.CurrentPlayer.AllMovesNoKing();
            if (possible.Count == 0) possible = This.Game.CurrentPlayer.AllMoves();
            return possible.Random();
        }
    }
}