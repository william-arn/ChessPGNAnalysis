using System;
using System.Collections.Generic;

namespace PGNLibrary
{
    public class ChessGame
    {
        string EventType { get; set; }
        string GameURL { get; set; }
        string WhiteUsername { get; set; }
        string BlackUsername { get; set; }
        int Result { get; set; }
        string UTCDate { get; set; }
        string UTCTime { get; set; }
        int WhiteELO { get; set; }
        int BlackELO { get; set; }
        int WhiteRatingDiff { get; set; }
        int BlackRatingDiff { get; set; }
        string Variant { get; set; }
        TimeControl TimeControl { get; set; }
        List<ChessMove> MoveSet { get; set; }

        public ChessGame(string eventType, string gameURL, string whiteUsername, string blackUsername, 
            int result, string uTCDate, string uTCTime, int whiteELO, int blackELO, int whiteRatingDiff, 
            int blackRatingDiff, string variant, TimeControl timeControl, List<ChessMove> moveSet)
        {
            EventType = eventType ?? throw new ArgumentNullException(nameof(eventType));
            GameURL = gameURL ?? throw new ArgumentNullException(nameof(gameURL));
            WhiteUsername = whiteUsername ?? throw new ArgumentNullException(nameof(whiteUsername));
            BlackUsername = blackUsername ?? throw new ArgumentNullException(nameof(blackUsername));
            Result = result;
            UTCDate = uTCDate ?? throw new ArgumentNullException(nameof(uTCDate));
            UTCTime = uTCTime ?? throw new ArgumentNullException(nameof(uTCTime));
            WhiteELO = whiteELO;
            BlackELO = blackELO;
            WhiteRatingDiff = whiteRatingDiff;
            BlackRatingDiff = blackRatingDiff;
            Variant = variant ?? throw new ArgumentNullException(nameof(variant));
            TimeControl = timeControl ?? throw new ArgumentNullException(nameof(timeControl));
            MoveSet = moveSet ?? throw new ArgumentNullException(nameof(moveSet));
        }
    }
}
