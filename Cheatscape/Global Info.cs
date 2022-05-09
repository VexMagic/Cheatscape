using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
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
        public enum GameState { LevelSelect, PlayingLevel, MainMenu, Options };
        static GameState CurrentGameState = GameState.MainMenu;


        public static ContentManager AccessContentManager { get => ContentManager; set => ContentManager = value; }
        public static float AccessScreenScale { get => ScreenScale; set => ScreenScale = value; }
        public static Vector2 AccessWindowSize { get => WindowSize; set => WindowSize = value; }
        
        public static GameState AccessCurrentGameState { get => CurrentGameState; set => CurrentGameState = value; }


        public static void Load()
        {
            Global_Tracker.LoadCompletedBundles();
            Pause_Menu.Load();
            Game_Board.Load();
            Music_Player.Load();
            //File_Manager.LoadLevel();
            Main_Menu.Load();
            Hand_Animation_Manager.Load();
            Level_Select_Menu.Load();
            Rules_List.Load();
            Text_Manager.Load();
            Options_Menu.Load();
            Transition.Load();
        }

        public static void Update(GameTime gameTime)
        {
            Input_Manager.Update();

            switch (CurrentGameState)
            {
                case GameState.LevelSelect:
                    if (!Transition.transitioning)
                    {
                        Level_Select_Menu.Update();
                    }
                    Main_Menu.Update(gameTime);
                    Transition.Update(gameTime);
                    break;
                case GameState.PlayingLevel:
                    if (!Transition.transitioning)
                    {
                        Level_Manager.Update();
                        Hand_Animation_Manager.Update();
                    }
                    Transition.Update(gameTime);
                    Pause_Menu.Update(gameTime);
                    break;
                case GameState.MainMenu:
                    Main_Menu.Update(gameTime);
                    break;
                case GameState.Options:
                    if (!Transition.transitioning)
                    {
                        Options_Menu.Update();
                    }
                    Transition.Update(gameTime);
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
                    Transition.Draw(aSpriteBatch);
                    break;
                case GameState.PlayingLevel:
                    Game_Board.Draw(aSpriteBatch);
                    Hand_Animation_Manager.Draw(aSpriteBatch);
                    Level_Manager.Draw(aSpriteBatch);
                    Text_Manager.DrawTutorialBox(aSpriteBatch);
                    Text_Manager.DrawTurnCounter(aSpriteBatch);
                    Pause_Menu.Draw(aSpriteBatch);
                    Transition.Draw(aSpriteBatch);
                    break;
                case GameState.Options:
                    Options_Menu.Draw(aSpriteBatch);
                    Transition.Draw(aSpriteBatch);
                    break;
                case GameState.MainMenu:
                    Main_Menu.Draw(aSpriteBatch);
                    break;
            }
        }
    }
}
