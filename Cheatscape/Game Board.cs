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
        static Vector2 boardPosition = new Vector2(Global_Info.AccessWindowSize.X / 4 - 128, Global_Info.AccessWindowSize.Y / 4 - 128);
        static int tileSize = 32;

        public static Chess_Piece[,] AccessChessPiecesOnBoard { get => chessPiecesOnBoard; set => chessPiecesOnBoard = value; }
        public static Vector2 AccessBoardPosition { get => boardPosition; set => boardPosition = value; }
        public static int AccessTileSize { get => tileSize; set => tileSize = value; }

        static Texture2D chessBoard;
        static Texture2D background;

        public static void Load()
        {
            chessBoard = Global_Info.AccessContentManager.Load<Texture2D>("Chess Board");
            background = Global_Info.AccessContentManager.Load<Texture2D>("Background");

            SetBasicBoardState();
        }

        public static void ResetBoard()
        {
            chessPiecesOnBoard = new Chess_Piece[8, 8];
            capturedWhitePieces.Clear();
            capturedBlackPieces.Clear();
            Level_Manager.AccessCurrentSlide = 1;
            Rules_List.AccessCurrentRule = 0;
            Rules_List.AccessCurrentRuleList = 0;
            Text_Manager.tutorialText = null;
            SetBasicBoardState();

            for (int i = 0; i < Level_Manager.AccessAllMoves[0].Count; i++)
            {
                MoveChessPiece(Level_Manager.AccessAllMoves[0][i], true);
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
                    if (Level_Manager.AccessAllMoves[i][j].myMoveType == Chess_Move.MoveType.answerCheat)
                        Level_Manager.AccessAllAnswers.Add(new Tuple<Chess_Move, int>(Level_Manager.AccessAllMoves[i][j], i + 1));
                    else if (Level_Manager.AccessAllMoves[i][j].myMoveType == Chess_Move.MoveType.includeRule)
                    {
                        if (!Rules_List.allowedRules.Contains(Level_Manager.AccessAllMoves[i][j].myRule))
                            Rules_List.allowedRules.Add(Level_Manager.AccessAllMoves[i][j].myRule);
                    }
                    else if (Level_Manager.AccessAllMoves[i][j].myMoveType == Chess_Move.MoveType.includeList)
                        Rules_List.IncludeList(Level_Manager.AccessAllMoves[i][j].myRuleList);
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

        public static void MoveChessPiece(Chess_Move aMove, bool isCurrentTurn)
        {
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
                        Text_Manager.tutorialText = aMove.myText;
                    else
                        Text_Manager.tutorialText = "";
                    break;
            }
        }

        public static void SetBoardState()
        {
            SetBasicBoardState();
            capturedWhitePieces.Clear();
            capturedBlackPieces.Clear();

            for (int i = 0; i < Level_Manager.AccessCurrentSlide - 1; i++)
            {
                for (int j = 0; j < Level_Manager.AccessAllMoves[i].Count; j++)
                {
                    MoveChessPiece(Level_Manager.AccessAllMoves[i][j], false);
                }
            }
        }

        public static void Draw(SpriteBatch aSpriteBatch)
        {
            aSpriteBatch.Draw(background, new Rectangle(0, 0, (int)(Global_Info.AccessWindowSize.X / Global_Info.AccessScreenScale),
                (int)(Global_Info.AccessWindowSize.Y / Global_Info.AccessScreenScale)), Color.White);
            aSpriteBatch.Draw(chessBoard, boardPosition - new Vector2(26, 26), Color.White);

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
                Vector2 tempPiecePos = new Vector2(boardPosition.X - tileSize, boardPosition.Y + (i * (tileSize / 2)));
                capturedWhitePieces[i].Draw(aSpriteBatch, tempPiecePos);
            }

            for (int i = capturedBlackPieces.Count - 1; i >= 0; i--)
            {
                Vector2 tempPiecePos = new Vector2(boardPosition.X + (chessPiecesOnBoard.GetLength(0) * tileSize),
                    boardPosition.Y + ((chessPiecesOnBoard.GetLength(1) - 1) * tileSize) - (i * (tileSize / 2)));
                capturedBlackPieces[i].Draw(aSpriteBatch, tempPiecePos);
            }
        }
    }
}
