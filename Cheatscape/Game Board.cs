using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cheatscape
{
    static class Game_Board
    {
        static Chess_Piece[,] ChessBoard = new Chess_Piece[8, 8];

        static string[] tempPieceTextures = { "Pawn", "Knight", "Bishop", "Rook", "Queen", "King" };
        static Vector2 BoardPosition = new Vector2(32, 32);
        static int TileSize = 32;

        public static string[] AccessPieceTextures { get => tempPieceTextures; set => tempPieceTextures = value; }

        public static void Load()
        {

        }

        public static void Draw(SpriteBatch aSpriteBatch)
        {
            for (int i = 0; i < ChessBoard.GetLength(0); i++)
            {
                for (int j = 0; j < ChessBoard.GetLength(1); j++)
                {
                    Vector2 tempPiecePos = new Vector2(BoardPosition.X + (i * TileSize), BoardPosition.Y + (j * TileSize));
                    ChessBoard[i, j].Draw(aSpriteBatch, tempPiecePos);
                }
            }
        }
    }
}
