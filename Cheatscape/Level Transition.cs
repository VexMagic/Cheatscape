﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Threading;

namespace Cheatscape
{
    static class Level_Transition
    {
        static Texture2D transitionScreen;
        public static string specialRule;

        public static void Load()
        {
            transitionScreen = Global_Info.AccessContentManager.Load<Texture2D>("Pause_Menu");
        }

        public static void LoadSpecialRule()
        {
            for (int i = 0; i < Level_Manager.AccessAllMoves.Count; i++)
            {
                for (int j = 0; j < Level_Manager.AccessAllMoves[i].Count; j++)
                {
                    switch (Level_Manager.AccessAllMoves[i][j].MyMoveType)
                    {
                        case Chess_Move.MoveType.SpecialRule:
                                specialRule = Level_Manager.AccessAllMoves[i][j].myText.ToLower();
                            break;
                    }
                }
            }
        }

            public static void Draw(SpriteBatch aSpriteBatch)
        {
            aSpriteBatch.Draw(transitionScreen, new Rectangle(0, 0, (int)(Global_Info.WindowSize.X / Global_Info.AccessScreenScale), (int)(Global_Info.WindowSize.Y / Global_Info.AccessScreenScale)), Color.White);
            
            Text_Manager.DrawLargeText("Level: " + Level_Manager.CurrentBundle + "-" + Level_Manager.CurrentLevel, 300 - ((int)Text_Manager.LargeFont.MeasureString("Level: 0-0").Length() / 2), 90, aSpriteBatch);
            Text_Manager.DrawText("Special rules: " + specialRule, 130, 140, aSpriteBatch);
            Text_Manager.DrawLargeText("Press Enter to begin", 300 - ((int)Text_Manager.LargeFont.MeasureString("Press Enter to begin").Length() / 2), 235, aSpriteBatch);
        }
    }
}