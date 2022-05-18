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
        public static int timer = 60;
        public static int prevGameTime = 0;
        static bool playedThrough = false;
        public static int CurrentLevel = 0;
        public static int CurrentBundle = 0;
        static List<List<Chess_Move>> AllMoves = new List<List<Chess_Move>>();
        static List<Tuple<Chess_Move, int>> AllAnswers = new List<Tuple<Chess_Move, int>>();
        static int CurrentSlide = 1;
        static float rating;
        public static bool FindingCheat = false;
        static int AmountOfRuleLists = 3;
        public static bool isOnTransitionScreen = false;
        public static bool isOnFirstTransitionScreen = false;
        public static int exitedFirstTransitionScreen = 0;
        static bool completed = false;
        static bool displayingHint = false;
        static int currentHint = -1;
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
                if (Input_Manager.KeyPressed(Keys.Left) && CurrentSlide > 1 && !FindingCheat && !isOnTransitionScreen && !isOnFirstTransitionScreen && rating > 0 && playedThrough)
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
                else if (Input_Manager.KeyPressed(Keys.Right) && CurrentSlide < AllMoves.Count && !FindingCheat && !isOnTransitionScreen && !isOnFirstTransitionScreen && rating > 0 && playedThrough)
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

                else if (CurrentBundle != 0 && FindingCheat == true && Input_Manager.KeyPressed(Keys.H) && unlockedHints <= Hint_File_Manager.hintList.Count - 2 )
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

                else if (Input_Manager.KeyPressed(Keys.Space) && !isOnTransitionScreen && !isOnFirstTransitionScreen && rating > 0)
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
                                CurrentLevel++;
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

                if (rating == 0 && Input_Manager.KeyPressed(Keys.Enter))
                {
                    End_Screen.AccessCleared = false;
                    End_Screen.AccessIsEnded = true;
                }

            }

        }

        public static void Play(GameTime gameTime)
        {
            if (!playedThrough && !FindingCheat && Pause_Menu.gameIsPaused == false && isOnFirstTransitionScreen == false)
            {
                if (prevGameTime < gameTime.ElapsedGameTime.TotalSeconds)
                {
                    timer++;
                    prevGameTime = (int)gameTime.ElapsedGameTime.TotalSeconds;
                }
                if (CurrentSlide < AllMoves.Count)
                {
                    if (timer == 120)
                    {
                        Hand_Animation_Manager.ResetAllHands();
                        File_Manager.turnCounter--;
                        CurrentSlide++;
                        Game_Board.SetBoardState();
                        for (int i = 0; i < AllMoves[CurrentSlide - 1].Count; i++)
                        {
                            Game_Board.MakeAMove(AllMoves[CurrentSlide - 1][i], true);
                        }
                        timer = 0;
                    }
                }
                else
                {
                    playedThrough = true;
                }
            }

            if (CurrentLevel == 0 && exitedFirstTransitionScreen < 1 && CurrentBundle != 0)
            {
                isOnFirstTransitionScreen = true;
            }
            if (isOnFirstTransitionScreen == true && Input_Manager.KeyPressed(Keys.Enter))
            {
                isOnFirstTransitionScreen = false;
                exitedFirstTransitionScreen = 1;
            }

            if (isOnTransitionScreen == true && Input_Manager.KeyPressed(Keys.Enter))
            {
                isOnTransitionScreen = false;
                Pause_Menu.gameIsPaused = false;
                playedThrough = false;
                currentHint = -1;
                unlockedHints = -1;
                displayingHint = false;
                rating += 100;
                exitedFirstTransitionScreen = 0;
                File_Manager.LoadLevel();
                Hint_File_Manager.LoadHints();
            }

        }

        public static void Draw(SpriteBatch aSpriteBatch)
        {
            if (FindingCheat)
                Rules_List.Draw(aSpriteBatch);

            if (CurrentBundle != 0 && FindingCheat && displayingHint)
            {
                Text_Manager.DrawTextBox(Hint_File_Manager.hintList[currentHint], new Vector2(475, 120), Text_Manager.TextBoarder, aSpriteBatch);

                if(unlockedHints > 0)
                {

                    if (currentHint == 0)
                    Text_Manager.DrawText("    J >", 508, 100, aSpriteBatch);

                    if (currentHint == 1 && unlockedHints > 1)
                        Text_Manager.DrawText("< G J >", 508, 100, aSpriteBatch);

                    if (currentHint == 1 && unlockedHints < 2)
                        Text_Manager.DrawText("< G", 508, 100, aSpriteBatch);

                    if (currentHint == 2)
                        Text_Manager.DrawText("< G", 508, 100, aSpriteBatch);

                }

            }

            if (isOnTransitionScreen == false)
                Text_Manager.DrawText(Convert.ToString("Turn counter: " + CurrentLevel), (int)(Global_Info.AccessWindowSize.X / Global_Info.AccessScreenScale) - 450, 10, aSpriteBatch);

            if (isOnTransitionScreen || isOnFirstTransitionScreen)
                Level_Transition.Draw(aSpriteBatch);

            if (Options_Menu.AccessControlView == true && isOnTransitionScreen == false)
            Text_Manager.DrawText(Convert.ToString("Current rating: " + rating), (int)(Global_Info.AccessWindowSize.X / Global_Info.AccessScreenScale) - 300, 10
                        , aSpriteBatch);

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

                if (rating == 0)
                Text_Manager.DrawText(Convert.ToString("You've failed the tutorial..."), 20, (int)(Global_Info.AccessWindowSize.Y / Global_Info.AccessScreenScale) - 200
                        , aSpriteBatch);

            }
        }
    }
}
