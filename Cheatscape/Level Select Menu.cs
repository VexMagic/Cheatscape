using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cheatscape
{
    static class Level_Select_Menu
    {
        static Texture2D Panel;
        static Texture2D Numbers; 

        static int SelectedLevel = 0;
        static int LevelAmount = 2;

        public static void Load()
        {
            Panel = Global_Info.AccessContentManager.Load<Texture2D>("Level Panel");
            Numbers = Global_Info.AccessContentManager.Load<Texture2D>("Numbers");
        }

        public static void Update()
        {
            if (Input_Manager.KeyPressed(Keys.Left) && SelectedLevel > 0)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Left) && SelectedLevel > 0)
                {
                    SelectedLevel--;
                    Global_Info.AccessButtonCooldown = 12;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Right) && SelectedLevel < LevelAmount - 1)
                {
                    SelectedLevel++;
                    Global_Info.AccessButtonCooldown = 12;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    Global_Info.AccessCurrentGameState = Global_Info.GameState.PlayingLevel;
                    Level_Manager.AccessCurrentLevel = SelectedLevel;
                    File_Manager.LoadLevel();
                    Global_Info.AccessButtonCooldown = 12;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Back))
                {
                    Main_Menu.Return();
                    Global_Info.AccessCurrentGameState = Global_Info.GameState.MainMenu;
                }
            }
        }

        public static void Draw(SpriteBatch aSpriteBatch)
        {
            for (int i = 0; i < LevelAmount; i++)
            {
                aSpriteBatch.Draw(Numbers, new Rectangle(201 + (i - SelectedLevel) * 100, 205, 9, 5), new Rectangle(9 * i, 0, 9, 5), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
                aSpriteBatch.Draw(Panel, new Vector2(196 + (i - SelectedLevel) * 100, 200), Color.White);
            }
        }
    }
}
