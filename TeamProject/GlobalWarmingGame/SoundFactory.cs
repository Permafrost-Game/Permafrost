
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System.Media;
using Microsoft.Xna.Framework.Content;
using System;

namespace GlobalWarmingGame
{
    public static class SoundFactory
    {
        public static Dictionary<string, SoundEffect> sounds;
        public static Dictionary<string, Song> songs;
        public static void Loadsounds(ContentManager content)
        {
            sounds = new Dictionary<string, SoundEffect>();
            songs = new Dictionary<string, Song>();
            songs.Add("menu",content.Load<Song>("sound/songs/menu"));
            songs.Add("main",content.Load<Song>("sound/songs/ColdAtmosphericMusic"));
            songs.Add("enemy_zone", content.Load<Song>("sound/songs/enemy_zone"));
            sounds.Add("wood_chop", content.Load<SoundEffect>(@"sound/sounds/wood_chop"));
            sounds.Add("rabbit_death", content.Load<SoundEffect>(@"sound/sounds/rabbit_death"));
            sounds.Add("stone_pickup", content.Load<SoundEffect>(@"sound/sounds/stone_pickup"));
        }
 
        public static void PlaySong(Songs songS)
        {
            Song song = null;
            switch (songS)
            {
                case Songs.menu:
                    song = songs["menu"];
                    MediaPlayer.IsRepeating = true;
                    MediaPlayer.Play(song);
                    break;
                case Songs.main:
                    song = songs["main"];
                    MediaPlayer.IsRepeating = true;
                    MediaPlayer.Play(song);
                    break;
                case Songs.enemy_zone:
                    song = songs["enemy_zone"];
                    MediaPlayer.IsRepeating = true;
                    MediaPlayer.Play(song);
                    break;
                default:
                    throw new NotImplementedException(songS + " has not been implemented");
            }

        }
        public static void PlaySoundEffect(Sound sound)
        {
            SoundEffect mySound = null;
            switch (sound)
            {
                case Sound.wood_chop:
                    mySound = sounds["wood_chop"];
                    break;
                case Sound.rabbit_death:
                    mySound = sounds["rabbit_death"];
                    break;
                case Sound.stone_pickup:
                    mySound = sounds["stone_pickup"];
                    break;
                default:
                    throw new NotImplementedException(sound + " has not been implemented");
            }
             
            mySound.Play();
        } 
    }
}
public enum Songs
{
    main, 
    menu, 
    enemy_zone
}
public enum Sound
{
    wood_chop,
    rabbit_death,
    stone_pickup
}