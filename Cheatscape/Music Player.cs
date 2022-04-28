using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Cheatscape
{
    static class Music_Player
    {
        public static string file { get; set; }

        [DllImport("winmm.dll")]
        private static extern long mciSendString(string lpstrCommand, StringBuilder lpstrReturnString, int uReturnLength, int hwndCallback);
        public static void Open(string musicFile)
        {
            file = musicFile;
            string command = "open \"" + file + "\" type MPEGVideo alias MyMp3";
            mciSendString(command, null, 0, 0);
        }

        public static void Play()
        {
            string command = "play MyMp3";
            mciSendString(command, null, 0, 0);
        }

        public static void Stop()
        {
            string command = "stop MyMp3";
            mciSendString(command, null, 0, 0);

            command = "close MyMp3";
            mciSendString(command, null, 0, 0);
        }

    }
}
