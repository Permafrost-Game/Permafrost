
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
        public static void Loadsounds(ContentManager content)
        {
            sounds = new Dictionary<Sound, SoundEffect>
            {
                { Sound.WoodChop, content.Load<SoundEffect>(@"sound/sounds/wood_chop") },
                { Sound.RabbitDeath, content.Load<SoundEffect>(@"sound/sounds/rabbit_death") },
                { Sound.StonePickup, content.Load<SoundEffect>(@"sound/sounds/stone_pickup") },
                { Sound.roaringBear, content.Load<SoundEffect>(@"sound/sounds/bear_roar") },
                { Sound.robotShock, content.Load<SoundEffect>(@"sound/sounds/robot_electricity") },
                { Sound.slashSound, content.Load<SoundEffect>(@"sound/sounds/colonist_slashing") },
                { Sound.colonistDying, content.Load<SoundEffect>(@"sound/sounds/colonist_death") },
                { Sound.bearDying, content.Load<SoundEffect>(@"sound/sounds/bear_death") },
                { Sound.robotBreak, content.Load<SoundEffect>(@"sound/sounds/robot_death") },
                { Sound.Shotgun, content.Load<SoundEffect>(@"sound/sounds/shotgun-shot") },
                 { Sound.banditCut, content.Load<SoundEffect>(@"sound/sounds/bandit-slashing") },
                 { Sound.banditDying, content.Load<SoundEffect>(@"sound/sounds/banditDeath") },
                 { Sound.banditGiveUp, content.Load<SoundEffect>(@"sound/sounds/bandit-disarmed") },
                 { Sound.banditJoins, content.Load<SoundEffect>(@"sound/sounds/bandit joining colonist") },
                 { Sound.robotFire, content.Load<SoundEffect>(@"sound/sounds/robotFire") },
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
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(songs[songS]);
        }
        public static void PlaySoundEffect(Sound sound)
        {
            sounds[sound].Play();
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
    StonePickup,
    roaringBear,
    robotShock,
    slashSound,
    colonistDying,
    bearDying,
    robotBreak,
    looting,
    Shotgun,
    banditCut,
    banditDying,
    banditGiveUp,
    banditJoins,
    robotFire,
    Explosion
}