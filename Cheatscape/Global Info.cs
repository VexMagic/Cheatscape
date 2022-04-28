using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cheatscape
{
    static class Global_Info
    {
        static ContentManager ContentManager;
        static float ScreenScale = 2f;
        public static Vector2 WindowSize = new Vector2(600 * ScreenScale, 360 * ScreenScale);
        static int ButtonCooldown = 0;
        public enum GameState { LevelSelect, PlayingLevel, MainMenu };
        static GameState CurrentGameState = GameState.MainMenu;

        public static ContentManager AccessContentManager { get => ContentManager; set => ContentManager = value; }
        public static float AccessScreenScale { get => ScreenScale; set => ScreenScale = value; }
        public static Vector2 AccessWindowSize { get => WindowSize; set => WindowSize = value; }
        public static int AccessButtonCooldown { get => ButtonCooldown; set => ButtonCooldown = value; }
        public static GameState AccessCurrentGameState { get => CurrentGameState; set => CurrentGameState = value; }


        public static void Load()
        {
            Game_Board.Load();
            //File_Manager.LoadLevel();
            Main_Menu.Load();
            Hand_Animation_Manager.Load();
            Level_Select_Menu.Load();
            Rules_List.Load();
            Text_Manager.Load();
        }

        public static void Update(GameTime gameTime)
        {
            if (ButtonCooldown > 0)
                ButtonCooldown--;

            switch (CurrentGameState)
            {
                case GameState.LevelSelect:
                    Level_Select_Menu.Update();
                    Main_Menu.Update(gameTime);
                    break;
                case GameState.PlayingLevel:
                    Level_Manager.Update();
                    Hand_Animation_Manager.Update();
                    break;
                case GameState.MainMenu:
                    Main_Menu.Update(gameTime);
                    break;
            }
        }

        public static void Draw(SpriteBatch aSpriteBatch)
        {
            switch (CurrentGameState)
            {
                case GameState.LevelSelect:
                    Main_Menu.Draw(aSpriteBatch);
                    Level_Select_Menu.Draw(aSpriteBatch);
                    break;
                case GameState.PlayingLevel:
                    Game_Board.Draw(aSpriteBatch);
                    Hand_Animation_Manager.Draw(aSpriteBatch);
                    Level_Manager.Draw(aSpriteBatch);
                    Text_Manager.DrawTutorialBox(aSpriteBatch);
                    break;
                case GameState.MainMenu:
                    Main_Menu.Draw(aSpriteBatch);
                    break;
            }
        }
    }
}
