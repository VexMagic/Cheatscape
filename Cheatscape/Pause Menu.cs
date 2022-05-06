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

        public static void Load()
        {
            pauseMenu = Global_Info.AccessContentManager.Load<Texture2D>("Pause_Menu");
        }

        public static void Update(GameTime gameTime)
        {

        }
        public static void Draw(SpriteBatch aSpriteBatch)
        {
            if (gameIsPaused == true)
            {
                aSpriteBatch.Draw(pauseMenu, new Rectangle(0, 0, (int)(Global_Info.WindowSize.X / Global_Info.AccessScreenScale), (int)(Global_Info.WindowSize.Y / Global_Info.AccessScreenScale)), Color.White);
            }       
        }
    }
}