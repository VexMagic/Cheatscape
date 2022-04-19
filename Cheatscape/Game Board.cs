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
        static Vector2 BoardPosition = new Vector2(112, 5);
        static int TileSize = 32;

        public static Chess_Piece[,] AccessChessPiecesOnBoard { get => ChessPiecesOnBoard; set => ChessPiecesOnBoard = value; }
        public static Vector2 AccessBoardPosition { get => BoardPosition; set => BoardPosition = value; }
        public static int AccessTileSize { get => TileSize; set => TileSize = value; }

        static Texture2D ChessBoard;

        public static void Load()
        {
            ChessBoard = Global_Info.AccessContentManager.Load<Texture2D>("Chess Board");

            SetBasicBoardState();
        }

        public static void SetBasicBoardState()
        {
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

        public static void MoveChessPiece(Chess_Move aMove, bool isAnimated)
        {
            switch (aMove.MyMoveType)
            {
                case Chess_Move.MoveType.MovePiece:
                    if (isAnimated)
                        Hand_Animation_Manager.GiveHandDirection(aMove);
                    else
                    {
                        ChessPiecesOnBoard[(int)aMove.myEndingPos.X, (int)aMove.myEndingPos.Y].myPieceType = ChessPiecesOnBoard[(int)aMove.myStartingPos.X, (int)aMove.myStartingPos.Y].myPieceType;
                        ChessPiecesOnBoard[(int)aMove.myEndingPos.X, (int)aMove.myEndingPos.Y].isWhitePiece = ChessPiecesOnBoard[(int)aMove.myStartingPos.X, (int)aMove.myStartingPos.Y].isWhitePiece;
                        ChessPiecesOnBoard[(int)aMove.myStartingPos.X, (int)aMove.myStartingPos.Y].myPieceType = 0;
                    }
                    break;
                case Chess_Move.MoveType.AddPiece:
                    ChessPiecesOnBoard[(int)aMove.myStartingPos.X, (int)aMove.myStartingPos.Y] = new Chess_Piece(aMove.myPiece);
                    //ChessPiecesOnBoard[(int)aMove.myStartingPos.X, (int)aMove.myStartingPos.Y].myPieceType = aMove.myPiece.myPieceType;
                    //ChessPiecesOnBoard[(int)aMove.myStartingPos.X, (int)aMove.myStartingPos.Y].isWhitePiece = aMove.myPiece.isWhitePiece;
                    break;
                case Chess_Move.MoveType.RemovePiece:
                    break;
                case Chess_Move.MoveType.CapturePiece:
                    break;
            }
        }

        public static void SetBoardState()
        {
            SetBasicBoardState();

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
            aSpriteBatch.Draw(ChessBoard, BoardPosition, Color.White);

            for (int x = 0; x < ChessPiecesOnBoard.GetLength(0); x++)
            {
                for (int y = 0; y < ChessPiecesOnBoard.GetLength(1); y++)
                {
                    Vector2 tempPiecePos = new Vector2(BoardPosition.X + (x * TileSize), BoardPosition.Y + (y * TileSize));
                    ChessPiecesOnBoard[x, y].Draw(aSpriteBatch, tempPiecePos);
                }
            }
        }
    }
}
