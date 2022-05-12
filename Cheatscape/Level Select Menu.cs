﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Cheatscape
{
    static class Level_Select_Menu
    {
        static Texture2D panelTex;
        static Texture2D numbersTex;
        static Texture2D panelHighLightTex;
        static Texture2D bg1Tex;
        static Texture2D optionButtonTex;
        static Texture2D optionHighlightTex;

        public static int selectedBundleX = 0;
        public static int selectedBundleY = 0;

        static int levelAmountX = 5;
        static int levelAmountY = 2;

        public static bool optionHighlight = false;

        public static void Load()
        {
            panelTex = Global_Info.AccessContentManager.Load<Texture2D>("Level Panel");
            numbersTex = Global_Info.AccessContentManager.Load<Texture2D>("Numbers");
            panelHighLightTex = Global_Info.AccessContentManager.Load<Texture2D>("LevelPanelHighlight");
            bg1Tex = Global_Info.AccessContentManager.Load<Texture2D>("Background");
            optionButtonTex = Global_Info.AccessContentManager.Load<Texture2D>("OptionsButton");
            optionHighlightTex = Global_Info.AccessContentManager.Load<Texture2D>("OptionsButtonHighlight");
        }

        public static void Update()
        {
            if (Input_Manager.KeyPressed(Keys.Left) && selectedBundleX > 0)
            {
                selectedBundleX--;
            }
            else if (Input_Manager.KeyPressed(Keys.Right) && selectedBundleX < levelAmountX - 1)
            {
                selectedBundleX++;
            }
            else if (Input_Manager.KeyPressed(Keys.Up) && selectedBundleY > 0 && !optionHighlight)
            {
                selectedBundleY--;
            }
            else if (Input_Manager.KeyPressed(Keys.Down) && selectedBundleY < levelAmountY - 1)
            {
                selectedBundleY++;
            }
            else if (Input_Manager.KeyPressed(Keys.Down) && selectedBundleY == 1)
            {
                optionHighlight = true;
            }
            else if (Input_Manager.KeyPressed(Keys.Up) && optionHighlight)
            {
                optionHighlight = false;
            }
            else if (Input_Manager.KeyPressed(Keys.Back))
            {
                Main_Menu.Return();
                Global_Info.AccessCurrentGameState = Global_Info.GameState.mainMenu;
            }
            else if (optionHighlight && Input_Manager.KeyPressed(Keys.Space))
            {
                Transition.AccessNextTransitionState = Transition.TransitionState.ToLvSelect;

                Transition.StartTransition(Transition.TransitionState.ToOptions);
            }
            else if (!optionHighlight && Input_Manager.KeyPressed(Keys.Space))
            {
                Level_Manager.AccessCurrentLevel = 0;
                Music_Player.ChangeMusic(selectedBundleX);
                Music_Player.PlayMusic();
                Level_Manager.AccessRating = 1000;

                Transition.StartTransition(Transition.TransitionState.ToLevel);
            }
        }

        public static void Draw(SpriteBatch aSpriteBatch)
        {
            aSpriteBatch.Draw(bg1Tex, new Rectangle(50, 50, panelTex.Width, panelTex.Height), Color.White);

            if (optionHighlight)
            {
                aSpriteBatch.Draw(optionHighlightTex, new Vector2(50, 200), Color.White);
            }
            else
            {
                aSpriteBatch.Draw(panelHighLightTex, new Vector2(50 + selectedBundleX * 100, 50 + selectedBundleY * 75), Color.White);
            }

            aSpriteBatch.Draw(optionButtonTex, new Vector2(50, 200), Color.White);

            for (int i = 0; i < levelAmountY; i++)
            {
                for (int j = 0; j < levelAmountX; j++)
                {
                    aSpriteBatch.Draw(numbersTex, new Rectangle(55 + j * 100, 55 + i * 75, 9, 5), new Rectangle(9 * j + i * 45, 0, 9, 5), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
                    aSpriteBatch.Draw(panelTex, new Vector2(50 + j * 100, 50 + i * 75), Color.White);
                }
            }

            if (Options_Menu.AccessControlView)
            {
                Text_Manager.DrawText("Arrow keys: Navigate     Space: Select", 30,
                    (int)(Global_Info.AccessWindowSize.Y / Global_Info.AccessScreenScale - 40), aSpriteBatch);
            }
        }
    }
}
