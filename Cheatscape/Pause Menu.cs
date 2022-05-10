using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Threading;

namespace Cheatscape
{
    static class Pause_Menu
    {
        public static bool gameIsPaused = false;
        static Texture2D pauseMenu;
        static Texture2D backButton;
        static Texture2D optionsButton;
        static Texture2D exitButton;
        static Texture2D buttonHighLight;

        static int pauseIndex = 0;
        static int pauseAmount = 2;

        public static void Load()
        {
            pauseMenu = Global_Info.AccessContentManager.Load<Texture2D>("Pause_Menu");

            backButton = Global_Info.AccessContentManager.Load<Texture2D>("BackButton");
            optionsButton = Global_Info.AccessContentManager.Load<Texture2D>("OptionsButton");
            exitButton = Global_Info.AccessContentManager.Load<Texture2D>("ExitButton");

            buttonHighLight = Global_Info.AccessContentManager.Load<Texture2D>("OptionsButtonHighlight");
        }

        public static void Update(GameTime gameTime)
        {
            if (gameIsPaused)
            {
                if (Input_Manager.KeyPressed(Keys.Down) && pauseIndex < pauseAmount)
                {
                    pauseIndex++;
                }
                else if (Input_Manager.KeyPressed(Keys.Up) && pauseIndex > 0)
                {
                    pauseIndex--;
                }
                else if (Input_Manager.KeyPressed(Keys.Space))
                {
                    if (pauseIndex == 0)
                    {
                        gameIsPaused = false;
                    }
                    else if (pauseIndex == 1)
                    {
                        Transition.nextTransitionState = Transition.TransitionState.ToLevel;
                        Transition.StartTransition(Transition.TransitionState.ToOptions);
                    }
                    else if (pauseIndex == 2)
                    {
                        Transition.StartTransition(Transition.TransitionState.ToLvSelect);
                        gameIsPaused = false;
                    }
                }
            }
        }
        public static void Draw(SpriteBatch aSpriteBatch)
        {
            if (gameIsPaused == true)
            {
                aSpriteBatch.Draw(pauseMenu, new Rectangle(0, 0, (int)(Global_Info.WindowSize.X / Global_Info.AccessScreenScale), (int)(Global_Info.WindowSize.Y / Global_Info.AccessScreenScale)), Color.White);

                aSpriteBatch.Draw(buttonHighLight, new Vector2(240, 130 + 50 * pauseIndex), Color.White);

                Text_Manager.DrawLargeText("Cheatscape", 300 - ((int)Text_Manager.LargeFont.MeasureString("Cheatscape").Length() / 2), 85, aSpriteBatch);
                Text_Manager.DrawText("Continue Game", 280, 140, aSpriteBatch);
                aSpriteBatch.Draw(backButton, new Vector2(240, 130), Color.White);

                Text_Manager.DrawText("Options", 280, 190, aSpriteBatch);
                aSpriteBatch.Draw(optionsButton, new Vector2(240, 180), Color.White);

                Text_Manager.DrawText("Back to Menu", 280, 240, aSpriteBatch);
                aSpriteBatch.Draw(exitButton, new Vector2(240, 230), Color.White);
            }       
        }
    }
}