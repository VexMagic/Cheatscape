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
        static Texture2D Panel;
        static Texture2D Numbers;

        static int OptionIndex = 1;
        static int OptionAmount = 3;

        enum SelectedOption { None, Resolution, Music };
        static SelectedOption selectedOption = SelectedOption.None;

        public static void Load()
        {

        }

        public static void Update()
        {
            switch (selectedOption)
            {
                case SelectedOption.None:

                    if (Input_Manager.KeyPressed(Keys.Down) && OptionIndex > 1)
                    {
                        OptionIndex--;
                    }
                    else if (Input_Manager.KeyPressed(Keys.Up) && OptionIndex < OptionAmount)
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
                            selectedOption = SelectedOption.Music;
                        }
                    }
                    else if (Input_Manager.KeyPressed(Keys.Back))
                    {
                        Global_Info.AccessCurrentGameState = Global_Info.GameState.LevelSelect;
                    }
                    break;

                case SelectedOption.Resolution:

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
                case SelectedOption.Music:
                    
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
                case SelectedOption.Music:

                    break;
            }
        }
    }
}
