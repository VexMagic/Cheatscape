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
        static KeyboardState currentKS, previousKS;
        static MouseState currentMS, previousMS;
        static bool mouseActive;
        public static bool AccessMouseActivity
        {
            get => mouseActive;
        }

        public static void Update()
        {
            previousKS = currentKS;
            currentKS = Keyboard.GetState();

            previousMS = currentMS;
            currentMS = Mouse.GetState();

            if (currentKS.GetPressedKeys().Length > 0)
            {
                mouseActive = false;
            }

            if (previousMS != currentMS)
            {
                mouseActive = true;
            }
        }

        public static bool KeyPressed(Keys key)
        {
            if (currentKS.IsKeyDown(key) && previousKS.IsKeyUp(key))
            {
                return true;
            }

            return false;
        }

        public static bool KeyReleased(Keys key)
        {
            if (currentKS.IsKeyUp(key) && previousKS.IsKeyDown(key))
            {
                return true;
            }

            return false;
        }

        public static bool MouseLBPressed()
        {
            if (currentMS.LeftButton == ButtonState.Pressed && previousMS.LeftButton == ButtonState.Released)
            {
                return true;
            }

            return false;
        }

        public static bool MouseLBReleased()
        {
            if (currentMS.LeftButton == ButtonState.Released && previousMS.LeftButton == ButtonState.Pressed)
            {
                return true;
            }

            return false;
        }

        public static bool MouseLBHeldDown()
        {
            if (currentMS.LeftButton == ButtonState.Pressed && previousMS.LeftButton == ButtonState.Pressed)
            {
                return true;
            }

            return false;
        }

        public static Vector2 GetMousePosition()
        {
            return new Vector2(currentMS.X, currentMS.Y) / Global_Info.AccessScreenScale;
        }
    }
}
