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
        static Vector2 WindowSize = new Vector2(480 * ScreenScale, 270 * ScreenScale);
        public enum GameState { LevelSelect, PlayingLevel };
        static GameState CurrentGameState = GameState.LevelSelect;

        public static ContentManager AccessContentManager { get => ContentManager; set => ContentManager = value; }
        public static float AccessScreenScale { get => ScreenScale; set => ScreenScale = value; }
        public static Vector2 AccessWindowSize { get => WindowSize; set => WindowSize = value; }
        public static GameState AccessCurrentGameState { get => CurrentGameState; set => CurrentGameState = value; }


        public static void Load()
        {
            Game_Board.Load();
            //File_Manager.LoadLevel();
            Hand_Animation_Manager.Load();
            Level_Select_Menu.Load();
        }

        public static void Update()
        {
            switch (CurrentGameState)
            {
                case GameState.LevelSelect:
                    Level_Select_Menu.Update();
                    break;
                case GameState.PlayingLevel:
                    Level_Manager.Update();
                    Hand_Animation_Manager.Update();
                    break;
            }
        }

        public static void Draw(SpriteBatch aSpriteBatch)
        {
            switch (CurrentGameState)
            {
                case GameState.LevelSelect:
                    Level_Select_Menu.Draw(aSpriteBatch);
                    break;
                case GameState.PlayingLevel:
                    Game_Board.Draw(aSpriteBatch);
                    Hand_Animation_Manager.Draw(aSpriteBatch);
                    break;
            }
        }
    }
}
