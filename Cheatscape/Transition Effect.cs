using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cheatscape
{
    class Transition_Effect
    {
        static Texture2D transitionTex;

        static Vector2 transitionPos;

        public static bool transitioning = false;

        static int dir;

        static float transitionWidth = 300 + Global_Info.windowSize.X / Global_Info.AccessScreenScale;
        static float timeSinceLastMove;
        static float timeBetweenMoves = 30f;

        public enum TransitionState
        {
            toOptions, toLvSelect, toLevel
        };
        public static TransitionState transitionState, nextTransitionState;

        public static TransitionState AccessNextTransitionState
        {
            get => nextTransitionState; set => nextTransitionState = value;
        }
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
                    case TransitionState.toLvSelect:
                        dir = -1;
                        break;
                    case TransitionState.toOptions:
                        dir = 1;
                        break;
                    case TransitionState.toLevel:
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
                        case TransitionState.toOptions:

                            if (transitionPos.X >= transitionWidth / 2 - Global_Info.AccessWindowSize.X / Global_Info.AccessScreenScale)
                            {
                                Global_Info.AccessCurrentGameState = Global_Info.GameState.options;
                            }

                            break;
                        case TransitionState.toLvSelect:

                            if (transitionPos.X <= transitionWidth / 2 - Global_Info.AccessWindowSize.X / Global_Info.AccessScreenScale)
                            {
                                Global_Info.AccessCurrentGameState = Global_Info.GameState.levelSelect;
                                Level_Manager.AccessCurrentLevel = 0;
                                Level_Manager.AccessAllMoves.Clear();
                                Level_Manager.AccessCompleted = false;
                                Music_Player.StopMusic();
                            }

                            break;
                        case TransitionState.toLevel:

                            if (transitionPos.X <= transitionWidth / 2 - Global_Info.AccessWindowSize.X / Global_Info.AccessScreenScale)
                            {

                                Global_Info.AccessCurrentGameState = Global_Info.GameState.playingLevel;

                                if (!Pause_Menu.gameIsPaused)
                                {
                                    Level_Manager.AccessCurrentBundle = Level_Select_Menu.selectedBundleX + Level_Select_Menu.selectedBundleY * 5;
                                    Level_Manager.AccessRating = 1000;
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
                    (int)transitionWidth, (int)(Global_Info.windowSize.Y / Global_Info.AccessScreenScale)),
                    Color.White);
            }
        }
    }
}
