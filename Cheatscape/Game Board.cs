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
        static Vector2 BoardPosition = new Vector2(Global_Info.AccessWindowSize.X/4 - 128, Global_Info.AccessWindowSize.Y / 4 - 128);
        static int TileSize = 32;

        public static Chess_Piece[,] AccessChessPiecesOnBoard { get => ChessPiecesOnBoard; set => ChessPiecesOnBoard = value; }
        public static Vector2 AccessBoardPosition { get => BoardPosition; set => BoardPosition = value; }
        public static int AccessTileSize { get => TileSize; set => TileSize = value; }

        static Texture2D ChessBoard;
        static Texture2D Background;

        public static void Load()
        {
            ChessBoard = Global_Info.AccessContentManager.Load<Texture2D>("Chess Board");
            Background = Global_Info.AccessContentManager.Load<Texture2D>("Background");

            SetBasicBoardState();
        }

        public static void ResetBoard()
        {
            ChessPiecesOnBoard = new Chess_Piece[8, 8];
            CapturedWhitePieces.Clear();
            CapturedBlackPieces.Clear();
            Level_Manager.AccessCurrentSlide = 0;
            Rules_List.AccessCurrentRule = 0;
            Rules_List.AccessCurrentRuleList = 0;
            SetBasicBoardState();
        }

        public static void SetBasicBoardState()
        {
            Level_Manager.AccessAllAnswers.Clear();
            for (int i = 0; i < Level_Manager.AccessAllMoves.Count; i++)
            {
                for (int j = 0; j < Level_Manager.AccessAllMoves[i].Count; j++)
                {
                    if (Level_Manager.AccessAllMoves[i][j].MyMoveType == Chess_Move.MoveType.AnswerCheat)
                        Level_Manager.AccessAllAnswers.Add(new Tuple<Chess_Move, int>(Level_Manager.AccessAllMoves[i][j], i + 1));
                    else if (Level_Manager.AccessAllMoves[i][j].MyMoveType == Chess_Move.MoveType.IncludeRule)
                    {
                        if (!Rules_List.AllowedRules.Contains(Level_Manager.AccessAllMoves[i][j].myRule))
                            Rules_List.AllowedRules.Add(Level_Manager.AccessAllMoves[i][j].myRule);
                    }
                    else if (Level_Manager.AccessAllMoves[i][j].MyMoveType == Chess_Move.MoveType.IncludeList)
                        Rules_List.IncludeList(Level_Manager.AccessAllMoves[i][j].myRuleList);
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

        public static void MoveChessPiece(Chess_Move aMove, bool isAnimated)
        {
            switch (aMove.MyMoveType)
            {
                case Chess_Move.MoveType.MovePiece:
                    if (isAnimated)
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
            }
        }

        public static void SetBoardState()
        {
            SetBasicBoardState();
            CapturedWhitePieces.Clear();
            CapturedBlackPieces.Clear();

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
            aSpriteBatch.Draw(Background, new Rectangle(0, 0, (int)(Global_Info.AccessWindowSize.X / Global_Info.AccessScreenScale), 
                (int)(Global_Info.AccessWindowSize.Y / Global_Info.AccessScreenScale)), Color.White);
            aSpriteBatch.Draw(ChessBoard, BoardPosition, Color.White);

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
                Vector2 tempPiecePos = new Vector2(BoardPosition.X - TileSize, BoardPosition.Y + (i * (TileSize / 2)));
                CapturedWhitePieces[i].Draw(aSpriteBatch, tempPiecePos);
            }

            for (int i = CapturedBlackPieces.Count - 1; i >= 0; i--)
            {
                Vector2 tempPiecePos = new Vector2(BoardPosition.X + (ChessPiecesOnBoard.GetLength(0) * TileSize), 
                    BoardPosition.Y + ((ChessPiecesOnBoard.GetLength(1) - 1) * TileSize) - (i * (TileSize / 2)));
                CapturedBlackPieces[i].Draw(aSpriteBatch, tempPiecePos);
            }
        }
    }
}
