using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cheatscape
{
    static class Options_Menu
    {
        static Texture2D SliderTex;
        static Texture2D HighLightTex;
        static Texture2D BackgroundTex;

        static Vector2 SliderSize = new Vector2(32, 24);
        static Vector2 HighLightEdgeSize = new Vector2(36, 34);
        static Vector2 HighLightMidSize = new Vector2(32, 34);

        static int OptionIndexX = 1;
        static int OptionAmountX = 4;
        static int OptionIndexY = 1;
        static int OptionAmountY = 2;
        static Color HighLightColor = Color.White;

        enum SelectedOption { None, Resolution, ViewControls, MusicVolume, SFXVolume };
        static SelectedOption selectedOption = SelectedOption.None;

        //Screen Size
        static int FullScreenIndex = 0;
        static int FullScreenAmount = 1;
        static bool FullScreenHighLight = false;
        static string FullScreenOnOff = "OFF";

        //View Controls
        static int ViewControlsIndex = 1;
        static int ViewControlsAmount = 1;
        static bool ViewControlsHighLight = false;
        static bool showControls = true;
        static string ViewControlsOnOff = "ON";
        public static bool AccessControlView { get => showControls; set => showControls = value; }

        //Music Volume
        static int MusicVolumeIndex = 5;
        static int MusicVolumeAmount = 10;
        static bool MusicVolumeHighLight = false;
        static string MusicVolumePercent = "50%";

        //SFX Volume
        static int SFXVolumeIndex = 5;
        static int SFXVolumeAmount = 10;
        static bool SFXVolumeHighLight = false;
        static string SFXVolumePercent = "50%";

        public static void Load()
        {
            SliderTex = Global_Info.AccessContentManager.Load<Texture2D>("OptionSliders");
            HighLightTex = Global_Info.AccessContentManager.Load<Texture2D>("OptionHighLight");
            BackgroundTex = Global_Info.AccessContentManager.Load<Texture2D>("OptionsBackground");
        }

        public static void Update()
        {
            switch (selectedOption)
            {
                case SelectedOption.None:

                    if (OptionIndexX == 1 && OptionIndexY == 1)
                    {
                        FullScreenHighLight = true;

                        ViewControlsHighLight = false;
                    }
                    else if (OptionIndexX == 2 && OptionIndexY == 1)
                    {
                        ViewControlsHighLight = true;

                        FullScreenHighLight = false;
                        MusicVolumeHighLight = false;
                    }
                    else if (OptionIndexX == 3 && OptionIndexY == 1)
                    {
                        MusicVolumeHighLight = true;

                        ViewControlsHighLight = false;
                        SFXVolumeHighLight = false;
                    }
                    else if (OptionIndexX == 4 && OptionIndexY == 1)
                    {
                        SFXVolumeHighLight = true;

                        MusicVolumeHighLight = false;
                    }
                    
                    if (Input_Manager.KeyPressed(Keys.Up) && OptionIndexX > 1)
                    {
                        OptionIndexX--;
                    }
                    else if (Input_Manager.KeyPressed(Keys.Down) && OptionIndexX < OptionAmountX)
                    {
                        OptionIndexX++;
                    }
                    else if (Input_Manager.KeyPressed(Keys.Left) && OptionIndexY > 1)
                    {
                        OptionIndexY--;
                    }
                    else if (Input_Manager.KeyPressed(Keys.Right) && OptionIndexY < OptionAmountY)
                    {
                        OptionIndexY++;
                    }
                    else if (Input_Manager.KeyPressed(Keys.Space))
                    {
                        if (OptionIndexX == 1 && OptionIndexY == 1)
                        {
                            selectedOption = SelectedOption.Resolution;
                        }
                        else if (OptionIndexX == 2 && OptionIndexY == 1)
                        {
                            selectedOption = SelectedOption.ViewControls;
                        }
                        else if (OptionIndexX == 3 && OptionIndexY == 1)
                        {
                            selectedOption = SelectedOption.MusicVolume;
                        }
                        else if (OptionIndexX == 4 && OptionIndexY == 1)
                        {
                            selectedOption = SelectedOption.SFXVolume;
                        }
                    }
                    else if (Input_Manager.KeyPressed(Keys.Back))
                    {
                        Transition.StartTransition(Transition.TransitionState.ToLvSelect);
                    }

                    if (selectedOption != SelectedOption.None)
                    {
                        HighLightColor = Color.Blue;
                    }

                    break;

                case SelectedOption.Resolution:

                    if (Input_Manager.KeyPressed(Keys.Left) && FullScreenIndex > 0)
                    {
                        FullScreenIndex--;

                        FullScreenOnOff = "OFF";
                    }
                    else if (Input_Manager.KeyPressed(Keys.Right) && FullScreenIndex < FullScreenAmount)
                    {
                        FullScreenIndex++;

                        FullScreenOnOff = "ON";
                    }
                    else if (Input_Manager.KeyPressed(Keys.Space))
                    {
                        HighLightColor = Color.White;
                        
                        selectedOption = SelectedOption.None;

                        if (FullScreenIndex > 0)
                        {
                            //Fullscreen av
                            Game1.ControlFullScreen(true);
                        }
                        else
                        {
                            //Fullscreen på
                            Game1.ControlFullScreen(false);
                        }
                    }
                    
                    break;
                case SelectedOption.ViewControls:

                    if (Input_Manager.KeyPressed(Keys.Right) && ViewControlsIndex < ViewControlsAmount)
                    {
                        ViewControlsIndex++;
                        ViewControlsOnOff = "ON";
                        AccessControlView = true;
                    }
                    else if (Input_Manager.KeyPressed(Keys.Left) && ViewControlsIndex > 0)
                    {
                        ViewControlsIndex--;
                        ViewControlsOnOff = "OFF";
                        AccessControlView = false;
                    }
                    else if (Input_Manager.KeyPressed(Keys.Space))
                    {
                        HighLightColor = Color.White;

                        selectedOption = SelectedOption.None;
                    }

                    break;
                case SelectedOption.MusicVolume:
                    
                    if (Input_Manager.KeyPressed(Keys.Right) && MusicVolumeIndex < MusicVolumeAmount)
                    {
                        MusicVolumeIndex++;
                        MusicVolumePercent = (MusicVolumeIndex * 10).ToString() + "%";
                    }
                    else if (Input_Manager.KeyPressed(Keys.Left) && MusicVolumeIndex > 0)
                    {
                        MusicVolumeIndex--;
                        MusicVolumePercent = (MusicVolumeIndex * 10).ToString() + "%";
                    }
                    else if (Input_Manager.KeyPressed(Keys.Space))
                    {
                        HighLightColor = Color.White;
                        
                        selectedOption = SelectedOption.None;
                    }

                    break;
                case SelectedOption.SFXVolume:

                    if (Input_Manager.KeyPressed(Keys.Right) && SFXVolumeIndex < SFXVolumeAmount)
                    {
                        SFXVolumeIndex++;
                        SFXVolumePercent = (SFXVolumeIndex * 10).ToString() + "%";
                    }
                    else if (Input_Manager.KeyPressed(Keys.Left) && SFXVolumeIndex > 0)
                    {
                        SFXVolumeIndex--;
                        SFXVolumePercent = (SFXVolumeIndex * 10).ToString() + "%";
                    }
                    else if (Input_Manager.KeyPressed(Keys.Space))
                    {
                        HighLightColor = Color.White;

                        selectedOption = SelectedOption.None;
                    }

                    break;
            }
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(BackgroundTex, new Rectangle(0, 0, 
                (int)(Global_Info.WindowSize.X / Global_Info.AccessScreenScale), (int)(Global_Info.WindowSize.Y / Global_Info.AccessScreenScale)), 
                Color.White);
            
            if (selectedOption == SelectedOption.None)
            {
                Text_Manager.DrawText("Space: Select          Back: Return to Menu", 30, 
                    (int)(Global_Info.AccessWindowSize.Y / Global_Info.AccessScreenScale - 40), spriteBatch);
            }
            else
            {
                Text_Manager.DrawText("Left/Right: Adjust          Space: Confirm", 30, (int)(Global_Info.AccessWindowSize.Y / Global_Info.AccessScreenScale - 40), 
                    spriteBatch);
            }

            //Fullscreen
            DrawSlider(FullScreenAmount, FullScreenIndex, 1, spriteBatch, new Vector2(50, 45), FullScreenHighLight);
            Text_Manager.DrawText("Fullscreen " + FullScreenOnOff, 50, 30, spriteBatch);

            //View Controls
            DrawSlider(ViewControlsAmount, ViewControlsIndex, 1, spriteBatch, new Vector2(50, 95), ViewControlsHighLight);
            Text_Manager.DrawText("View Controls " + ViewControlsOnOff, 50, 80, spriteBatch);

            //Music Volume
            DrawSlider(MusicVolumeAmount, MusicVolumeIndex, 5, spriteBatch, new Vector2(50, 145), MusicVolumeHighLight);
            Text_Manager.DrawText("Music Volume " + MusicVolumePercent, 50, 130, spriteBatch);

            //SFX Volume
            DrawSlider(SFXVolumeAmount, SFXVolumeIndex, 5, spriteBatch, new Vector2(50, 195), SFXVolumeHighLight);
            Text_Manager.DrawText("SFX Volume " + SFXVolumePercent, 50, 180, spriteBatch);
        }

        private static void DrawSlider(int options, int selectionIndex, int size, SpriteBatch spriteBatch, Vector2 position, bool highLight)
        {
            for (int i = 0; i <= size; i++)
            {
                if (i == 0)
                {
                    spriteBatch.Draw(SliderTex, new Rectangle((int)position.X, (int)position.Y, (int)SliderSize.X, (int)SliderSize.Y),
                        new Rectangle(100, 0, 100, 75), Color.White, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);

                    if (highLight)
                    {
                        spriteBatch.Draw(HighLightTex, new Rectangle((int)position.X - 4, (int)position.Y - 5, (int)HighLightEdgeSize.X, (int)HighLightEdgeSize.Y),
                            new Rectangle(0, 0, 112, 99), HighLightColor);
                    }
                }
                else if (i == size)
                {
                    spriteBatch.Draw(SliderTex, new Rectangle((int)position.X + 32 * i, (int)position.Y, (int)SliderSize.X, (int)SliderSize.Y),
                        new Rectangle(100, 0, 100, 75), Color.White);

                    if (highLight)
                    {
                        spriteBatch.Draw(HighLightTex, new Rectangle((int)position.X + 32 * i, (int)position.Y - 5, (int)HighLightEdgeSize.X, (int)HighLightEdgeSize.Y),
                            new Rectangle(HighLightTex.Width - 112, 0, 112, 99), HighLightColor);
                    }
                }
                else
                {
                    spriteBatch.Draw(SliderTex, new Rectangle((int)position.X + 32 * i, (int)position.Y, (int)SliderSize.X, (int)SliderSize.Y),
                        new Rectangle(200, 0, 100, 75), Color.White);

                    if (highLight)
                    {
                        spriteBatch.Draw(HighLightTex, new Rectangle((int)position.X + 32 * i, (int)position.Y - 5, (int)HighLightMidSize.X, (int)HighLightMidSize.Y),
                            new Rectangle(100, 0, 100, 99), HighLightColor);
                    }
                }
            }

            spriteBatch.Draw(SliderTex, new Rectangle((int)(position.X + 32 * size / options * selectionIndex), (int)position.Y, 32, 24),
                new Rectangle(0, 0, 100, 75), Color.White);
        }
    }
}
