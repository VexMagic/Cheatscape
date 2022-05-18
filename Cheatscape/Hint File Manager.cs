using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace Cheatscape
{
    static class Hint_File_Manager
    {
        public static string hintsDirectory;
        public static List<string> hintList;
        public static void LoadHints()
        {
            try
            {
                hintsDirectory = @"..\..\..\Text_Files\Hint_Files\Hints" + Level_Manager.AccessCurrentBundle + "-" + Level_Manager.AccessCurrentLevel + ".txt";
                hintList = File.ReadLines(hintsDirectory).ToList();
            }
            catch (Exception)
            {
                Transition.StartTransition(Transition.TransitionState.ToLvSelect);
            }

        }


    }
}
