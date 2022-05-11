using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Threading;

namespace Cheatscape
{
    static class Level_Transition
    {
        static Texture2D transitionScreen;

        public static void Load()
        {
            transitionScreen = Global_Info.AccessContentManager.Load<Texture2D>("Transition_Screen");
        }

        public static void Draw(SpriteBatch aSpriteBatch)
        {
            aSpriteBatch.Draw(transitionScreen, new Rectangle(0, 0, (int)(Global_Info.WindowSize.X / Global_Info.AccessScreenScale), (int)(Global_Info.WindowSize.Y / Global_Info.AccessScreenScale)), Color.White);
            
            Text_Manager.DrawLargeText("Level: " + Level_Manager.CurrentBundle+1 + "-" + Level_Manager.CurrentLevel, 300 - ((int)Text_Manager.LargeFont.MeasureString("Level: 0-0").Length() / 2), 35, aSpriteBatch);
            Text_Manager.DrawText("Removed rules:", 60, 90, aSpriteBatch);
            Text_Manager.DrawLargeText("Press Enter to begin", 300 - ((int)Text_Manager.LargeFont.MeasureString("Press Enter to begin").Length() / 2), 290, aSpriteBatch);
        }
    }
}