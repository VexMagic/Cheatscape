using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Cheatscape
{
    static class Level_Manager
    {
        public static int timer = 60;
        public static int prevGameTime = 0;
        static bool playedThrough = false;
        public static int currentLevel = 0;
        public static int currentBundle = 0;
        static List<List<Chess_Move>> allMoves = new List<List<Chess_Move>>();
        static List<Tuple<Chess_Move, int>> allAnswers = new List<Tuple<Chess_Move, int>>();
        static int currentSlide = 1;
        static float rating;
        public static bool findingCheat = false;
        static int amountOfRuleLists = 3;
        public static bool isOnTransitionScreen = false;
        static bool completed = false;

        public static float AccessRating { get => rating; set => rating = value; }
        public static int AccessCurrentLevel { get => currentLevel; set => currentLevel = value; }
        public static int AccessCurrentBundle { get => currentBundle; set => currentBundle = value; }
        public static List<List<Chess_Move>> AccessAllMoves { get => allMoves; set => allMoves = value; }
        public static List<Tuple<Chess_Move, int>> AccessAllAnswers { get => allAnswers; set => allAnswers = value; }
        public static int AccessCurrentSlide { get => currentSlide; set => currentSlide = value; }

        public static bool AccessCompleted { get => completed; set => completed = value; }


        public static void Update()
        {
            if (Input_Manager.KeyPressed(Keys.Tab))
            {
                Pause_Menu.gameIsPaused = !Pause_Menu.gameIsPaused;
            }

            if (Pause_Menu.gameIsPaused == false)
            {
                if (Input_Manager.KeyPressed(Keys.Left) && currentSlide > 1 && !findingCheat && !isOnTransitionScreen && rating > 0 && playedThrough)
                {
                    Hand_Animation_Manager.ResetAllHands();
                    File_Manager.turnCounter++;
                    currentSlide--;
                    Game_Board.SetBoardState();
                    Music_Player.MoveEffect();
                }
                else if (Input_Manager.KeyPressed(Keys.Left) && findingCheat)
                {
                    Rules_List.AccessCurrentRuleList--;
                    if (Rules_List.AccessCurrentRuleList < 0)
                    {
                        Rules_List.AccessCurrentRuleList = amountOfRuleLists - 1;
                    }
                    Rules_List.AccessCurrentRule = 0;
                }
                else if (Input_Manager.KeyPressed(Keys.Right) && currentSlide < allMoves.Count && !findingCheat && !isOnTransitionScreen && rating > 0 && playedThrough)
                {
                    Hand_Animation_Manager.ResetAllHands();
                    File_Manager.turnCounter--;
                    currentSlide++;
                    Game_Board.SetBoardState();
                    for (int i = 0; i < allMoves[currentSlide - 1].Count; i++)
                    {
                        Game_Board.MakeAMove(allMoves[currentSlide - 1][i], true);
                    }

                }
                else if (Input_Manager.KeyPressed(Keys.Right) && findingCheat)
                {
                    Rules_List.AccessCurrentRuleList++;
                    if (Rules_List.AccessCurrentRuleList >= amountOfRuleLists)
                    {
                        Rules_List.AccessCurrentRuleList = 0;
                    }
                    Rules_List.AccessCurrentRule = 0;
                }
                else if (Input_Manager.KeyPressed(Keys.Space) && !isOnTransitionScreen && rating > 0)
                {
                    if (!findingCheat)
                        findingCheat = true;
                    else
                    {
                        for (int i = 0; i < allAnswers.Count; i++)
                        {
                            if (allAnswers[i].Item1.myRule.X == Rules_List.AccessCurrentRuleList &&
                                allAnswers[i].Item1.myRule.Y == Rules_List.AccessCurrentRule &&
                                allAnswers[i].Item2 == currentSlide)
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
                        findingCheat = false;
                    }
                }

                else if (Input_Manager.KeyPressed(Keys.Up) && findingCheat)
                {
                    Rules_List.MoveThroughRules(1);
                }
                else if (Input_Manager.KeyPressed(Keys.Down) && findingCheat)
                {
                    Rules_List.AccessCurrentRule++;
                }

                if (rating == 0 && Input_Manager.KeyPressed(Keys.Enter))
                {
                    End_Screen.AccessCleared = false;
                    End_Screen.AccessIsEnded = true;
                }

            }

        }

        public static void Play(GameTime gameTime)
        {
            if (!playedThrough && !findingCheat && Pause_Menu.gameIsPaused == false)
            {
                if (prevGameTime < gameTime.ElapsedGameTime.TotalSeconds)
                {
                    timer++;
                    prevGameTime = (int)gameTime.ElapsedGameTime.TotalSeconds;
                }
                if (currentSlide < allMoves.Count)
                {
                    if (timer == 120)
                    {
                        Hand_Animation_Manager.ResetAllHands();
                        File_Manager.turnCounter--;
                        currentSlide++;
                        Game_Board.SetBoardState();
                        for (int i = 0; i < allMoves[currentSlide - 1].Count; i++)
                        {
                            Game_Board.MakeAMove(allMoves[currentSlide - 1][i], true);
                        }
                        timer = 0;
                    }
                }
                else
                {
                    playedThrough = true;
                }

            }
            if (isOnTransitionScreen == true && Input_Manager.KeyPressed(Keys.Enter))
            {
                isOnTransitionScreen = false;
                Pause_Menu.gameIsPaused = false;
                playedThrough = false;
                rating += 100;
                currentLevel++;
                File_Manager.LoadLevel();
            }

        }

        public static void Draw(SpriteBatch aSpriteBatch)
        {
            if (findingCheat)
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
                if (findingCheat)
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

            }
        }
    }
}
