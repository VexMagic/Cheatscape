using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
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
        static Texture2D backButtonTex;
        static Texture2D backHighLightTex;

        static Vector2 SliderSize = new Vector2(32, 24);
        static Vector2 HighLightEdgeSize = new Vector2(36, 34);
        static Vector2 HighLightMidSize = new Vector2(32, 34);

        static int OptionIndex = 1;
        static int OptionAmount = 5;
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

        //Back button
        static bool backHighLight = false;

        public static void Load()
        {
            SliderTex = Global_Info.AccessContentManager.Load<Texture2D>("OptionSliders");
            HighLightTex = Global_Info.AccessContentManager.Load<Texture2D>("OptionHighLight");
            BackgroundTex = Global_Info.AccessContentManager.Load<Texture2D>("OptionsBackground");
            backButtonTex = Global_Info.AccessContentManager.Load<Texture2D>("BackButton");
            backHighLightTex = Global_Info.AccessContentManager.Load<Texture2D>("OptionsButtonHighlight");
        }

        public static void Update()
        {
            switch (selectedOption)
            {
                case SelectedOption.None:

                    if (OptionIndex == 1)
                    {
                        FullScreenHighLight = true;

                        ViewControlsHighLight = false;
                    }
                    else if (OptionIndex == 2)
                    {
                        ViewControlsHighLight = true;

                        FullScreenHighLight = false;
                        MusicVolumeHighLight = false;
                    }
                    else if (OptionIndex == 3)
                    {
                        MusicVolumeHighLight = true;

                        ViewControlsHighLight = false;
                        SFXVolumeHighLight = false;
                    }
                    else if (OptionIndex == 4)
                    {
                        SFXVolumeHighLight = true;

                        MusicVolumeHighLight = false;
                        backHighLight = false;
                    }
                    else if (OptionIndex == 5)
                    {
                        backHighLight = true;

                        SFXVolumeHighLight = false;
                    }
                    
                    if (Input_Manager.KeyPressed(Keys.Up) && OptionIndex > 1)
                    {
                        OptionIndex--;
                    }
                    else if (Input_Manager.KeyPressed(Keys.Down) && OptionIndex < OptionAmount)
                    {
                        OptionIndex++;
                    }
                    else if (Input_Manager.KeyPressed(Keys.Space))
                    {
                        if (OptionIndex == 1)
                        {
                            selectedOption = SelectedOption.Resolution;
                        }
                        else if (OptionIndex == 2)
                        {
                            selectedOption = SelectedOption.ViewControls;
                        }
                        else if (OptionIndex == 3)
                        {
                            selectedOption = SelectedOption.MusicVolume;
                        }
                        else if (OptionIndex == 4)
                        {
                            selectedOption = SelectedOption.SFXVolume;
                        }
                        else if (OptionIndex == 5)
                        {
                            Transition.StartTransition(Transition.AccessNextTransitionState);
                        }
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
                            Game1.ControlFullScreen(true);
                        }
                        else
                        {
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

                        float newVol = MusicVolumeIndex / 10f;
                        MediaPlayer.Volume = newVol;
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

                        float newVol = SFXVolumeIndex / 10f;
                        SoundEffect.MasterVolume = newVol;
                    }

                    break;
            }
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(BackgroundTex, new Rectangle(0, 0, 
                (int)(Global_Info.WindowSize.X / Global_Info.AccessScreenScale), (int)(Global_Info.WindowSize.Y / Global_Info.AccessScreenScale)), 
                Color.White);
            if (AccessControlView)
            {
                if (selectedOption == SelectedOption.None)
                {
                    Text_Manager.DrawText("Up/Down: Scroll             Space: Select", 30,
                        (int)(Global_Info.AccessWindowSize.Y / Global_Info.AccessScreenScale - 40), spriteBatch);
                }
                else
                {
                    Text_Manager.DrawText("Left/Right: Adjust          Space: Confirm", 30, (int)(Global_Info.AccessWindowSize.Y / Global_Info.AccessScreenScale - 40),
                        spriteBatch);
                }
            }

            //Fullscreen
            DrawSlider(FullScreenAmount, FullScreenIndex, 1, spriteBatch, new Vector2(263, 45), FullScreenHighLight);
            Text_Manager.DrawText("Fullscreen " + FullScreenOnOff, 262, 30, spriteBatch);

            //View Controls
            DrawSlider(ViewControlsAmount, ViewControlsIndex, 1, spriteBatch, new Vector2(263, 95), ViewControlsHighLight);
            Text_Manager.DrawText("View Controls " + ViewControlsOnOff,259, 80, spriteBatch);

            //Music Volume
            DrawSlider(MusicVolumeAmount, MusicVolumeIndex, 5, spriteBatch, new Vector2(200, 145), MusicVolumeHighLight);
            Text_Manager.DrawText("Music Volume " + MusicVolumePercent, 255, 130, spriteBatch);

            //SFX Volume
            DrawSlider(SFXVolumeAmount, SFXVolumeIndex, 5, spriteBatch, new Vector2(200, 195), SFXVolumeHighLight);
            Text_Manager.DrawText("SFX Volume " + SFXVolumePercent, 259, 180, spriteBatch);

            //Back button
            if (backHighLight)
            {
                spriteBatch.Draw(backHighLightTex, new Vector2(284, 245), Color.White);
            }
            spriteBatch.Draw(backButtonTex, new Vector2(284, 245), Color.White);
            Text_Manager.DrawText("Return", 284, 230, spriteBatch);
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
