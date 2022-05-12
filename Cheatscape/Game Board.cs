using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Cheatscape
{
    static class Game_Board
    {
        static Chess_Piece[,] chessPiecesOnBoard = new Chess_Piece[8, 8];
        static List<Chess_Piece> capturedWhitePieces = new List<Chess_Piece>();
        static List<Chess_Piece> capturedBlackPieces = new List<Chess_Piece>();
        public static Vector2 boardPosition = new Vector2(Global_Info.AccessWindowSize.X / 4 - 128, Global_Info.AccessWindowSize.Y / 4 - 128);
        static int tileSize = 32;

        public static Chess_Piece[,] AccessChessPiecesOnBoard { get => chessPiecesOnBoard; set => chessPiecesOnBoard = value; }
        public static Vector2 AccessBoardPosition { get => boardPosition; set => boardPosition = value; }
        public static int AccessTileSize { get => tileSize; set => tileSize = value; }

        static Rectangle splashArtSize = new Rectangle(400, 0, 200, 100);

        static Texture2D chessBoard;
        static Texture2D background;
        static Texture2D checkArt;
        static Texture2D checkmateArt;

        enum SplashArt { none, check, checkmate }
        static SplashArt currentSplashArt = SplashArt.none;

        static int pausedMove;

        public static void Load()
        {
            chessBoard = Global_Info.AccessContentManager.Load<Texture2D>("Chess Board");
            background = Global_Info.AccessContentManager.Load<Texture2D>("Background");
            checkArt = Global_Info.AccessContentManager.Load<Texture2D>("Check Art");
            checkmateArt = Global_Info.AccessContentManager.Load<Texture2D>("Checkmate Art");

            SetBasicBoardState();
        }

        public static void ResetBoard()
        {
            capturedWhitePieces.Clear();
            capturedBlackPieces.Clear();
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
            Rules_List.allowedRules.Clear();
            for (int i = 0; i < Level_Manager.AccessAllMoves.Count; i++)
            {
                for (int j = 0; j < Level_Manager.AccessAllMoves[i].Count; j++)
                {
                    switch (Level_Manager.AccessAllMoves[i][j].myMoveType)
                    {
                        case Chess_Move.MoveType.answerCheat:
                            Level_Manager.AccessAllAnswers.Add(new Tuple<Chess_Move, int>(Level_Manager.AccessAllMoves[i][j], i + 1));
                            break;
                        case Chess_Move.MoveType.includeRule:
                            if (!Rules_List.allowedRules.Contains(Level_Manager.AccessAllMoves[i][j].myRule))
                                Rules_List.allowedRules.Add(Level_Manager.AccessAllMoves[i][j].myRule);
                            break;
                        case Chess_Move.MoveType.includeList:
                            Rules_List.IncludeList(Level_Manager.AccessAllMoves[i][j].myRuleList);
                            break;
                        case Chess_Move.MoveType.chessBackground:
                            switch (Level_Manager.AccessAllMoves[i][j].myText.ToLower())
                            {
                                default:
                                    background = Global_Info.AccessContentManager.Load<Texture2D>("Background");
                                    break;
                                case "kindergarden":
                                    background = Global_Info.AccessContentManager.Load<Texture2D>("Background");
                                    break;
                            }
                            break;
                        case Chess_Move.MoveType.chessBoard:
                            switch (Level_Manager.AccessAllMoves[i][j].myText.ToLower())
                            {
                                default:
                                    chessBoard = Global_Info.AccessContentManager.Load<Texture2D>("Chess Board");
                                    break;
                            }
                            break;
                    }
                }
            }

            if (Rules_List.allowedRules.Count == 0)
            {
                for (int i = 0; i < Rules_List.amountOfRuleLists; i++)
                {
                    Rules_List.IncludeList(i);
                }
            }

            for (int x = 0; x < chessPiecesOnBoard.GetLength(0); x++)
            {
                for (int y = 0; y < chessPiecesOnBoard.GetLength(1); y++)
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

                    chessPiecesOnBoard[x, y] = new Chess_Piece(tempPiece, tempIsWhite);
                }
            }
        }

        public static void CapturePiece(Vector2 aPosition)
        {
            if (chessPiecesOnBoard[(int)aPosition.X, (int)aPosition.Y].myPieceType != 0)
            {
                if (chessPiecesOnBoard[(int)aPosition.X, (int)aPosition.Y].isWhitePiece)
                    capturedWhitePieces.Add(new Chess_Piece(chessPiecesOnBoard[(int)aPosition.X, (int)aPosition.Y]));
                else if (!chessPiecesOnBoard[(int)aPosition.X, (int)aPosition.Y].isWhitePiece)
                    capturedBlackPieces.Add(new Chess_Piece(chessPiecesOnBoard[(int)aPosition.X, (int)aPosition.Y]));
            }
        }

        public static void MakeAMove(Chess_Move aMove, bool isCurrentTurn)
        {
            if (isCurrentTurn)
                currentSplashArt = SplashArt.none;

            switch (aMove.myMoveType)
            {
                case Chess_Move.MoveType.movePiece:
                    if (isCurrentTurn)
                        Hand_Animation_Manager.GiveHandDirection(aMove);
                    else
                    {
                        CapturePiece(aMove.myEndingPos);
                        chessPiecesOnBoard[(int)aMove.myEndingPos.X, (int)aMove.myEndingPos.Y].myPieceType = chessPiecesOnBoard[(int)aMove.myStartingPos.X, (int)aMove.myStartingPos.Y].myPieceType;
                        chessPiecesOnBoard[(int)aMove.myEndingPos.X, (int)aMove.myEndingPos.Y].isWhitePiece = chessPiecesOnBoard[(int)aMove.myStartingPos.X, (int)aMove.myStartingPos.Y].isWhitePiece;
                        chessPiecesOnBoard[(int)aMove.myStartingPos.X, (int)aMove.myStartingPos.Y].myPieceType = 0;
                    }
                    break;
                case Chess_Move.MoveType.addPiece:
                    chessPiecesOnBoard[(int)aMove.myStartingPos.X, (int)aMove.myStartingPos.Y] = new Chess_Piece(aMove.myPiece);
                    break;
                case Chess_Move.MoveType.removePiece:
                    chessPiecesOnBoard[(int)aMove.myStartingPos.X, (int)aMove.myStartingPos.Y].myPieceType = 0;
                    break;
                case Chess_Move.MoveType.capturePiece:
                    if (aMove.myPiece.isWhitePiece)
                        capturedWhitePieces.Add(new Chess_Piece(aMove.myPiece));
                    else if (!aMove.myPiece.isWhitePiece)
                        capturedBlackPieces.Add(new Chess_Piece(aMove.myPiece));
                    break;
                case Chess_Move.MoveType.answerCheat:
                    break;
                case Chess_Move.MoveType.tutorialText:
                    if (isCurrentTurn)
                        Text_Manager.TutorialText = aMove.myText;
                    else
                        Text_Manager.TutorialText = "";
                    break;
                case Chess_Move.MoveType.callCheck:
                    if (isCurrentTurn)
                        currentSplashArt = SplashArt.check;
                    break;
                case Chess_Move.MoveType.callCheckmate:
                    if (isCurrentTurn)
                        currentSplashArt = SplashArt.checkmate;
                    break;
            }
        }

        public static void SetBoardState()
        {
            SetBasicBoardState();
            capturedWhitePieces.Clear();
            capturedBlackPieces.Clear();
            pausedMove = 0;

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
            for (int i = pausedMove; i < Level_Manager.AccessAllMoves[Level_Manager.AccessCurrentSlide - 1].Count; i++)
            {
                MakeAMove(Level_Manager.AccessAllMoves[Level_Manager.AccessCurrentSlide - 1][i], true);
                if (Level_Manager.AccessAllMoves[Level_Manager.AccessCurrentSlide - 1][i].myMoveType == Chess_Move.MoveType.movePiece)
                {
                    pausedMove = i + 1;
                    break;
                }
            }
        }

        public static void Draw(SpriteBatch aSpriteBatch)
        {
            aSpriteBatch.Draw(background, new Rectangle(0, 0, (int)(Global_Info.AccessWindowSize.X / Global_Info.AccessScreenScale),
                (int)(Global_Info.AccessWindowSize.Y / Global_Info.AccessScreenScale)), Color.White);

            Vector2 BoardOffset = new Vector2(boardPosition.X - ((chessBoard.Width - (tileSize * 8)) / 2),
                boardPosition.Y - ((chessBoard.Height - (tileSize * 8)) / 2));

            aSpriteBatch.Draw(chessBoard, BoardOffset, Color.White);

            for (int x = 0; x < chessPiecesOnBoard.GetLength(0); x++)
            {
                for (int y = 0; y < chessPiecesOnBoard.GetLength(1); y++)
                {
                    Vector2 tempPiecePos = new Vector2(boardPosition.X + (x * tileSize), boardPosition.Y + (y * tileSize));
                    chessPiecesOnBoard[x, y].Draw(aSpriteBatch, tempPiecePos);
                }
            }

            for (int i = 0; i < capturedWhitePieces.Count; i++)
            {
                Vector2 tempPiecePos = new Vector2(BoardOffset.X - tileSize, BoardOffset.Y + (i * (tileSize / 2)));
                capturedWhitePieces[i].Draw(aSpriteBatch, tempPiecePos);
            }

            for (int i = capturedBlackPieces.Count - 1; i >= 0; i--)
            {
                Vector2 tempPiecePos = new Vector2(BoardOffset.X + chessBoard.Width,
                    BoardOffset.Y + (chessBoard.Height - tileSize) - (i * (tileSize / 2)));
                capturedBlackPieces[i].Draw(aSpriteBatch, tempPiecePos);
            }

            if (currentSplashArt == SplashArt.check)
                aSpriteBatch.Draw(checkArt, splashArtSize, Color.White);
            else if (currentSplashArt == SplashArt.checkmate)
                aSpriteBatch.Draw(checkmateArt, splashArtSize, Color.White);
        }
    }
}
