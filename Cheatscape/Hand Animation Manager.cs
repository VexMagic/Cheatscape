using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cheatscape
{
    static class Hand_Animation_Manager
    {
        static List<Hand> allHands = new List<Hand>();

        public static void Load()
        {
            allHands.Add(new Hand(new Vector2(Global_Info.AccessWindowSize.X / 2 - 64, Global_Info.AccessWindowSize.Y / 2 - 64),false)); //White Hand
            allHands.Add(new Hand(new Vector2(0, 0), true)); //Black Hand
        }

        public static void GiveHandDirection(Chess_Move aMove)
        {
            if (Game_Board.AccessChessPiecesOnBoard[(int)aMove.myStartingPos.X, (int)aMove.myStartingPos.Y].isWhitePiece)
                allHands[0].GainDirection(aMove);
            else
                allHands[1].GainDirection(aMove);
        }

        public static void ResetAllHands()
        {
            for (int i = 0; i < allHands.Count; i++)
            {
                allHands[i].ResetHand();
            }
        }

        public static void Update()
        {
            for (int i = 0; i < allHands.Count; i++)
            {
                allHands[i].Update();
            }
        }

        public static void Draw(SpriteBatch aSpriteBatch)
        {
            for (int i = 0; i < allHands.Count; i++)
            {
                allHands[i].Draw(aSpriteBatch);
            }
        }
    }
}
