using System;
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
        static List<Song> songs = new List<Song>();
        static Song mainTheme, currentSong;

        //song 1, 2 & soundeffects kan flyttas till load och behöver inte deklareras där.

        public static void Load()
        {

            SoundEffect move1 = Global_Info.AccessContentManager.Load<SoundEffect>("move");
            SoundEffect move2 = Global_Info.AccessContentManager.Load<SoundEffect>("move2");
            SoundEffect move3 = Global_Info.AccessContentManager.Load<SoundEffect>("move3");
            SoundEffect move4 = Global_Info.AccessContentManager.Load<SoundEffect>("move4");
            SoundEffect move5 = Global_Info.AccessContentManager.Load<SoundEffect>("move5");
            mainTheme = Global_Info.AccessContentManager.Load<Song>("song0");
            Song song1 = Global_Info.AccessContentManager.Load<Song>("LevelMusic");
            Song song2 = Global_Info.AccessContentManager.Load<Song>("song2");
            songs.Add(song1);
            songs.Add(song2);

            soundEffects.Add(move1);
            soundEffects.Add(move2);
            soundEffects.Add(move3);
            soundEffects.Add(move4);
            soundEffects.Add(move5);
            
            MediaPlayer.Volume = 0.5f;
            SoundEffect.MasterVolume = 0.5f;
            PlayMainTheme();
        }

        public static void MoveEffect()
        {
            Random rnd = new Random();
            int x = rnd.Next(0, soundEffects.Count);
            soundEffects[x].Play();
        }

        public static void PlayMusic()
        {
            MediaPlayer.Play(currentSong);
            MediaPlayer.IsRepeating = true;
        }

        public static void StopMusic()
        {
            MediaPlayer.Pause();
        }

        public static void ChangeMusic(int selectedSong)
        {
            try
            {
                currentSong = songs[0];
            }
            catch
            {
                MediaPlayer.Play(songs[0]);
            }
        }

        public static void PlayMainTheme()
        {
            if (currentSong == mainTheme)
            {
                MediaPlayer.Resume();
            }
            else
            {
                currentSong = mainTheme;
                PlayMusic();
            }
        }
    }

}

