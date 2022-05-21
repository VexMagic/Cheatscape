using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cheatscape
{
    static class Mouse_Controller
    {
        public static MouseState CurrentMS, PreviousMS;
        static Vector2 MousePosition = new Vector2();
        static Rectangle[,] BoardTiles = new Rectangle[8, 8];
        static Rectangle[,] LevelButtons = new Rectangle[5, 2];
        static Rectangle OptionsButton = new Rectangle(50, 200, 32, 32);
        static Rectangle[] ScrollButtons = { new Rectangle(0, 138, 20, 20), new Rectangle(0, 340, 20, 20) };
        static Rectangle[] BannerButtons = { new Rectangle(20, 118, 16, 20), new Rectangle(116, 118, 16, 20) };
        static Rectangle[] SlideButtons = { new Rectangle(113, 148, 31, 64), new Rectangle(456, 148, 31, 64) };
        public static List<Rectangle> RuleBoxes = new List<Rectangle>();
        static Texture2D TileSelect;
        static Vector2 SelectedTile;
        public static Rectangle[] optionRects = { new Rectangle(263, 45, 64, 24), new Rectangle(263, 95, 64, 24), new Rectangle(200, 145, 192, 24), 
            new Rectangle(200, 195, 192, 24), new Rectangle(284, 245, 32, 32) };
        static int SelectedRule = 100;

        public static void Load()
        {
            TileSelect = Global_Info.AccessContentManager.Load<Texture2D>("Tile Select");

            Vector2 tempBoardPos = Game_Board.AccessBoardPosition;
            int tempTileSize = Game_Board.AccessTileSize;

            for (int i = 0; i < BoardTiles.GetLength(0); i++)
            {
                for (int j = 0; j < BoardTiles.GetLength(1); j++)
                {
                    BoardTiles[i, j] = new Rectangle((int)tempBoardPos.X + (i * tempTileSize), 
                        (int)tempBoardPos.Y + (j * tempTileSize), tempTileSize, tempTileSize);
                }
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
            PreviousMS = CurrentMS;
            CurrentMS = Mouse.GetState();
            MousePosition = new Vector2(CurrentMS.X, CurrentMS.Y) / Global_Info.AccessScreenScale;

            switch (Global_Info.AccessCurrentGameState)
            {
                case Global_Info.GameState.LevelSelect:
                    LevelSelectUpdate();
                    break;
                case Global_Info.GameState.PlayingLevel:
                    LevelUpdate();
                    break;
                case Global_Info.GameState.MainMenu:
                    if (CurrentMS.LeftButton == ButtonState.Pressed && PreviousMS.LeftButton == ButtonState.Released)
                        Main_Menu.animating = true;
                    break;
                case Global_Info.GameState.Options:
                    OptionUpdate();
                    break;
            }
        }

        static void LevelSelectUpdate()
        {
            for (int i = 0; i < LevelButtons.GetLength(0); i++)
            {
                for (int j = 0; j < LevelButtons.GetLength(1); j++)
                {
                    if (LevelButtons[i, j].Contains(MousePosition) && CurrentMS.Position != PreviousMS.Position)
                    {
                        Level_Select_Menu.SelectedBundleX = i;
                        Level_Select_Menu.SelectedBundleY = j;
                        Level_Select_Menu.optionHighlight = false;
                    }
                }
            }

            if (OptionsButton.Contains(MousePosition))
            {
                Level_Select_Menu.optionHighlight = true;
            }

            if (CurrentMS.LeftButton == ButtonState.Pressed && PreviousMS.LeftButton == ButtonState.Released)
            {
                if (Level_Select_Menu.optionHighlight && OptionsButton.Contains(MousePosition))
                {
                    Transition.AccessNextTransitionState = Transition.TransitionState.ToLvSelect;

                    Transition.StartTransition(Transition.TransitionState.ToOptions);
                }
                else if (!Level_Select_Menu.optionHighlight && 
                    LevelButtons[Level_Select_Menu.SelectedBundleX, Level_Select_Menu.SelectedBundleY].Contains(MousePosition))
                {
                    Music_Player.PlayMusic();
                    Level_Manager.AccessRating = 1000;
                    Transition.StartTransition(Transition.TransitionState.ToLevel);
                }
            }
        }

        static void LevelUpdate()
        {
            SelectedTile.X = 100;
            SelectedRule = 100;

            Rules_List.AllowedRuleIndexes.Clear();

            for (int i = 0; i < Rules_List.GetList().Length; i++)
            {
                if (Rules_List.AllowedRules.Contains(new Vector2(Rules_List.AccessCurrentRuleList, i)))
                {
                    Rules_List.AllowedRuleIndexes.Add(i);
                }
            }

            Rules_List.AllowedRuleIndexes.Sort();

            for (int i = 0; i < BoardTiles.GetLength(0); i++)
            {
                for (int j = 0; j < BoardTiles.GetLength(1); j++)
                {
                    if (BoardTiles[i, j].Contains(MousePosition))
                        SelectedTile = new Vector2(i, j);
                }
            }

            if (CurrentMS.LeftButton == ButtonState.Pressed && PreviousMS.LeftButton == ButtonState.Released && LevelBool())
            {
                if (Level_Manager.FindingCheat && (ScrollButtons[0].Contains(MousePosition) || ScrollButtons[1].Contains(MousePosition)))
                {
                    if (ScrollButtons[0].Contains(MousePosition))
                        Rules_List.MoveThroughRules(1);
                    else
                        Rules_List.AccessCurrentRule++;
                }
                else if (Level_Manager.FindingCheat && Rules_List.AmountOfUsedLists() > 1 && (BannerButtons[0].Contains(MousePosition) || BannerButtons[1].Contains(MousePosition)))
                {
                    if (BannerButtons[0].Contains(MousePosition))
                    {
                        Rules_List.AccessCurrentRuleList--;
                        if (Rules_List.AccessCurrentRuleList < 0)
                        {
                            Rules_List.AccessCurrentRuleList = Rules_List.AmountOfRuleLists - 1;
                        }
                        Rules_List.AccessCurrentRule = 0;
                    }
                    else
                    {
                        Rules_List.AccessCurrentRuleList++;
                        if (Rules_List.AccessCurrentRuleList >= Rules_List.AmountOfRuleLists)
                        {
                            Rules_List.AccessCurrentRuleList = 0;
                        }
                        Rules_List.AccessCurrentRule = 0;
                    }
                }
                else if (!Level_Manager.FindingCheat && ((SlideButtons[0].Contains(MousePosition) && Level_Manager.AccessCurrentSlide > 1) 
                    || (SlideButtons[1].Contains(MousePosition) && Level_Manager.AccessCurrentSlide < Level_Manager.AccessAllMoves.Count)))
                {
                    if (SlideButtons[0].Contains(MousePosition))
                        Level_Manager.ChangeSlide(false);
                    else
                        Level_Manager.ChangeSlide(true);
                }
                else if (Level_Manager.FindingCheat && SelectedRule != 100)
                {
                    if (SelectedRule == Rules_List.AllowedRuleIndexes.Count)
                    {
                        if (Rules_List.AccessCurrentRule == Rules_List.GetList().Length)
                            Level_Manager.SelectCheat();
                        else
                            Rules_List.AccessCurrentRule = Rules_List.GetList().Length;
                    }
                    else
                    {
                        if (Rules_List.AccessCurrentRule == Rules_List.AllowedRuleIndexes[SelectedRule])
                            Level_Manager.SelectCheat();
                        else
                            Rules_List.AccessCurrentRule = Rules_List.AllowedRuleIndexes[SelectedRule];
                    }
                }
                else if (SelectedTile.X != 100)
                {
                    if (!Level_Manager.FindingCheat)
                    {
                        if (Level_Manager.CurrentLevel != 0 || Level_Manager.CurrentBundle != 0 || Level_Manager.AccessCurrentSlide > 2)
                        {
                            Level_Manager.FindingCheat = true;
                        }
                    }
                }
                else
                    Level_Manager.FindingCheat = false;
            }

            if (Level_Manager.FindingCheat)
            {
                if (CurrentMS.ScrollWheelValue > PreviousMS.ScrollWheelValue)
                {
                    Rules_List.MoveThroughRules(1);
                }
                if (CurrentMS.ScrollWheelValue < PreviousMS.ScrollWheelValue)
                {
                    Rules_List.AccessCurrentRule++;
                }
            }
        }

        public static void OptionUpdate()
        {
            switch (Options_Menu.AccessSelectedOption)
            {
                case Options_Menu.SelectedOption.None:

                    if (optionRects[0].Contains(MousePosition))
                    {
                        Options_Menu.OptionIndex = 1;
                    }
                    else if (optionRects[1].Contains(MousePosition))
                    {
                        Options_Menu.OptionIndex = 2;
                    }
                    else if (optionRects[2].Contains(MousePosition))
                    {
                        Options_Menu.OptionIndex = 3;
                    }
                    else if (optionRects[3].Contains(MousePosition))
                    {
                        Options_Menu.OptionIndex = 4;
                    }
                    else if (optionRects[4].Contains(MousePosition))
                    {
                        Options_Menu.OptionIndex = 5;
                    }

                    if (CurrentMS.LeftButton == ButtonState.Pressed && PreviousMS.LeftButton == ButtonState.Released)
                    {
                        if (Options_Menu.OptionIndex == 1 && optionRects[0].Contains(MousePosition))
                        {
                            Options_Menu.AccessSelectedOption = Options_Menu.SelectedOption.FullScreen;
                        }
                        else if (Options_Menu.OptionIndex == 2 && optionRects[1].Contains(MousePosition))
                        {
                            Options_Menu.AccessSelectedOption = Options_Menu.SelectedOption.ViewControls;
                        }
                        else if (Options_Menu.OptionIndex == 3 && optionRects[2].Contains(MousePosition))
                        {
                            Options_Menu.AccessSelectedOption = Options_Menu.SelectedOption.MusicVolume;
                        }
                        else if (Options_Menu.OptionIndex == 4 && optionRects[3].Contains(MousePosition))
                        {
                            Options_Menu.AccessSelectedOption = Options_Menu.SelectedOption.SFXVolume;
                        }
                        else if (Options_Menu.OptionIndex == 5 && optionRects[4].Contains(MousePosition))
                        {
                            Transition.StartTransition(Transition.AccessNextTransitionState);
                        }
                    }

                    if (Options_Menu.AccessSelectedOption != Options_Menu.SelectedOption.None)
                    {
                        Options_Menu.HighLightColor = Color.Blue;
                    }

                    break;
                case Options_Menu.SelectedOption.FullScreen:

                    if (CurrentMS.LeftButton == ButtonState.Released && PreviousMS.LeftButton == ButtonState.Pressed)
                    {
                        Options_Menu.AccessSelectedOption = Options_Menu.SelectedOption.None;

                        Options_Menu.HighLightColor = Color.White;

                        if (Options_Menu.FullScreenIndex > 0)
                        {
                            Game1.ControlFullScreen(true);
                        }
                        else
                        {
                            Game1.ControlFullScreen(false);
                        }
                    }
                    else if (MousePosition.X - 32 > 263 + Options_Menu.FullScreenIndex * 32 && Options_Menu.FullScreenIndex < Options_Menu.FullScreenAmount)
                    {
                        Options_Menu.FullScreenIndex++;
                    }
                    else if (MousePosition.X < 263 + Options_Menu.FullScreenIndex * 32 && Options_Menu.FullScreenIndex > 0)
                    {
                        Options_Menu.FullScreenIndex--;
                    }

                    break;
                case Options_Menu.SelectedOption.ViewControls:

                    if (CurrentMS.LeftButton == ButtonState.Released && PreviousMS.LeftButton == ButtonState.Pressed)
                    {
                        Options_Menu.AccessSelectedOption = Options_Menu.SelectedOption.None;

                        Options_Menu.HighLightColor = Color.White;
                    }
                    else if (MousePosition.X - 32 > 263 + Options_Menu.ViewControlsIndex * 32 && Options_Menu.ViewControlsIndex < Options_Menu.ViewControlsAmount)
                    {
                        Options_Menu.ViewControlsIndex++;
                    }
                    else if (MousePosition.X < 263 + Options_Menu.ViewControlsIndex * 32 && Options_Menu.ViewControlsIndex > 0)
                    {
                        Options_Menu.ViewControlsIndex--;
                    }

                    break;
                case Options_Menu.SelectedOption.MusicVolume:

                    if (CurrentMS.LeftButton == ButtonState.Released && PreviousMS.LeftButton == ButtonState.Pressed)
                    {
                        Options_Menu.AccessSelectedOption = Options_Menu.SelectedOption.None;

                        Options_Menu.HighLightColor = Color.White;

                        Music_Player.StopMusic();
                    }
                    else if (MousePosition.X - 32 > 200 + Options_Menu.MusicVolumeIndex * 16 && Options_Menu.MusicVolumeIndex < Options_Menu.MusicVolumeAmount)
                    {
                        Options_Menu.MusicVolumeIndex++;
                    }
                    else if (MousePosition.X < 200 + Options_Menu.MusicVolumeIndex * 16 && Options_Menu.MusicVolumeIndex > 0)
                    {
                        Options_Menu.MusicVolumeIndex--;
                    }

                    break;
                case Options_Menu.SelectedOption.SFXVolume:

                    if (CurrentMS.LeftButton == ButtonState.Released && PreviousMS.LeftButton == ButtonState.Pressed)
                    {
                        Options_Menu.AccessSelectedOption = Options_Menu.SelectedOption.None;

                        Options_Menu.HighLightColor = Color.White;
                    }
                    else if (MousePosition.X - 32 > 200 + Options_Menu.SFXVolumeIndex * 16 && Options_Menu.SFXVolumeIndex < Options_Menu.SFXVolumeAmount)
                    {
                        Options_Menu.SFXVolumeIndex++;
                    }
                    else if (MousePosition.X < 200 + Options_Menu.SFXVolumeIndex * 16 && Options_Menu.SFXVolumeIndex > 0)
                    {
                        Options_Menu.SFXVolumeIndex--;
                    }

                    break;
            }
        }

        public static void LevelDraw(SpriteBatch aSpriteBatch)
        {
            if (LevelBool())
            {
                if (SelectedTile.X != 100)
                {
                    Vector2 tempPosition = new Vector2((int)(Game_Board.AccessBoardPosition.X + (SelectedTile.X * Game_Board.AccessTileSize)),
                            (int)(Game_Board.AccessBoardPosition.Y + (SelectedTile.Y * Game_Board.AccessTileSize)));
                    aSpriteBatch.Draw(TileSelect, tempPosition, Color.White);
                }
            }
            if (SelectedRule != 100)
            {
                aSpriteBatch.Draw(TileSelect, RuleBoxes[SelectedRule], new Rectangle(0, 0, 1, 1), Color.White * 0.75f);
            }
        }

        public static bool LevelBool()
        {
            if (Pause_Menu.gameIsPaused || End_Screen.AccessIsEnded || Level_Manager.isOnTransitionScreen || Level_Manager.isOnTransitionScreen)
            {
                return false;
            }

            return true;
        }
    }
}
