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
        int myPieceType;

        public Chess_Piece(int aPieceType)
        {
            myPieceType = aPieceType;
            myTexture = Global_Info.AccessContentManager.Load<Texture2D>(Game_Board.AccessPieceTextures[myPieceType]);
        }

        public void Draw(SpriteBatch aSpriteBatch, Vector2 aPosition)
        {
            aSpriteBatch.Draw(myTexture, aPosition, Color.White);
        }
    }
}
