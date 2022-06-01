using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Cheatscape
{
    static class End_Screen
    {
        static bool gameIsEnded;
        static bool cleared;

        public static bool AccessIsEnded 
        {
            get => gameIsEnded; set => gameIsEnded = value; 
        }
        public static bool AccessCleared 
        {
            get => cleared; set => cleared = value;
        }

        static Texture2D levelClear;
        static Texture2D gameOver;
        static Texture2D continueButton;
        static Texture2D restartButton;
        static Texture2D exitButton;
        static Texture2D buttonHighLight;

        static int endIndex = 0;
        static int endAmount = 1;

        public static void Load()
        {
            levelClear = Global_Info.AccessContentManager.Load<Texture2D>("LevelClearPanel");
            gameOver = Global_Info.AccessContentManager.Load<Texture2D>("GameOverPanel");

            continueButton = Global_Info.AccessContentManager.Load<Texture2D>("ContinueButton");
            restartButton = Global_Info.AccessContentManager.Load<Texture2D>("BackButton");
            exitButton = Global_Info.AccessContentManager.Load<Texture2D>("ExitButton");

            buttonHighLight = Global_Info.AccessContentManager.Load<Texture2D>("OptionsButtonHighlight");
        }

        public static void Update()
        {
            if (gameIsEnded)
            {
                if (Keyboard_Inputs.KeyPressed(Keys.Right) && endIndex < endAmount)
                {
                    endIndex++;
                }
                else if (Keyboard_Inputs.KeyPressed(Keys.Left) && endIndex > 0)
                {
                    endIndex--;
                }
                else if (Keyboard_Inputs.KeyPressed(Keys.Space))
                {
                    if (cleared)
                    {
                        if (endIndex == 0) //Retry
                        {
                            Level_Manager.AccessCurrentLevel = 0;
                            Transition_Effect.StartTransition(Transition_Effect.TransitionState.toLevel);
                        }
                        else if (endIndex == 1) //Continue
                        {
                            Transition_Effect.StartTransition(Transition_Effect.TransitionState.toLvSelect);
                        }

                        if (Level_Manager.AccessRating > 
                            Level_Select_Menu.AccessHighScores[Level_Select_Menu.selectedBundleX + Level_Select_Menu.selectedBundleY * 5])
                        {
                            Level_Select_Menu.AccessHighScores[Level_Select_Menu.selectedBundleX + Level_Select_Menu.selectedBundleY * 5] =
                                Level_Manager.AccessRating;
                        }
                    }
                    else
                    {
                        if (endIndex == 0) //Exit
                        {
                            Transition_Effect.StartTransition(Transition_Effect.TransitionState.toLvSelect);
                        }
                        else if (endIndex == 1) //Retry
                        {
                            Level_Manager.AccessCurrentLevel = 0;
                            Transition_Effect.StartTransition(Transition_Effect.TransitionState.toLevel);
                        }
                    }

                    gameIsEnded = false;
                    cleared = false;
                }
            }
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            if (gameIsEnded)
            {
                if (cleared)
                {
                    spriteBatch.Draw(levelClear, new Rectangle(0, 0, (int)(Global_Info.windowSize.X / Global_Info.AccessScreenScale), 
                        (int)(Global_Info.windowSize.Y / Global_Info.AccessScreenScale)), Color.White);

                    spriteBatch.Draw(buttonHighLight, new Vector2(150 + 268 * endIndex, 230), Color.White);

                    Text_Manager.DrawText(Convert.ToString("Final rating: " + Level_Manager.AccessRating), (int)(Global_Info.AccessWindowSize.X / Global_Info.AccessScreenScale) - 350, 170
                        , spriteBatch);

                    Text_Manager.DrawText("Retry", 190, 240, spriteBatch);
                    spriteBatch.Draw(restartButton, new Vector2(150, 230), Color.White);

                    Text_Manager.DrawText("Continue", 350, 240, spriteBatch);
                    spriteBatch.Draw(continueButton, new Vector2(418, 230), Color.White);
                }
                else
                {
                    spriteBatch.Draw(gameOver, new Rectangle(0, 0, (int)(Global_Info.windowSize.X / Global_Info.AccessScreenScale),
                        (int)(Global_Info.windowSize.Y / Global_Info.AccessScreenScale)), Color.White);

                    spriteBatch.Draw(buttonHighLight, new Vector2(150 + 268 * endIndex, 230), Color.White);

                    Text_Manager.DrawText(Convert.ToString("Final rating: " + Level_Manager.AccessRating), (int)(Global_Info.AccessWindowSize.X / Global_Info.AccessScreenScale) - 300, 150
                        , spriteBatch);

                    Text_Manager.DrawText("Return to Menu", 190, 240, spriteBatch);
                    spriteBatch.Draw(exitButton, new Vector2(150, 230), Color.White);

                    Text_Manager.DrawText("Retry", 350, 240, spriteBatch);
                    spriteBatch.Draw(restartButton, new Vector2(418, 230), Color.White);
                }
            }
        }
    }
}
