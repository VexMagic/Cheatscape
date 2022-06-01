using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

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

        static int bundleamountX = 5;
        static int bundleamountY = 2;

        public static bool optionHighlight = false;

        static List<float> highScores;
        public static List<float> AccessHighScores
        {
            get => highScores; set => highScores = value;
        }

        public static void Load()
        {
            panelTex = Global_Info.AccessContentManager.Load<Texture2D>("Level Panel");
            numbersTex = Global_Info.AccessContentManager.Load<Texture2D>("Numbers");
            panelHighLightTex = Global_Info.AccessContentManager.Load<Texture2D>("LevelPanelHighlight");
            bg1Tex = Global_Info.AccessContentManager.Load<Texture2D>("Background");
            optionButtonTex = Global_Info.AccessContentManager.Load<Texture2D>("OptionsButton");
            optionHighlightTex = Global_Info.AccessContentManager.Load<Texture2D>("OptionsButtonHighlight");

            highScores = new List<float>();

            for (int i = 0; i < 10; i++)
            {
                highScores.Add(0);
            }
        }

        public static void Update()
        {
            if (Keyboard_Inputs.KeyPressed(Keys.Left) && selectedBundleX > 0)
            {
                selectedBundleX--;
            }
            else if (Keyboard_Inputs.KeyPressed(Keys.Right) && selectedBundleX < bundleamountX - 1)
            {
                selectedBundleX++;
            }
            else if (Keyboard_Inputs.KeyPressed(Keys.Up) && selectedBundleY > 0 && !optionHighlight)
            {
                selectedBundleY--;
            }
            else if (Keyboard_Inputs.KeyPressed(Keys.Down) && selectedBundleY < bundleamountY - 1)
            {
                selectedBundleY++;
            }
            else if (Keyboard_Inputs.KeyPressed(Keys.Down) && selectedBundleY == 1)
            {
                optionHighlight = true;
            }
            else if (Keyboard_Inputs.KeyPressed(Keys.Up) && optionHighlight)
            {
                optionHighlight = false;
            }
            else if (Keyboard_Inputs.KeyPressed(Keys.Back))
            {
                Main_Menu.Return();
                Global_Info.AccessCurrentGameState = Global_Info.GameState.mainMenu;
            }
            else if (optionHighlight && Keyboard_Inputs.KeyPressed(Keys.Space))
            {
                Transition_Effect.AccessNextTransitionState = Transition_Effect.TransitionState.toLvSelect;

                Transition_Effect.StartTransition(Transition_Effect.TransitionState.toOptions);
            }
            else if (!optionHighlight && Keyboard_Inputs.KeyPressed(Keys.Space))
            {
                Level_Manager.AccessCurrentLevel = 0;
                Music_Player.ChangeMusic(selectedBundleX);
                Music_Player.PlayMusic();
                Level_Manager.AccessRating = 1000;

                Transition_Effect.StartTransition(Transition_Effect.TransitionState.toLevel);
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

            for (int i = 0; i < bundleamountY; i++)
            {
                for (int j = 0; j < bundleamountX; j++)
                {
                    aSpriteBatch.Draw(numbersTex, new Rectangle(55 + j * 100, 55 + i * 75, 9, 5), new Rectangle(9 * j + i * 45, 0, 9, 5), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
                    aSpriteBatch.Draw(panelTex, new Vector2(50 + j * 100, 50 + i * 75), Color.White);
                    Text_Manager.DrawText(highScores[j + i * 5].ToString(), 55 + j * 100, 99 + i * 75, aSpriteBatch);
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
