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
        static int CurrentSlide = 0;
        static bool FindingCheat = false;
        static int AmountOfRuleLists = 3;

        public static int AccessCurrentLevel { get => CurrentLevel; set => CurrentLevel = value; }
        public static List<List<Chess_Move>> AccessAllMoves { get => AllMoves; set => AllMoves = value; }
        public static List<Tuple<Chess_Move, int>> AccessAllAnswers { get => AllAnswers; set => AllAnswers = value; }
        public static int AccessCurrentSlide { get => CurrentSlide; set => CurrentSlide = value; }

        public static void Update()
        {
            if (Global_Info.AccessButtonCooldown == 0)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Left) && CurrentSlide > 0 && !FindingCheat)
                {
                    Hand_Animation_Manager.ResetAllHands();
                    Game_Board.SetBoardState();
                    CurrentSlide--;
                    Global_Info.AccessButtonCooldown = 12;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Left) && FindingCheat)
                {
                    Rules_List.AccessCurrentRuleList--;
                    if (Rules_List.AccessCurrentRuleList < 0)
                    {
                        Rules_List.AccessCurrentRuleList = AmountOfRuleLists - 1;
                    }
                    Rules_List.AccessCurrentRule = 0;
                    Global_Info.AccessButtonCooldown = 12;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Right) && CurrentSlide < AllMoves.Count && !FindingCheat)
                {
                    Hand_Animation_Manager.ResetAllHands();
                    CurrentSlide++;
                    Game_Board.SetBoardState();
                    for (int i = 0; i < AllMoves[CurrentSlide - 1].Count; i++)
                    {
                        Game_Board.MoveChessPiece(AllMoves[CurrentSlide - 1][i], true);
                    }
                    Global_Info.AccessButtonCooldown = 12;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Right) && FindingCheat)
                {
                    Rules_List.AccessCurrentRuleList++;
                    if (Rules_List.AccessCurrentRuleList >= AmountOfRuleLists)
                    {
                        Rules_List.AccessCurrentRuleList = 0;
                    }
                    Rules_List.AccessCurrentRule = 0;
                    Global_Info.AccessButtonCooldown = 12;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    if (!FindingCheat)
                        FindingCheat = true;
                    else
                    {
                        for (int i = 0; i < AllAnswers.Count; i++)
                        {
                            if (AllAnswers[i].Item1.myCheatAnswer.X == Rules_List.AccessCurrentRuleList && 
                                AllAnswers[i].Item1.myCheatAnswer.Y == Rules_List.AccessCurrentRule && 
                                AllAnswers[i].Item2 == CurrentSlide)
                            {
                                Global_Info.AccessCurrentGameState = Global_Info.GameState.LevelSelect;
                            }
                        }
                        FindingCheat = false;
                    }

                    Global_Info.AccessButtonCooldown = 12;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Up) && FindingCheat)
                {
                    Rules_List.AccessCurrentRule--;
                    Global_Info.AccessButtonCooldown = 12;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Down) && FindingCheat)
                {
                    Rules_List.AccessCurrentRule++;
                    Global_Info.AccessButtonCooldown = 12;
                }
            }
        }

        public static void Draw(SpriteBatch aSpriteBatch)
        {
            if (FindingCheat)
                Rules_List.Draw(aSpriteBatch);
        }
    }
}
