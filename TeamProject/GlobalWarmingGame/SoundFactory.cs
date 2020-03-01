
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
            sounds = new Dictionary<Sound, SoundEffect>
            {
                { Sound.WoodChop, content.Load<SoundEffect>(@"sound/sounds/wood_chop") },
                { Sound.RabbitDeath, content.Load<SoundEffect>(@"sound/sounds/rabbit_death") },
                { Sound.StonePickup, content.Load<SoundEffect>(@"sound/sounds/stone_pickup") }
            };
            songs = new Dictionary<Songs, Song>
            {
                { Songs.Menu, content.Load<Song>("sound/songs/menu") },
                { Songs.Main, content.Load<Song>("sound/songs/ColdAtmosphericMusic") },
                { Songs.EnemyZone, content.Load<Song>("sound/songs/enemy_zone") }
            };

        }
 
        public static void PlaySong(Songs songS)
        {
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(songs[songS]);
        }
        public static void PlaySoundEffect(Sound sound)
        {
            sounds[sound].Play();
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