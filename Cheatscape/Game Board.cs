using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cheatscape
{
    static class Game_Board
    {
        static Chess_Piece[,] ChessPiecesOnBoard = new Chess_Piece[8, 8];
        static List<Chess_Piece> CapturedWhitePieces = new List<Chess_Piece>();
        static List<Chess_Piece> CapturedBlackPieces = new List<Chess_Piece>();
        public static Vector2 BoardPosition = new Vector2(Global_Info.AccessWindowSize.X / 4 - 128, Global_Info.AccessWindowSize.Y / 4 - 128);
        static int TileSize = 32;

        public static Chess_Piece[,] AccessChessPiecesOnBoard { get => ChessPiecesOnBoard; set => ChessPiecesOnBoard = value; }
        public static Vector2 AccessBoardPosition { get => BoardPosition; set => BoardPosition = value; }
        public static int AccessTileSize { get => TileSize; set => TileSize = value; }

        static Rectangle SplashArtSize = new Rectangle(400, 0, 200, 100);

        static Texture2D ChessBoard;
        static Texture2D Background;
        static Texture2D CheckArt;
        static Texture2D CheckmateArt;

        enum SplashArt { None, Check, Checkmate }
        static SplashArt CurrentSplashArt = SplashArt.None;

        static int PausedMove;

        public static void Load()
        {
            ChessBoard = Global_Info.AccessContentManager.Load<Texture2D>("Chess Board");
            Background = Global_Info.AccessContentManager.Load<Texture2D>("Background");
            CheckArt = Global_Info.AccessContentManager.Load<Texture2D>("Check Art");
            CheckmateArt = Global_Info.AccessContentManager.Load<Texture2D>("Checkmate Art");

            SetBasicBoardState();
        }

        public static void ResetBoard()
        {
            CapturedWhitePieces.Clear();
            CapturedBlackPieces.Clear();
            Level_Manager.AccessCurrentSlide = 1;
            Rules_List.AccessCurrentRule = 0;
            Rules_List.AccessCurrentRuleList = 0;
            Text_Manager.TutorialText = null;
            Hand_Animation_Manager.ResetAllHands();
            SetBasicBoardState();

            for (int i = 0; i < Level_Manager.AccessAllMoves[0].Count; i++)
            {
                MakeAMove(Level_Manager.AccessAllMoves[0][i], true);
            }
        }

        public static void SetBasicBoardState()
        {
            Level_Manager.AccessAllAnswers.Clear();
            Rules_List.AllowedRules.Clear();
            for (int i = 0; i < Level_Manager.AccessAllMoves.Count; i++)
            {
                for (int j = 0; j < Level_Manager.AccessAllMoves[i].Count; j++)
                {
                    switch (Level_Manager.AccessAllMoves[i][j].MyMoveType)
                    {
                        case Chess_Move.MoveType.AnswerCheat:
                            Level_Manager.AccessAllAnswers.Add(new Tuple<Chess_Move, int>(Level_Manager.AccessAllMoves[i][j], i + 1));
                            break;
                        case Chess_Move.MoveType.IncludeRule:
                            if (!Rules_List.AllowedRules.Contains(Level_Manager.AccessAllMoves[i][j].myRule))
                                Rules_List.AllowedRules.Add(Level_Manager.AccessAllMoves[i][j].myRule);
                            break;
                        case Chess_Move.MoveType.IncludeList:
                            Rules_List.IncludeList(Level_Manager.AccessAllMoves[i][j].myRuleList);
                            break;
                        case Chess_Move.MoveType.ChessBackground:
                            switch (Level_Manager.AccessAllMoves[i][j].myText.ToLower())
                            {
                                default:
                                    Background = Global_Info.AccessContentManager.Load<Texture2D>("Background");
                                    break;
                                case "kindergarden":
                                    Background = Global_Info.AccessContentManager.Load<Texture2D>("Background");
                                    break;
                            }
                            break;
                        case Chess_Move.MoveType.ChessBoard:
                            switch (Level_Manager.AccessAllMoves[i][j].myText.ToLower())
                            {
                                default:
                                    ChessBoard = Global_Info.AccessContentManager.Load<Texture2D>("Chess Board");
                                    break;
                            }
                            break;
                    }
                }
            }

            if (Rules_List.AllowedRules.Count == 0)
            {
                for (int i = 0; i < Rules_List.AmountOfRuleLists; i++)
                {
                    Rules_List.IncludeList(i);
                }
            }

            for (int x = 0; x < ChessPiecesOnBoard.GetLength(0); x++)
            {
                for (int y = 0; y < ChessPiecesOnBoard.GetLength(1); y++)
                {
                    int tempPiece = 0;
                    bool tempIsWhite = true;

                    if (y <= 3)
                        tempIsWhite = false;
                    else
                        tempIsWhite = true;

                    switch (y)
                    {
                        case 0:
                        case 7:
                            switch (x)
                            {
                                case 0:
                                case 7:
                                    tempPiece = 2;
                                    break;
                                case 1:
                                case 6:
                                    tempPiece = 4;
                                    break;
                                case 2:
                                case 5:
                                    tempPiece = 3;
                                    break;
                                case 3:
                                    tempPiece = 5;
                                    break;
                                case 4:
                                    tempPiece = 6;
                                    break;
                            }
                            break;
                        case 1:
                        case 6:
                            tempPiece = 1;
                            break;
                        default:
                            tempPiece = 0;
                            break;
                    }

                    ChessPiecesOnBoard[x, y] = new Chess_Piece(tempPiece, tempIsWhite);
                }
            }
        }

        public static void CapturePiece(Vector2 aPosition)
        {
            if (ChessPiecesOnBoard[(int)aPosition.X, (int)aPosition.Y].myPieceType != 0)
            {
                if (ChessPiecesOnBoard[(int)aPosition.X, (int)aPosition.Y].isWhitePiece)
                    CapturedWhitePieces.Add(new Chess_Piece(ChessPiecesOnBoard[(int)aPosition.X, (int)aPosition.Y]));
                else if (!ChessPiecesOnBoard[(int)aPosition.X, (int)aPosition.Y].isWhitePiece)
                    CapturedBlackPieces.Add(new Chess_Piece(ChessPiecesOnBoard[(int)aPosition.X, (int)aPosition.Y]));
            }
        }

        public static void MakeAMove(Chess_Move aMove, bool isCurrentTurn)
        {
            if (isCurrentTurn)
                CurrentSplashArt = SplashArt.None;

            switch (aMove.MyMoveType)
            {
                case Chess_Move.MoveType.MovePiece:
                    if (isCurrentTurn)
                        Hand_Animation_Manager.GiveHandDirection(aMove);
                    else
                    {
                        CapturePiece(aMove.myEndingPos);
                        ChessPiecesOnBoard[(int)aMove.myEndingPos.X, (int)aMove.myEndingPos.Y].myPieceType = ChessPiecesOnBoard[(int)aMove.myStartingPos.X, (int)aMove.myStartingPos.Y].myPieceType;
                        ChessPiecesOnBoard[(int)aMove.myEndingPos.X, (int)aMove.myEndingPos.Y].isWhitePiece = ChessPiecesOnBoard[(int)aMove.myStartingPos.X, (int)aMove.myStartingPos.Y].isWhitePiece;
                        ChessPiecesOnBoard[(int)aMove.myStartingPos.X, (int)aMove.myStartingPos.Y].myPieceType = 0;
                    }
                    break;
                case Chess_Move.MoveType.AddPiece:
                    ChessPiecesOnBoard[(int)aMove.myStartingPos.X, (int)aMove.myStartingPos.Y] = new Chess_Piece(aMove.myPiece);
                    break;
                case Chess_Move.MoveType.RemovePiece:
                    ChessPiecesOnBoard[(int)aMove.myStartingPos.X, (int)aMove.myStartingPos.Y].myPieceType = 0;
                    break;
                case Chess_Move.MoveType.CapturePiece:
                    if (aMove.myPiece.isWhitePiece)
                        CapturedWhitePieces.Add(new Chess_Piece(aMove.myPiece));
                    else if (!aMove.myPiece.isWhitePiece)
                        CapturedBlackPieces.Add(new Chess_Piece(aMove.myPiece));
                    break;
                case Chess_Move.MoveType.AnswerCheat:
                    break;
                case Chess_Move.MoveType.TutorialText:
                    if (isCurrentTurn)
                        Text_Manager.TutorialText = aMove.myText;
                    else
                        Text_Manager.TutorialText = "";
                    break;
                case Chess_Move.MoveType.CallCheck:
                    if (isCurrentTurn)
                        CurrentSplashArt = SplashArt.Check;
                    break;
                case Chess_Move.MoveType.CallCheckmate:
                    if (isCurrentTurn)
                        CurrentSplashArt = SplashArt.Checkmate;
                    break;
            }
        }

        public static void SetBoardState()
        {
            SetBasicBoardState();
            CapturedWhitePieces.Clear();
            CapturedBlackPieces.Clear();
            PausedMove = 0;

            for (int i = 0; i < Level_Manager.AccessCurrentSlide - 1; i++)
            {
                for (int j = 0; j < Level_Manager.AccessAllMoves[i].Count; j++)
                {
                    MakeAMove(Level_Manager.AccessAllMoves[i][j], false);
                }
            }

            CurrentTurnMoves();
        }

        public static void CurrentTurnMoves()
        {
            for (int i = PausedMove; i < Level_Manager.AccessAllMoves[Level_Manager.AccessCurrentSlide - 1].Count; i++)
            {
                MakeAMove(Level_Manager.AccessAllMoves[Level_Manager.AccessCurrentSlide - 1][i], true);
                if (Level_Manager.AccessAllMoves[Level_Manager.AccessCurrentSlide - 1][i].MyMoveType == Chess_Move.MoveType.MovePiece)
                {
                    PausedMove = i + 1;
                    break;
                }
            }
        }

        public static void Draw(SpriteBatch aSpriteBatch)
        {
            aSpriteBatch.Draw(Background, new Rectangle(0, 0, (int)(Global_Info.AccessWindowSize.X / Global_Info.AccessScreenScale),
                (int)(Global_Info.AccessWindowSize.Y / Global_Info.AccessScreenScale)), Color.White);

            Vector2 BoardOffset = new Vector2(BoardPosition.X - ((ChessBoard.Width - (TileSize * 8)) / 2), 
                BoardPosition.Y - ((ChessBoard.Height - (TileSize * 8)) / 2));

            aSpriteBatch.Draw(ChessBoard, BoardOffset, Color.White);

            for (int x = 0; x < ChessPiecesOnBoard.GetLength(0); x++)
            {
                for (int y = 0; y < ChessPiecesOnBoard.GetLength(1); y++)
                {
                    Vector2 tempPiecePos = new Vector2(BoardPosition.X + (x * TileSize), BoardPosition.Y + (y * TileSize));
                    ChessPiecesOnBoard[x, y].Draw(aSpriteBatch, tempPiecePos);
                }
            }

            for (int i = 0; i < CapturedWhitePieces.Count; i++)
            {
                Vector2 tempPiecePos = new Vector2(BoardOffset.X - TileSize, BoardOffset.Y + (i * (TileSize / 2)));
                CapturedWhitePieces[i].Draw(aSpriteBatch, tempPiecePos);
            }

            for (int i = CapturedBlackPieces.Count - 1; i >= 0; i--)
            {
                Vector2 tempPiecePos = new Vector2(BoardOffset.X + ChessBoard.Width,
                    BoardOffset.Y + (ChessBoard.Height - TileSize) - (i * (TileSize / 2)));
                CapturedBlackPieces[i].Draw(aSpriteBatch, tempPiecePos);
            }

            if (CurrentSplashArt == SplashArt.Check)
                aSpriteBatch.Draw(CheckArt, SplashArtSize, Color.White);
            else if (CurrentSplashArt == SplashArt.Checkmate)
                aSpriteBatch.Draw(CheckmateArt, SplashArtSize, Color.White);
        }
    }
}
