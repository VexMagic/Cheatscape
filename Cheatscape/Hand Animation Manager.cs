using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cheatscape
{
    static class Hand_Animation_Manager
    {
        static List<Hand> AllHands = new List<Hand>();

        public static void Load()
        {
            AllHands.Add(new Hand(new Vector2(416, 206))); //White Hand
            AllHands.Add(new Hand(new Vector2(0, 0))); //Black Hand
        }

        public static void GiveHandDirection(Chess_Move aMove)
        {
            if (Game_Board.AccessChessPiecesOnBoard[(int)aMove.myStartingPos.X, (int)aMove.myStartingPos.Y].isWhitePiece)
                AllHands[0].GainDirection(aMove);
            else
                AllHands[1].GainDirection(aMove);
        }

        public static void ResetAllHands()
        {
            for (int i = 0; i < AllHands.Count; i++)
            {
                AllHands[i].ResetHand();
            }
        }

        public static void Update()
        {
            for (int i = 0; i < AllHands.Count; i++)
            {
                AllHands[i].Update();
            }
        }

        public static void Draw(SpriteBatch aSpriteBatch)
        {
            for (int i = 0; i < AllHands.Count; i++)
            {
                AllHands[i].Draw(aSpriteBatch);
            }
        }
    }
}
