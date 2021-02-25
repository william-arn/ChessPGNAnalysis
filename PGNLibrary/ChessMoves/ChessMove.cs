using System;
using System.Collections.Generic;
using System.Text;

namespace PGNLibrary
{
    public class ChessMove
    {
        ChessPieces ChessPiece { get; set; }
        int Rank { get; set; }
        char File { get; set; }
        bool WhitePlayer { get; set; }
        bool IsCheck { get; set; }
        bool IsMate { get; set; }
        bool IsCapture { get; set; }
        int MoveNumber { get; set; }

        public ChessMove(ChessPieces chessPiece, int rank, char file, bool whitePlayer, bool isCheck, bool isMate, bool isCapture, int moveNumber)
        {
            ChessPiece = chessPiece;
            Rank = rank;
            File = file;
            WhitePlayer = whitePlayer;
            IsCheck = isCheck;
            IsMate = isMate;
            IsCapture = isCapture;
            MoveNumber = moveNumber;

        }
    }
}
