using Microsoft.Xna.Framework.Input;

namespace Cheatscape
{
    class Input_Manager
    {
        public static KeyboardState currentKS, previousKS;

        public static void Update()
        {
            previousKS = currentKS;
            currentKS = Keyboard.GetState();
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
    }
}
