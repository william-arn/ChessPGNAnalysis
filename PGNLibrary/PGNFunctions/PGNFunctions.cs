using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using System.IO;

namespace PGNLibrary
{
    public class PGNFunctions
    {
        public static List<ChessGame> Deserialize(string content)
        {
            var result = new List<ChessGame>();
            var filename = ExtractFileName(content);

            if (!Path.GetExtension(filename).Contains("pgn"))
                throw new InvalidDataException("Uploaded file does not have a .pgn extension.");

            var games = ExtractAllGames(content);

            if (games.Count == 0)
                throw new InvalidDataException("Uploaded pgn contained no chess games in the expected format");

            foreach (Match game in games)
            {
                result.Add(ParseChessGame(game.Value));
            }

            return result;
        }

        private static string ExtractFileName(string content)
        {
            Regex fileNameRegex = new Regex(@"filename=\\" + @"(?<FileName>[^\\]+\.(?<FileExtension>[a - z]+))\\",
                RegexOptions.Compiled | RegexOptions.IgnoreCase);
            Match match = fileNameRegex.Match(content);
            return match.Groups["FileName"].Value;
        }
        private static MatchCollection ExtractAllGames(string content)
        {
            Regex gameTextRegex = new Regex(@"\[Event.+?\\n\\n\\n",
                RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return gameTextRegex.Matches(content);
        }

        private static ChessGame ParseChessGame(string content)
        {
            Regex fileNameRegex = new Regex(@"filename=\\" + @"(?<FileName>[^\\]+\.(?<FileExtension>[a - z]+))\\",
                RegexOptions.Compiled | RegexOptions.IgnoreCase);
            Match match = fileNameRegex.Match(content);

            string eventType = match.Groups["Event"].Value;
            string gameURL = match.Groups["URL"].Value;
            string whiteUsername = match.Groups["WhiteUsername"].Value;
            string blackUsername = match.Groups["BlackUsername"].Value;
            int result = Convert.ToInt32(match.Groups["Result"].Value);
            string utcDate = match.Groups["UTCDate"].Value;
            string utcTime = match.Groups["UTCTime"].Value;
            int whiteELO = Convert.ToInt32(match.Groups["WhiteELO"].Value);
            int blackELO = Convert.ToInt32(match.Groups["BlackELO"].Value);
            int whiteRatingDiff = Convert.ToInt32(match.Groups["WhiteRatingDiff"].Value); ;
            int blackRatingDiff = Convert.ToInt32(match.Groups["BlackRatingDiff"].Value); ;
            string variant = match.Groups["Variant"].Value;
            int timeLimit = Convert.ToInt32(match.Groups["TimeLimit"].Value);
            int increment = Convert.ToInt32(match.Groups["Increment"].Value);
            TimeControl timeControl = new TimeControl(timeLimit, increment);
            List<ChessMove> moveSet = ParseChessMoves(match.Groups["ChessMoves"].Value);

            return new ChessGame(eventType, gameURL, whiteUsername, blackUsername,
                result, utcDate, utcTime, whiteELO, blackELO, whiteRatingDiff,
                blackRatingDiff, variant, timeControl, moveSet);
        }
        private static List<ChessMove> ParseChessMoves(string content)
        {
            var result = new List<ChessMove>();
            Regex moveParseRegex = new Regex(@"(?<MoveNumber>\d+?)\.\s(?<WhiteMove>O-O|O-O-O|
                (?<WhitePiece>(N([a-h]|))|B|(R([a-h]|))|Q|K|[a-h]|)(?<WhiteIsCapture>x|)
                (?<WhiteFile>[a-h])(?<WhiteRank>[1-8])(?<WhiteIsCheck>\+|)(?<WhiteIsMate>#|))\s
                (?<BlackMove>O-O|O-O-O|(?<BlackPiece>(N([a-h]|))|B|(R([a-h]|))|Q|K|[a-h]|)
                (?<BlackIsCapture>x|)(?<BlackFile>[a-h])(?<BlackRank>[1-8])(?<BlackIsCheck>\+|)
                (?<BlackIsMate>#|)|)",RegexOptions.Compiled | RegexOptions.IgnoreCase);
            MatchCollection moves = moveParseRegex.Matches(content);

            foreach(Match move in moves)
            {
                ChessMove whiteMove = BuildChessMove(move, true);
                if (whiteMove != null)
                    result.Add(whiteMove);

                ChessMove blackMove = BuildChessMove(move, false);
                if (blackMove != null)
                    result.Add(whiteMove);
            }
            return result;
        }

        private static ChessMove BuildChessMove(Match move, bool isWhite)
        {
            string color = isWhite ? "White" : "Black";
            string moveString = move.Groups[color + "Move"].Value;
            ChessPieces chessPiece;
            int rank;
            char file;
            bool isCapture;
            
            if (moveString.Length == 0)
                return null;
            else if (moveString.Equals("O-O"))
            {
                chessPiece = ChessPieces.King;
                rank = isWhite ? 1 : 8;
                file = 'g';
                isCapture = false;
            }
            else if (moveString.Equals("O-O-O"))
            {
                chessPiece = ChessPieces.King;
                rank = isWhite ? 1 : 8;
                file = 'c';
                isCapture = false;
            }
            else
            {
                string piece = move.Groups[color + "Piece"].Value.Length > 1 ?
                    move.Groups[color + "Piece"].Value.Substring(0, 1) :
                    move.Groups[color + "Piece"].Value;
                switch (piece)
                {
                    case "K":
                        chessPiece = ChessPieces.King;
                        break;
                    case "R":
                        chessPiece = ChessPieces.Rook;
                        break;
                    case "N":
                        chessPiece = ChessPieces.Knight;
                        break;
                    case "B":
                        chessPiece = ChessPieces.Bishop;
                        break;
                    case "Q":
                        chessPiece = ChessPieces.Queen;
                        break;
                    default:
                        chessPiece = ChessPieces.Pawn;
                        break;
                }
                rank = Convert.ToInt32(move.Groups[color + "Rank"].Value);
                file = Convert.ToChar(move.Groups[color + "File"].Value);
                isCapture = move.Groups[color + "IsCapture"].Value.Length == 0 ? false : true;
            }
                
            bool isCheck = move.Groups[color + "IsCheck"].Value.Length == 0 ? false : true;
            bool isMate = move.Groups[color + "IsMate"].Value.Length == 0 ? false : true;
            int moveNumber = Convert.ToInt32(move.Groups["MoveNumber"].Value);

            return new ChessMove(chessPiece,rank, file, isWhite,isCheck,isMate,isCapture,moveNumber);
        }
    }
}
