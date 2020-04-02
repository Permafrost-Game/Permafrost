
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;

namespace GlobalWarmingGame
{
    public static class SoundFactory
    {
        public static Dictionary<Sound, SoundEffect> sounds;
        public static Dictionary<Songs, Song> songs;
        public static float Volume { get; set; } = 0.1f;
        private static Songs currSong;
        public static void Loadsounds(ContentManager content)
        {
            sounds = new Dictionary<Sound, SoundEffect>
            {
                { Sound.WoodChop, content.Load<SoundEffect>(@"sound/sounds/wood_chop") },
                { Sound.RabbitDeath, content.Load<SoundEffect>(@"sound/sounds/rabbit_death") },
                { Sound.FoxDeath, content.Load<SoundEffect>(@"sound/sounds/foxDeath") },
                { Sound.GoatDeath, content.Load<SoundEffect>(@"sound/sounds/goatDeath") },
                { Sound.StonePickup, content.Load<SoundEffect>(@"sound/sounds/stone_pickup") },
                { Sound.BearAttack, content.Load<SoundEffect>(@"sound/sounds/bear_roar") },
                { Sound.RobotAttack, content.Load<SoundEffect>(@"sound/sounds/robot_electricity") },
                { Sound.Slash, content.Load<SoundEffect>(@"sound/sounds/colonist_slashing") },
                { Sound.ColonistDeath, content.Load<SoundEffect>(@"sound/sounds/colonist_death") },
                { Sound.BearDeath, content.Load<SoundEffect>(@"sound/sounds/bear_death") },
                { Sound.RobotDeath, content.Load<SoundEffect>(@"sound/sounds/robot_death") },
                { Sound.Shotgun, content.Load<SoundEffect>(@"sound/sounds/shotgun-shot") },
                { Sound.BandidtAttack, content.Load<SoundEffect>(@"sound/sounds/bandit-slashing") },
                { Sound.BanditDeath, content.Load<SoundEffect>(@"sound/sounds/banditDeath") },
                { Sound.BanditGiveUp, content.Load<SoundEffect>(@"sound/sounds/bandit-disarmed") },
                { Sound.BanditJoins, content.Load<SoundEffect>(@"sound/sounds/bandit joining colonist") },
                { Sound.RobotFire, content.Load<SoundEffect>(@"sound/sounds/robotFire") },
                { Sound.Explosion, content.Load<SoundEffect>(@"sound/sounds/explosion") }
            };
            songs = new Dictionary<Songs, Song>
            {
                { Songs.Menu, content.Load<Song>("sound/songs/menu") },
                { Songs.Main, content.Load<Song>("sound/songs/ColdAtmosphericMusic") },
                { Songs.EnemyZone, content.Load<Song>("sound/songs/enemy_zone") },
            };

        }
 
        public static void PlaySong(Songs songS)
        {
            if (currSong != songS)
            {
                currSong = songS;
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Volume = Volume;
                MediaPlayer.Play(songs[currSong]);
            }
        }
        public static void PlaySoundEffect(Sound sound)
        {
            sounds[sound].Play(volume: Volume, pitch: 0.0f, pan: 0.0f);
        }

        internal static void StopSong()
        {
            MediaPlayer.Stop();
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
    GoatDeath,
    FoxDeath,
    StonePickup,
    BearAttack,
    RobotAttack,
    Slash,
    ColonistDeath,
    BearDeath,
    RobotDeath,
    Looting,
    Shotgun,
    BandidtAttack,
    BanditDeath,
    BanditGiveUp,
    BanditJoins,
    RobotFire,
    Explosion
}