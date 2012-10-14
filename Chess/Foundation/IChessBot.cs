using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Foundation
{
    public interface IChessBot
    {
        string Name { get; }
        Move SelectNextMove();
    }
}