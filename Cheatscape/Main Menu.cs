using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Cheatscape
{
    static class Main_Menu
    {

        static int CurrentFrame = 0;
        const int MAXFRAME = 8;
        static bool animating = false;
        static int frame = 1;
        static float timer;
        static float framesPerSecond = 151f;
        static Texture2D menuScreen;
        static Rectangle srcRect;
        static int width = 1920;
        static int height = 1080;

        public static void Load()
        {
            menuScreen = Global_Info.AccessContentManager.Load<Texture2D>("Main_Menu");
        }
        public static void Update(GameTime gameTime)
        {
            if (animating == true)
            {
                timer += (float)gameTime.ElapsedGameTime.Milliseconds;

                if (timer >= framesPerSecond)
                {
                    CurrentFrame += frame;
                    timer = 0;
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                animating = true;
            }

            if (CurrentFrame == MAXFRAME)
            {
                animating = false;
                Global_Info.AccessCurrentGameState = Global_Info.GameState.LevelSelect;

            }

            srcRect = new Rectangle(0, height * CurrentFrame, width, height);

        }
        public static void Draw(SpriteBatch aSpriteBatch)
        {
            aSpriteBatch.Draw(menuScreen, new Rectangle(0,0,(int)(Global_Info.WindowSize.X/Global_Info.AccessScreenScale), (int)(Global_Info.WindowSize.Y / Global_Info.AccessScreenScale)), srcRect, Color.White);
        }
    }
}
