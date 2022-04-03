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

        public static void Load()
        {
            Game_Board.Load();
            File_Manager.LoadLevel();
        }

        public static void Update()
        {
            Level_Manager.Update();
        }

        public static void Draw(SpriteBatch aSpriteBatch)
        {
            Game_Board.Draw(aSpriteBatch);
        }
    }
}
