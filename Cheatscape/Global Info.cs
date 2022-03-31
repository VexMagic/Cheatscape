using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cheatscape
{
    static class Global_Info
    {
        static ContentManager ContentManager;
        static float ScreenScale = 2f;
        static Vector2 WindowSize = new Vector2(480 * ScreenScale, 270 * ScreenScale);

        public static ContentManager AccessContentManager { get => ContentManager; set => ContentManager = value; }
        public static float AccessScreenScale { get => ScreenScale; set => ScreenScale = value; }
        public static Vector2 AccessWindowSize { get => WindowSize; set => WindowSize = value; }

        static Texture2D Test;

        public static void Load()
        {
            Test = ContentManager.Load<Texture2D>("Box");
        }

        public static void Update()
        {
            
        }

        public static void Draw(SpriteBatch aSpriteBatch)
        {
            aSpriteBatch.Draw(Test, new Vector2(0, 0), Color.White);
        }
    }
}
