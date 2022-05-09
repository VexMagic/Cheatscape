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
        static ContentManager contentManager;
        static float screenScale = 2f;
        public static Vector2 windowSize = new Vector2(600 * screenScale, 360 * screenScale);
        public enum GameState { levelSelect, playingLevel, mainMenu, options };
        static GameState currentGameState = GameState.mainMenu;


        public static ContentManager AccessContentManager { get => contentManager; set => contentManager = value; }
        public static float AccessScreenScale { get => screenScale; set => screenScale = value; }
        public static Vector2 AccessWindowSize { get => windowSize; set => windowSize = value; }
        
        public static GameState AccessCurrentGameState { get => currentGameState; set => currentGameState = value; }


        public static void Load()
        {
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

            switch (currentGameState)
            {
                case GameState.levelSelect:
                    if (!Transition.transitioning)
                    {
                        Level_Select_Menu.Update();
                    }
                    Main_Menu.Update(gameTime);
                    Transition.Update(gameTime);
                    break;
                case GameState.playingLevel:
                    if (!Transition.transitioning)
                    {
                        Level_Manager.Play(gameTime);
                        Level_Manager.Update();
                        Hand_Animation_Manager.Update();
                    }
                    Transition.Update(gameTime);
                    Pause_Menu.Update(gameTime);
                    break;
                case GameState.mainMenu:
                    Main_Menu.Update(gameTime);
                    break;
                case GameState.options:
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
            switch (currentGameState)
            {
                case GameState.levelSelect:
                    Main_Menu.Draw(aSpriteBatch);
                    Level_Select_Menu.Draw(aSpriteBatch);
                    Transition.Draw(aSpriteBatch);
                    break;
                case GameState.playingLevel:
                    Game_Board.Draw(aSpriteBatch);
                    Hand_Animation_Manager.Draw(aSpriteBatch);
                    Level_Manager.Draw(aSpriteBatch);
                    Text_Manager.DrawTutorialBox(aSpriteBatch);
                    Text_Manager.DrawTurnCounter(aSpriteBatch);
                    Pause_Menu.Draw(aSpriteBatch);
                    Transition.Draw(aSpriteBatch);
                    break;
                case GameState.options:
                    Options_Menu.Draw(aSpriteBatch);
                    Transition.Draw(aSpriteBatch);
                    break;
                case GameState.mainMenu:
                    Main_Menu.Draw(aSpriteBatch);
                    break;
            }
        }
    }
}
