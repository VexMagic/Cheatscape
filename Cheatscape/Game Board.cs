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

        static Vector2 BoardPosition = new Vector2(112, 7);
        static int TileSize = 32;
        static Texture2D ChessBoard;

        public static void Load()
        {
            ChessBoard = Global_Info.AccessContentManager.Load<Texture2D>("Chess Board");

            int tempPiece = 0;
            bool tempOwner = true;

            for (int i = 0; i < ChessPiecesOnBoard.GetLength(0); i++)
            {
                for (int j = 0; j < ChessPiecesOnBoard.GetLength(1); j++)
                {
                    ChessPiecesOnBoard[i, j] = new Chess_Piece(tempPiece, tempOwner);
                    tempPiece++;
                    if (tempPiece >= 7)
                    {
                        tempPiece = 0;
                    }
                    if (tempOwner)
                        tempOwner = false;
                    else
                        tempOwner = true;
                }
            }
        }

        public static void MoveChessPiece(Chess_Move aMove)
        {
            ChessPiecesOnBoard[(int)aMove.myEndingPos.X, (int)aMove.myEndingPos.Y].myPieceType = ChessPiecesOnBoard[(int)aMove.myStartingPos.X, (int)aMove.myStartingPos.Y].myPieceType;
            ChessPiecesOnBoard[(int)aMove.myStartingPos.X, (int)aMove.myStartingPos.Y].myPieceType = 0;
        }

        public static void Draw(SpriteBatch aSpriteBatch)
        {
            aSpriteBatch.Draw(ChessBoard, BoardPosition, Color.White);

            for (int i = 0; i < ChessPiecesOnBoard.GetLength(0); i++)
            {
                for (int j = 0; j < ChessPiecesOnBoard.GetLength(1); j++)
                {
                    Vector2 tempPiecePos = new Vector2(BoardPosition.X + (i * TileSize), BoardPosition.Y + (j * TileSize));
                    ChessPiecesOnBoard[i, j].Draw(aSpriteBatch, tempPiecePos);
                }
            }
        }
    }
}
