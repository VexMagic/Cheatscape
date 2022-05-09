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
        static bool findingCheat = false;
        static int amountOfRuleLists = 3;

        public static int AccessCurrentLevel { get => currentLevel; set => currentLevel = value; }
        public static int AccessCurrentBundle { get => currentBundle; set => currentBundle = value; }
        public static List<List<Chess_Move>> AccessAllMoves { get => allMoves; set => allMoves = value; }
        public static List<Tuple<Chess_Move, int>> AccessAllAnswers { get => allAnswers; set => allAnswers = value; }
        public static int AccessCurrentSlide { get => currentSlide; set => currentSlide = value; }

        public static void Update()
        {
            if (Input_Manager.KeyPressed(Keys.Tab))
            {
                Pause_Menu.gameIsPaused = !Pause_Menu.gameIsPaused;
            }

            if (Pause_Menu.gameIsPaused == false)
            {
                if (Input_Manager.KeyPressed(Keys.Left) && currentSlide > 1 && !findingCheat)
                {
                    Hand_Animation_Manager.ResetAllHands();
                    File_Manager.turnCounter++;
                    currentSlide--;
                    Game_Board.SetBoardState();
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
                else if (Input_Manager.KeyPressed(Keys.Right) && currentSlide < allMoves.Count && !findingCheat)
                {
                    Hand_Animation_Manager.ResetAllHands();
                    File_Manager.turnCounter--;
                    currentSlide++;
                    Game_Board.SetBoardState();
                    for (int i = 0; i < allMoves[currentSlide - 1].Count; i++)
                    {
                        Game_Board.MoveChessPiece(allMoves[currentSlide - 1][i], true);
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
                else if (Input_Manager.KeyPressed(Keys.Space))
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
                                //Global_Info.AccessCurrentGameState = Global_Info.GameState.LevelSelect;
                                currentLevel++;
                                File_Manager.LoadLevel();
                                Music_Player.StopMusic();
                            }
                            else if (Rules_List.AccessCurrentRule != Rules_List.GetList().Length)
                            {
                                //lose life
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

            }

        }

        public static void Play(GameTime gameTime)
        {
            if (playedThrough == false && !findingCheat && Pause_Menu.gameIsPaused == false)
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
                            Game_Board.MoveChessPiece(allMoves[currentSlide - 1][i], true);
                        }
                        timer = 0;
                    }
                }
                else
                {
                    playedThrough = true;
                }

            }

        }

        public static void Draw(SpriteBatch aSpriteBatch)
        {
            if (findingCheat)
                Rules_List.Draw(aSpriteBatch);

            if (Options_Menu.AccessControlView == true)
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
            }
        }
    }
}
