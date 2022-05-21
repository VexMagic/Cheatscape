using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace Cheatscape
{
    static class File_Manager
    {
        public static int turnCounter = 0;

        public static void LoadLevel()
        {
            string tempLine;
            string tempDirectory = @"..\..\..\Text_Files\Level" + Level_Manager.AccessCurrentBundle + "-" + Level_Manager.AccessCurrentLevel + ".txt";
            Level_Manager.AccessAllMoves.Clear();
            try
            {
                StreamReader file = new StreamReader(tempDirectory);
                while ((tempLine = file.ReadLine()) != null)
                {
                    string[] tempSeperatedMoves = tempLine.Split(',');
                    List<Chess_Move> tempMoves = new List<Chess_Move>();

                    for (int i = 0; i < tempSeperatedMoves.Length; i++)
                    {
                        string[] tempSeperatedData = tempSeperatedMoves[i].Split(';');

                        tempMoves.Add(new Chess_Move(tempSeperatedData));
                    }

                    Level_Manager.AccessAllMoves.Add(tempMoves);

                }
                turnCounter = File.ReadAllLines(tempDirectory).Length - 1;
                file.Close();
                Game_Board.ResetBoard();
            }


            catch
            {
                End_Screen.AccessCleared = true;
                End_Screen.AccessIsEnded = false;
            }
        }
    }
}
