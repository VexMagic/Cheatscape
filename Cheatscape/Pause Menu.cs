using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Cheatscape
{
    static class Pause_Menu
    {
        public static bool gameIsPaused = false;
        static Texture2D pauseMenu;
        static Texture2D continueButton;
        static Texture2D restartButton;
        static Texture2D optionsButton;
        static Texture2D exitButton;
        static Texture2D buttonHighLight;

        static int pauseIndexX = 0;
        static int pauseIndexY = 0;
        static int pauseAmount = 1;

        public static void Load()
        {
            pauseMenu = Global_Info.AccessContentManager.Load<Texture2D>("Pause_Menu");

            continueButton = Global_Info.AccessContentManager.Load<Texture2D>("ContinueButton");
            restartButton = Global_Info.AccessContentManager.Load<Texture2D>("BackButton");
            optionsButton = Global_Info.AccessContentManager.Load<Texture2D>("OptionsButton");
            exitButton = Global_Info.AccessContentManager.Load<Texture2D>("ExitButton");

            buttonHighLight = Global_Info.AccessContentManager.Load<Texture2D>("OptionsButtonHighlight");
        }

        public static void Update()
        {
            if (gameIsPaused)
            {

                if (Keyboard_Inputs.KeyPressed(Keys.Right) && pauseIndexX < pauseAmount)
                {
                    pauseIndexX++;
                }
                else if (Keyboard_Inputs.KeyPressed(Keys.Left) && pauseIndexX > 0)
                {
                    pauseIndexX--;
                }
                else if (Keyboard_Inputs.KeyPressed(Keys.Down) && pauseIndexY < pauseAmount)
                {
                    pauseIndexY++;
                }
                else if (Keyboard_Inputs.KeyPressed(Keys.Up) && pauseIndexY > 0)
                {
                    pauseIndexY--;
                }
                else if (Keyboard_Inputs.KeyPressed(Keys.Space))
                {
                    if (pauseIndexX == 0 && pauseIndexY == 0) //Continue
                    {
                        gameIsPaused = false;
                    }
                    else if (pauseIndexX == 1 && pauseIndexY == 0) //Restart
                    {
                        Transition_Effect.StartTransition(Transition_Effect.TransitionState.toLevel);
                        gameIsPaused = false;
                    }
                    else if (pauseIndexX == 0 && pauseIndexY == 1) //Options
                    {
                        Transition_Effect.nextTransitionState = Transition_Effect.TransitionState.toLevel;
                        Transition_Effect.StartTransition(Transition_Effect.TransitionState.toOptions);
                    }
                    else if (pauseIndexX == 1 && pauseIndexY == 1) //Back to Menu
                    {
                        Transition_Effect.StartTransition(Transition_Effect.TransitionState.toLvSelect);
                        gameIsPaused = false;
                    }
                }
            }
        }
        public static void Draw(SpriteBatch aSpriteBatch)
        {
            if (gameIsPaused)
            {
                aSpriteBatch.Draw(pauseMenu, new Rectangle(0, 0, (int)(Global_Info.windowSize.X / Global_Info.AccessScreenScale), (int)(Global_Info.windowSize.Y / Global_Info.AccessScreenScale)), Color.White);

                aSpriteBatch.Draw(buttonHighLight, new Vector2(150 + 268 * pauseIndexX, 130 + 100 * pauseIndexY), Color.White);

                Text_Manager.DrawLargeText("Cheatscape", 300 - ((int)Text_Manager.largeFont.MeasureString("Cheatscape").Length() / 2), 85, aSpriteBatch);

                Text_Manager.DrawText("Continue", 190, 140, aSpriteBatch);
                aSpriteBatch.Draw(continueButton, new Vector2(150, 130), Color.White);

                Text_Manager.DrawText("Restart Level", 348, 140, aSpriteBatch);
                aSpriteBatch.Draw(restartButton, new Vector2(418, 130), Color.White);

                Text_Manager.DrawText("Options", 190, 240, aSpriteBatch);
                aSpriteBatch.Draw(optionsButton, new Vector2(150, 230), Color.White);

                Text_Manager.DrawText("Back to Menu", 350, 240, aSpriteBatch);
                aSpriteBatch.Draw(exitButton, new Vector2(418, 230), Color.White);
            }
        }
    }
}