using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Cheatscape
{
    static class Level_Manager
    {
        static int currentLevel = 0;
        static List<List<Chess_Move>> allMoves = new List<List<Chess_Move>>();
        static List<Tuple<Chess_Move, int>> allAnswers = new List<Tuple<Chess_Move, int>>();
        static int currentSlide = 0;
        static bool findingCheat = false;
        static int amountOfRuleLists = 3;

        public static int AccessCurrentLevel { get => currentLevel; set => currentLevel = value; }
        public static List<List<Chess_Move>> AccessAllMoves { get => allMoves; set => allMoves = value; }
        public static List<Tuple<Chess_Move, int>> AccessAllAnswers { get => allAnswers; set => allAnswers = value; }
        public static int AccessCurrentSlide { get => currentSlide; set => currentSlide = value; }

        public static void Update()
        {
            if (Input_Manager.KeyPressed(Keys.Left) && currentSlide > 0 && !findingCheat)
            {
                Hand_Animation_Manager.ResetAllHands();
                Game_Board.SetBoardState();
                currentSlide--;
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
                            Global_Info.AccessCurrentGameState = Global_Info.GameState.levelSelect;
                        }
                    }
                    findingCheat = false;
                }
            }
            else if (Input_Manager.KeyPressed(Keys.Up) && findingCheat)
            {
                Rules_List.AccessCurrentRule--;
            }
            else if (Input_Manager.KeyPressed(Keys.Down) && findingCheat)
            {
                Rules_List.AccessCurrentRule++;
            }
        }

        public static void Draw(SpriteBatch aSpriteBatch)
        {
            if (findingCheat)
                Rules_List.Draw(aSpriteBatch);
        }
    }
}
