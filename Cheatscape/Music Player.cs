﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Cheatscape
{
    static class Music_Player
    {
        static List<SoundEffect> soundEffects = new List<SoundEffect>();
        static string tempString;
        static SoundEffect move1, move2, move3, move4, move5;
        static Song song1;

        public static void Load()
        {
            move1 = Global_Info.AccessContentManager.Load<SoundEffect>("move");
            move2 = Global_Info.AccessContentManager.Load<SoundEffect>("move2");
            move3 = Global_Info.AccessContentManager.Load<SoundEffect>("move3");
            move4 = Global_Info.AccessContentManager.Load<SoundEffect>("move4");
            move5 = Global_Info.AccessContentManager.Load<SoundEffect>("move5");
            song1 = Global_Info.AccessContentManager.Load<Song>("song1");
            soundEffects.Add(move1);
            soundEffects.Add(move2);
            soundEffects.Add(move3);
            soundEffects.Add(move4);
            soundEffects.Add(move5);
            //for (int i = 1; i < 3; i++)
            //{
            //    tempString = "move" + i;
            //    soundEffects.Add(tempString);
            //}
        }

        public static void MoveEffect()
        {
            Random rnd = new Random();
            int x = rnd.Next(0, soundEffects.Count);
            soundEffects[x].Play();
        }

        public static void BackgroundMusic()
        {
            MediaPlayer.Play(song1);
            MediaPlayer.Volume = 0;
        }

        public static void StopMusic()
        {
            MediaPlayer.Pause();
        }

    }

}

