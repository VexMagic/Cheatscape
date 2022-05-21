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
        public static List<float> highScores;
        public static List<float> AccessHighScores { get => highScores; set => highScores = value; }


        public static int SelectedBundleX = 0;
        public static int SelectedBundleY = 0;

        static int LevelAmountX = 5;
        static int LevelAmountY = 2;

        public static bool optionHighlight = false;

        

        public static void Load()
        {
            PanelTex = Global_Info.AccessContentManager.Load<Texture2D>("Level Panel");
            NumbersTex = Global_Info.AccessContentManager.Load<Texture2D>("Numbers");
            PanelHighLightTex = Global_Info.AccessContentManager.Load<Texture2D>("LevelPanelHighlight");
            Bg1Tex = Global_Info.AccessContentManager.Load<Texture2D>("Kindergarten");
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
            if (Input_Manager.KeyPressed(Keys.Left) && SelectedBundleX > 0)
            {
                SelectedBundleX--;
            }
            else if (Input_Manager.KeyPressed(Keys.Right) && SelectedBundleX < LevelAmountX - 1)
            {
                SelectedBundleX++;
            }
            else if (Input_Manager.KeyPressed(Keys.Up) && SelectedBundleY > 0 && !optionHighlight)
            {
                SelectedBundleY--;
            }
            else if (Input_Manager.KeyPressed(Keys.Down) && SelectedBundleY < LevelAmountY - 1)
            {
                SelectedBundleY++;
            }
            else if (Input_Manager.KeyPressed(Keys.Down) && SelectedBundleY == 1)
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
                Global_Info.AccessCurrentGameState = Global_Info.GameState.MainMenu;
            }
            else if (optionHighlight && Input_Manager.KeyPressed(Keys.Space))
            {
                Transition.AccessNextTransitionState = Transition.TransitionState.ToLvSelect;

                Transition.StartTransition(Transition.TransitionState.ToOptions);
            }
            else if (!optionHighlight && Input_Manager.KeyPressed(Keys.Space))
            {
                try
                {
                    if ((SelectedBundleX + SelectedBundleY * 5) == 0)
                    {

                        Music_Player.ChangeMusic((SelectedBundleX + SelectedBundleY * 5));
                        Music_Player.PlayMusic();
                        Level_Manager.AccessRating = 1000;

                        Transition.StartTransition(Transition.TransitionState.ToLevel);
                    }

                    else if (Global_Tracker.completedBundels.Count == (SelectedBundleX + SelectedBundleY * 5) || Global_Tracker.completedBundels.Count > (SelectedBundleX + SelectedBundleY * 5))
                    {
                        Music_Player.ChangeMusic(SelectedBundleX); // Den här hindrar koden från att köras eftersom att det inte finns n¨gon song3 att ladda in
                        Music_Player.PlayMusic();
                        Level_Manager.AccessRating = 1000;
                        Transition.StartTransition(Transition.TransitionState.ToLevel);
                    }


                }
                catch
                {
                    
                }

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
                aSpriteBatch.Draw(PanelHighLightTex, new Vector2(50 + SelectedBundleX * 100, 50 + SelectedBundleY * 75), Color.White);
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
