
using Microsoft.Xna.Framework.Media;
using System.Media; 
namespace GlobalWarmingGame
{
    internal class MusicManager
    {
        Song song;
        public void PlayGameSoundtrack(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            this.song = content.Load<Song>("sound/There _is_a_light_that_never_goes_out");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(song);
            MediaPlayer.MediaStateChanged += MediaPlayer_MediaStateChanged;
        }
        void MediaPlayer_MediaStateChanged(object sender, System.EventArgs e)
        {
            MediaPlayer.Volume -= 0.1f;
            MediaPlayer.Play(song);
        }
    }
}