
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
        public static Dictionary<Sound, SoundEffect> sounds;
        public static Dictionary<Songs, Song> songs;
        public static void Loadsounds(ContentManager content)
        {
            sounds = new Dictionary<Sound, SoundEffect>();
            songs = new Dictionary<Songs, Song>();
            songs.Add(Songs.Menu,content.Load<Song>("sound/songs/menu"));
            songs.Add(Songs.Main,content.Load<Song>("sound/songs/ColdAtmosphericMusic"));
            songs.Add(Songs.EnemyZone, content.Load<Song>("sound/songs/enemy_zone"));
            sounds.Add(Sound.WoodChop, content.Load<SoundEffect>(@"sound/sounds/wood_chop"));
            sounds.Add(Sound.RabbitDeath, content.Load<SoundEffect>(@"sound/sounds/rabbit_death"));
            sounds.Add(Sound.StonePickup, content.Load<SoundEffect>(@"sound/sounds/stone_pickup"));
        }
 
        public static void PlaySong(Songs songS)
        {
            Song song = null;
            switch (songS)
            {
                case Songs.Menu:
                    song = songs[Songs.Menu];
                    MediaPlayer.IsRepeating = true;
                    MediaPlayer.Play(song);
                    break;
                case Songs.Main:
                    song = songs[Songs.Main];
                    MediaPlayer.IsRepeating = true;
                    MediaPlayer.Play(song);
                    break;
                case Songs.EnemyZone:
                    song = songs[Songs.EnemyZone];
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
                case Sound.WoodChop:
                    mySound = sounds[Sound.WoodChop];
                    break;
                case Sound.RabbitDeath:
                    mySound = sounds[Sound.RabbitDeath];
                    break;
                case Sound.StonePickup:
                    mySound = sounds[Sound.StonePickup];
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
    Main, 
    Menu, 
    EnemyZone
}
public enum Sound
{
    WoodChop,
    RabbitDeath,
    StonePickup
}