using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cheatscape
{
    static class Level_Manager
    {
        static int CurrentLevel = 0;
        static List<List<Chess_Move>> AllMoves = new List<List<Chess_Move>>();
        static List<Tuple<Chess_Move, int>> AllAnswers = new List<Tuple<Chess_Move, int>>();
        static int CurrentSlide = 1;
        static bool FindingCheat = false;
        static int AmountOfRuleLists = 3;

        public static int AccessCurrentLevel { get => CurrentLevel; set => CurrentLevel = value; }
        public static List<List<Chess_Move>> AccessAllMoves { get => AllMoves; set => AllMoves = value; }
        public static List<Tuple<Chess_Move, int>> AccessAllAnswers { get => AllAnswers; set => AllAnswers = value; }
        public static int AccessCurrentSlide { get => CurrentSlide; set => CurrentSlide = value; }

        public static void Update()
        {
            if (Input_Manager.KeyPressed(Keys.Left) && CurrentSlide > 1 && !FindingCheat)
            {
                Hand_Animation_Manager.ResetAllHands();
                CurrentSlide--;
                Game_Board.SetBoardState();
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
            else if (Input_Manager.KeyPressed(Keys.Right) && CurrentSlide < AllMoves.Count && !FindingCheat)
            {
                Hand_Animation_Manager.ResetAllHands();
                CurrentSlide++;
                Game_Board.SetBoardState();
                for (int i = 0; i < AllMoves[CurrentSlide - 1].Count; i++)
                {
                    Game_Board.MoveChessPiece(AllMoves[CurrentSlide - 1][i], true);
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
            else if (Input_Manager.KeyPressed(Keys.Space))
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
                            //Global_Info.AccessCurrentGameState = Global_Info.GameState.LevelSelect;
                            CurrentLevel++;
                            File_Manager.LoadLevel();
                        }
                        else if (Rules_List.AccessCurrentRule != Rules_List.GetList().Length)
                        {
                            //lose life
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
        }

        public static void Draw(SpriteBatch aSpriteBatch)
        {
            if (FindingCheat)
                Rules_List.Draw(aSpriteBatch);

            if (Options_Menu.AccessControlView == true)
            {
                if (FindingCheat)
                {
                    Text_Manager.DrawText("Right/Left: Toggle categories     Up/Down: Toggle rules     Space: Select rule", 120, 
                        (int)(Global_Info.AccessWindowSize.Y / Global_Info.AccessScreenScale) - 20, aSpriteBatch);
                }
                else
                {
                    Text_Manager.DrawText("Right: Forward               Left: Back               Space: Rules", 120, 
                        (int)(Global_Info.AccessWindowSize.Y / Global_Info.AccessScreenScale) - 20, aSpriteBatch);
                }
            }
        }
    }
}
