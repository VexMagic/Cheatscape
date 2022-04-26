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
        static Texture2D Sliders = Global_Info.AccessContentManager.Load<Texture2D>("OptionSliders");
        static Vector2 SliderSize = new Vector2(100, 75);

        static int OptionIndex = 1;
        static int OptionAmount = 3;

        enum SelectedOption { None, Resolution, MusicVolume };
        static SelectedOption selectedOption = SelectedOption.None;

        static Vector2 TempWindowSize;
        static int ScreenSizeIndex = 1;
        static int ScreenSizeAmount = 3;

        public static void Update()
        {
            switch (selectedOption)
            {
                case SelectedOption.None:

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
                    }
                    else if (Input_Manager.KeyPressed(Keys.Back))
                    {
                        Global_Info.AccessCurrentGameState = Global_Info.GameState.LevelSelect;
                    }
                    break;

                case SelectedOption.Resolution:

                    if (Input_Manager.KeyPressed(Keys.Left) && ScreenSizeIndex > 1)
                    {
                        ScreenSizeIndex--;
                    }
                    else if (Input_Manager.KeyPressed(Keys.Right) && ScreenSizeIndex < ScreenSizeAmount)
                    {
                        ScreenSizeIndex++;
                    }
                    else if (Input_Manager.KeyPressed(Keys.Back))
                    {
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
                    
                    if (Input_Manager.KeyPressed(Keys.Right))
                    {

                    }
                    else if (Input_Manager.KeyPressed(Keys.Left))
                    {

                    }
                    else if (Input_Manager.KeyPressed(Keys.Back))
                    {
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
        }

        private static void DrawSlider(int options, int selectionIndex, SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Begin();

            for (int i = 0; i <= options; i++)
            {
                if (i == 0)
                {
                    spriteBatch.Draw(Sliders, new Rectangle((int)position.X, (int)position.Y, (int)SliderSize.X, (int)SliderSize.Y),
                        new Rectangle(100, 0, 100, 75), Color.White, 0, Vector2.Zero, SpriteEffects.FlipVertically, 0);
                }
                else if (i == options)
                {
                    spriteBatch.Draw(Sliders, new Rectangle((int)position.X + 100 * i, (int)position.Y, (int)SliderSize.X, (int)SliderSize.Y),
                        new Rectangle(100, 0, 100, 75), Color.White);
                }
                else
                {
                    spriteBatch.Draw(Sliders, new Rectangle((int)position.X + 100 * i, (int)position.Y, (int)SliderSize.X, (int)SliderSize.Y),
                        new Rectangle(200, 0, 100, 75), Color.White);
                }
            }

            spriteBatch.Draw(Sliders, new Rectangle((int)position.X + 100 * selectionIndex, (int)position.Y, 100, 75),
                new Rectangle(0, 0, 100, 75), Color.White);
        }
    }
}
