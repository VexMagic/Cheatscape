using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Cheatscape
{
    static class File_Manager
    {
        public static void LoadLevel()
        {
            int tempCounter = 0;
            string tempLine;
            string tempDirectory = Directory.GetCurrentDirectory() + @"\Text Files\Level" + Level_Manager.AccessCurrentLevel + ".txt";
            Level_Manager.AccessAllMoves.Clear();

            StreamReader file = new StreamReader(tempDirectory);
            while ((tempLine = file.ReadLine()) != null)
            {
                string[] tempSeperatedMoves = tempLine.Split(',');
                List<Chess_Move> tempMoves = new List<Chess_Move>();

                for (int i = 0; i < tempSeperatedMoves.Length; i++)
                {
                    string[] tempSeperatedData = tempSeperatedMoves[i].Split(';');

                    tempMoves.Add(new Chess_Move(tempSeperatedData[0], tempSeperatedData[1]));
                }

                Level_Manager.AccessAllMoves.Add(tempMoves);

                tempCounter++;
            }
            file.Close();
        }

        public static void SaveTest() //save the winrate of each card
        {
            string tempDirectory = Directory.GetCurrentDirectory() + @"\Text Files\Level" + Level_Manager.AccessCurrentLevel + ".txt";

            using (StreamWriter tempFileStream = File.CreateText(tempDirectory)) //skriver in i filen
            {
                AddText(tempFileStream, "A2;B2");
            }
        }
        static void AddText(StreamWriter aStreamWriter, string aStringToAdd) //Add Text to file
        {
            char[] tempTextToWrite = aStringToAdd.ToCharArray();
            aStreamWriter.WriteLine(tempTextToWrite, 0, tempTextToWrite.Length);
        }
    }
}
