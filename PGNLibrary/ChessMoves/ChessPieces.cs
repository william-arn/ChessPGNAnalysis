using System;
using System.Collections.Generic;
using System.Text;

namespace PGNLibrary
{
    [Flags]
    public enum ChessPieces
    {
        King = 0,
        Pawn = 1,
        Knight = 1 << 1,
        Bishop = 1 << 2,
        Rook = 1<<3,
        Queen = 1<<4
    }
}
