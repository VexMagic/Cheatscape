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
        static bool isOnTransitionScreen = false;
        static bool completed = false;

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
                    Hand_Animation_Manager.ResetAllHands();
                    File_Manager.turnCounter++;
                    CurrentSlide--;
                    Game_Board.SetBoardState();
                    Music_Player.MoveEffect();
                }
                else if (Input_Manager.KeyPressed(Keys.Left) && FindingCheat)
                {
                    Rules_List.AccessCurrentRuleList--;
                    if (Rules_List.AccessCurrentRuleList < 0)
                    {
                        Rules_List.AccessCurrentRuleList = AmountOfRuleLists - 1;
                    }
                    Rules_List.AccessCurrentRule = 0;
                }
                else if (Input_Manager.KeyPressed(Keys.Right) && CurrentSlide < AllMoves.Count && !FindingCheat && !isOnTransitionScreen && rating > 0)
                {
                    Hand_Animation_Manager.ResetAllHands();
                    File_Manager.turnCounter--;
                    CurrentSlide++;
                    Game_Board.SetBoardState();
                    for (int i = 0; i < AllMoves[CurrentSlide - 1].Count; i++)
                    {
                        Game_Board.MakeAMove(AllMoves[CurrentSlide - 1][i], true);
                    }

                }
                else if (Input_Manager.KeyPressed(Keys.Right) && FindingCheat)
                {
                    Rules_List.AccessCurrentRuleList++;
                    if (Rules_List.AccessCurrentRuleList >= AmountOfRuleLists)
                    {
                        Rules_List.AccessCurrentRuleList = 0;
                    }
                    Rules_List.AccessCurrentRule = 0;
                }
                else if (Input_Manager.KeyPressed(Keys.Space) && !isOnTransitionScreen && rating > 0)
                {
                    if (!FindingCheat)
                        FindingCheat = true;
                    else
                    {
                        for (int i = 0; i < AllAnswers.Count; i++)
                        {
                            if (AllAnswers[i].Item1.myRule.X == Rules_List.AccessCurrentRuleList &&
                                AllAnswers[i].Item1.myRule.Y == Rules_List.AccessCurrentRule &&
                                AllAnswers[i].Item2 == CurrentSlide)
                            {
                                isOnTransitionScreen = true;
                                Pause_Menu.gameIsPaused = true;
                            }
                            else if (Rules_List.AccessCurrentRule != Rules_List.GetList().Length)
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
                }

                else if (Input_Manager.KeyPressed(Keys.Up) && FindingCheat)
                {
                    Rules_List.MoveThroughRules(1);
                }
                else if (Input_Manager.KeyPressed(Keys.Down) && FindingCheat)
                {
                    Rules_List.AccessCurrentRule++;
                }

                //if (completed == true)
                //{
                //    if (Input_Manager.KeyPressed(Keys.Enter))
                //    {
                //        CurrentLevel++;
                //        File_Manager.LoadLevel();
                //        //completed = false;
                //    }
                //}
                if (rating == 0 && Input_Manager.KeyPressed(Keys.Enter))
                {
                    End_Screen.AccessCleared = false;
                    End_Screen.AccessIsEnded = true;
                }

            }
            if (isOnTransitionScreen == true && Input_Manager.KeyPressed(Keys.Enter ))
            {
                isOnTransitionScreen = false;
                Pause_Menu.gameIsPaused = false;

                rating += 100;
                completed = true;
                //Global_Info.AccessCurrentGameState = Global_Info.GameState.LevelSelect;
                CurrentLevel++;
                File_Manager.LoadLevel();
               
            }

        }

        public static void Draw(SpriteBatch aSpriteBatch)
        {
            if (FindingCheat)
                Rules_List.Draw(aSpriteBatch);
            if (isOnTransitionScreen == false)
                Text_Manager.DrawTurnCounter(aSpriteBatch);

            if (isOnTransitionScreen)
                Level_Transition.Draw(aSpriteBatch);

            if (Options_Menu.AccessControlView == true && isOnTransitionScreen == false)
            Text_Manager.DrawText(Convert.ToString("Current rating: " + rating), (int)(Global_Info.AccessWindowSize.X / Global_Info.AccessScreenScale) - 300, 10
                        , aSpriteBatch);

            if (Options_Menu.AccessControlView == true && isOnTransitionScreen == false)
            {
                if (FindingCheat)
                {
                    Text_Manager.DrawText("Right/Left: Toggle categories     Up/Down: Toggle rules     Space: Select rule", 120, 
                        (int)(Global_Info.AccessWindowSize.Y / Global_Info.AccessScreenScale) - 20, aSpriteBatch);
                }
                else
                {
                    Text_Manager.DrawText("Right: Next move               Left: Previous move               Space: Rules", 120, 
                        (int)(Global_Info.AccessWindowSize.Y / Global_Info.AccessScreenScale) - 20, aSpriteBatch);
                }

                if (rating == 0)
                Text_Manager.DrawText(Convert.ToString("You've failed the tutorial..."), 20, (int)(Global_Info.AccessWindowSize.Y / Global_Info.AccessScreenScale) - 200
                        , aSpriteBatch);

                //if (completed)
                //    Text_Manager.DrawText(Convert.ToString("Nice! \n Press Enter to continue" ), 20, (int)(Global_Info.AccessWindowSize.Y / Global_Info.AccessScreenScale) - 200
                //        , aSpriteBatch);


            }
        }
    }
}
