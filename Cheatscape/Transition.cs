using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Cheatscape
{
    class Transition
    {
        static Texture2D transitionTex;

        static Vector2 transitionPos;

        public static bool transitioning = false;

        static int dir;

        static float transitionWidth = 300 + Global_Info.WindowSize.X / Global_Info.AccessScreenScale;
        static float timeSinceLastMove;
        static float timeBetweenMoves = 30f;

        public enum TransitionState { ToOptions, ToLvSelect, ToLevel };
        public static TransitionState transitionState, nextTransitionState;

        public static TransitionState AccessNextTransitionState { get => nextTransitionState; set => nextTransitionState = value; }
        public static void Load()
        {
            transitionTex = Global_Info.AccessContentManager.Load<Texture2D>("TransitionShadow");
        }

        public static void StartTransition(TransitionState state)
        {
            if (!transitioning)
            {
                transitioning = true;
                transitionState = state;

                switch (transitionState)
                {
                    case TransitionState.ToLvSelect:
                        dir = -1;
                        break;
                    case TransitionState.ToOptions:
                        dir = 1;
                        break;
                    case TransitionState.ToLevel:
                        dir = -1;
                        break;
                }

                transitionPos.X = -dir * transitionWidth;
            }
        }

        public static void Update(GameTime gameTime)
        {
            if (transitioning)
            {
                timeSinceLastMove += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                if (timeSinceLastMove >= timeBetweenMoves)
                {
                    timeSinceLastMove -= timeBetweenMoves;

                    transitionPos.X += 70 * dir;

                    switch (transitionState)
                    {
                        case TransitionState.ToOptions:
                            
                            if (transitionPos.X >= transitionWidth / 2 - Global_Info.AccessWindowSize.X / Global_Info.AccessScreenScale)
                            {
                                Global_Info.AccessCurrentGameState = Global_Info.GameState.Options;
                            }

                            break;
                        case TransitionState.ToLvSelect:
                            
                            if (transitionPos.X <= transitionWidth / 2 - Global_Info.AccessWindowSize.X / Global_Info.AccessScreenScale)
                            {
                                Global_Info.AccessCurrentGameState = Global_Info.GameState.LevelSelect;
                                Level_Manager.AccessCurrentLevel = 0;
                                Level_Manager.AccessAllMoves.Clear();
                                Level_Manager.AccessCompleted = false;
                                Music_Player.StopMusic();
                            }

                            break;
                        case TransitionState.ToLevel:

                            if (transitionPos.X <= transitionWidth / 2 - Global_Info.AccessWindowSize.X / Global_Info.AccessScreenScale)
                            {
                                
                                Global_Info.AccessCurrentGameState = Global_Info.GameState.PlayingLevel;

                                if (!Pause_Menu.gameIsPaused)
                                {
                                    Level_Manager.AccessCurrentBundle = Level_Select_Menu.SelectedBundleX + Level_Select_Menu.SelectedBundleY * 5;
                                    Level_Manager.AccessRating = 1000;
                                    Level_Manager.isOnTransitionScreen = true;
                                    File_Manager.LoadLevel();
                                    Hint_File_Manager.LoadHints();
                                }
                                
                            }

                            break;
                    }

                    if (transitionPos.X * dir >= transitionWidth)
                    {
                        transitioning = false;
                    }
                }
            }
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            if (transitioning)
            {
                spriteBatch.Draw(transitionTex, new Rectangle((int)transitionPos.X, (int)transitionPos.Y, 
                    (int)transitionWidth, (int)(Global_Info.WindowSize.Y / Global_Info.AccessScreenScale)), 
                    Color.White);
            }
        }
    }
}
