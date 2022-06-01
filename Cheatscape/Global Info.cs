using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Cheatscape
{
    static class Global_Info
    {
        static ContentManager contentManager;
        static float screenScale = 2f;
        public static Vector2 windowSize = new Vector2(600 * screenScale, 360 * screenScale);
        public enum GameState
        {
            levelSelect, playingLevel, mainMenu, options
        };
        static GameState currentGameState = GameState.mainMenu;


        public static ContentManager AccessContentManager
        {
            get => contentManager; set => contentManager = value;
        }
        public static float AccessScreenScale
        {
            get => screenScale; set => screenScale = value;
        }
        public static Vector2 AccessWindowSize
        {
            get => windowSize; set => windowSize = value;
        }

        public static GameState AccessCurrentGameState
        {
            get => currentGameState; set => currentGameState = value;
        }


        public static void Load()
        {
            Hint_File_Manager.LoadHints();
            Level_Transition.Load();
            Global_Tracker.LoadCompletedBundles();
            Pause_Menu.Load();
            Game_Board.Load();
            Music_Player.Load();
            Main_Menu.Load();
            Hand_Animation_Manager.Load();
            Level_Select_Menu.Load();
            Rules_List.Load();
            Text_Manager.Load();
            Options_Menu.Load();
            Transition_Effect.Load();
            Mouse_Controller.Load();
            End_Screen.Load();
        }

        public static void Update(GameTime gameTime)
        {
            Keyboard_Inputs.Update();
            Mouse_Controller.Update();

            switch (currentGameState)
            {
                case GameState.levelSelect:
                    if (!Transition_Effect.transitioning)
                    {
                        Level_Select_Menu.Update();
                    }
                    Main_Menu.Update(gameTime);
                    Transition_Effect.Update(gameTime);

                    break;
                case GameState.playingLevel:
                    if (!Transition_Effect.transitioning && !End_Screen.AccessIsEnded)
                    {
                        Level_Manager.Play(gameTime);
                        Level_Manager.Update();
                        Hand_Animation_Manager.Update();
                    }
                    Pause_Menu.Update();
                    End_Screen.Update();
                    Transition_Effect.Update(gameTime);
                    break;
                case GameState.mainMenu:
                    Main_Menu.Update(gameTime);
                    break;
                case GameState.options:
                    if (!Transition_Effect.transitioning)
                    {
                        Options_Menu.Update();
                    }
                    Transition_Effect.Update(gameTime);
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
                    Transition_Effect.Draw(aSpriteBatch);
                    break;
                case GameState.playingLevel:
                    Game_Board.Draw(aSpriteBatch);
                    Hand_Animation_Manager.Draw(aSpriteBatch);
                    Level_Manager.Draw(aSpriteBatch);
                    Text_Manager.DrawTutorialBox(aSpriteBatch);
                    Pause_Menu.Draw(aSpriteBatch);
                    Transition_Effect.Draw(aSpriteBatch);
                    End_Screen.Draw(aSpriteBatch);
                    Mouse_Controller.LevelDraw(aSpriteBatch);
                    break;
                case GameState.options:
                    Options_Menu.Draw(aSpriteBatch);
                    Transition_Effect.Draw(aSpriteBatch);
                    break;
                case GameState.mainMenu:
                    Main_Menu.Draw(aSpriteBatch);
                    break;
            }
        }
    }
}
