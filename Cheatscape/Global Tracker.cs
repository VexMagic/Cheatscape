using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using System.Runtime.CompilerServices;

namespace Cheatscape
{
    static class Global_Tracker
    {
        public static List<Tuple<int, float>> completedBundels = new List<Tuple<int, float>>();
        public static char[] MyChar = { '(', ')', ' ' };

        public static void Deconstruct<T>(this IList<T> completedBundels, out T first, out T second, out IList<T> rest)
        {
            first = completedBundels.Count > 0 ? completedBundels[0] : default(T);
            second = completedBundels.Count > 1 ? completedBundels[1] : default(T);
            rest = completedBundels.Skip(2).ToList();           
        }
        public static void AddCompletedLevel(int completedBundle, float grade) //Add Text to file
        {
            try
            {
                if (grade >= completedBundels[completedBundle].Item2)
                {
                    completedBundels[completedBundle] = new Tuple<int, float>(completedBundle, grade);
                }
                else
                {
                    
                }
            }
            catch
            {
                completedBundels.Add(new Tuple<int, float>(completedBundle, grade));
            }
            

            File.WriteAllText(@"..\..\..\Text_Files\Global_Tracker.txt", String.Empty);

            TextWriter tw = new StreamWriter(@"..\..\..\Text_Files\Global_Tracker.txt");

            foreach (var s in completedBundels)      
                tw.WriteLine(s);           

            tw.Close();
        }
        public static void LoadCompletedBundles()
        {
            try
            {
                foreach (string line in File.ReadLines(@"..\..\..\Text_Files\Global_Tracker.txt"))
                {
                    var (first, second, rest) = line.Split(',');
                    string firstConverted = Regex.Replace(first, "[^0-9]", "");
                    string secondConverted = Regex.Replace(second, "[^0-9]", "");

                    int bundleID = Int32.Parse(firstConverted);
                    float grade = float.Parse(secondConverted);

                    completedBundels.Add(new Tuple<int, float>(bundleID, grade));
                }
            }
            catch
            {

            }
        }
    }
}
