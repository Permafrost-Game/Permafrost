
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
        public static Dictionary<PFSong, Song> songs;
        public static Song song; 
        public static void Loadsounds(ContentManager content)
        {
            songs = new Dictionary<PFSong, Song>
            {
                { PFSong.Menu,      content.Load<Song>(@"sound/songs/menu") },
                { PFSong.Game,      content.Load<Song>(@"sound/songs/ColdAtmosphericMusic") },
                { PFSong.EnemyZone, content.Load<Song>(@"sound/songs/enemy_zone") }
            };
            sounds = new Dictionary<Sound, SoundEffect>
            {
                { Sound.WoodChop,       content.Load<SoundEffect>(@"sound/sounds/wood_chop") },
                { Sound.RabbitDeath,    content.Load<SoundEffect>(@"sound/sounds/rabbit_death") },
                { Sound.StonePickup,    content.Load<SoundEffect>(@"sound/sounds/stone_pickup") }
            };
        }
        static public void PlaySong(PFSong s, bool repeate = true)
        {
            song = songs[s];
            MediaPlayer.IsRepeating = repeate;
            MediaPlayer.Play(song);
            MediaPlayer.MediaStateChanged += MediaPlayer_MediaStateChanged;
        }
        private static void MediaPlayer_MediaStateChanged(object sender, System.EventArgs e)
        {
            MediaPlayer.Volume -= 100f;
            MediaPlayer.Play(song);
        }
        public static void PlaySoundEffect(Sound sound) => sounds[sound].Play();
    }
}
public enum Sound
{
    WoodChop,
    RabbitDeath,
    StonePickup,
}

public enum PFSong
{
    Menu,
    Game,
    EnemyZone,
}