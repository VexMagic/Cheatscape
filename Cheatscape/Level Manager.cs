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
        static int CurrentSlide = 0;
        static int ButtonCooldown = 0;

        public static int AccessCurrentLevel { get => CurrentLevel; set => CurrentLevel = value; }
        public static List<List<Chess_Move>> AccessAllMoves { get => AllMoves; set => AllMoves = value; }
        public static int AccessCurrentSlide { get => CurrentSlide; set => CurrentSlide = value; }

        public static void Update()
        {
            if (ButtonCooldown > 0)
                ButtonCooldown--;

            if (Keyboard.GetState().IsKeyDown(Keys.Left) && CurrentSlide > 0 && ButtonCooldown == 0)
            {
                Game_Board.MoveBack();
                CurrentSlide--;
                ButtonCooldown = 12;
            }
            else if(Keyboard.GetState().IsKeyDown(Keys.Right) && CurrentSlide < AllMoves.Count && ButtonCooldown == 0)
            {
                for (int i = 0; i < AllMoves[CurrentSlide].Count; i++)
                {
                    Game_Board.MoveChessPiece(AllMoves[CurrentSlide][i]);
                }
                CurrentSlide++;
                ButtonCooldown = 12;
            }
        }
    }
}
