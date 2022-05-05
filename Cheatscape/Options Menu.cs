using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Cheatscape
{
    static class Options_Menu
    {
        static Texture2D sliderTex;
        static Texture2D highLightTex;
        static Vector2 sliderSize = new Vector2(32, 24);
        static Vector2 highLightEdgeSize = new Vector2(36, 34);
        static Vector2 highLightMidSize = new Vector2(32, 34);

        static int optionIndexX = 1;
        static int optionAmountX = 3;
        static int optionIndexY = 1;
        static int optionAmountY = 2;
        static Color highLightColor = Color.White;

        enum SelectedOption { none, resolution, musicVolume, viewControls };
        static SelectedOption selectedOption = SelectedOption.none;

        //Screen Size
        static int screenSizeIndex = 1;
        static int screenSizeAmount = 3;
        static bool screenSizeHighLight = false;
        static string screenSizeText = " ";

        //Music Volume
        static int musicVolumeIndex = 1;
        static int musicVolumeAmount = 6;
        static bool musicVolumeHighLight = false;
        static string musicVolumeText = " ";

        //View Controls
        static int viewControlsIndex = 1;
        static int viewControlsAmount = 1;
        static bool viewControlsHighLight = false;
        static string viewControlsOnOff = "ON";

        public static void Load()
        {
            sliderTex = Global_Info.AccessContentManager.Load<Texture2D>("OptionSliders");
            highLightTex = Global_Info.AccessContentManager.Load<Texture2D>("OptionHighLight");
        }

        public static void Update()
        {
            switch (selectedOption)
            {
                case SelectedOption.none:

                    if (optionIndexX == 1 && optionIndexY == 1)
                    {
                        screenSizeHighLight = true;

                        musicVolumeHighLight = false;
                    }
                    else if (optionIndexX == 2 && optionIndexY == 1)
                    {
                        musicVolumeHighLight = true;

                        screenSizeHighLight = false;
                        viewControlsHighLight = false;
                    }
                    else if (optionIndexX == 3 && optionIndexY == 1)
                    {
                        viewControlsHighLight = true;

                        musicVolumeHighLight = false;
                    }

                    if (Input_Manager.KeyPressed(Keys.Up) && optionIndexX > 1)
                    {
                        optionIndexX--;
                    }
                    else if (Input_Manager.KeyPressed(Keys.Down) && optionIndexX < optionAmountX)
                    {
                        optionIndexX++;
                    }
                    else if (Input_Manager.KeyPressed(Keys.Left) && optionIndexY > 1)
                    {
                        optionIndexY--;
                    }
                    else if (Input_Manager.KeyPressed(Keys.Right) && optionIndexY < optionAmountY)
                    {
                        optionIndexY++;
                    }
                    else if (Input_Manager.KeyPressed(Keys.Space))
                    {
                        if (optionIndexX == 1 && optionIndexY == 1)
                        {
                            selectedOption = SelectedOption.resolution;
                        }
                        else if (optionIndexX == 2 && optionIndexY == 1)
                        {
                            selectedOption = SelectedOption.musicVolume;
                        }
                        else if (optionIndexX == 3 && optionIndexY == 1)
                        {
                            selectedOption = SelectedOption.viewControls;
                        }
                    }
                    else if (Input_Manager.KeyPressed(Keys.Back))
                    {
                        Global_Info.AccessCurrentGameState = Global_Info.GameState.levelSelect;
                    }

                    if (selectedOption != SelectedOption.none)
                    {
                        highLightColor = Color.Blue;
                    }

                    break;

                case SelectedOption.resolution:

                    if (Input_Manager.KeyPressed(Keys.Left) && screenSizeIndex > 0)
                    {
                        screenSizeIndex--;
                    }
                    else if (Input_Manager.KeyPressed(Keys.Right) && screenSizeIndex < screenSizeAmount)
                    {
                        screenSizeIndex++;
                    }
                    else if (Input_Manager.KeyPressed(Keys.Back))
                    {
                        highLightColor = Color.White;

                        selectedOption = SelectedOption.none;

                        if (screenSizeIndex == 1)
                        {

                        }
                        else if (screenSizeIndex == 2)
                        {
                            Global_Info.AccessWindowSize = new Vector2(480 * Global_Info.AccessScreenScale, 270 * Global_Info.AccessScreenScale);
                        }
                        else if (screenSizeIndex == 3)
                        {

                        }
                    }

                    break;
                case SelectedOption.musicVolume:

                    if (Input_Manager.KeyPressed(Keys.Right) && musicVolumeIndex < musicVolumeAmount)
                    {
                        musicVolumeIndex++;
                    }
                    else if (Input_Manager.KeyPressed(Keys.Left) && musicVolumeIndex > 0)
                    {
                        musicVolumeIndex--;
                    }
                    else if (Input_Manager.KeyPressed(Keys.Back))
                    {
                        highLightColor = Color.White;

                        selectedOption = SelectedOption.none;
                    }

                    break;
                case SelectedOption.viewControls:

                    if (Input_Manager.KeyPressed(Keys.Right) && viewControlsIndex < viewControlsAmount)
                    {
                        viewControlsIndex++;
                        viewControlsOnOff = "ON";
                    }
                    else if (Input_Manager.KeyPressed(Keys.Left) && viewControlsIndex > 0)
                    {
                        viewControlsIndex--;
                        viewControlsOnOff = "OFF";
                    }
                    else if (Input_Manager.KeyPressed(Keys.Back))
                    {
                        highLightColor = Color.White;

                        selectedOption = SelectedOption.none;
                    }

                    break;
            }
        }

        public static void Draw(SpriteBatch spriteBatch)
        {


            //Screen Size
            DrawSlider(screenSizeAmount, screenSizeIndex, 3, spriteBatch, new Vector2(50, 35), screenSizeHighLight);
            Text_Manager.DrawText("Screen Size " + screenSizeText, 50, 20, spriteBatch);

            // Music Volume
            DrawSlider(musicVolumeAmount, musicVolumeIndex, 3, spriteBatch, new Vector2(50, 85), musicVolumeHighLight);
            Text_Manager.DrawText("Music Volume " + musicVolumeText, 50, 70, spriteBatch);

            //View Controls
            DrawSlider(viewControlsAmount, viewControlsIndex, 1, spriteBatch, new Vector2(50, 135), viewControlsHighLight);
            Text_Manager.DrawText("View Controls " + viewControlsOnOff, 50, 120, spriteBatch);
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

            spriteBatch.Draw(sliderTex, new Rectangle((int)position.X + 32 * size / options * selectionIndex, (int)position.Y, 32, 24),
                new Rectangle(0, 0, 100, 75), Color.White);
        }
    }
}
