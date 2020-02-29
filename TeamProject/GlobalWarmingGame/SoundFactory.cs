
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System.Media;
using Microsoft.Xna.Framework.Content;
namespace GlobalWarmingGame
{
    public static class SoundFactory
    {
        public static Song song; 
        public static Dictionary<string, SoundEffect> sounds;
        public static void Loadsounds(ContentManager content)
        {
            sounds = new Dictionary<string, SoundEffect>();
            song = content.Load<Song>("sound/There _is_a_light_that_never_goes_out");
            sounds.Add("wood_chop", content.Load<SoundEffect>(@"sound/sounds/wood_chop"));
            sounds.Add("rabbit_death", content.Load<SoundEffect>(@"sound/sounds/rabbit_death"));
            sounds.Add("stone_pickup", content.Load<SoundEffect>(@"sound/sounds/stone_pickup"));
        }
        static public void PlayGameSoundtrack()
        {
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(song);
            MediaPlayer.MediaStateChanged += MediaPlayer_MediaStateChanged;
        }
        public static void MediaPlayer_MediaStateChanged(object sender, System.EventArgs e)
        {
            MediaPlayer.Volume -= 0.1f;
            MediaPlayer.Play(song);
        }
    }
}