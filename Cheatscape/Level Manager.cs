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
        public static int CurrentLevel = 0;
        public static int CurrentBundle = 0;
        static List<List<Chess_Move>> AllMoves = new List<List<Chess_Move>>();
        static List<Tuple<Chess_Move, int>> AllAnswers = new List<Tuple<Chess_Move, int>>();
        static int CurrentSlide = 1;
        static float rating;
        public static bool FindingCheat = false;
        static int AmountOfRuleLists = 3;
        public static bool isOnTransitionScreen = false;
        static bool completed = false;
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

        public static void Update()
        {
            if (Input_Manager.KeyPressed(Keys.Tab))
            {
                Pause_Menu.gameIsPaused = !Pause_Menu.gameIsPaused;
            }

            if (Pause_Menu.gameIsPaused == false)
            {
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
                            }

                        }
                        if (!isOnTransitionScreen && !End_Screen.AccessCleared)
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

            if (isOnTransitionScreen == true && Input_Manager.KeyPressed(Keys.Enter))
            {
                isOnTransitionScreen = false;
                Pause_Menu.gameIsPaused = false;
            }
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
                
                if (Rules_List.AccessCurrentRule != Rules_List.GetList().Length)
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
            }
            FindingCheat = false;
        }

        public static void Draw(SpriteBatch aSpriteBatch)
        {
            if (FindingCheat)
                Rules_List.Draw(aSpriteBatch);

            if (CurrentBundle != 0 && FindingCheat && displayingHint)
            {
                Text_Manager.DrawTextBox(Hint_File_Manager.hintList[currentHint], new Vector2(473, 127), Text_Manager.TextBoarder, aSpriteBatch);

                if(unlockedHints > 0)
                {
                    if (currentHint == 0)
                    {
                        aSpriteBatch.Draw(Rules_List.hintArrowsRight, Mouse_Controller.hintArrowRects[0], Color.White);
                        Text_Manager.DrawText("J", Mouse_Controller.hintArrowRects[0].X + Rules_List.hintArrowsRight.Width - 24, Mouse_Controller.hintArrowRects[0].Y + 2, aSpriteBatch);
                    }

                    if (currentHint == 1 && unlockedHints > 1)
                    {
                        aSpriteBatch.Draw(Rules_List.hintArrows, Mouse_Controller.hintArrowRects[0], Color.White);
                        Text_Manager.DrawText("G", Mouse_Controller.hintArrowRects[0].X +19, Mouse_Controller.hintArrowRects[0].Y+2, aSpriteBatch);
                        Text_Manager.DrawText("J", Mouse_Controller.hintArrowRects[0].X + Rules_List.hintArrowsRight.Width -24, Mouse_Controller.hintArrowRects[0].Y+2, aSpriteBatch);
                    }

                    if (currentHint == 1 && unlockedHints < 2)
                    {
                        aSpriteBatch.Draw(Rules_List.hintArrowsLeft, Mouse_Controller.hintArrowRects[0], Color.White);
                        Text_Manager.DrawText("G", Mouse_Controller.hintArrowRects[0].X + 19, Mouse_Controller.hintArrowRects[0].Y + 2, aSpriteBatch);
                    }

                    if (currentHint == 2)
                    {
                        aSpriteBatch.Draw(Rules_List.hintArrowsLeft, Mouse_Controller.hintArrowRects[0], Color.White);
                        Text_Manager.DrawText("G", Mouse_Controller.hintArrowRects[0].X + 19, Mouse_Controller.hintArrowRects[0].Y + 2, aSpriteBatch);
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
    }
}
