using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cheatscape
{
    class Chess_Piece
    {
        Texture2D myTexture;
        public int myPieceType;
        public bool isWhitePiece;

        public Chess_Piece(int aPieceType, bool isWhite)
        {
            myPieceType = aPieceType;
            isWhitePiece = isWhite;
            myTexture = Global_Info.AccessContentManager.Load<Texture2D>("Chess Pieces");
        }

        public Chess_Piece(Chess_Piece aPiece)
        {
            myPieceType = aPiece.myPieceType;
            isWhitePiece = aPiece.isWhitePiece;
            myTexture = Global_Info.AccessContentManager.Load<Texture2D>("Chess Pieces");
        }

        public void Draw(SpriteBatch aSpriteBatch, Vector2 aPos)
        {

            if (isWhitePiece)
                aSpriteBatch.Draw(myTexture, new Rectangle((int)aPos.X, (int)aPos.Y, 32, 32), new Rectangle(32 * myPieceType, 0, 32, 32), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
            else
            {
                aSpriteBatch.Draw(myTexture, new Rectangle((int)aPos.X, (int)aPos.Y, 32, 32), new Rectangle(32 * myPieceType, 32, 32, 32), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
            }
        }
    }
}