using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Cheatscape
{
    static class Mouse_Controller
    {
        public static MouseState currentMS, previousMS;
        static Vector2 mousePosition = new Vector2();
        static Rectangle[,] boardTiles = new Rectangle[8, 8];
        static Rectangle[,] levelButtons = new Rectangle[5, 2];
        static Rectangle optionsButton = new Rectangle(50, 200, 32, 32);
        static Rectangle[] scrollButtons = { new Rectangle(0, 138, 20, 20), new Rectangle(0, 340, 20, 20) };
        static Rectangle[] bannerButtons = { new Rectangle(20, 118, 16, 20), new Rectangle(116, 118, 16, 20) };
        static Rectangle[] slideButtons = { new Rectangle(113, 148, 31, 64), new Rectangle(456, 148, 31, 64) };
        public static List<Rectangle> ruleBoxes = new List<Rectangle>();
        static Texture2D tileSelect;
        static Vector2 selectedTile;
        public static Rectangle[] optionRects = { new Rectangle(263, 45, 64, 24), new Rectangle(263, 95, 64, 24), new Rectangle(200, 145, 192, 24),
            new Rectangle(200, 195, 192, 24), new Rectangle(284, 245, 32, 32) };
        static int selectedRule = 100;

        public static void Load()
        {
            tileSelect = Global_Info.AccessContentManager.Load<Texture2D>("Tile Select");

            Vector2 tempBoardPos = Game_Board.AccessBoardPosition;
            int tempTileSize = Game_Board.AccessTileSize;

            for (int i = 0; i < boardTiles.GetLength(0); i++)
            {
                for (int j = 0; j < boardTiles.GetLength(1); j++)
                {
                    boardTiles[i, j] = new Rectangle((int)tempBoardPos.X + (i * tempTileSize),
                        (int)tempBoardPos.Y + (j * tempTileSize), tempTileSize, tempTileSize);
                }
            }

            for (int i = 0; i < levelButtons.GetLength(0); i++)
            {
                for (int j = 0; j < levelButtons.GetLength(1); j++)
                {
                    levelButtons[i, j] = new Rectangle(50 + i * 100, 50 + j * 75, 96, 64);
                }
            }
        }

        public static void Update()
        {
            previousMS = currentMS;
            currentMS = Mouse.GetState();
            mousePosition = new Vector2(currentMS.X, currentMS.Y) / Global_Info.AccessScreenScale;

            switch (Global_Info.AccessCurrentGameState)
            {
                case Global_Info.GameState.levelSelect:
                    LevelSelectUpdate();
                    break;
                case Global_Info.GameState.playingLevel:
                    LevelUpdate();
                    break;
                case Global_Info.GameState.mainMenu:
                    if (currentMS.LeftButton == ButtonState.Pressed && previousMS.LeftButton == ButtonState.Released)
                        Main_Menu.animating = true;
                    break;
                case Global_Info.GameState.options:
                    OptionUpdate();
                    break;
            }
        }

        static void LevelSelectUpdate()
        {
            for (int i = 0; i < levelButtons.GetLength(0); i++)
            {
                for (int j = 0; j < levelButtons.GetLength(1); j++)
                {
                    if (levelButtons[i, j].Contains(mousePosition) && currentMS.Position != previousMS.Position)
                    {
                        Level_Select_Menu.selectedBundleX = i;
                        Level_Select_Menu.selectedBundleY = j;
                        Level_Select_Menu.optionHighlight = false;
                    }
                }
            }

            if (optionsButton.Contains(mousePosition))
            {
                Level_Select_Menu.optionHighlight = true;
            }

            if (currentMS.LeftButton == ButtonState.Pressed && previousMS.LeftButton == ButtonState.Released)
            {
                if (Level_Select_Menu.optionHighlight)
                {
                    Transition_Effect.AccessNextTransitionState = Transition_Effect.TransitionState.toLvSelect;

                    Transition_Effect.StartTransition(Transition_Effect.TransitionState.toOptions);
                }
                else
                {
                    Music_Player.PlayMusic();
                    Level_Manager.AccessRating = 1000;
                    Transition_Effect.StartTransition(Transition_Effect.TransitionState.toLevel);
                }
            }
        }

        static void LevelUpdate()
        {
            selectedTile.X = 100;
            selectedRule = 100;

            Rules_List.allowedRuleIndexes.Clear();

            for (int i = 0; i < Rules_List.GetList().Length; i++)
            {
                if (Rules_List.allowedRules.Contains(new Vector2(Rules_List.AccessCurrentRuleList, i)))
                {
                    Rules_List.allowedRuleIndexes.Add(i);
                }
            }

            Rules_List.allowedRuleIndexes.Sort();

            for (int i = 0; i < boardTiles.GetLength(0); i++)
            {
                for (int j = 0; j < boardTiles.GetLength(1); j++)
                {
                    if (boardTiles[i, j].Contains(mousePosition))
                        selectedTile = new Vector2(i, j);
                }
            }

            for (int i = 0; i < ruleBoxes.Count; i++)
            {
                if (ruleBoxes[i].Contains(mousePosition))
                {
                    selectedRule = i;
                }
            }

            if (currentMS.LeftButton == ButtonState.Pressed && previousMS.LeftButton == ButtonState.Released)
            {
                if (Level_Manager.findingCheat && (scrollButtons[0].Contains(mousePosition) || scrollButtons[1].Contains(mousePosition)))
                {
                    if (scrollButtons[0].Contains(mousePosition))
                        Rules_List.MoveThroughRules(1);
                    else
                        Rules_List.AccessCurrentRule++;
                }
                else if (Level_Manager.findingCheat && Rules_List.AmountOfUsedLists() > 1 && (bannerButtons[0].Contains(mousePosition) || bannerButtons[1].Contains(mousePosition)))
                {
                    if (bannerButtons[0].Contains(mousePosition))
                    {
                        Rules_List.AccessCurrentRuleList--;
                        if (Rules_List.AccessCurrentRuleList < 0)
                        {
                            Rules_List.AccessCurrentRuleList = Rules_List.amountOfRuleLists - 1;
                        }
                        Rules_List.AccessCurrentRule = 0;
                    }
                    else
                    {
                        Rules_List.AccessCurrentRuleList++;
                        if (Rules_List.AccessCurrentRuleList >= Rules_List.amountOfRuleLists)
                        {
                            Rules_List.AccessCurrentRuleList = 0;
                        }
                        Rules_List.AccessCurrentRule = 0;
                    }
                }
                else if (!Level_Manager.findingCheat && ((slideButtons[0].Contains(mousePosition) && Level_Manager.AccessCurrentSlide > 1)
                    || (slideButtons[1].Contains(mousePosition) && Level_Manager.AccessCurrentSlide < Level_Manager.AccessAllMoves.Count)))
                {
                    if (slideButtons[0].Contains(mousePosition))
                        Level_Manager.ChangeSlide(false);
                    else
                        Level_Manager.ChangeSlide(true);
                }
                else if (Level_Manager.findingCheat && selectedRule != 100)
                {
                    if (selectedRule == Rules_List.allowedRuleIndexes.Count)
                    {
                        if (Rules_List.AccessCurrentRule == Rules_List.GetList().Length)
                            Level_Manager.SelectCheat();
                        else
                            Rules_List.AccessCurrentRule = Rules_List.GetList().Length;
                    }
                    else
                    {
                        if (Rules_List.AccessCurrentRule == Rules_List.allowedRuleIndexes[selectedRule])
                            Level_Manager.SelectCheat();
                        else
                            Rules_List.AccessCurrentRule = Rules_List.allowedRuleIndexes[selectedRule];
                    }
                }
                else if (selectedTile.X != 100)
                {
                    if (!Level_Manager.findingCheat)
                    {
                        if (Level_Manager.currentLevel != 0 || Level_Manager.currentBundle != 0 || Level_Manager.AccessCurrentSlide > 2)
                        {
                            Level_Manager.findingCheat = true;
                        }
                    }
                }
                else
                    Level_Manager.findingCheat = false;
            }

            if (Level_Manager.findingCheat)
            {
                if (currentMS.ScrollWheelValue > previousMS.ScrollWheelValue)
                {
                    Rules_List.MoveThroughRules(1);
                }
                if (currentMS.ScrollWheelValue < previousMS.ScrollWheelValue)
                {
                    Rules_List.AccessCurrentRule++;
                }
            }
        }

        public static void OptionUpdate()
        {
            switch (Options_Menu.AccessSelectedOption)
            {
                case Options_Menu.SelectedOption.none:

                    if (optionRects[0].Contains(mousePosition))
                    {
                        Options_Menu.optionIndex = 1;
                    }
                    else if (optionRects[1].Contains(mousePosition))
                    {
                        Options_Menu.optionIndex = 2;
                    }
                    else if (optionRects[2].Contains(mousePosition))
                    {
                        Options_Menu.optionIndex = 3;
                    }
                    else if (optionRects[3].Contains(mousePosition))
                    {
                        Options_Menu.optionIndex = 4;
                    }
                    else if (optionRects[4].Contains(mousePosition))
                    {
                        Options_Menu.optionIndex = 5;
                    }

                    if (currentMS.LeftButton == ButtonState.Pressed && previousMS.LeftButton == ButtonState.Released)
                    {
                        if (Options_Menu.optionIndex == 1 && optionRects[0].Contains(mousePosition))
                        {
                            Options_Menu.AccessSelectedOption = Options_Menu.SelectedOption.fullScreen;
                        }
                        else if (Options_Menu.optionIndex == 2 && optionRects[1].Contains(mousePosition))
                        {
                            Options_Menu.AccessSelectedOption = Options_Menu.SelectedOption.viewControls;
                        }
                        else if (Options_Menu.optionIndex == 3 && optionRects[2].Contains(mousePosition))
                        {
                            Options_Menu.AccessSelectedOption = Options_Menu.SelectedOption.musicVolume;
                        }
                        else if (Options_Menu.optionIndex == 4 && optionRects[3].Contains(mousePosition))
                        {
                            Options_Menu.AccessSelectedOption = Options_Menu.SelectedOption.sFXVolume;
                        }
                        else if (Options_Menu.optionIndex == 5 && optionRects[4].Contains(mousePosition))
                        {
                            Transition_Effect.StartTransition(Transition_Effect.AccessNextTransitionState);
                        }
                    }

                    if (Options_Menu.AccessSelectedOption != Options_Menu.SelectedOption.none)
                    {
                        Options_Menu.highLightColor = Color.Blue;
                    }

                    break;
                case Options_Menu.SelectedOption.fullScreen:

                    if (currentMS.LeftButton == ButtonState.Released && previousMS.LeftButton == ButtonState.Pressed)
                    {
                        Options_Menu.AccessSelectedOption = Options_Menu.SelectedOption.none;

                        Options_Menu.highLightColor = Color.White;

                        if (Options_Menu.fullScreenIndex > 0)
                        {
                            Game1.ControlFullScreen(true);
                        }
                        else
                        {
                            Game1.ControlFullScreen(false);
                        }
                    }
                    else if (mousePosition.X - 32 > 263 + Options_Menu.fullScreenIndex * 32 && Options_Menu.fullScreenIndex < Options_Menu.fullScreenAmount)
                    {
                        Options_Menu.fullScreenIndex++;
                    }
                    else if (mousePosition.X < 263 + Options_Menu.fullScreenIndex * 32 && Options_Menu.fullScreenIndex > 0)
                    {
                        Options_Menu.fullScreenIndex--;
                    }

                    break;
                case Options_Menu.SelectedOption.viewControls:

                    if (currentMS.LeftButton == ButtonState.Released && previousMS.LeftButton == ButtonState.Pressed)
                    {
                        Options_Menu.AccessSelectedOption = Options_Menu.SelectedOption.none;

                        Options_Menu.highLightColor = Color.White;
                    }
                    else if (mousePosition.X - 32 > 263 + Options_Menu.viewControlsIndex * 32 && Options_Menu.viewControlsIndex < Options_Menu.viewControlsAmount)
                    {
                        Options_Menu.viewControlsIndex++;
                    }
                    else if (mousePosition.X < 263 + Options_Menu.viewControlsIndex * 32 && Options_Menu.viewControlsIndex > 0)
                    {
                        Options_Menu.viewControlsIndex--;
                    }

                    break;
                case Options_Menu.SelectedOption.musicVolume:

                    if (currentMS.LeftButton == ButtonState.Released && previousMS.LeftButton == ButtonState.Pressed)
                    {
                        Options_Menu.AccessSelectedOption = Options_Menu.SelectedOption.none;

                        Options_Menu.highLightColor = Color.White;

                        Music_Player.StopMusic();
                    }
                    else if (mousePosition.X - 32 > 200 + Options_Menu.musicVolumeIndex * 16 && Options_Menu.musicVolumeIndex < Options_Menu.musicVolumeAmount)
                    {
                        Options_Menu.musicVolumeIndex++;
                    }
                    else if (mousePosition.X < 200 + Options_Menu.musicVolumeIndex * 16 && Options_Menu.musicVolumeIndex > 0)
                    {
                        Options_Menu.musicVolumeIndex--;
                    }

                    break;
                case Options_Menu.SelectedOption.sFXVolume:

                    if (currentMS.LeftButton == ButtonState.Released && previousMS.LeftButton == ButtonState.Pressed)
                    {
                        Options_Menu.AccessSelectedOption = Options_Menu.SelectedOption.none;

                        Options_Menu.highLightColor = Color.White;
                    }
                    else if (mousePosition.X - 32 > 200 + Options_Menu.sFXVolumeIndex * 16 && Options_Menu.sFXVolumeIndex < Options_Menu.sFXVolumeAmount)
                    {
                        Options_Menu.sFXVolumeIndex++;
                    }
                    else if (mousePosition.X < 200 + Options_Menu.sFXVolumeIndex * 16 && Options_Menu.sFXVolumeIndex > 0)
                    {
                        Options_Menu.sFXVolumeIndex--;
                    }

                    break;
            }
        }

        public static void LevelDraw(SpriteBatch aSpriteBatch)
        {
            if (selectedTile.X != 100)
            {
                Vector2 tempPosition = new Vector2((int)(Game_Board.AccessBoardPosition.X + (selectedTile.X * Game_Board.AccessTileSize)),
                        (int)(Game_Board.AccessBoardPosition.Y + (selectedTile.Y * Game_Board.AccessTileSize)));
                aSpriteBatch.Draw(tileSelect, tempPosition, Color.White);
            }
            if (selectedRule != 100)
            {
                aSpriteBatch.Draw(tileSelect, ruleBoxes[selectedRule], new Rectangle(0, 0, 1, 1), Color.White * 0.75f);
            }
        }
    }
}
