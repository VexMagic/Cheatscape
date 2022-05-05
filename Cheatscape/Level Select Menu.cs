using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Cheatscape
{
    static class Level_Select_Menu
    {
        static Texture2D panel;
        static Texture2D numbers;

        static int selectedLevel = 0;
        static int levelAmount = 2;

        public static void Load()
        {
            panel = Global_Info.AccessContentManager.Load<Texture2D>("Level Panel");
            numbers = Global_Info.AccessContentManager.Load<Texture2D>("Numbers");
        }

        public static void Update()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Left) && selectedLevel > 0)
            {
                selectedLevel--;
            }
            else if (Input_Manager.KeyPressed(Keys.Right) && selectedLevel < levelAmount - 1)
            {
                selectedLevel++;
            }
            else if (Input_Manager.KeyPressed(Keys.Space))
            {
                Global_Info.AccessCurrentGameState = Global_Info.GameState.playingLevel;
                Level_Manager.AccessCurrentLevel = selectedLevel;
                File_Manager.LoadLevel();
            }
            else if (Input_Manager.KeyPressed(Keys.Back))
            {
                Main_Menu.Return();
                Global_Info.AccessCurrentGameState = Global_Info.GameState.mainMenu;
            }
            else if (Input_Manager.KeyPressed(Keys.Left))
            {
                Global_Info.AccessCurrentGameState = Global_Info.GameState.options;
            }
        }

        public static void Draw(SpriteBatch aSpriteBatch)
        {
            for (int i = 0; i < levelAmount; i++)
            {
                aSpriteBatch.Draw(numbers, new Rectangle(201 + (i - selectedLevel) * 100, 205, 9, 5), new Rectangle(9 * i, 0, 9, 5), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
                aSpriteBatch.Draw(panel, new Vector2(196 + (i - selectedLevel) * 100, 200), Color.White);
            }
        }
    }
}
