using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cheatscape
{
    class Chess_Piece
    {
        Texture2D myTexture;
        public int myPieceType;

        public Chess_Piece(int aPieceType)
        {
            myPieceType = aPieceType;
            myTexture = Global_Info.AccessContentManager.Load<Texture2D>("Chess Pieces");
        }

        public void Draw(SpriteBatch aSpriteBatch, Vector2 aPos)
        {
            aSpriteBatch.Draw(myTexture, new Rectangle((int)aPos.X, (int)aPos.Y, 32, 32), new Rectangle(32 * myPieceType, 0, 32, 32), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
        }
    }
}
