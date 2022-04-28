using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Cheatscape
{
    class Input_Manager
    {
        public static KeyboardState CurrentKS, PreviousKS;

        public static void Update()
        {
            PreviousKS = CurrentKS;
            CurrentKS = Keyboard.GetState();
        }

        public static bool KeyPressed(Keys key)
        {
            if (CurrentKS.IsKeyDown(key) && PreviousKS.IsKeyUp(key))
            {
                return true;
            }

            return false;
        }

        public static bool KeyReleased(Keys key)
        {
            if (CurrentKS.IsKeyUp(key) && PreviousKS.IsKeyDown(key))
            {
                return true;
            }

            return false;
        }
    }
}
