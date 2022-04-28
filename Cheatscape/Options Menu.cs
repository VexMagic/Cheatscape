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
        static Texture2D SliderTex = Global_Info.AccessContentManager.Load<Texture2D>("OptionSliders");
        static Texture2D HighLightTex = Global_Info.AccessContentManager.Load<Texture2D>("OptionHighLight");
        static Vector2 SliderSize = new Vector2(32, 24);
        static Vector2 HighLightEdgeSize = new Vector2(36, 34);
        static Vector2 HighLightMidSize = new Vector2(32, 34);

        static int OptionIndex = 1;
        static int OptionAmount = 3;
        static Color HighLightColor = Color.White;

        enum SelectedOption { None, Resolution, MusicVolume, ViewControls };
        static SelectedOption selectedOption = SelectedOption.None;

        static Vector2 TempWindowSize;
        static int ScreenSizeIndex = 1;
        static int ScreenSizeAmount = 3;
        static bool ScreenSizeHighLight = false;

        static int MusicVolumeIndex = 1;
        static int MusicVolumeAmount = 6;
        static bool MusicVolumeHighLight = false;

        static int ViewControlsIndex = 1;
        static int ViewControlsAmount = 1;
        static bool ViewControlsHighLight = false;

        public static void Update()
        {
            switch (selectedOption)
            {
                case SelectedOption.None:

                    if (OptionIndex == 1)
                    {
                        ScreenSizeHighLight = true;

                        MusicVolumeHighLight = false;
                    }
                    else if (OptionIndex == 2)
                    {
                        MusicVolumeHighLight = true;

                        ScreenSizeHighLight = false;
                        ViewControlsHighLight = false;
                    }
                    else if (OptionIndex == 3)
                    {
                        ViewControlsHighLight = true;

                        MusicVolumeHighLight = false;
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
                            selectedOption = SelectedOption.MusicVolume;
                        }
                        else if (OptionIndex == 3)
                        {
                            selectedOption = SelectedOption.ViewControls;
                        }
                    }
                    else if (Input_Manager.KeyPressed(Keys.Back))
                    {
                        Global_Info.AccessCurrentGameState = Global_Info.GameState.LevelSelect;
                    }

                    if (selectedOption != SelectedOption.None)
                    {
                        HighLightColor = Color.Blue;
                    }

                    break;

                case SelectedOption.Resolution:

                    if (Input_Manager.KeyPressed(Keys.Left) && ScreenSizeIndex > 0)
                    {
                        ScreenSizeIndex--;
                    }
                    else if (Input_Manager.KeyPressed(Keys.Right) && ScreenSizeIndex < ScreenSizeAmount)
                    {
                        ScreenSizeIndex++;
                    }
                    else if (Input_Manager.KeyPressed(Keys.Back))
                    {
                        HighLightColor = Color.White;
                        
                        selectedOption = SelectedOption.None;

                        if (ScreenSizeIndex == 1)
                        {
                            
                        }
                        else if (ScreenSizeIndex == 2)
                        {
                            TempWindowSize = new Vector2(480 * Global_Info.AccessScreenScale, 270 * Global_Info.AccessScreenScale);
                        }
                        else if (ScreenSizeIndex == 3)
                        {

                        }

                        Global_Info.AccessWindowSize = TempWindowSize;
                    }
                    
                    break;
                case SelectedOption.MusicVolume:
                    
                    if (Input_Manager.KeyPressed(Keys.Right) && MusicVolumeIndex < MusicVolumeAmount)
                    {
                        MusicVolumeIndex++;
                    }
                    else if (Input_Manager.KeyPressed(Keys.Left) && MusicVolumeIndex > 0)
                    {
                        MusicVolumeIndex--;
                    }
                    else if (Input_Manager.KeyPressed(Keys.Back))
                    {
                        HighLightColor = Color.White;
                        
                        selectedOption = SelectedOption.None;
                    }

                    break;
                case SelectedOption.ViewControls:

                    if (Input_Manager.KeyPressed(Keys.Right) && ViewControlsIndex < ViewControlsAmount)
                    {
                        ViewControlsIndex++;
                    }
                    else if (Input_Manager.KeyPressed(Keys.Left) && ViewControlsIndex > 0)
                    {
                        ViewControlsIndex--;
                    }
                    else if (Input_Manager.KeyPressed(Keys.Back))
                    {
                        HighLightColor = Color.White;

                        selectedOption = SelectedOption.None;
                    }

                    break;
            }
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            
            
            switch (selectedOption)
            {
                
                case SelectedOption.None:

                    break;
                case SelectedOption.Resolution:

                    break;
                case SelectedOption.MusicVolume:

                    break;
            }

            DrawSlider(ScreenSizeAmount, ScreenSizeIndex, 3, spriteBatch, new Vector2(100, 100), ScreenSizeHighLight); //Screen Size

            DrawSlider(MusicVolumeAmount, MusicVolumeIndex, 3, spriteBatch, new Vector2(100, 150), MusicVolumeHighLight); // Music Volume

            DrawSlider(ViewControlsAmount, ViewControlsIndex, 1, spriteBatch, new Vector2(100, 200), ViewControlsHighLight); //View Controls
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

            spriteBatch.Draw(SliderTex, new Rectangle((int)position.X + 32 * size / options * selectionIndex, (int)position.Y, 32, 24),
                new Rectangle(0, 0, 100, 75), Color.White);
        }
    }
}
