using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Cheatscape
{
    static class Main_Menu
    {

        public static int currentFrame = 0;
        const int mAXFRAME = 8;
        public static bool animating = false;
        static int frame = 1;
        static float timer;
        static float framesPerSecond = 90f;
        static Texture2D menuScreen;
        static Rectangle srcRect;
        static int width = 1920;
        static int height = 1080;

        public static void Load()
        {
            menuScreen = Global_Info.AccessContentManager.Load<Texture2D>("Main_Menu");
        }

        public static void Return()
        {
            animating = true;
            frame = -1;
        }
        public static void Stop()
        {
            animating = false;
            frame = 1;
            currentFrame = 0;
        }
        public static void Update(GameTime gameTime)
        {
            if (animating == true)
            {
                timer += gameTime.ElapsedGameTime.Milliseconds;

                if (timer >= framesPerSecond)
                {
                    currentFrame += frame;
                    timer = 0;
                }
            }

            if (Keyboard_Inputs.KeyPressed(Keys.Space) && Global_Info.AccessCurrentGameState == Global_Info.GameState.mainMenu)
            {
                animating = true;
            }

            if (currentFrame == mAXFRAME && Global_Info.AccessCurrentGameState == Global_Info.GameState.mainMenu && frame > 0)
            {
                animating = false;
                Global_Info.AccessCurrentGameState = Global_Info.GameState.levelSelect;
            }


            if (currentFrame < 0)
            {
                Stop();
            }

            srcRect = new Rectangle(0, height * currentFrame, width, height);

        }
        public static void Draw(SpriteBatch aSpriteBatch)
        {
            aSpriteBatch.Draw(menuScreen, new Rectangle(0, 0, (int)(Global_Info.windowSize.X / Global_Info.AccessScreenScale), (int)(Global_Info.windowSize.Y / Global_Info.AccessScreenScale)), srcRect, Color.White);
        }
    }
}
