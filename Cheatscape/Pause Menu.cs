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

        public static void Update(GameTime gameTime)
        {
            if (gameIsPaused)
            {

                if (Input_Manager.KeyPressed(Keys.Right) && pauseIndexX < pauseAmount)
                {
                    pauseIndexX++;
                }
                else if (Input_Manager.KeyPressed(Keys.Left) && pauseIndexX > 0)
                {
                    pauseIndexX--;
                }
                else if (Input_Manager.KeyPressed(Keys.Down) && pauseIndexY < pauseAmount)
                {
                    pauseIndexY++;
                }
                else if (Input_Manager.KeyPressed(Keys.Up) && pauseIndexY > 0)
                {
                    pauseIndexY--;
                }
                else if (Input_Manager.KeyPressed(Keys.Space))
                {
                    if (pauseIndexX == 0 && pauseIndexY == 0) //Continue
                    {
                        gameIsPaused = false;
                    }
                    else if (pauseIndexX == 1 && pauseIndexY == 0) //Restart
                    {
                        Transition.StartTransition(Transition.TransitionState.ToLevel);
                        gameIsPaused = false;
                    }
                    else if (pauseIndexX == 0 && pauseIndexY == 1) //Options
                    {
                        Transition.nextTransitionState = Transition.TransitionState.ToLevel;
                        Transition.StartTransition(Transition.TransitionState.ToOptions);
                    }
                    else if (pauseIndexX == 1 && pauseIndexY == 1) //Back to Menu
                    {
                        Transition.StartTransition(Transition.TransitionState.ToLvSelect);
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

                Text_Manager.DrawLargeText("Cheatscape", 300 - ((int)Text_Manager.LargeFont.MeasureString("Cheatscape").Length() / 2), 85, aSpriteBatch);

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