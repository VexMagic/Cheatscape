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
        public static bool isOnTransitionScreen = false;
        public static bool isOnFirstTransitionScreen = false;
        public static int exitedFirstTransitionScreen = 0;
        static bool completed = false;
        static bool displayingHint = false;
        static int currentHint = -1;
        public static int unlockedHints = -1;
        public static float AccessRating
        {
            get => rating; set => rating = value;
        }
        public static int AccessCurrentLevel
        {
            get => currentLevel; set => currentLevel = value;
        }
        public static int AccessCurrentBundle
        {
            get => currentBundle; set => currentBundle = value;
        }
        public static List<List<Chess_Move>> AccessAllMoves
        {
            get => allMoves; set => allMoves = value;
        }
        public static List<Tuple<Chess_Move, int>> AccessAllAnswers
        {
            get => allAnswers; set => allAnswers = value;
        }
        public static int AccessCurrentSlide
        {
            get => currentSlide; set => currentSlide = value;
        }
        public static bool AccessCompleted
        {
            get => completed; set => completed = value;
        }

        public static void Update()
        {
            if (Keyboard_Inputs.KeyPressed(Keys.Tab))
            {
                Pause_Menu.gameIsPaused = !Pause_Menu.gameIsPaused;
            }

            if (Pause_Menu.gameIsPaused == false)
            {
                if (Keyboard_Inputs.KeyPressed(Keys.Left) && currentSlide > 1 && !findingCheat && !isOnTransitionScreen && !isOnFirstTransitionScreen && rating > 0 && playedThrough)
                {
                    ChangeSlide(false);
                }
                else if (Keyboard_Inputs.KeyPressed(Keys.Left) && findingCheat)
                {
                    Rules_List.AccessCurrentRuleList--;
                    if (Rules_List.AccessCurrentRuleList < 0)
                    {
                        Rules_List.AccessCurrentRuleList = Rules_List.amountOfRuleLists - 1;
                    }
                    Rules_List.AccessCurrentRule = 0;
                }
                else if (Keyboard_Inputs.KeyPressed(Keys.Right) && currentSlide < allMoves.Count && !findingCheat && !isOnTransitionScreen && !isOnFirstTransitionScreen && rating > 0 && playedThrough)
                {
                    ChangeSlide(true);
                }
                else if (Keyboard_Inputs.KeyPressed(Keys.Right) && findingCheat)
                {
                    Rules_List.AccessCurrentRuleList++;
                    if (Rules_List.AccessCurrentRuleList >= Rules_List.amountOfRuleLists)
                    {
                        Rules_List.AccessCurrentRuleList = 0;
                    }
                    Rules_List.AccessCurrentRule = 0;
                }

                else if (currentBundle != 0 && findingCheat == true && Keyboard_Inputs.KeyPressed(Keys.H) && unlockedHints <= Hint_File_Manager.hintList.Count - 2)
                {
                    displayingHint = true;
                    rating -= 100;
                    currentHint++;
                    unlockedHints++;
                    if (currentHint < unlockedHints)
                        currentHint = unlockedHints;
                }

                else if (currentBundle != 0 && findingCheat == true && displayingHint == true && Keyboard_Inputs.KeyPressed(Keys.G) && currentHint > 0)
                {
                    currentHint--;
                }

                else if (currentBundle != 0 && findingCheat == true && displayingHint == true && Keyboard_Inputs.KeyPressed(Keys.J) && currentHint < unlockedHints)
                {
                    currentHint++;
                }

                else if (Keyboard_Inputs.KeyPressed(Keys.Space) && !isOnTransitionScreen && !isOnFirstTransitionScreen && rating > 0)
                {
                    if (!findingCheat)
                    {
                        if (currentLevel != 0 || currentBundle != 0 || currentSlide > 2)
                        {
                            findingCheat = true;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < allAnswers.Count; i++)
                        {
                            if (allAnswers[i].Item1.myRule.X == Rules_List.AccessCurrentRuleList &&
                                allAnswers[i].Item1.myRule.Y == Rules_List.AccessCurrentRule &&
                                allAnswers[i].Item2 == currentSlide)
                            {
                                currentLevel++;
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

                else if (Keyboard_Inputs.KeyPressed(Keys.Up) && findingCheat)
                {
                    Rules_List.MoveThroughRules(1);
                }
                else if (Keyboard_Inputs.KeyPressed(Keys.Down) && findingCheat)
                {
                    Rules_List.AccessCurrentRule++;
                }

                if (rating == 0 && Keyboard_Inputs.KeyPressed(Keys.Enter))
                {
                    End_Screen.AccessCleared = false;
                    End_Screen.AccessIsEnded = true;
                }

            }

        }

        public static void Play(GameTime gameTime)
        {
            if (!playedThrough && !findingCheat && Pause_Menu.gameIsPaused == false && isOnFirstTransitionScreen == false)
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

            if (currentLevel == 0 && exitedFirstTransitionScreen < 1 && currentBundle != 0)
            {
                isOnFirstTransitionScreen = true;
            }
            if (isOnFirstTransitionScreen == true && Keyboard_Inputs.KeyPressed(Keys.Enter))
            {
                isOnFirstTransitionScreen = false;
                exitedFirstTransitionScreen = 1;
            }

            if (isOnTransitionScreen == true && Keyboard_Inputs.KeyPressed(Keys.Enter))
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

        public static void ChangeSlide(bool isMoveForward)
        {
            if (isMoveForward)
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
            else
            {
                Hand_Animation_Manager.ResetAllHands();
                File_Manager.turnCounter++;
                currentSlide--;
                Game_Board.SetBoardState();
                Music_Player.MoveEffect();
            }
        }

        public static void SelectCheat()
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

        public static void Draw(SpriteBatch aSpriteBatch)
        {
            if (findingCheat)
                Rules_List.Draw(aSpriteBatch);

            if (currentBundle != 0 && findingCheat && displayingHint)
            {
                Text_Manager.DrawTextBox(Hint_File_Manager.hintList[currentHint], new Vector2(475, 120), Text_Manager.textBoarder, aSpriteBatch);

                if (unlockedHints > 0)
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
                Text_Manager.DrawText(Convert.ToString("Turn counter: " + File_Manager.turnCounter), (int)(Global_Info.AccessWindowSize.X / Global_Info.AccessScreenScale) - 450, 10, aSpriteBatch);

            if (isOnTransitionScreen || isOnFirstTransitionScreen)
                Level_Transition.Draw(aSpriteBatch);

            if (Options_Menu.AccessControlView == true && isOnTransitionScreen == false)
            {
                if (currentBundle != 0)
                    Text_Manager.DrawText(Convert.ToString("Current rating: " + rating),
                        (int)(Global_Info.AccessWindowSize.X / Global_Info.AccessScreenScale) - 300, 10, aSpriteBatch);
            }

            if (Options_Menu.AccessControlView == true && isOnTransitionScreen == false)
            {
                if (findingCheat)
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
