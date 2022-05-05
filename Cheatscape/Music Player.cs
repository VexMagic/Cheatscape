using System.Runtime.InteropServices;
using System.Text;

namespace Cheatscape
{
    static class Music_Player
    {
        public static string File { get; set; }

        [DllImport("winmm.dll")]
        private static extern long mciSendString(string lpstrCommand, StringBuilder lpstrReturnString, int uReturnLength, int hwndCallback);
        public static void Open(string musicFile)
        {
            File = musicFile;
            string command = "open \"" + File + "\" type MPEGVideo alias MyMp3";
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
