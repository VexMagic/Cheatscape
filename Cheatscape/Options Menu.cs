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
        static Texture2D sliderTex;
        static Texture2D highLightTex;
        static Texture2D backgroundTex;
        static Texture2D backButtonTex;
        static Texture2D backHighLightTex;

        static Vector2 sliderSize = new Vector2(32, 24);
        static Vector2 highLightEdgeSize = new Vector2(36, 34);
        static Vector2 highLightMidSize = new Vector2(32, 34);

        static int optionIndex = 1;
        static int optionAmount = 5;
        static Color highLightColor = Color.White;

        enum SelectedOption { none, resolution, ViewControls, MusicVolume, SFXVolume };
        static SelectedOption selectedOption = SelectedOption.none;

        //Screen Size
        static int fullScreenIndex = 0;
        static int fullScreenAmount = 1;
        static bool fullScreenHighLight = false;
        static string fullScreenOnOff = "OFF";

        //View Controls
        static int viewControlsIndex = 1;
        static int viewControlsAmount = 1;
        static bool viewControlsHighLight = false;
        static bool showControls = true;
        static string viewControlsOnOff = "ON";
        public static bool AccessControlView { get => showControls; set => showControls = value; }

        //Music Volume
        static int musicVolumeIndex = 5;
        static int musicVolumeAmount = 10;
        static bool musicVolumeHighLight = false;
        static string musicVolumePercent = "50%";

        //SFX Volume
        static int sFXVolumeIndex = 5;
        static int sFXVolumeAmount = 10;
        static bool sFXVolumeHighLight = false;
        static string sFXVolumePercent = "50%";

        //Back button
        static bool backHighLight = false;

        public static void Load()
        {
            sliderTex = Global_Info.AccessContentManager.Load<Texture2D>("OptionSliders");
            highLightTex = Global_Info.AccessContentManager.Load<Texture2D>("OptionHighLight");
            backgroundTex = Global_Info.AccessContentManager.Load<Texture2D>("OptionsBackground");
            backButtonTex = Global_Info.AccessContentManager.Load<Texture2D>("BackButton");
            backHighLightTex = Global_Info.AccessContentManager.Load<Texture2D>("OptionsButtonHighlight");
        }

        public static void Update()
        {
            switch (selectedOption)
            {
                case SelectedOption.none:

                    if (optionIndex == 1)
                    {
                        fullScreenHighLight = true;

                        viewControlsHighLight = false;
                    }
                    else if (optionIndex == 2)
                    {
                        viewControlsHighLight = true;

                        fullScreenHighLight = false;
                        musicVolumeHighLight = false;
                    }
                    else if (optionIndex == 3)
                    {
                        musicVolumeHighLight = true;

                        viewControlsHighLight = false;
                        sFXVolumeHighLight = false;
                    }
                    else if (optionIndex == 4)
                    {
                        sFXVolumeHighLight = true;

                        musicVolumeHighLight = false;
                        backHighLight = false;
                    }
                    else if (optionIndex == 5)
                    {
                        backHighLight = true;

                        sFXVolumeHighLight = false;
                    }
                    
                    if (Input_Manager.KeyPressed(Keys.Up) && optionIndex > 1)
                    {
                        optionIndex--;
                    }
                    else if (Input_Manager.KeyPressed(Keys.Down) && optionIndex < optionAmount)
                    {
                        optionIndex++;
                    }
                    else if (Input_Manager.KeyPressed(Keys.Space))
                    {
                        if (optionIndex == 1)
                        {
                            selectedOption = SelectedOption.resolution;
                        }
                        else if (optionIndex == 2)
                        {
                            selectedOption = SelectedOption.ViewControls;
                        }
                        else if (optionIndex == 3)
                        {
                            selectedOption = SelectedOption.MusicVolume;
                        }
                        else if (optionIndex == 4)
                        {
                            selectedOption = SelectedOption.SFXVolume;
                        }
                        else if (optionIndex == 5)
                        {
                            Transition.StartTransition(Transition.AccessNextTransitionState);
                        }
                    }

                    if (selectedOption != SelectedOption.none)
                    {
                        highLightColor = Color.Blue;
                    }

                    break;

                case SelectedOption.resolution:

                    if (Input_Manager.KeyPressed(Keys.Left) && fullScreenIndex > 0)
                    {
                        fullScreenIndex--;

                        fullScreenOnOff = "OFF";
                    }
                    else if (Input_Manager.KeyPressed(Keys.Right) && fullScreenIndex < fullScreenAmount)
                    {
                        fullScreenIndex++;

                        fullScreenOnOff = "ON";
                    }
                    else if (Input_Manager.KeyPressed(Keys.Space))
                    {
                        highLightColor = Color.White;
                        
                        selectedOption = SelectedOption.none;

                        if (fullScreenIndex > 0)
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

                    if (Input_Manager.KeyPressed(Keys.Right) && viewControlsIndex < viewControlsAmount)
                    {
                        viewControlsIndex++;
                        viewControlsOnOff = "ON";
                        AccessControlView = true;
                    }
                    else if (Input_Manager.KeyPressed(Keys.Left) && viewControlsIndex > 0)
                    {
                        viewControlsIndex--;
                        viewControlsOnOff = "OFF";
                        AccessControlView = false;
                    }
                    else if (Input_Manager.KeyPressed(Keys.Space))
                    {
                        highLightColor = Color.White;

                        selectedOption = SelectedOption.none;
                    }

                    break;
                case SelectedOption.MusicVolume:
                    
                    if (Input_Manager.KeyPressed(Keys.Right) && musicVolumeIndex < musicVolumeAmount)
                    {
                        musicVolumeIndex++;
                        musicVolumePercent = (musicVolumeIndex * 10).ToString() + "%";
                    }
                    else if (Input_Manager.KeyPressed(Keys.Left) && musicVolumeIndex > 0)
                    {
                        musicVolumeIndex--;
                        musicVolumePercent = (musicVolumeIndex * 10).ToString() + "%";
                    }
                    else if (Input_Manager.KeyPressed(Keys.Space))
                    {
                        highLightColor = Color.White;
                        
                        selectedOption = SelectedOption.none;

                        float newVol = musicVolumeIndex / 10f;
                        MediaPlayer.Volume = newVol;
                    }

                    break;
                case SelectedOption.SFXVolume:

                    if (Input_Manager.KeyPressed(Keys.Right) && sFXVolumeIndex < sFXVolumeAmount)
                    {
                        sFXVolumeIndex++;
                        sFXVolumePercent = (sFXVolumeIndex * 10).ToString() + "%";
                    }
                    else if (Input_Manager.KeyPressed(Keys.Left) && sFXVolumeIndex > 0)
                    {
                        sFXVolumeIndex--;
                        sFXVolumePercent = (sFXVolumeIndex * 10).ToString() + "%";
                    }
                    else if (Input_Manager.KeyPressed(Keys.Space))
                    {
                        highLightColor = Color.White;

                        selectedOption = SelectedOption.none;

                        float newVol = sFXVolumeIndex / 10f;
                        SoundEffect.MasterVolume = newVol;
                    }

                    break;
            }
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(backgroundTex, new Rectangle(0, 0, 
                (int)(Global_Info.windowSize.X / Global_Info.AccessScreenScale), (int)(Global_Info.windowSize.Y / Global_Info.AccessScreenScale)), 
                Color.White);
            if (AccessControlView)
            {
                if (selectedOption == SelectedOption.none)
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
            DrawSlider(fullScreenAmount, fullScreenIndex, 1, spriteBatch, new Vector2(263, 45), fullScreenHighLight);
            Text_Manager.DrawText("Fullscreen " + fullScreenOnOff, 262, 30, spriteBatch);

            //View Controls
            DrawSlider(viewControlsAmount, viewControlsIndex, 1, spriteBatch, new Vector2(263, 95), viewControlsHighLight);
            Text_Manager.DrawText("View Controls " + viewControlsOnOff,259, 80, spriteBatch);

            //Music Volume
            DrawSlider(musicVolumeAmount, musicVolumeIndex, 5, spriteBatch, new Vector2(200, 145), musicVolumeHighLight);
            Text_Manager.DrawText("Music Volume " + musicVolumePercent, 255, 130, spriteBatch);

            //SFX Volume
            DrawSlider(sFXVolumeAmount, sFXVolumeIndex, 5, spriteBatch, new Vector2(200, 195), sFXVolumeHighLight);
            Text_Manager.DrawText("SFX Volume " + sFXVolumePercent, 259, 180, spriteBatch);

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
                    spriteBatch.Draw(sliderTex, new Rectangle((int)position.X, (int)position.Y, (int)sliderSize.X, (int)sliderSize.Y),
                        new Rectangle(100, 0, 100, 75), Color.White, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);

                    if (highLight)
                    {
                        spriteBatch.Draw(highLightTex, new Rectangle((int)position.X - 4, (int)position.Y - 5, (int)highLightEdgeSize.X, (int)highLightEdgeSize.Y),
                            new Rectangle(0, 0, 112, 99), highLightColor);
                    }
                }
                else if (i == size)
                {
                    spriteBatch.Draw(sliderTex, new Rectangle((int)position.X + 32 * i, (int)position.Y, (int)sliderSize.X, (int)sliderSize.Y),
                        new Rectangle(100, 0, 100, 75), Color.White);

                    if (highLight)
                    {
                        spriteBatch.Draw(highLightTex, new Rectangle((int)position.X + 32 * i, (int)position.Y - 5, (int)highLightEdgeSize.X, (int)highLightEdgeSize.Y),
                            new Rectangle(highLightTex.Width - 112, 0, 112, 99), highLightColor);
                    }
                }
                else
                {
                    spriteBatch.Draw(sliderTex, new Rectangle((int)position.X + 32 * i, (int)position.Y, (int)sliderSize.X, (int)sliderSize.Y),
                        new Rectangle(200, 0, 100, 75), Color.White);

                    if (highLight)
                    {
                        spriteBatch.Draw(highLightTex, new Rectangle((int)position.X + 32 * i, (int)position.Y - 5, (int)highLightMidSize.X, (int)highLightMidSize.Y),
                            new Rectangle(100, 0, 100, 99), highLightColor);
                    }
                }
            }

            spriteBatch.Draw(sliderTex, new Rectangle((int)(position.X + 32 * size / options * selectionIndex), (int)position.Y, 32, 24),
                new Rectangle(0, 0, 100, 75), Color.White);
        }
    }
}
