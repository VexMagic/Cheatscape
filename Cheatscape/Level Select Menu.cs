using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cheatscape
{
    static class Level_Select_Menu
    {
        static Texture2D PanelTex;
        static Texture2D NumbersTex;
        static Texture2D PanelHighLightTex;
        static Texture2D Bg1Tex;
        static Texture2D optionButtonTex;
        static Texture2D optionHighlightTex;

        static int SelectedLevelX = 0;
        static int SelectedLevelY = 0;

        static int LevelAmountX = 5;
        static int LevelAmountY = 2;

        static bool optionHighlight = false;

        public static void Load()
        {
            PanelTex = Global_Info.AccessContentManager.Load<Texture2D>("Level Panel");
            NumbersTex = Global_Info.AccessContentManager.Load<Texture2D>("Numbers");
            PanelHighLightTex = Global_Info.AccessContentManager.Load<Texture2D>("LevelPanelHighlight");
            Bg1Tex = Global_Info.AccessContentManager.Load<Texture2D>("Background");
            optionButtonTex = Global_Info.AccessContentManager.Load<Texture2D>("OptionsButton");
            optionHighlightTex = Global_Info.AccessContentManager.Load<Texture2D>("OptionsButtonHighlight");
        }

        public static void Update()
        {
            if (Input_Manager.KeyPressed(Keys.Left) && SelectedLevelX > 0)
            {
                SelectedLevelX--;
            }
            else if (Input_Manager.KeyPressed(Keys.Right) && SelectedLevelX < LevelAmountX - 1)
            {
                SelectedLevelX++;
            }
            else if (Input_Manager.KeyPressed(Keys.Up) && optionHighlight)
            {
                optionHighlight = false;
            }
            else if (Input_Manager.KeyPressed(Keys.Up) && SelectedLevelY > 0)
            {
                SelectedLevelY--;
            }
            else if (Input_Manager.KeyPressed(Keys.Down) && SelectedLevelY < LevelAmountY - 1)
            {
                SelectedLevelY++;
            }
            else if (Input_Manager.KeyPressed(Keys.Down))
            {
                optionHighlight = true;
            }
            else if (Input_Manager.KeyPressed(Keys.Space) && !optionHighlight)
            {
                Global_Info.AccessCurrentGameState = Global_Info.GameState.PlayingLevel;
                Level_Manager.AccessCurrentLevel = SelectedLevelX + SelectedLevelY * 5;
                File_Manager.LoadLevel();
            }
            else if (Input_Manager.KeyPressed(Keys.Back))
            {
                Main_Menu.Return();
                Global_Info.AccessCurrentGameState = Global_Info.GameState.MainMenu;
            }
            else if (optionHighlight && Input_Manager.KeyPressed(Keys.Space))
            {
                Transition.StartTransition(Transition.TransitionState.ToOptions);
            }
        }

        public static void Draw(SpriteBatch aSpriteBatch)
        {
            aSpriteBatch.Draw(Bg1Tex, new Rectangle(50, 50, PanelTex.Width, PanelTex.Height), Color.White);
            
            if (optionHighlight)
            {
                aSpriteBatch.Draw(optionHighlightTex, new Vector2(50, 200), Color.White);
            }
            else
            {
                aSpriteBatch.Draw(PanelHighLightTex, new Vector2(50 + SelectedLevelX * 100, 50 + SelectedLevelY * 75), Color.White);
            }

            aSpriteBatch.Draw(optionButtonTex, new Vector2(50, 200), Color.White);
            
            for (int i = 0; i < LevelAmountY; i++)
            {
                for (int j = 0; j < LevelAmountX; j++)
                {
                    aSpriteBatch.Draw(NumbersTex, new Rectangle(55 + j * 100, 55 + i * 75, 9, 5), new Rectangle(9 * j + i * 45, 0, 9, 5), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
                    aSpriteBatch.Draw(PanelTex, new Vector2(50 + j * 100, 50 + i * 75), Color.White);
                }
            }
        }
    }
}
