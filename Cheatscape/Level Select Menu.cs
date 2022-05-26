﻿using Microsoft.Xna.Framework;
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
        static Texture2D Bg2Tex;
        static Texture2D Bg3Tex;
        static Texture2D Bg4Tex;
        static Texture2D Bg5Tex;
        static Texture2D optionButtonTex;
        static Texture2D optionHighlightTex;
        static Texture2D lockTex;

        static Rectangle[,] LevelButtons = new Rectangle[5, 2];
        static Rectangle OptionsButton = new Rectangle(50, 200, 32, 32);

        public static List<float> highScores;
        public static List<float> AccessHighScores { get => highScores; set => highScores = value; }


        public static int SelectedBundleX = 0;
        public static int SelectedBundleY = 0;

        static int LevelAmountX = 5;
        static int LevelAmountY = 2;

        //kan döpas om till levelamount

        public static bool optionHighlight = false;

        

        public static void Load()
        {
            PanelTex = Global_Info.AccessContentManager.Load<Texture2D>("Level Panel");
            NumbersTex = Global_Info.AccessContentManager.Load<Texture2D>("Numbers");
            PanelHighLightTex = Global_Info.AccessContentManager.Load<Texture2D>("LevelPanelHighlight");
            Bg1Tex = Global_Info.AccessContentManager.Load<Texture2D>("Kindergarten");
            Bg2Tex = Global_Info.AccessContentManager.Load<Texture2D>("Desk");
            Bg3Tex = Global_Info.AccessContentManager.Load<Texture2D>("Park");
            Bg4Tex = Global_Info.AccessContentManager.Load<Texture2D>("cheatscape Train");
            Bg5Tex = Global_Info.AccessContentManager.Load<Texture2D>("soon");
            optionButtonTex = Global_Info.AccessContentManager.Load<Texture2D>("OptionsButton");
            optionHighlightTex = Global_Info.AccessContentManager.Load<Texture2D>("OptionsButtonHighlight");
            lockTex = Global_Info.AccessContentManager.Load<Texture2D>("Lock");

            highScores = new List<float>();          
            foreach (var item in Global_Tracker.completedBundels)
            {
                highScores.Add(item.Item2);
            }
            for (int i = 0; i < 10; i++)
            {
                highScores.Add(1);
            }

            for (int i = 0; i < LevelButtons.GetLength(0); i++)
            {
                for (int j = 0; j < LevelButtons.GetLength(1); j++)
                {
                    LevelButtons[i, j] = new Rectangle(50 + i * 100, 50 + j * 75, 96, 64);
                }
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
            else if (Input_Manager.AccessMouseActivity)
            {
                for (int i = 0; i < LevelButtons.GetLength(0); i++)
                {
                    for (int j = 0; j < LevelButtons.GetLength(1); j++)
                    {
                        if (LevelButtons[i, j].Contains(Input_Manager.GetMousePosition()))
                        {
                            SelectedBundleX = i;
                            SelectedBundleY = j;
                            optionHighlight = false;
                        }
                    }
                }

                if (OptionsButton.Contains(Input_Manager.GetMousePosition()))
                {
                    optionHighlight = true;
                }
            }

            if (Input_Manager.KeyPressed(Keys.Back))
            {
                Main_Menu.Return();
                Global_Info.AccessCurrentGameState = Global_Info.GameState.MainMenu;
            }
            else if (optionHighlight && Input_Manager.KeyPressed(Keys.Space) || (OptionsButton.Contains(Input_Manager.GetMousePosition()) 
                && Input_Manager.MouseLBPressed()))
            {
                Transition.AccessNextTransitionState = Transition.TransitionState.ToLvSelect;

                Transition.StartTransition(Transition.TransitionState.ToOptions);
            }
            else if (!optionHighlight && Input_Manager.KeyPressed(Keys.Space) || 
                LevelButtons[SelectedBundleX, SelectedBundleY].Contains(Input_Manager.GetMousePosition()) && Input_Manager.MouseLBPressed())
            {
                try
                {
                    if ((SelectedBundleX + SelectedBundleY * 5) == 0)
                    {

                        Music_Player.ChangeMusic((SelectedBundleX + SelectedBundleY * 5));
                        Music_Player.PlayMusic();
                        Game_Board.AdjustMap((SelectedBundleX + SelectedBundleY * 5));
                        Hint_File_Manager.LoadHints();
                        Level_Transition.LoadSpecialRule();
                        Level_Manager.isOnTransitionScreen = true;
                        Level_Manager.AccessRating = 1000;
                        Level_Manager.CurrentLevel = 0;
                        Level_Manager.AccessCurrentBundle = SelectedBundleX + SelectedBundleY * 5;
                        File_Manager.LoadLevel();
                        Game_Board.ResetBoard();

                        Transition.StartTransition(Transition.TransitionState.ToLevel);
                    }

                    else if (Global_Tracker.completedBundels.Count == (SelectedBundleX + SelectedBundleY * 5) || Global_Tracker.completedBundels.Count > (SelectedBundleX + SelectedBundleY * 5))
                    {
                        /*Music_Player.ChangeMusic(SelectedBundleX);*/ // Den här hindrar koden från att köras eftersom att det inte finns n¨gon song3 att ladda in
                        Music_Player.ChangeMusic((SelectedBundleX + SelectedBundleY * 5));
                        Music_Player.PlayMusic();
                        Game_Board.AdjustMap((SelectedBundleX + SelectedBundleY * 5));
                        Level_Manager.currentHint = -1;
                        Level_Manager.unlockedHints = -1;
                        Level_Manager.displayingHint = false;
                        Hint_File_Manager.LoadHints();
                        Level_Transition.LoadSpecialRule();
                        Level_Manager.isOnTransitionScreen = true;
                        Level_Manager.AccessRating = 1000;
                        Level_Manager.CurrentLevel = 0;
                        Level_Manager.AccessCurrentBundle = SelectedBundleX + SelectedBundleY * 5;
                        File_Manager.LoadLevel();
                        Game_Board.ResetBoard();

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
            aSpriteBatch.Draw(Bg1Tex, new Rectangle(150, 50, PanelTex.Width, PanelTex.Height), Color.White);
            aSpriteBatch.Draw(Bg2Tex, new Rectangle(250, 50, PanelTex.Width, PanelTex.Height), Color.White);
            aSpriteBatch.Draw(Bg3Tex, new Rectangle(350, 50, PanelTex.Width, PanelTex.Height), Color.White);
            aSpriteBatch.Draw(Bg4Tex, new Rectangle(450, 50, PanelTex.Width, PanelTex.Height),new Rectangle(0,0,1920,1080), Color.White);
            for (int i = 50; i < 550; i+=100)
            {
                aSpriteBatch.Draw(Bg5Tex, new Rectangle(i, 125, PanelTex.Width, PanelTex.Height), Color.White);
            }

            if (optionHighlight)
            {
                aSpriteBatch.Draw(optionHighlightTex, new Vector2(50, 200), Color.White);
            }
            else
            {
                aSpriteBatch.Draw(PanelHighLightTex, new Vector2(50 + SelectedBundleX * 100, 50 + SelectedBundleY * 75), Color.White);
            }
            Text_Manager.DrawText("Arrow Keys: Change Level             Space: Select", 30,
                (int)(Global_Info.AccessWindowSize.Y / Global_Info.AccessScreenScale - 40), aSpriteBatch);

            aSpriteBatch.Draw(optionButtonTex, new Vector2(50, 200), Color.White);
            
            for (int i = 0; i < LevelAmountY; i++)
            {
                for (int j = 0; j < LevelAmountX; j++)
                {
                    aSpriteBatch.Draw(NumbersTex, new Rectangle(55 + j * 100, 55 + i * 75, 9, 5), new Rectangle(9 * j + i * 45, 0, 9, 5), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
                    aSpriteBatch.Draw(PanelTex, new Vector2(50 + j * 100, 50 + i * 75), Color.White);

                    if (highScores[j + i * 5] != 1)
                    {                        
                        Text_Manager.DrawText(highScores[j + i * 5].ToString(), 55 + j * 100, 99 + i * 75, aSpriteBatch);
                    }

                    else
                    {
                        aSpriteBatch.Draw(lockTex, new Vector2((55 + j * 100) - 1, (99 + i * 75) - 11), Color.White);
                    }
                }
            }
        }
    }
}
