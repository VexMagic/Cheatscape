using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cheatscape
{
    static class Level_Manager
    {
        static Texture2D TileSelect;
        static Vector2 SelectedTile;
        public static List<Rectangle> RuleBoxes = new List<Rectangle>();
        static Rectangle[,] BoardTiles = new Rectangle[8, 8];
        static Rectangle[,] LevelButtons = new Rectangle[5, 2];
        static Rectangle hintRect = new Rectangle((int)Global_Info.AccessWindowSize.X / 2 - 95, 10, 40, 40);
        static Rectangle[] hintArrowRects = { new Rectangle(467, 100, 112, 20), new Rectangle(467, 100, 16, 20), new Rectangle(563, 100, 16, 20) };
        static Rectangle[] ScrollButtons = { new Rectangle(0, 138, 20, 20), new Rectangle(0, 340, 20, 20) };
        static Rectangle[] BannerButtons = { new Rectangle(20, 118, 16, 20), new Rectangle(116, 118, 16, 20) };
        static Rectangle[] SlideButtons = { new Rectangle(113, 148, 31, 64), new Rectangle(456, 148, 31, 64) };
        static int SelectedRule = 100;
        public static int CurrentLevel = 0;
        public static int CurrentBundle = 0;
        static List<List<Chess_Move>> AllMoves = new List<List<Chess_Move>>();
        static List<Tuple<Chess_Move, int>> AllAnswers = new List<Tuple<Chess_Move, int>>();
        static int CurrentSlide = 1;
        static float rating;
        public static bool FindingCheat = false;
        public static bool isOnTransitionScreen = false;
        static bool completed = false;
        static bool levelComplete = false;
        public static bool displayingHint = false;
        public static int currentHint = -1;
        public static int unlockedHints = -1;

        public static float AccessRating { get  => rating; set => rating = value; }
        public static int AccessCurrentLevel { get => CurrentLevel; set => CurrentLevel = value; }
        public static int AccessCurrentBundle { get => CurrentBundle; set => CurrentBundle = value; }
        public static List<List<Chess_Move>> AccessAllMoves { get => AllMoves; set => AllMoves = value; }
        public static List<Tuple<Chess_Move, int>> AccessAllAnswers { get => AllAnswers; set => AllAnswers = value; }
        public static int AccessCurrentSlide { get => CurrentSlide; set => CurrentSlide = value; }
        public static bool AccessCompleted { get => completed; set => completed = value; }

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
            if (Input_Manager.KeyPressed(Keys.Tab))
            {
                Pause_Menu.gameIsPaused = !Pause_Menu.gameIsPaused;
            }

            if (Pause_Menu.gameIsPaused == false)
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
                        if (BoardTiles[i, j].Contains(Input_Manager.GetMousePosition()))
                            SelectedTile = new Vector2(i, j);
                    }
                }

                for (int i = 0; i < RuleBoxes.Count; i++)
                {
                    if (RuleBoxes[i].Contains(Input_Manager.GetMousePosition()))
                    {
                        SelectedRule = i;
                    }
                }

                if (Input_Manager.MouseLBPressed() && LevelBool())
                {
                    if (FindingCheat && (ScrollButtons[0].Contains(Input_Manager.GetMousePosition()) || ScrollButtons[1].Contains(Input_Manager.GetMousePosition())))
                    {
                        if (ScrollButtons[0].Contains(Input_Manager.GetMousePosition()))
                            Rules_List.MoveThroughRules(1);
                        else
                            Rules_List.AccessCurrentRule++;
                    }
                    else if (FindingCheat && Rules_List.AmountOfUsedLists() > 1 && (BannerButtons[0].Contains(Input_Manager.GetMousePosition()) || BannerButtons[1].Contains(Input_Manager.GetMousePosition())))
                    {
                        if (BannerButtons[0].Contains(Input_Manager.GetMousePosition()))
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
                    else if (!FindingCheat && ((SlideButtons[0].Contains(Input_Manager.GetMousePosition()) && AccessCurrentSlide > 1)
                        || (SlideButtons[1].Contains(Input_Manager.GetMousePosition()) && AccessCurrentSlide < AccessAllMoves.Count)))
                    {
                        if (SlideButtons[0].Contains(Input_Manager.GetMousePosition()))
                            ChangeSlide(false);
                        else
                            ChangeSlide(true);
                    }
                    else if (FindingCheat && SelectedRule != 100)
                    {
                        if (SelectedRule == Rules_List.AllowedRuleIndexes.Count)
                        {
                            if (Rules_List.AccessCurrentRule == Rules_List.GetList().Length)
                                SelectCheat();
                            else
                                Rules_List.AccessCurrentRule = Rules_List.GetList().Length;
                        }
                        else
                        {
                            if (Rules_List.AccessCurrentRule == Rules_List.AllowedRuleIndexes[SelectedRule])
                                SelectCheat();
                            else
                                Rules_List.AccessCurrentRule = Rules_List.AllowedRuleIndexes[SelectedRule];
                        }
                    }
                    else if (SelectedTile.X != 100)
                    {
                        if (!FindingCheat)
                        {
                            if (CurrentLevel != 0 || CurrentBundle != 0 || AccessCurrentSlide > 2)
                            {
                                FindingCheat = true;
                            }
                        }
                    }
                    else if (CurrentBundle != 0 && hintArrowRects[0].Contains(Input_Manager.GetMousePosition()) && displayingHint == true)
                    {
                        if (hintArrowRects[1].Contains(Input_Manager.GetMousePosition()) && currentHint > 0)
                        {
                            currentHint--;
                        }
                        else if (hintArrowRects[2].Contains(Input_Manager.GetMousePosition()) && currentHint < unlockedHints)
                        {
                            currentHint++;
                        }
                    }
                    else if (CurrentBundle != 0 && hintRect.Contains(Input_Manager.GetMousePosition()) && unlockedHints <= Hint_File_Manager.hintList.Count - 2)
                    {
                        displayingHint = true;
                        AccessRating -= 100;
                        currentHint++;
                        unlockedHints++;
                        if (currentHint < unlockedHints)
                            currentHint = unlockedHints;
                    }
                    else
                        FindingCheat = false;
                }

                if (FindingCheat)
                {
                    if (Input_Manager.CurrentScrollValue() > Input_Manager.PreviousScrollValue())
                    {
                        Rules_List.MoveThroughRules(1);
                    }
                    if (Input_Manager.CurrentScrollValue() < Input_Manager.PreviousScrollValue())
                    {
                        Rules_List.AccessCurrentRule++;
                    }
                }

                if (Input_Manager.KeyPressed(Keys.Left) && CurrentSlide > 1 && !FindingCheat && !isOnTransitionScreen && rating > 0)
                {
                    ChangeSlide(false);
                }
                else if (Input_Manager.KeyPressed(Keys.Left) && FindingCheat)
                {
                    Rules_List.AccessCurrentRuleList--;
                    if (Rules_List.AccessCurrentRuleList < 0)
                    {
                        Rules_List.AccessCurrentRuleList = Rules_List.AmountOfRuleLists - 1;
                    }
                    Rules_List.AccessCurrentRule = 0;
                }
                else if (Input_Manager.KeyPressed(Keys.Right) && CurrentSlide < AllMoves.Count && !FindingCheat && !isOnTransitionScreen && rating > 0)
                {
                    ChangeSlide(true);
                }
                else if (Input_Manager.KeyPressed(Keys.Right) && FindingCheat)
                {
                    Rules_List.AccessCurrentRuleList++;
                    if (Rules_List.AccessCurrentRuleList >= Rules_List.AmountOfRuleLists)
                    {
                        Rules_List.AccessCurrentRuleList = 0;
                    }
                    Rules_List.AccessCurrentRule = 0;
                }

                else if (CurrentBundle != 0 && FindingCheat == true && Input_Manager.KeyPressed(Keys.H) && unlockedHints <= Hint_File_Manager.hintList.Count - 2)
                {
                    displayingHint = true;
                    rating -= 100;
                    currentHint++;
                    unlockedHints++;
                    if(currentHint < unlockedHints)
                        currentHint = unlockedHints;
                }

                else if (CurrentBundle != 0 && FindingCheat == true && displayingHint == true && Input_Manager.KeyPressed(Keys.G) && currentHint > 0)
                {
                    currentHint--;
                }

                else if (CurrentBundle != 0 && FindingCheat == true && displayingHint == true && Input_Manager.KeyPressed(Keys.J) && currentHint < unlockedHints)
                {
                    currentHint++;
                }

                else if (Input_Manager.KeyPressed(Keys.Space) && !isOnTransitionScreen && rating > 0)
                {
                    if (!FindingCheat)
                    {
                        if (CurrentLevel != 0 || CurrentBundle != 0 || CurrentSlide > 2)
                        {
                            FindingCheat = true;
                        }
                    }
                    else
                    {
                        SelectCheat();
                    }
                }

                else if (Input_Manager.KeyPressed(Keys.Up) && FindingCheat)
                {
                    Rules_List.MoveThroughRules(1);
                }
                else if (Input_Manager.KeyPressed(Keys.Down) && FindingCheat)
                {
                    Rules_List.AccessCurrentRule++;
                }

                if (rating <= 0)
                {
                    End_Screen.AccessCleared = false;
                    End_Screen.AccessIsEnded = true;
                }

            }

        }

        public static void Play(GameTime gameTime)
        {
            if (!levelComplete && isOnTransitionScreen && (Input_Manager.KeyPressed(Keys.Enter) || Input_Manager.MouseLBPressed()))
            {
                Hint_File_Manager.LoadHints();
                isOnTransitionScreen = false;
                Pause_Menu.gameIsPaused = false;
            }
            levelComplete = false;
        }

        public static void ChangeSlide(bool isMoveForward)
        {
            if (isMoveForward)
            {
                Hand_Animation_Manager.ResetAllHands();
                File_Manager.turnCounter--;
                CurrentSlide++;
                Game_Board.SetBoardState();
            }
            else
            {
                Hand_Animation_Manager.ResetAllHands();
                File_Manager.turnCounter++;
                CurrentSlide--;
                Game_Board.SetBoardState();
                Music_Player.MoveEffect();
            }
        }

        public static void SelectCheat()
        {
            for (int i = 0; i < AllAnswers.Count; i++)
            {
                if (AllAnswers[i].Item1.myRule.X == Rules_List.AccessCurrentRuleList &&
                    AllAnswers[i].Item1.myRule.Y == Rules_List.AccessCurrentRule &&
                    AllAnswers[i].Item2 == CurrentSlide)
                {
                    isOnTransitionScreen = true;
                    currentHint = -1;
                    unlockedHints = -1;
                    displayingHint = false;
                    rating += 100;
                    Hint_File_Manager.LoadHints();
                    Pause_Menu.gameIsPaused = false;
                    Level_Transition.LoadSpecialRule();
                    CurrentLevel++;
                    File_Manager.LoadLevel();
                    levelComplete = true;
                }

            }
            if (Rules_List.AccessCurrentRule != Rules_List.GetList().Length && !isOnTransitionScreen && !End_Screen.AccessCleared)
            {

                if (rating / 2 > 400)
                {
                    rating /= 2;
                }
                else if (rating - 400 > 0)
                {
                    rating -= 400;
                }
                else
                    rating = 0;
            }      
            FindingCheat = false;
        }

        public static void Draw(SpriteBatch aSpriteBatch)
        {
            if (FindingCheat && !isOnTransitionScreen)
                Rules_List.Draw(aSpriteBatch);

            if (CurrentBundle != 0 && FindingCheat && displayingHint)
            {
                Text_Manager.DrawTextBox(Hint_File_Manager.hintList[currentHint], new Vector2(473, 127), Text_Manager.TextBoarder, aSpriteBatch);

                if(unlockedHints > 0)
                {
                    if (currentHint == 0)
                    {
                        aSpriteBatch.Draw(Rules_List.hintArrowsRight, hintArrowRects[0], Color.White);
                        Text_Manager.DrawText("J", hintArrowRects[0].X + Rules_List.hintArrowsRight.Width - 24, hintArrowRects[0].Y + 2, aSpriteBatch);
                    }

                    if (currentHint == 1 && unlockedHints > 1)
                    {
                        aSpriteBatch.Draw(Rules_List.hintArrows, hintArrowRects[0], Color.White);
                        Text_Manager.DrawText("G", hintArrowRects[0].X +19, hintArrowRects[0].Y+2, aSpriteBatch);
                        Text_Manager.DrawText("J", hintArrowRects[0].X + Rules_List.hintArrowsRight.Width -24, hintArrowRects[0].Y+2, aSpriteBatch);
                    }

                    if (currentHint == 1 && unlockedHints < 2)
                    {
                        aSpriteBatch.Draw(Rules_List.hintArrowsLeft, hintArrowRects[0], Color.White);
                        Text_Manager.DrawText("G", hintArrowRects[0].X + 19, hintArrowRects[0].Y + 2, aSpriteBatch);
                    }

                    if (currentHint == 2)
                    {
                        aSpriteBatch.Draw(Rules_List.hintArrowsLeft, hintArrowRects[0], Color.White);
                        Text_Manager.DrawText("G", hintArrowRects[0].X + 19, hintArrowRects[0].Y + 2, aSpriteBatch);
                    }
                }
            }

            if (isOnTransitionScreen == false)
                Text_Manager.DrawText(Convert.ToString("Moves Left: " + File_Manager.turnCounter), (int)(Global_Info.AccessWindowSize.X / Global_Info.AccessScreenScale) - 450, 10, aSpriteBatch);

            if (isOnTransitionScreen)                
                Level_Transition.Draw(aSpriteBatch);

            if (Options_Menu.AccessControlView == true && isOnTransitionScreen == false)
            {
                if (CurrentBundle != 0)
                    Text_Manager.DrawText(Convert.ToString("Current rating: " + rating), 
                        (int)(Global_Info.AccessWindowSize.X / Global_Info.AccessScreenScale) - 300, 10, aSpriteBatch);
            }

            if (Options_Menu.AccessControlView == true && isOnTransitionScreen == false)
            {
                if (FindingCheat)
                {
                    Text_Manager.DrawText("Right/Left: Toggle categories     Up/Down: Toggle rules     Space: Select rule", 150, 
                        (int)(Global_Info.AccessWindowSize.Y / Global_Info.AccessScreenScale) - 20, aSpriteBatch);
                }
                else
                {
                    Text_Manager.DrawText("Right: Next move               Left: Previous move               Space: Rules", 150, 
                        (int)(Global_Info.AccessWindowSize.Y / Global_Info.AccessScreenScale) - 20, aSpriteBatch);
                }
            }
        }

        public static void LevelDraw(SpriteBatch aSpriteBatch)
        {
            if (LevelBool() && SelectedTile.X != 100)
            {
                Vector2 tempPosition = new Vector2((int)(Game_Board.AccessBoardPosition.X + (SelectedTile.X * Game_Board.AccessTileSize)),
                            (int)(Game_Board.AccessBoardPosition.Y + (SelectedTile.Y * Game_Board.AccessTileSize)));
                aSpriteBatch.Draw(TileSelect, tempPosition, Color.White);
            }
            if (SelectedRule != 100 && FindingCheat)
            {
                aSpriteBatch.Draw(TileSelect, RuleBoxes[SelectedRule], new Rectangle(0, 0, 1, 1), Color.White * 0.75f);
            }
        }

        public static bool LevelBool()
        {
            if (Pause_Menu.gameIsPaused || End_Screen.AccessIsEnded || isOnTransitionScreen)
            {
                return false;
            }

            return true;
        }
    }
}
